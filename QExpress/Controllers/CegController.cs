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
    public class CegController : ControllerBase
    {
        private readonly QExpressDbContext _context;

        public CegController(QExpressDbContext context)
        {
            _context = context;
        }

        /*
         * Az osszes ceg lekerese.
         * api/Ceg/GetCegek
         */
        [HttpGet]
        [Route("GetCegek")]
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

        /*
         * A parameterkent kapott ID-val rendelkezo ceg lekerese.
         * api/Ceg/GetCeg/{id}
         * param: lekérendő cég id
         */
        [HttpGet("GetCeg/{id}")]
        public async Task<ActionResult<CegDTO>> GetCeg([FromRoute] int id)
        {
            if (!CegExists(id))
            {
                ModelState.AddModelError(nameof(id), "A megadott azonosítóval nem létezik cég.");
                return BadRequest(ModelState);
            }

            var ceg = await _context.Ceg.FindAsync(id);

            return new CegDTO(ceg);
        }

        /*
         * Cégadminhoz tartozó cég dolgozóinak lekérése
         * api/Kategoria/GetDolgozokCegadmin
         */
        [HttpGet]
        [Route("GetDolgozokCegadmin")]
        public async Task<ActionResult<IEnumerable<FelhasznaloTelephelyDTO>>> GetDolgozokCegadmin()
        {
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            var cegadmin = await _context.Felhasznalo.FindAsync(user_id);

            if (cegadmin.jogosultsagi_szint != 3)
            {
                ModelState.AddModelError(nameof(cegadmin.jogosultsagi_szint), "Nincs jogosultsága a parancs végrehajtásához.");
                return BadRequest(ModelState);
            }

            if (!_context.Ceg.Any(c => c.CegadminId == user_id))
            {
                ModelState.AddModelError(nameof(user_id), "A felhasználóhoz nem tartozik cég.");
                return BadRequest(ModelState);
            }

            var ceg = await _context.Ceg.Where(c => c.CegadminId == user_id).FirstAsync();
            var telephelyek = await _context.Telephely.Where(t => t.Ceg_id == ceg.Id).Select(tt=>tt.Id).ToListAsync();

            var dolgozok = await _context.FelhasznaloTelephely.Where(ft => telephelyek.Contains(ft.TelephelyId)).ToListAsync();

            var dto = new List<FelhasznaloTelephelyDTO>();
            foreach (var d in dolgozok)
            {
                dto.Add(new FelhasznaloTelephelyDTO(d));
            }
            return dto;
        }



        /*
         * A parameterkent kapott ID-val rendelkezo ceg kategoriainak lekerese.
         * api/Ceg/{id}/Kategoriak
         * param: id - annak a cégnek az id-ja, aminek a kategóriái kellenek
         */
        [HttpGet("{id}/Kategoriak")]
        public async Task<ActionResult<IEnumerable<KategoriaDTO>>> GetCegKategoriai([FromRoute] int id)
        {
            if (!CegExists(id))
            {
                ModelState.AddModelError(nameof(id), "A megadott azonosítóval nem létezik cég.");
                return BadRequest(ModelState);
            }
            var kategoriak = await _context.Kategoria.Where(k => k.CegId == id).ToListAsync();

            if (kategoriak.Count == 0)
            {
                ModelState.AddModelError("", "A céghez nem tartozik kategória.");
                return BadRequest(ModelState);
            }

            var dto = new List<KategoriaDTO>();
            foreach (var k in kategoriak)
            {
                dto.Add(new KategoriaDTO(k));
            }
            return dto;
        }

        /*
         * A parameterkent kapott ID-val rendelkezo ceg adatait frissiti.
         * api/Ceg/{ceg_id}/UpdateCeg
         * params: id: ceg id-ja, ceg: CegDTO a frissitett adatokkal
         */
        [HttpPut("UpdateCeg")]
        public async Task<IActionResult> UpdateCeg([FromBody] CegDTO ceg)
        {
            if (ceg.Nev.Trim().Length == 0)
            {
                ModelState.AddModelError(nameof(ceg.Nev), "A cég neve nem lehet üres.");
                return BadRequest(ModelState);
            }
            if (!CegExists(ceg.Id))
            {
                ModelState.AddModelError(nameof(ceg.Id), "A megadott azonosítóval nem létezik cég.");
                return BadRequest(ModelState);
            }
            if(!_context.Felhasznalo.Any(f => f.Id.Equals(ceg.CegadminId)))
            {
                ModelState.AddModelError(nameof(ceg.CegadminId), "A megadott azonosítóhoz nem tartozik felhasználó.");
                return BadRequest(ModelState);
            }


            var frissitendo_ceg = await _context.Ceg.FindAsync(ceg.Id);

            var regi_admin = await _context.Felhasznalo.FindAsync(ceg.CegadminId);
            regi_admin.jogosultsagi_szint = 1;

            var uj_admin = await _context.Felhasznalo.FindAsync(ceg.CegadminId);
            uj_admin.jogosultsagi_szint = 3;

            frissitendo_ceg.CegadminId = ceg.CegadminId;
            frissitendo_ceg.nev = ceg.Nev;

            await _context.SaveChangesAsync();
            var dto = new CegDTO(frissitendo_ceg);


            return CreatedAtAction(nameof(GetCeg), new { id = frissitendo_ceg.Id }, dto);

        }


        /*
         * Uj ceg rogzitese
         * api/Ceg/AddCeg
         * params: cegnev: cég neve, cegadmin_id: cégadmin id-ja
         */
        [HttpPost]
        [Route("AddCeg")]
        public async Task<ActionResult<CegDTO>> AddCeg([FromBody] CegDTO ceg)
        {
            if (ceg.Nev.Trim().Length == 0)
            {
                ModelState.AddModelError(nameof(ceg.Nev), "A cég neve nem lehet üres.");
                return BadRequest(ModelState);
            }
            if (!_context.Felhasznalo.Any(f => f.Id.Equals(ceg.CegadminId)))
            {
                ModelState.AddModelError(nameof(ceg.CegadminId), "A megadott azonosítóhoz nem tartozik felhasználó.");
                return BadRequest(ModelState);
            }

            var cegadmin = await _context.Felhasznalo.FindAsync(ceg.CegadminId);
            cegadmin.jogosultsagi_szint = 2;
            Ceg ujCeg = new Ceg { nev = ceg.Nev, CegadminId = ceg.CegadminId };

            _context.Ceg.Add(ujCeg);
            await _context.SaveChangesAsync();

            var dto = new CegDTO(ujCeg);

            return CreatedAtAction(nameof(GetCeg), new { id = ujCeg.Id }, dto);
        }


        /*
         * A parameterkent kapott ID-val rendelkezo ceg torlese.
         * Torles sorban: 
         * - ceg ugyflevelei
         * - sorszamok
         * - dolgozok hozzarendelesei
         * - telephelyek
         * - kategoriak
         * - ceg
         * 
         * api/Ceg/Delete/{id}
         * param: id: törlendő cég id-ja
         */
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCeg([FromRoute] int id)
        {
            if (!CegExists(id))
            {
                ModelState.AddModelError(nameof(id), "A megadott azonosítóval nem létezik cég.");
                return BadRequest(ModelState);
            }
            var ceg = await _context.Ceg.FindAsync(id);
            
            // sorrend: levelek, sorszamok, dolgozok, telephelyek, kategoriak, ceg

            var ugyfLevelek = await _context.UgyfLevelek.Where(uf => uf.CegId == id).ToListAsync();
            var telephelyek = await _context.Telephely.Where(t => t.Ceg_id == id).ToListAsync();
            var telephelyek_id = new List<int>();
            foreach (var item in telephelyek)
            {
                telephelyek_id.Add(item.Id);
            }
            var felhasznalo_telephely = await _context.FelhasznaloTelephely.Where(ft => telephelyek_id.Contains(ft.TelephelyId)).ToListAsync();
            
            var dolgozoIDs = await _context.FelhasznaloTelephely.Where(ft => telephelyek_id.Contains(ft.TelephelyId)).Select(f => f.FelhasznaloId).ToListAsync();
            var dolgozok = await _context.Felhasznalo.Where(d => dolgozoIDs.Contains(d.Id)).ToListAsync();
            foreach (var dolgozo in dolgozok)
                dolgozo.jogosultsagi_szint = 1;

            var sorszamok = await _context.Sorszam.Where(s => telephelyek_id.Contains(s.TelephelyId)).ToListAsync();
            var kategoriak = await _context.Kategoria.Where(k => k.CegId == id).ToListAsync();

            _context.UgyfLevelek.RemoveRange(ugyfLevelek);
            _context.Sorszam.RemoveRange(sorszamok);
            _context.FelhasznaloTelephely.RemoveRange(felhasznalo_telephely);
            _context.Telephely.RemoveRange(telephelyek);
            _context.Kategoria.RemoveRange(kategoriak);
            _context.Ceg.Remove(ceg);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*
        * Ellenorzes, hogy a parameterkent kapott ID-val letezik-e ceg.
        */
        private bool CegExists(int id)
        {
            return _context.Ceg.Any(e => e.Id == id);
        }




































        /*
 * Aktuális felhasználó ha cégadmin, akkor a cégének a nevét változtatja meg a kapott paraméterre.
 * api/Ceg/NewName
 * param: Aktuális felhasz
 */
        [HttpPut]
        [Route("{ceg_id}/NewName")]
        public async Task<IActionResult> EdigCegNev([FromRoute] int ceg_id, [FromBody] String uj_nev)
        {
            if (!CegExists(ceg_id))
                return NotFound();

            var ceg = await _context.Ceg.FindAsync(ceg_id);

            if (string.IsNullOrEmpty(uj_nev))
                ModelState.AddModelError(nameof(uj_nev), "Cégnév megadása kötelező");


            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            ceg.nev = uj_nev;

            await _context.SaveChangesAsync();

            var dto = new CegDTO(ceg);


            return CreatedAtAction(nameof(GetCeg), new { id = ceg.Id }, dto);
        }

        /*
         * A parameterkent kapott ID-val rendelkezo ceg elere uj felhasznalo beallitasa adminnak.
         * api/Ceg/{ceg_id}/UpdateAdmin
         * params: id: ceg id-ja, uj_admin_id: új admin felhasználó id-ja
         */
        [HttpPut("{ceg_id}/UpdateAdmin")]
        public async Task<IActionResult> EditCegAdmin([FromRoute] int ceg_id, [FromBody] String uj_admin_felhasznalonev)
        {
            if (!_context.Felhasznalo.Any(f => f.UserName.Equals(uj_admin_felhasznalonev)))
            {
                return NotFound();
            }

            if (_context.Ceg.Any(c => c.CegadminId == uj_admin_felhasznalonev))
            {
                return BadRequest(new { error = "user is admin already" });
            }

            var uj_admin = await _context.Felhasznalo.Where(f => f.UserName.Equals(uj_admin_felhasznalonev)).FirstAsync();


            if (!CegExists(ceg_id))
            {
                return NotFound();
            }

            Ceg ceg = await _context.Ceg.FindAsync(ceg_id);

            ceg.CegadminId = uj_admin.Id;
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
