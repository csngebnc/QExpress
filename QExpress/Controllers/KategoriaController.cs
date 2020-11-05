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
    public class KategoriaController : ControllerBase
    {
        private readonly QExpressDbContext _context;

        public KategoriaController(QExpressDbContext context)
        {
            _context = context;
        }

        /*
         * Az osszes kategoria lekerdezese
         * api/Kategoria/GetKategoriak
         */
        [HttpGet("/GetKategoriak")]
        public async Task<ActionResult<IEnumerable<KategoriaDTO>>> GetKategoriak()
        {
            var katekoriak = await _context.Kategoria.ToListAsync();
            var dto = new List<KategoriaDTO>();
            foreach (var k in katekoriak)
            {
                dto.Add(new KategoriaDTO(k));
            }
            return dto;
        }

        /*
         * Parameterkent kapott ID-val rendelkezo kategoria lekerese.
         * api/Ceg/GetCeg/{id}
         * param: id: lekérendő cég id-ja
         */
        [HttpGet("/GetCeg/{id}")]
        public async Task<ActionResult<KategoriaDTO>> GetKategoria(int id)
        {
            var kategoria = await _context.Kategoria.FindAsync(id);

            if (kategoria == null)
            {
                return NotFound();
            }

            return new KategoriaDTO(kategoria);
        }

        /*
         * Parameterkent kapott ID-val rendelkezo kategoria nevenek megvaltoztatasa.
         * api/Kategoria/{id}/NewName
         * params: id: kategoria id-ja, uj_megnevezes: kategoria uj neve
         */
        [HttpPut("{id}/NewName")]
        public async Task<IActionResult> EditKategoria(int id, String uj_megnevezes)
        {
            Kategoria kategoria = await _context.Kategoria.FindAsync(id);
            kategoria.Megnevezes = uj_megnevezes;
            await _context.SaveChangesAsync();

            var dto = new KategoriaDTO(kategoria);
            return CreatedAtAction(nameof(GetKategoria), new { id = kategoria.Id }, dto);
        }


        /*
         * Uj kategoria rogzitese megadott ceghez.
         * api/Kategoria/AddKategoria
         * params: nev: kategoria neve, ceg_id: cég id-ja
         */
        [HttpPost("/AddKategoria")]
        public async Task<ActionResult<KategoriaDTO>> AddKategoria(String nev, int ceg_id)
        {
            Kategoria newKat = new Kategoria { Megnevezes = nev, CegId = ceg_id };
            _context.Kategoria.Add(newKat);
            await _context.SaveChangesAsync();

            var dto = new KategoriaDTO(newKat);
            return CreatedAtAction(nameof(GetKategoria), new { id = newKat.Id }, dto);
        }


        /*
         * Parameterkent kapott ID-val rendelkezo kategoria torlese, valamint a kategoriahoz tartozo sorszamok torlese.
         * api/Kategoria/DeleteKategoria/{id}
         * param: id: törlendő cég id-ja
         */
        [HttpDelete("/DeleteKategoria/{id}")]
        public async Task<IActionResult> DeleteKategoria(int id)
        {
            var kategoria = await _context.Kategoria.FindAsync(id);
            if (kategoria == null)
            {
                return NotFound();
            }

            var sorszamok = await _context.Sorszam.Where(s => s.KategoriaId == kategoria.Id).ToListAsync();
            _context.Sorszam.RemoveRange(sorszamok);
            _context.Kategoria.Remove(kategoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*
        * Ellenorzes, hogy a parameterkent kapott ID-val letezik-e kategoria.
        */
        private bool KategoriaExists(int id)
        {
            return _context.Kategoria.Any(e => e.Id == id);
        }

    }

}
