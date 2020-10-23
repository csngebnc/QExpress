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
    public class CegController : ControllerBase
    {
        private readonly QExpressDbContext _context;

        public CegController(QExpressDbContext context)
        {
            _context = context;
        }

        // Cegek lekerese
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

        // Egy ceg lekerese
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

        // Uj cegnev
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

        // Ceg felvetele
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


        // Felhasznalo torlese
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCeg(int id)
        {
            var ceg = await _context.Ceg.FindAsync(id);
            if (ceg == null || !CegExists(id))
            {
                return NotFound();
            }

            _context.Ceg.Remove(ceg);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CegExists(int id)
        {
            return _context.Ceg.Any(e => e.Id == id);
        }
    }
}
