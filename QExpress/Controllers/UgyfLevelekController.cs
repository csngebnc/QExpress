using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
        [HttpGet]
        [Route("GetUgyfLevelek")]
        public async Task<ActionResult<IEnumerable<UgyfLevelekDTO>>> GetUgyfLevelek()
        {
            var levelek = await _context.UgyfLevelek.ToListAsync();
            var dto = new List<UgyfLevelekDTO>();
            foreach (var ufl in levelek)
            {
                dto.Add(new UgyfLevelekDTO(ufl));
            }
            return dto;
        }

        /*
         * Adott id-vel rendelező ügyféllevél lekérése
         * api/UgyfLevelek/GetUgyfLevel/{id}
         */
        [HttpGet("GetUgyfLevel/{id}")]
        public async Task<ActionResult<UgyfLevelekDTO>> GetUgyfLevel([FromRoute] int id)
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
        [HttpPost]
        [Route("AddUgyfLevel")]
        public async Task<ActionResult<UgyfLevelekDTO>> AddUgyfLevel([FromBody] UgyfLevelek ugyfelLevel)
        {
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            if(!_context.Ceg.Any(c=>c.Id == ugyfelLevel.CegId))
            {
                return NotFound();
            }

            UgyfLevelek ujPanasz = new UgyfLevelek { Panasz = ugyfelLevel.Panasz, CegId = ugyfelLevel.CegId, PanaszoloId = user_id };
            _context.UgyfLevelek.Add(ujPanasz);
            await _context.SaveChangesAsync();

            var dto = new UgyfLevelekDTO(ujPanasz);

            return CreatedAtAction(nameof(GetUgyfLevelek), new { id = ujPanasz.Id }, dto);
        }

        /*
         * Visszaadja annak a cégnek a beérkezett ügyfél leveleit, amelyhez a bejelentkezett felhasználó (ügyintéző) tartozik.
         * api/UgyfLevelek/GetCegLevelei
         */
        [HttpGet("GetCegLevelei")]
        public async Task<ActionResult<IEnumerable<UgyfLevelekDTO>>> GetCegLevelei()
        {
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            if (_context.FelhasznaloTelephely.Any(ft => ft.FelhasznaloId.Equals(user_id)))
            {
                ModelState.AddModelError("Jogosultság", "Nincs jogosultsága a parancs végrehajtásához.");
                return BadRequest(ModelState);
            }

            var egyTelephelyHozzarendeles = await _context.FelhasznaloTelephely.Where(ft => ft.FelhasznaloId.Equals(user_id)).FirstAsync();
            var telephely = await _context.Telephely.Where(t => t.Id == egyTelephelyHozzarendeles.TelephelyId).FirstAsync();

            var ceg = await _context.Ceg.FindAsync(telephely.Ceg_id);
            var levelek = await _context.UgyfLevelek.Where(uf => uf.CegId == ceg.Id).ToListAsync();

            var dtoList = new List<UgyfLevelekDTO>();
            foreach (var item in levelek)
            {
                dtoList.Add(new UgyfLevelekDTO(item));
            }
            return dtoList;
        }

        /*
         * Adott id-val rendelkező ügyféllevél törlése
         * api/UgyfLevelek/DeleteUgyfLevel/{id}
         */
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<UgyfLevelekDTO>> DeleteUgyfLevel([FromRoute] int id)
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
