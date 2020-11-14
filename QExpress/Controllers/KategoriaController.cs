using Microsoft.AspNetCore.Authorization;
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
    public class KategoriaController : ControllerBase
    {
        private readonly QExpressDbContext _context;

        public KategoriaController(QExpressDbContext context)
        {
            _context = context;
        }

        /*
         * Az osszes kategoria lekerdezese
         * api/Kategoria/GetOsszesKategoria
         */
        [HttpGet]
        [Route("GetOsszesKategoria")]
        public async Task<ActionResult<IEnumerable<KategoriaDTO>>> GetOsszesKategoria()
        {
            var kategoriak = await _context.Kategoria.ToListAsync();
            var dto = new List<KategoriaDTO>();
            foreach (var k in kategoriak)
            {
                dto.Add(new KategoriaDTO(k));
            }
            return dto;
        }

        /*
         * Cég kategóriáinak lekérdezése
         */
        [HttpGet]
        [Route("GetKategoriak/{id}")]
        public async Task<ActionResult<IEnumerable<KategoriaDTO>>> GetKategoriak(int id)
        {
            if(!_context.Ceg.Any(c => c.Id == id))
            {
                ModelState.AddModelError(nameof(id), "A megadott azonosítóval nem létezik cég.");
                return BadRequest(ModelState);
            }

            var kategoriak = await _context.Kategoria.Where(k => k.CegId == id).ToListAsync();
            var dto = new List<KategoriaDTO>();
            foreach (var k in kategoriak)
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
        [HttpGet("GetKategoria/{id}")]
        public async Task<ActionResult<KategoriaDTO>> GetKategoria([FromRoute] int id)
        {
            if (!KategoriaExists(id))
            {
                ModelState.AddModelError(nameof(id), "A megadott azonosítóhoz nem tartozik kategória.");
                return BadRequest(ModelState);
            }

            var kategoria = await _context.Kategoria.FindAsync(id);
            return new KategoriaDTO(kategoria);
        }
        
        /*
         * Cégadminhoz tartozó cég kategóriáinak lekérése
         * api/Kategoria/GetKategoriakCegadmin
         */
        [HttpGet]
        [Route("GetKategoriakCegadmin")]
        public async Task<ActionResult<IEnumerable<KategoriaDTO>>> GetKategoriakCegadmin()
        {
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            var cegadmin = await _context.Felhasznalo.FindAsync(user_id);
            if (cegadmin.jogosultsagi_szint != 3)
                return BadRequest();

            if (!_context.Ceg.Any(c => c.CegadminId == user_id))
                return BadRequest();

            var ceg = await _context.Ceg.Where(c => c.CegadminId == user_id).FirstAsync();
            var kategoriak = await _context.Kategoria.Where(c => c.CegId == ceg.Id).ToListAsync();

            var dto = new List<KategoriaDTO>();
            foreach (var k in kategoriak)
            {
                dto.Add(new KategoriaDTO(k));
            }
            return dto;
        }

        /*
         * Parameterkent kapott kategoriat frissiti.
         * api/Kategoria/UpdateKategoria
         * param: KategoriaDTO --> benne Id, Megnevezes
         */
        [HttpPut("UpdateKategoria")]
        public async Task<IActionResult> EditKategoria([FromBody] KategoriaDTO putKategoria)
        {
            if (!KategoriaExists(putKategoria.Id))
            {
                ModelState.AddModelError(nameof(putKategoria.Id), "A megadott azonosítóhoz nem tartozik kategória.");
                return BadRequest(ModelState);
            }
            if(string.IsNullOrEmpty(putKategoria.Megnevezes) || string.IsNullOrWhiteSpace(putKategoria.Megnevezes))
            {
                ModelState.AddModelError(nameof(putKategoria.Megnevezes), "A kategória neve nem lehet üres, vagy csak szóköz.");
                return BadRequest(ModelState);
            }

            Kategoria kategoria = await _context.Kategoria.FindAsync(putKategoria.Id);
            kategoria.Megnevezes = putKategoria.Megnevezes;
            await _context.SaveChangesAsync();

            var dto = new KategoriaDTO(kategoria);
            return Ok();
        }


        /*
         * Uj kategoria rogzitese megadott ceghez.
         * api/Kategoria/AddKategoria
         * params: nev: kategoria neve, ceg_id: cég id-ja
         */
        [HttpPost]
        [Route("AddKategoria")]
        public async Task<ActionResult<KategoriaDTO>> AddKategoria([FromBody] KategoriaDTO kategoria)
        {
            if(string.IsNullOrEmpty(kategoria.Megnevezes) || string.IsNullOrWhiteSpace(kategoria.Megnevezes))
            {
                ModelState.AddModelError(nameof(kategoria.Megnevezes), "A kategória neve nem lehet üres, vagy csak szóköz.");
                return BadRequest(ModelState);
            }
            if(!_context.Ceg.Any(c => c.Id == kategoria.CegId))
            {
                ModelState.AddModelError(nameof(kategoria.CegId), "A megadott azonosítóhoz nem tartozik cég.");
                return BadRequest(ModelState);
            }


            Kategoria newKat = new Kategoria { Megnevezes = kategoria.Megnevezes, CegId = kategoria.CegId };
            _context.Kategoria.Add(newKat);
            await _context.SaveChangesAsync();

            var dto = new KategoriaDTO(newKat);
            return CreatedAtAction(nameof(GetKategoria), new { id = newKat.Id }, dto);
        }


        /*
         * Parameterkent kapott ID-val rendelkezo kategoria torlese, valamint a kategoriahoz tartozo sorszamok torlese.
         * api/Kategoria/Delete/{id}
         * param: id: törlendő cég id-ja
         */
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteKategoria([FromRoute] int id)
        {
            if (!KategoriaExists(id))
            {
                ModelState.AddModelError(nameof(id), "A megadott azonosítóhoz nem tartozik kategória.");
                return BadRequest(ModelState);
            }
            var kategoria = await _context.Kategoria.FindAsync(id);
            var kategoria_sorszamai = await _context.Sorszam.Where(s => s.KategoriaId == kategoria.Id).ToListAsync();

            _context.Sorszam.RemoveRange(kategoria_sorszamai);
            _context.Kategoria.Remove(kategoria);
            await _context.SaveChangesAsync();

            return Ok();
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
