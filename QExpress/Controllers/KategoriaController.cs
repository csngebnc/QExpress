﻿using Microsoft.AspNetCore.Authorization;
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
         * Az osszes kategoria lekerdezese
         * api/Kategoria/GetKategoriak
         */
        [HttpGet]
        [Route("GetKategoriak")]
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
        [HttpGet("GetKategoria/{id}")]
        public async Task<ActionResult<KategoriaDTO>> GetKategoria([FromRoute] int id)
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
        public async Task<IActionResult> EditKategoria([FromRoute] int id, [FromBody] String uj_megnevezes)
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
        [HttpPost]
        [Route("AddKategoria")]
        public async Task<ActionResult<KategoriaDTO>> AddKategoria([FromBody] KategoriaDTO kategoria)
        {
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
