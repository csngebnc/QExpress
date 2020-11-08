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
            string id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            if(id == null)
            {
                return new FelhasznaloDTO{ jogosultsagi_szint = 0, Email= "None", Id = "", UserName = "None" };
            }
            var currentUser = await _context.Felhasznalo.FindAsync(id);
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
        public async Task<ActionResult<FelhasznaloDTO>> GetFelhasznalo(String id)
        {
            var felhasznalo = await _context.Felhasznalo.FindAsync(id);

            if (!FelhasznaloExists(id))
            {
                return NotFound();
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

        /*
         * A bejelentkezett felhasznalo e-mail cimenek megvaltoztatasa
         * api/Felhasznalo/NewEmail
         * param: uj email
         */
        [HttpPost]
        [Route("NewEmail")]
        public async Task<IActionResult> EditFelhasznaloEmail(String uj_email)
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
        [HttpPost]
        [Route("NewPassword")]
        public async Task<IActionResult> EditFelhasznaloJelszo(String regi_jelszo, String uj_jelszo)
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

            if (felh.PasswordHash.Equals(regi_jelszo))
                felh.PasswordHash = uj_jelszo;
            await _context.SaveChangesAsync();

            var dto = new FelhasznaloDTO(felh);

            return CreatedAtAction(nameof(GetFelhasznalo), new { id = felh.Id }, dto);
        }


        /*
         * Egy felhasznalo felvetele
         * ###### Aktualis login mellett nem feltetlenul szukseges. ######
         * api/Felhasznalo/AddFelhasznalo
         */
        [HttpPost]
        [Route("AddFelhasznalo")]
        public async Task<ActionResult<FelhasznaloDTO>> AddFelhasznaloParams(String name, String email, String pw, int jog_szint)
        {
            Felhasznalo newUser = new Felhasznalo { UserName = name, Email = email, PasswordHash = pw, jogosultsagi_szint = jog_szint };
            _context.Felhasznalo.Add(newUser);
            await _context.SaveChangesAsync();

            var dto = new FelhasznaloDTO(newUser);

            return CreatedAtAction(nameof(GetFelhasznalo), new { id = newUser.Id }, dto);
        }

        /*
         * Megadott ID-val rendelkezo felhasznalo torlese
         * api/Felhasznalo/Delete/{id}
         */
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<FelhasznaloDTO>> DeleteFelhasznalo(String id)
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
        public async Task<IActionResult> SetTelephely(FelhasznaloTelephelyDTO felhasznaloTelephely)
        {
            Felhasznalo felh = _context.Felhasznalo.Where(f => f.Id.Equals(felhasznaloTelephely.FelhasznaloId)).First();
            if (!FelhasznaloExists(felhasznaloTelephely.FelhasznaloId))
            {
                return NotFound();
            }

            if (!_context.Telephely.Any(e => e.Id == felhasznaloTelephely.TelephelyId))
            {
                return NotFound();
            }

            _context.FelhasznaloTelephely.Add(new FelhasznaloTelephely { FelhasznaloId = felhasznaloTelephely.FelhasznaloId, TelephelyId = felhasznaloTelephely.TelephelyId });
            felh.jogosultsagi_szint = 2;

            await _context.SaveChangesAsync();

            var dto = new FelhasznaloDTO(felh);

            return CreatedAtAction(nameof(GetFelhasznalo), new { id = felh.Id }, dto);
        }

        /*
         * A megadott felhasznalo eltavolitasa egy megadott telephely alkalmazotti listajabol.
         * api/Felhasznalo/DelFromTelephely
         * param: FelhasznaloTelephelyDTO --> FelhasznaloId, TelephelyId
         */
        [HttpPost]
        [Route("DelFromTelephely")]
        public async Task<IActionResult> DelFromTelephely(FelhasznaloTelephelyDTO felhasznaloTelephely)
        {
            if (!FelhasznaloExists(felhasznaloTelephely.FelhasznaloId))
            {
                return NotFound();
            }

            Felhasznalo felh = await _context.Felhasznalo.FindAsync(felhasznaloTelephely.FelhasznaloId);

            if (!_context.Telephely.Any(e => e.Id == felhasznaloTelephely.TelephelyId))
            {
                return NotFound();
            }

            var felhTelep = await _context.FelhasznaloTelephely
                .Where(f => f.FelhasznaloId.Equals(felhasznaloTelephely.FelhasznaloId))
                .Where(t => t.TelephelyId == felhasznaloTelephely.TelephelyId).FirstAsync();

            _context.FelhasznaloTelephely.Remove(felhTelep);

            if (!_context.FelhasznaloTelephely.Any(f => f.FelhasznaloId.Equals(felhasznaloTelephely.FelhasznaloId)))
                felh.jogosultsagi_szint = 1;

            await _context.SaveChangesAsync();


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
