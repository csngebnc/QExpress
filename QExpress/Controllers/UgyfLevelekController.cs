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
    [ApiController]
    public class UgyfLevelekController : Controller
    {

        private readonly QExpressDbContext _context;

        public UgyfLevelekController(QExpressDbContext context)
        {
            _context = context;
        }

        // Ugyfél levelek lekerese
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UgyfLevelek>>> GetUgyfLevelek()
        {
            return await _context.UgyfLevelek.ToListAsync();
        }

        // Ugyfél levelek lekerese
        [HttpGet("{id}")]
        public async Task<ActionResult<UgyfLevelekDTO>> GetUgyfLevelek(int id)
        {
            var level = await _context.UgyfLevelek.FindAsync(id);

            if (!UgyfLevelekExists(id))
            {
                return NotFound();
            }

            return new UgyfLevelekDTO(level);
        }

        // Ugyfél levelek felvetele
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<UgyfLevelekDTO>> AddUgyfLevelek(String panasz)
        {
            UgyfLevelek newPanasz = new UgyfLevelek { Panasz = panasz };
            _context.UgyfLevelek.Add(newPanasz);
            await _context.SaveChangesAsync();

            var dto = new UgyfLevelekDTO(newPanasz);

            return CreatedAtAction(nameof(GetUgyfLevelek), new { id = newPanasz.Id }, dto);
        }

        // Ugyfél levelek torlese  
        [HttpDelete("{id}")]
        public async Task<ActionResult<UgyfLevelekDTO>> DeleteUgyfLevelek(int id)
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
