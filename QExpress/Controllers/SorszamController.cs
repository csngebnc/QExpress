using Microsoft.AspNetCore.Mvc;
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
    [ApiController]
    public class SorszamController : Controller
    {

        private readonly QExpressDbContext _context;

        public SorszamController(QExpressDbContext context)
        {
            _context = context;
        }

        //Sorszám létrehozása
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<SorszamDTO>> AddSorszam(int idTelephely)
        {

            Sorszam newSorszam = new Sorszam { SorszamIdTelephelyen = idTelephely };
            _context.Sorszam.Add(newSorszam);
            await _context.SaveChangesAsync();

            var dto = new SorszamDTO(newSorszam);
            return CreatedAtAction(nameof(GetSorzsamok), new { id = newSorszam.Id }, dto);
        }

        //Sorszámok lekérdezése
        [HttpGet]
        public async Task<ActionResult<SorszamDTO>> GetSorzsamok(int id)
        {
            var sorszam = await _context.Sorszam.FindAsync(id);

            if (!SorszamExists(id))
            {
                return NotFound();
            }

            return new SorszamDTO(sorszam);
        }

        //Sorszám törlése  
        [HttpDelete("{id}")]
        public async Task<ActionResult<SorszamDTO>> DeleteSorszam(int id)
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
