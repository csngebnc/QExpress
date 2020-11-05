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
    public class UgyfLevelekController : Controller
    {

        private readonly QExpressDbContext _context;

        public UgyfLevelekController(QExpressDbContext context)
        {
            _context = context;
        }

        /*
         * Minden ügyfél levél lekérése
         * api/UgyfLevelek/GetUgyfLevelek
         */
        [HttpGet("/GetUgyfLevelek")]
        public async Task<ActionResult<IEnumerable<UgyfLevelek>>> GetUgyfLevelek()
        {
            return await _context.UgyfLevelek.ToListAsync();
        }

        /*
         * Adott id-vel rendelező ügyféllevél lekérése
         * api/UgyfLevelek/GetUgyfLevel/{id}
         */
        [HttpGet("/GetUgyfLevel/{id}")]
        public async Task<ActionResult<UgyfLevelekDTO>> GetUgyfLevel(int id)
        {
            var level = await _context.UgyfLevelek.FindAsync(id);

            if (!UgyfLevelekExists(id))
            {
                return NotFound();
            }

            return new UgyfLevelekDTO(level);
        }

        /*
         * Aktuálisan bejelentkezett felhasználóhoz panasz rögzítése paraméterként kapott céghez.
         * api/UgyfLevelek/AddUgyfLevel
         * params: panasz: a panasz, ceg_id: bepanaszolt cég id-ja
         */
        [HttpPost("/AddUgyfLevel")]
        public async Task<ActionResult<UgyfLevelekDTO>> AddUgyfLevel(String panasz, int ceg_id)
        {
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            if(!_context.Ceg.Any(c=>c.Id == ceg_id))
            {
                return NotFound();
            }

            UgyfLevelek ujPanasz = new UgyfLevelek { Panasz = panasz, CegId = ceg_id, PanaszoloId = user_id };
            _context.UgyfLevelek.Add(ujPanasz);
            await _context.SaveChangesAsync();

            var dto = new UgyfLevelekDTO(ujPanasz);

            return CreatedAtAction(nameof(GetUgyfLevelek), new { id = ujPanasz.Id }, dto);
        }

        /*
         * Adott id-val rendelkező ügyféllevél törlése
         * api/UgyfLevelek/DeleteUgyfLevel/{id}
         */
        [HttpDelete("/DeleteUgyfLevel/{id}")]
        public async Task<ActionResult<UgyfLevelekDTO>> DeleteUgyfLevel(int id)
        {
            var panasz = await _context.UgyfLevelek.FindAsync(id);
            if (panasz == null)
            {
                return NotFound();
            }

            _context.UgyfLevelek.Remove(panasz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // segédfüggvény - telephely létezik e
        private bool UgyfLevelekExists(int id)
        {
            return _context.UgyfLevelek.Any(e => e.Id == id);
        }
    }
}
