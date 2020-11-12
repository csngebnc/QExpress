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


        /*
         * Ügyintéző telephelyeinek lekérése
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
            var telephelyek = ceg.Telephely;

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
         * Egy adott id-val rendelkező telephelyhez tartozó sorszámok lekérése, amennyiben az éppen bejelentkezett felhasználó az adott telephelyhez tartozik
         * api/Telephely/GetTelephelySorszamai/{id}
         * param: adott telephely id-je
         */

        [HttpGet("GetTelephelySorszamai/{id}")]
        public async Task<ActionResult<IEnumerable<SorszamDTO>>> GetTelephelySorszamai([FromRoute]int id)
        {
            if(!_context.Telephely.Any(t => t.Id == id))
            {
                return NotFound();
            }

            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            if(!_context.FelhasznaloTelephely.Any(ft => ft.TelephelyId == id && ft.FelhasznaloId == user_id))
            {
                return BadRequest();
            }

            var sorszamok = await _context.Sorszam.Where(s => s.TelephelyId == id).ToListAsync();
            var dtoList = new List<SorszamDTO>();
            foreach (var item in sorszamok)
            {
                dtoList.Add(new SorszamDTO(item));
            }

            return dtoList;
        }


        /*
         * Aktuális felhasználó ha cégadmin, akkor az ő cégéhez egy telephely felvétele
         * api/Telephely/AddTelephely
         * param: cim: telephely címe
         */
        [HttpPost]
        [Route("AddTelephely")]
        public async Task<ActionResult<TelephelyDTO>> AddTelephely([FromBody] String cim)
        {
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            Felhasznalo felh = await _context.Felhasznalo.FindAsync(user_id);
            if (!_context.Ceg.Any(c => c.CegadminId.Equals(user_id)) || felh.jogosultsagi_szint<3)
            {
                return NotFound();
            }

            var ceg = await _context.Ceg.Where(c => c.CegadminId.Equals(user_id)).FirstAsync();

            Telephely ujTelephely = new Telephely { Cim = cim, Ceg_id = ceg.Id };
            _context.Telephely.Add(ujTelephely);
            await _context.SaveChangesAsync();

            var dto = new TelephelyDTO(ujTelephely);

            return CreatedAtAction(nameof(GetTelephely), new { id = ujTelephely.Id }, dto);
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
