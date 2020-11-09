using Microsoft.AspNetCore.Mvc;
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

            int sorszam_counter = _context.Sorszam.Where(t => t.TelephelyId == sorszam.TelephelyId).Max(n => n.SorszamIdTelephelyen);

            Sorszam ujSorszam = new Sorszam { 
                UgyfelId = ugyfel_id,
                Allapot = "Aktív", 
                KategoriaId = sorszam.KategoriaId, 
                TelephelyId = sorszam.TelephelyId, 
                Idopont = DateTime.Now, 
                SorszamIdTelephelyen = sorszam_counter
            };
            _context.Sorszam.Add(ujSorszam);
            await _context.SaveChangesAsync();

            var dto = new SorszamDTO(ujSorszam);
            return CreatedAtAction(nameof(GetSorszam), new { id = ujSorszam.Id }, dto);
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
        public async Task<ActionResult<SorszamDTO>> UpdateSorszam([FromRoute] int id, [FromBody] SorszamDTO putSorszam)
        {
            if (!SorszamExists(putSorszam.Id))
                return NotFound();

            if (id != putSorszam.Id)
                return BadRequest();

            var sorszam = await _context.Sorszam.FindAsync(putSorszam.Id);
            sorszam.Allapot = putSorszam.Allapot;

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
            var sorszam = await _context.Sorszam.FindAsync(id);
            if (sorszam == null)
            {
                return NotFound();
            }

            _context.Sorszam.Remove(sorszam);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //segédfüggvény - létezik e adott számú sorszám
        private bool SorszamExists(int id)
        {
            return _context.Sorszam.Any(e => e.Id == id);
        }
    }
}
