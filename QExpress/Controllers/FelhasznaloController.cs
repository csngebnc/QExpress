using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QExpress.Data;
using QExpress.Models;
using QExpress.Models.DTOs;
using System;
using System.Collections.Generic;
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
         */
        [HttpGet("/GetCurrentFelhasznalo", Name = "CurrentFelhasznalo")]
        public async Task<ActionResult<FelhasznaloDTO>> GetCurrentFelhasznalo()
        {
            string id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            var currentUser = await _context.Felhasznalo.FindAsync(id);
            return new FelhasznaloDTO(currentUser);
        }

        /*
         * Osszes felhasznalo lekerese
         */
        [HttpGet("/GetFelhasznalok", Name = "GetFelhasznalok")]
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
         */
        [HttpGet("GetFelhasznalo/{id}", Name = "GetFelhasznalo")]
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
         */
        [HttpGet("/AktivSorszamok", Name = "GetAktivSorszamok")]
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
         */
        [HttpGet("/KorabbiSorszamok", Name = "GetKorabbiSorszamok")]
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
         */
        [HttpPost("/NewEmail", Name = "NewEmail")]
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
         */
        [HttpPost("/NewPassword", Name = "NewPassword")]
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
         * Aktualis login mellett nem feltetlenul szukseges.
         */
        [HttpPost("AddFelhasznalo", Name = "AddFelhasznalo")]
        public async Task<ActionResult<FelhasznaloDTO>> AddFelhasznaloParams(String name, String email, String pw, int jog_szint)
        {
            Felhasznalo newUser = new Felhasznalo { UserName = name, Email = email, PasswordHash = pw, jogosultsagi_szint = jog_szint };
            _context.Felhasznalo.Add(newUser);
            await _context.SaveChangesAsync();

            var dto = new FelhasznaloDTO(newUser);

            return CreatedAtAction(nameof(GetFelhasznalo), new { id = newUser.Id }, dto);
        }

        // Felhasznalo torlese
        [HttpDelete("{id}")]
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
         */
        [HttpPost("SetTelephely", Name = "SetTelephely")]
        public async Task<IActionResult> SetTelephely(String id, int telephely_id)
        {
            Felhasznalo felh = _context.Felhasznalo.Where(f => f.Id.Equals(id)).First();
            if (!FelhasznaloExists(id))
            {
                return NotFound();
            }
            if (!id.Equals(felh.Id))
            {
                return BadRequest();
            }

            if (!_context.Telephely.Any(e => e.Id == telephely_id))
            {
                return NotFound();
            }

            _context.FelhasznaloTelephely.Add(new FelhasznaloTelephely { FelhasznaloId = id, TelephelyId = telephely_id });
            await _context.SaveChangesAsync();

            var dto = new FelhasznaloDTO(felh);

            return CreatedAtAction(nameof(GetFelhasznalo), new { id = felh.Id }, dto);
        }

        /*
         * A megadott felhasznalo eltavolitasa egy megadott telephely alkalmazotti listajabol.
         */
        [HttpPost("DelFromTelephely", Name = "DelFromTelephely")]
        public async Task<IActionResult> DelFromTelephely(String id, int telephely_id)
        {
            if (!FelhasznaloExists(id))
            {
                return NotFound();
            }

            if (!_context.Telephely.Any(e => e.Id == telephely_id))
            {
                return NotFound();
            }

            var felhTelep = _context.FelhasznaloTelephely.Where(fh => fh.FelhasznaloId.Equals(id)).Where(fh => fh.TelephelyId == telephely_id).ToList();
            foreach (var item in felhTelep)
            {
                _context.FelhasznaloTelephely.Remove(item);
            }

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
