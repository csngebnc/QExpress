using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NJsonSchema;
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
    [Authorize]
    public class SorszamController : Controller
    {

        private readonly QExpressDbContext _context;

        public SorszamController(QExpressDbContext context)
        {
            _context = context;
        }

        /*
         * Sorszám felvétele aktuális felhasználóhoz
         * api/Sorszam/AddSorszam
         * params: ugyfel_id, kategoria_id, telephely_id
         */
        [HttpPost]
        [Route("AddSorszam")]
        public async Task<ActionResult<SorszamDTO>> AddSorszam([FromBody] SorszamDTO sorszam)
        {
            string ugyfel_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            int sorszam_counter;
            if (_context.Sorszam.Any(s => s.TelephelyId == sorszam.TelephelyId))
            {
                sorszam_counter = (await _context.Sorszam.Where(s => s.TelephelyId == sorszam.TelephelyId).ToListAsync()).Count;
            }
            else
            {
                sorszam_counter = 0;
            }

            Sorszam ujSorszam = new Sorszam { 
                UgyfelId = ugyfel_id,
                Allapot = "Aktív", 
                KategoriaId = sorszam.KategoriaId, 
                TelephelyId = sorszam.TelephelyId, 
                Idopont = DateTime.Now, 
                SorszamIdTelephelyen = sorszam_counter+1
            };
            _context.Sorszam.Add(ujSorszam);
            await _context.SaveChangesAsync();

            //var dto = new SorszamDTO(ujSorszam);
            //return CreatedAtAction(nameof(GetSorszam), new { id = ujSorszam.Id }, dto);
            return Ok();
        }

        /*
         * Adott id-val rendelkező sorszám lekérése
         * api/Sorszam/GetSorszam/{id}
         */
        [HttpGet("GetSorszam/{id}")]
        public async Task<ActionResult<SorszamDTO>> GetSorszam([FromRoute] int id)
        {
            var sorszam = await _context.Sorszam.FindAsync(id);

            if (!SorszamExists(id))
            {
                return NotFound();
            }

            return new SorszamDTO(sorszam);
        }

        /*
         * Adott id-vel rendelkező sorszám frissítése.
         * api/Sorszam/{id}/Update
         * params: id - queryből, sorszám id-je, putSorszam - frissített sorszám dto
         * megjegyzés: paramétereket meg kell még beszélni itt!
         */
        [HttpPut("{id}/Update")]
        public async Task<ActionResult<SorszamDTO>> UpdateSorszam([FromRoute] int id)
        {
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            var ugyintezo = await _context.Felhasznalo.FindAsync(user_id);

            if (ugyintezo.jogosultsagi_szint != 2)
            {
                ModelState.AddModelError(nameof(ugyintezo.jogosultsagi_szint), "Nincs jogosultsága a parancs végrehajtásához.");
                return BadRequest(ModelState);
            }
            if(_context.FelhasznaloTelephely.Any(ft => ft.FelhasznaloId.Equals(user_id)))
            {
                ModelState.AddModelError(nameof(ugyintezo.jogosultsagi_szint), "Nincs jogosultsága a parancs végrehajtásához.");
                return BadRequest(ModelState);
            }
            if (!SorszamExists(id))
            {
                ModelState.AddModelError(nameof(id), "A megadott azonosítóval nem létezik sorszám.");
                return BadRequest(ModelState);
            }
            var hozzarendeles = await _context.FelhasznaloTelephely.Where(ft => ft.FelhasznaloId.Equals(user_id)).FirstAsync();
            var sorszam = await _context.Sorszam.FindAsync(id);
            if(hozzarendeles.TelephelyId != sorszam.TelephelyId)
            {
                ModelState.AddModelError(nameof(ugyintezo.jogosultsagi_szint), "Nincs jogosultsága a parancs végrehajtásához.");
                return BadRequest(ModelState);
            }
            if (sorszam.Allapot.Equals("Behívott"))
            {
                ModelState.AddModelError(nameof(sorszam.Allapot), "A sorszámot már behívták.");
                return BadRequest(ModelState);
            }
            
            sorszam.Allapot = "Behívott";

            await _context.SaveChangesAsync();

            return new SorszamDTO(sorszam);
        }

        /*
         * Adott id-val rendelkező sorszám törlése
         * api/Sorszam/DeleteSorszam/{id}
         */
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<SorszamDTO>> DeleteSorszam([FromRoute] int id)
        {
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            var felhasznalo = await _context.Felhasznalo.FindAsync(user_id);
            if (!SorszamExists(id))
            {
                ModelState.AddModelError("Sorszam", "A megadott azonosítóval nem létezik sorszám.");
                return BadRequest(ModelState);
            }
            var sorszam = await _context.Sorszam.FindAsync(id);
            _context.Sorszam.Remove(sorszam);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //segédfüggvény - létezik e adott számú sorszám
        private bool SorszamExists(int id)
        {
            return _context.Sorszam.Any(e => e.Id == id);
        }
    }
}
