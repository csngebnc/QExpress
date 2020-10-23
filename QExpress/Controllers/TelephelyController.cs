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
    public class TelephelyController : Controller
    {
        //comment

        private readonly QExpressDbContext _context;

        public TelephelyController(QExpressDbContext context)
        {
            _context = context;
        }


        // Telephelyek lekerese  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Telephely>>> GetTelephelyek()
        {
            return await _context.Telephely.ToListAsync();
        }


        // Egy telephely lekerese
        [HttpGet("{id}")]
        public async Task<ActionResult<TelephelyDTO>> GetTelephely(int id)
        {
            var telephely = await _context.Telephely.FindAsync(id);

            if (!TelephelyExists(id))
            {
                return NotFound();
            }

            return new TelephelyDTO(telephely);
        }



        // Telephely felvetele
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<TelephelyDTO>> AddTelephely(String cim)
        {
            Telephely newTelephely = new Telephely { Cim = cim };
            _context.Telephely.Add(newTelephely);
            await _context.SaveChangesAsync();

            var dto = new TelephelyDTO(newTelephely);

            return CreatedAtAction(nameof(GetTelephely), new { id = newTelephely.Id }, dto);
        }

        // Telephely torlese
        [HttpDelete("{id}")]
        public async Task<ActionResult<TelephelyDTO>> DeleteTelephely(int id)
        {
            var telephely = await _context.Telephely.FindAsync(id);
            if (telephely == null)
            {
                return NotFound();
            }

            _context.Telephely.Remove(telephely);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // segédfüggvény - telephely létezik e
        private bool TelephelyExists(int id)
        {
            return _context.Telephely.Any(e => e.Id == id);
        }
        

    }
}
