using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QExpress.Data;
using QExpress.Models;
using QExpress.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FelhasznaloController : ControllerBase
    {
        private readonly QExpressDbContext _context;
        public FelhasznaloController(QExpressDbContext context)
        {
            _context = context;
        }

        /*
         * Aktualisan bejelentkezett felhasznalo lekerese.
         * api/Felhasznalo/GetCurrentFelhasznalo
         */
        [HttpGet]
        [Route("GetCurrentFelhasznalo")]
        public async Task<ActionResult<FelhasznaloDTO>> GetCurrentFelhasznalo()
        {
            var user = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
            if(user == null)
            {
                return new FelhasznaloDTO{ jogosultsagi_szint = 0, Email= "None", Id = "", UserName = "None" };
            }
            var currentUser = await _context.Felhasznalo.FindAsync(user.Value);
            return new FelhasznaloDTO(currentUser);
        }

        /*
         * Osszes felhasznalo lekerese
         * api/Felhasznalo/GetFelhasznalok
         */
        [HttpGet]
        [Route("GetFelhasznalok")]
        public async Task<ActionResult<IEnumerable<FelhasznaloDTO>>> GetFelhasznalok()
        {
            var felhasznalok = await _context.Felhasznalo.ToListAsync();
            var dto = new List<FelhasznaloDTO>();
            foreach (var f in felhasznalok)
            {
                dto.Add(new FelhasznaloDTO(f));
            }
            return dto;
        }

        /*
         * Egy felhasznalo lekerese megadott ID alapjan.
         * api/Felhasznalo/GetFelhasznalo/{id}
         */
        [HttpGet("GetFelhasznalo/{id}")]
        public async Task<ActionResult<FelhasznaloDTO>> GetFelhasznalo([FromRoute] String id)
        {
            if (!FelhasznaloExists(id))
            {
                ModelState.AddModelError(nameof(id), "A megadott felhasználó nem létezik.");
                return BadRequest(ModelState);
            }
            var felhasznalo = await _context.Felhasznalo.FindAsync(id);

            return new FelhasznaloDTO(felhasznalo);
        }

        /*
         * Egy felhasznalo lekerese megadott email alapjan.
         * api/Felhasznalo/GetFelhasznaloByEmail/{email}
         */
        [HttpGet("GetFelhasznaloByEmail/{email}")]
        public async Task<ActionResult<FelhasznaloDTO>> GetFelhasznaloByEmail([FromRoute] String email)
        {
            var felhasznalo = await _context.Felhasznalo.Where(f => f.Email.Equals(email)).FirstAsync();
            if (felhasznalo == null)
            {
                ModelState.AddModelError(nameof(email), "A megadott e-mail címmel nem létezik felhasználó.");
                return BadRequest(ModelState);
            }
            return new FelhasznaloDTO(felhasznalo);
        }

        /*
         * A bejelentkezett felhasznalo aktiv sorszamainak lekerese
         * api/Felhasznalo/AktivSorszamok
         */
        [HttpGet]
        [Route("AktivSorszamok")]
        public async Task<ActionResult<IEnumerable<SorszamDTO>>> GetFelhasznaloSorszamai()
        {
            string id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            var sorszamok = await _context.Sorszam.Where(s => s.UgyfelId.Equals(id) && s.Allapot.Equals("aktív")).ToListAsync();
            var dto = new List<SorszamDTO>();
            foreach (var s in sorszamok)
            {
                dto.Add(new SorszamDTO(s));
            }
            return dto;
        }

        /*
         * A bejelentkezett felhasznalo mar korabban behivott sorszamainak lekerese
         * api/Felhasznalo/KorabbiSorszamok
         */
        [HttpGet]
        [Route("KorabbiSorszamok")]
        public async Task<ActionResult<IEnumerable<SorszamDTO>>> GetKorabbiSorszamok()
        {
            string id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            var sorszamok = await _context.Sorszam.Where(s => s.UgyfelId.Equals(id) && s.Allapot.Equals("Behivott")).ToListAsync();
            var dto = new List<SorszamDTO>();
            foreach (var s in sorszamok)
            {
                dto.Add(new SorszamDTO(s));
            }
            return dto;
        }

        [HttpGet]
        [Route("GetTelephelySorszamai")]
        public async Task<ActionResult<IEnumerable<SorszamDTO>>> GetTelephelySorszamai()
        {
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            var telephelyHozzarendeles = await _context.FelhasznaloTelephely.Where(ft => ft.FelhasznaloId.Equals(user_id)).FirstAsync();
            var telephely = await _context.Telephely.FindAsync(telephelyHozzarendeles.TelephelyId);
            

            var dto = new List<SorszamDTO>();
            foreach (var s in telephely.Sorszam)
            {
                dto.Add(new SorszamDTO(s));
            }

            return dto;
        }

        /*
         * A bejelentkezett felhasznalo e-mail cimenek megvaltoztatasa
         * api/Felhasznalo/NewEmail
         * param: uj email
         */
        [HttpPut]
        [Route("NewEmail")]
        public async Task<IActionResult> EditFelhasznaloEmail([FromBody] String uj_email)
        {
            string id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            Felhasznalo felh = _context.Felhasznalo.Where(f => f.Id.Equals(id)).First();
            if (!FelhasznaloExists(id))
            {
                return NotFound();
            }
            if (id != felh.Id)
            {
                return BadRequest();
            }

            felh.Email = uj_email;
            await _context.SaveChangesAsync();

            var dto = new FelhasznaloDTO(felh);

            return CreatedAtAction(nameof(GetFelhasznalo), new { id = felh.Id }, dto);
        }

        /*
         * A bejelentkezett felhasznalo jelszavanak megvaltoztatasa
         * api/Felhasznalo/NewPassword
         * params: regi_jelszo, uj_jelszo
         */
        [HttpPut]
        [Route("NewPassword")]
        public async Task<IActionResult> EditFelhasznaloJelszo([FromBody] String[] jelszavak)
        {
            string id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            Felhasznalo felh = await _context.Felhasznalo.FindAsync(id);
            if (!FelhasznaloExists(id))
            {
                return NotFound();
            }
            if (!id.Equals(felh.Id))
            {
                return BadRequest();
            }

            if (felh.PasswordHash.Equals(jelszavak[0]))
                felh.PasswordHash = jelszavak[1];
            await _context.SaveChangesAsync();

            var dto = new FelhasznaloDTO(felh);

            return CreatedAtAction(nameof(GetFelhasznalo), new { id = felh.Id }, dto);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<FelhasznaloDTO>> DeleteFelhasznalo([FromRoute] String id)
        {
            var felhasznalo = await _context.Felhasznalo.FindAsync(id);
            if (felhasznalo == null)
            {
                return NotFound();
            }

            var ceg = await _context.Ceg.Where(c => c.CegadminId.Equals(felhasznalo.Id)).FirstAsync();
            if (ceg != null)
                return BadRequest();

            _context.Felhasznalo.Remove(felhasznalo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*
         * Megadott ID-val rendelkezo felhasznalo hozzarendelese a megadott telephelyhez.
         * api/Felhasznalo/SetTelephely
         * param: FelhasznaloTelephelyDTO --> FelhasznaloId, TelephelyId
         */
        [HttpPost]
        [Route("SetTelephely")]
        public async Task<IActionResult> SetTelephely([FromBody] FelhasznaloTelephelyDTO felhasznaloTelephely)
        {
            if (_context.FelhasznaloTelephely.Any(ft => ft.TelephelyId == felhasznaloTelephely.TelephelyId && ft.FelhasznaloId.Equals(felhasznaloTelephely.FelhasznaloId)))
                return BadRequest();

            if (!FelhasznaloExists(felhasznaloTelephely.FelhasznaloId))
            {
                return NotFound();
            }

            if (!_context.Telephely.Any(e => e.Id == felhasznaloTelephely.TelephelyId))
            {
                return NotFound();
            }

            Felhasznalo felh = _context.Felhasznalo.Where(f => f.Id.Equals(felhasznaloTelephely.FelhasznaloId)).First();
            _context.FelhasznaloTelephely.Add(new FelhasznaloTelephely { FelhasznaloId = felhasznaloTelephely.FelhasznaloId, TelephelyId = felhasznaloTelephely.TelephelyId });
            felh.jogosultsagi_szint = 2;

            await _context.SaveChangesAsync();

            var dto = new FelhasznaloDTO(felh);

            return CreatedAtAction(nameof(GetFelhasznalo), new { id = felh.Id }, dto);
        }

        [HttpPut]
        [Route("UpdateUgyintezoTelephely")]
        public async Task<IActionResult> UpdateUgyintezoTelephely([FromBody] FelhasznaloTelephelyDTO felhasznaloTelephely)
        {
            if (!_context.FelhasznaloTelephely.Any(ft => ft.FelhasznaloId.Equals(felhasznaloTelephely.FelhasznaloId)))
            {
                return NotFound();
            }

            if(!_context.Telephely.Any(t => t.Id == felhasznaloTelephely.TelephelyId))
            {
                return NotFound();
            }

            var felhTelep = await _context.FelhasznaloTelephely.Where(f => f.FelhasznaloId.Equals(felhasznaloTelephely.FelhasznaloId)).FirstAsync();

            if (felhTelep != null)
            {
                felhTelep.TelephelyId = felhasznaloTelephely.TelephelyId;
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        /*
         * A megadott felhasznalo eltavolitasa egy megadott telephely alkalmazotti listajabol.
         * api/Felhasznalo/DelFromTelephely
         * param: FelhasznaloTelephelyDTO --> FelhasznaloId, TelephelyId
         */
        [HttpPost]
        [Route("DelFromTelephely")]
        public async Task<IActionResult> DelFromTelephely([FromBody] FelhasznaloTelephelyDTO felhasznaloTelephely)
        {
            if (!FelhasznaloExists(felhasznaloTelephely.FelhasznaloId))
            {
                return NotFound();
            }

            if (!_context.Telephely.Any(e => e.Id == felhasznaloTelephely.TelephelyId))
            {
                return NotFound();
            }

            Felhasznalo felh = await _context.Felhasznalo.FindAsync(felhasznaloTelephely.FelhasznaloId);
            var felhTelep = await _context.FelhasznaloTelephely
                .Where(f => f.FelhasznaloId.Equals(felhasznaloTelephely.FelhasznaloId))
                .Where(t => t.TelephelyId == felhasznaloTelephely.TelephelyId).FirstAsync();

            if (felhTelep != null)
            {
                _context.FelhasznaloTelephely.Remove(felhTelep);
                felh.jogosultsagi_szint = 1;
                await _context.SaveChangesAsync();
            }                

            return NoContent();
        }

        /*
         * Ellenorzes, hogy a parameterkent kapott ID-val letezik-e felhasznalo.
         */
        private bool FelhasznaloExists(String id)
        {
            return _context.Felhasznalo.Any(e => e.Id == id);
        }
    }
}
