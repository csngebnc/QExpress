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
    public class TelephelyController : Controller
    {
        private readonly QExpressDbContext _context;

        public TelephelyController(QExpressDbContext context)
        {
            _context = context;
        }


        // Cég telephelyeinek lekérdezése
        [HttpGet]
        [Route("GetTelephelyek/{id}")]
        public async Task<ActionResult<IEnumerable<TelephelyDTO>>> GetTelephelyek(int id)
        {
            if(!_context.Ceg.Any(c => c.Id == id))
            {
                ModelState.AddModelError(nameof(id), "A megadott azonosítóval nem létezik cég.");
                return BadRequest(ModelState);
            }

            var ceg = await _context.Ceg.FindAsync(id);
            var telephelyek = await _context.Telephely.Where(c => c.Ceg_id == ceg.Id).ToListAsync();

            var dto = new List<TelephelyDTO>();
            foreach (var t in telephelyek)
            {
                dto.Add(new TelephelyDTO(t));
            }
            return dto;
        }


        /*
         * Cégadminhoz tartozó cég telephelyeinek lekérése
         * api/Telephely/GetTelephelyekCegadmin
         */
        [HttpGet]
        [Route("GetTelephelyekCegadmin")]
        public async Task<ActionResult<IEnumerable<TelephelyDTO>>> GetTelephelyekCegadmin()
        {
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            var cegadmin = await _context.Felhasznalo.FindAsync(user_id);
            if(cegadmin.jogosultsagi_szint != 3)
                return BadRequest();

            if (!_context.Ceg.Any(c => c.CegadminId == user_id))
                return BadRequest();

            var ceg = await _context.Ceg.Where(c => c.CegadminId == user_id).FirstAsync();
            var telephelyek = await _context.Telephely.Where(c=>c.Ceg_id == ceg.Id).ToListAsync();

            var dto = new List<TelephelyDTO>();
            foreach (var t in telephelyek)
            {
                dto.Add(new TelephelyDTO(t));
            }
            return dto;
        }


        /*
         * Egy adott id-val rendelkező telephely lekérése.
         * api/Telephely/GetTelephely/{id}
         * param: id: telephely id-ja   
         */
        [HttpGet("GetTelephely/{id}")]
        public async Task<ActionResult<TelephelyDTO>> GetTelephely([FromRoute] int id)
        {
            var telephely = await _context.Telephely.FindAsync(id);

            if (!TelephelyExists(id))
            {
                return NotFound();
            }

            return new TelephelyDTO(telephely);
        }

        /*
         * Aktuális felhasználó ha cégadmin, akkor az ő cégéhez egy telephely felvétele
         * api/Telephely/AddTelephely
         * param: cim: telephely címe
         */
        [HttpPost]
        [Route("AddTelephely")]
        public async Task<ActionResult<TelephelyDTO>> AddTelephely([FromBody] TelephelyDTO telephely)
        {

            if(telephely.Cim ==null || telephely.Cim.Length == 0) {
                //TODO validáció
            }

            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            Felhasznalo felh = await _context.Felhasznalo.FindAsync(user_id);
            if (!_context.Ceg.Any(c => c.CegadminId.Equals(user_id)) || felh.jogosultsagi_szint<3)
            {
                return NotFound();
            }

            var ceg = await _context.Ceg.Where(c => c.CegadminId.Equals(user_id)).FirstAsync();

            Telephely ujTelephely = new Telephely { Cim = telephely.Cim, Ceg_id = ceg.Id };
            _context.Telephely.Add(ujTelephely);
            await _context.SaveChangesAsync();

            var dto = new TelephelyDTO(ujTelephely);

            return CreatedAtAction(nameof(GetTelephely), new { id = ujTelephely.Id }, dto);
        }


        /*
         * Paraméterként kapott telephely frissítése
         * param: telephely: egy telephelydto
         */
        [HttpPut]
        [Route("UpdateTelephely")]
        public async Task<ActionResult<TelephelyDTO>> UpdateTelephely([FromBody] TelephelyDTO telephely)
        {

            if (telephely.Cim == null || telephely.Cim.Length == 0)
            {
                //TODO validáció
            }

            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            Felhasznalo felh = await _context.Felhasznalo.FindAsync(user_id);
            if (!_context.Ceg.Any(c => c.CegadminId.Equals(user_id) && c.Id == telephely.Ceg_id) || felh.jogosultsagi_szint < 3)
            {
                return BadRequest();
            }
            if (!TelephelyExists(telephely.Id))
            {
                NotFound();
            }

            var ceg = await _context.Ceg.Where(c => c.CegadminId.Equals(user_id)).FirstAsync();
            if(telephely.Ceg_id != ceg.Id)
            {
                return BadRequest();
            }

            var frissitendo_telephely = await _context.Telephely.FindAsync(telephely.Id);
            frissitendo_telephely.Cim = telephely.Cim;

            await _context.SaveChangesAsync();

            var dto = new TelephelyDTO(frissitendo_telephely);

            return CreatedAtAction(nameof(GetTelephely), new { id = frissitendo_telephely.Id }, dto);
        }


        /*
         * Megadott id-val rendelkező telephely törlése
         * api/Telephely/Delete/{id}
         * param: id: törlendő telephely id-ja
         */
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<TelephelyDTO>> DeleteTelephely([FromRoute] int id)
        {
            var telephely = await _context.Telephely.FindAsync(id);
            if (telephely == null)
            {
                return NotFound();
            }

            var eltavolitandoDolgozok = await _context.FelhasznaloTelephely.Where(t => t.TelephelyId == id).ToListAsync();
            _context.FelhasznaloTelephely.RemoveRange(eltavolitandoDolgozok);

            var eltavolitandoSorszamok = await _context.Sorszam.Where(s => s.TelephelyId == id).ToListAsync();
            _context.Sorszam.RemoveRange(eltavolitandoSorszamok);

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
