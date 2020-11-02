using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QExpress.Data;
using QExpress.Models;
using QExpress.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QExpress.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CegController : ControllerBase
    {
        private readonly QExpressDbContext _context;

        public CegController(QExpressDbContext context)
        {
            _context = context;
        }

        /*
         * Az osszes ceg lekerese.
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CegDTO>>> GetCegek()
        {
            var cegek = await _context.Ceg.ToListAsync();
            var dto = new List<CegDTO>();
            foreach (var c in cegek)
            {
                dto.Add(new CegDTO(c));
            }
            return dto;
        }

        /*
         * A parameterkent kapott ID-val rendelkezo ceg lekerese.
         */
        [HttpGet("{id}")]
        public async Task<ActionResult<CegDTO>> GetCeg(int id)
        {
            var ceg = await _context.Ceg.FindAsync(id);

            if (ceg == null)
            {
                return NotFound();
            }

            return new CegDTO(ceg);
        }

        /*
         * A parameterkent kapott ID-val rendelkezo ceg kategoriainak lekerese.
         */
        [HttpGet("{id}/Kategoriak")]
        public async Task<ActionResult<IEnumerable<KategoriaDTO>>> GetCegKategoriai(int id)
        {
            if (!CegExists(id))
            {
                return NotFound();
            }
            var kategoriak = await _context.Kategoria.Where(k => k.CegId == id).ToListAsync();

            if (kategoriak.Count == 0)
            {
                return NoContent();
            }

            var dto = new List<KategoriaDTO>();
            foreach (var k in kategoriak)
            {
                dto.Add(new KategoriaDTO(k));
            }
            return dto;
        }

        /*
         * A parameterkent kapott ID-val rendelkezo ceg nevenek megvaltoztatasa.
         */
        [HttpPost("{id}/NewName")]
        public async Task<IActionResult> EdigCegNev(int id, String uj_nev)
        {
            var ceg = await _context.Ceg.FindAsync(id);
            if (!CegExists(id))
            {
                return NotFound();
            }
            if (id != ceg.Id)
            {
                return BadRequest();
            }

            ceg.nev = uj_nev;

            await _context.SaveChangesAsync();

            var dto = new CegDTO(ceg);

            return CreatedAtAction(nameof(GetCeg), new { id = ceg.Id }, dto);
        }

        /*
         * A parameterkent kapott ID-val rendelkezo ceg elere uj felhasznalo beallitasa adminnak.
         */
        [HttpPost("{ceg_id}/UpdateAdmin")]
        public async Task<IActionResult> EditCegAdmin(int id, String uj_admin_id)
        {
            Ceg ceg = await _context.Ceg.FindAsync(id);
            if (!CegExists(id))
            {
                return NotFound();
            }
            if (id != ceg.Id)
            {
                return BadRequest();
            }
            ceg.CegadminId = uj_admin_id;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*
         * Uj ceg rogzitese
         */
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<Ceg>> AddCegParams(String cegnev, String cegadmin_id)
        {
            Ceg newCeg = new Ceg { nev = cegnev, CegadminId = cegadmin_id };
            _context.Ceg.Add(newCeg);
            await _context.SaveChangesAsync();

            var dto = new CegDTO(newCeg);

            return CreatedAtAction(nameof(GetCeg), new { id = newCeg.Id }, dto);
        }


        /*
         * A parameterkent kapott ID-val rendelkezo ceg torlese.
         * Torles sorban: 
         * - ceg ugyflevelei
         * - sorszamok
         * - dolgozok hozzarendelesei
         * - telephelyek
         * - kategoriak
         * - ceg
         */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCeg(int id)
        {
            var ceg = await _context.Ceg.FindAsync(id);
            if (ceg == null || !CegExists(id))
            {
                return NotFound();
            }

            // sorrend: levelek, sorszamok, dolgozok, telephelyek, kategoriak, ceg

            var ugyfLevelek = await _context.UgyfLevelek.Where(uf => uf.CegId == id).ToListAsync();
            var telephelyek = await _context.Telephely.Where(t => t.Ceg_id == id).ToListAsync();
            var telephelyek_id = new List<int>();
            foreach (var item in telephelyek)
            {
                telephelyek_id.Add(item.Id);
            }
            var felhasznalo_telephely = await _context.FelhasznaloTelephely.Where(ft => telephelyek_id.Contains(ft.TelephelyId)).ToListAsync();

            var sorszamok = await _context.Sorszam.Where(s => telephelyek_id.Contains(s.TelephelyId)).ToListAsync();
            var kategoriak = await _context.Kategoria.Where(k => k.CegId == id).ToListAsync();

            _context.UgyfLevelek.RemoveRange(ugyfLevelek);
            _context.Sorszam.RemoveRange(sorszamok);
            _context.FelhasznaloTelephely.RemoveRange(felhasznalo_telephely);
            _context.Telephely.RemoveRange(telephelyek);
            _context.Kategoria.RemoveRange(kategoriak);
            _context.Ceg.Remove(ceg);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*
        * Ellenorzes, hogy a parameterkent kapott ID-val letezik-e ceg.
        */
        private bool CegExists(int id)
        {
            return _context.Ceg.Any(e => e.Id == id);
        }
    }
}
