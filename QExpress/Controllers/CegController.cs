using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QExpress.Data;
using QExpress.Models;
using QExpress.Models.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace QExpress.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CegController : ControllerBase
    {
        private readonly QExpressDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CegController(QExpressDbContext context, IWebHostEnvironment _webHostEnvironment)
        {
            _context = context;
            webHostEnvironment = _webHostEnvironment;
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
         * A cegadmin cegenek lekerese a cegadmin userId-val
         * api/Ceg/GetOwnCeg/{userId}
         * param: current cegadmin userId
         */
        [HttpGet("GetOwnCeg/{userId}")]
        public async Task<ActionResult<CegDTO>> GetOwnCeg([FromRoute] string userId)
        {
            if(!_context.Felhasznalo.Any(f => f.Id.Equals(userId)))
            {
                ModelState.AddModelError(nameof(userId), "A megadott azonosítóhoz nem tartozik felhasználó.");
                return BadRequest(ModelState);
            }

            var cegadmin = await _context.Felhasznalo.FindAsync(userId);

            if (cegadmin.jogosultsagi_szint != 3)
            {
                ModelState.AddModelError(nameof(cegadmin.jogosultsagi_szint), "Nincs jogosultsága a parancs végrehajtásához.");
                return BadRequest(ModelState);
            }
            if (!_context.Ceg.Any(c => c.CegadminId == userId))
            {
                ModelState.AddModelError(nameof(userId), "A felhasználóhoz nem tartozik cég.");
                return BadRequest(ModelState);
            }

            var ceg = await _context.Ceg.Where(c => c.CegadminId == userId).FirstAsync();

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

            if (!_context.Ceg.Any(c => c.CegadminId.Equals(user_id)))
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


        [HttpGet("GetAlkalmazottTelephely/{id}")]
        public async Task<ActionResult<FelhasznaloTelephelyDTO>> GetAlkalmazottTelephely([FromRoute] String id)
        {
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            var cegadmin = await _context.Felhasznalo.FindAsync(user_id);

            if (cegadmin.jogosultsagi_szint != 3)
            {
                ModelState.AddModelError(nameof(cegadmin.jogosultsagi_szint), "Nincs jogosultsága a parancs végrehajtásához.");
                return BadRequest(ModelState);
            }
            if (!_context.Ceg.Any(c => c.CegadminId.Equals(user_id)))
            {
                ModelState.AddModelError(nameof(user_id), "A felhasználóhoz nem tartozik cég.");
                return BadRequest(ModelState);
            }
            if (!_context.FelhasznaloTelephely.Any(ft => ft.FelhasznaloId.Equals(id)))
            {
                ModelState.AddModelError(nameof(id), "A megadott azonosítóval nincs regisztrált munkavállaló.");
                return BadRequest(ModelState);
            }

            var hozzarendeles = await _context.FelhasznaloTelephely.Where(ft => ft.FelhasznaloId.Equals(id)).FirstAsync();

            return new FelhasznaloTelephelyDTO(hozzarendeles);
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
        public async Task<IActionResult> UpdateCeg([FromForm] CegKepDTO ceg)
        {
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            var szuperadmin = await _context.Felhasznalo.FindAsync(user_id);

            if (!_context.Felhasznalo.Any(f => f.Id.Equals(ceg.CegadminId)))
            {
                ModelState.AddModelError("email", "A megadott azonosítóhoz nem tartozik felhasználó. " + ceg.CegadminId);
                return BadRequest(ModelState);
            }
            if (_context.Ceg.Any(c => c.CegadminId.Equals(ceg.CegadminId) && c.Id != ceg.Id))
            {
                ModelState.AddModelError("email", "A megadott felhasználó már egy másik cég adminja.");
                return BadRequest(ModelState);
            }
            if (!CegExists(ceg.Id))
            {
                ModelState.AddModelError("ceg", "A megadott azonosítóval nem létezik cég.");
                return BadRequest(ModelState);
            }

            var frissitendo_ceg = await _context.Ceg.FindAsync(ceg.Id);

            var regi_admin = await _context.Felhasznalo.FindAsync(ceg.CegadminId);
            regi_admin.jogosultsagi_szint = 1;

            var uj_admin = await _context.Felhasznalo.FindAsync(ceg.CegadminId);
            uj_admin.jogosultsagi_szint = 3;

            frissitendo_ceg.CegadminId = ceg.CegadminId;
            frissitendo_ceg.nev = ceg.Nev;

            var imagePath = await UploadFileGeneratePath(ceg.image, ceg.Id.ToString());

            if (System.IO.File.Exists(Path.Combine(webHostEnvironment.WebRootPath, frissitendo_ceg.ImagePath)) && imagePath != null)
                System.IO.File.Delete(Path.Combine(webHostEnvironment.WebRootPath, frissitendo_ceg.ImagePath));

            if (imagePath != null)
                frissitendo_ceg.ImagePath = imagePath;
            else
                frissitendo_ceg.ImagePath = "http://via.placeholder.com/160x160";

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
        public async Task<ActionResult<CegKepDTO>> AddCeg([FromForm] CegKepDTO ceg)
        {
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            var szuperadmin = await _context.Felhasznalo.FindAsync(user_id);

            if (!_context.Felhasznalo.Any(f => f.Id.Equals(ceg.CegadminId)))
            {
                ModelState.AddModelError(nameof(ceg.CegadminId), "A megadott azonosítóhoz nem tartozik felhasználó.");
                return BadRequest(ModelState);
            }
            if (_context.Ceg.Any(c => c.CegadminId.Equals(ceg.CegadminId)))
            {
                ModelState.AddModelError("email", "A megadott felhasználó már egy másik cég adminja.");
                return BadRequest(ModelState);
            }

            var cegadmin = await _context.Felhasznalo.FindAsync(ceg.CegadminId);
            cegadmin.jogosultsagi_szint = 3;
            Ceg ujCeg = new Ceg();
            ujCeg.nev = ceg.Nev;
            ujCeg.CegadminId = ceg.CegadminId;
            Console.WriteLine(ujCeg.ImagePath);
            _context.Ceg.Add(ujCeg);

            
            await _context.SaveChangesAsync();
            var ceg_ujra = await _context.Ceg.Where(c => c.CegadminId.Equals(ceg.CegadminId)).FirstAsync();
            var image_path = await UploadFileGeneratePath(ceg.image, ceg_ujra.Id.ToString());

            if (image_path != null)
                ceg_ujra.ImagePath = image_path;
            else
                ceg_ujra.ImagePath = "http://via.placeholder.com/160x160";

            await _context.SaveChangesAsync();
            var dto = new CegDTO(ujCeg);
            return CreatedAtAction(nameof(GetCeg), new { id = ujCeg.Id }, dto);
        }


        private async Task<string> UploadFileGeneratePath(IFormFile uploaded_file, String ceg_id)
        {
            string path = null;

            if (uploaded_file != null)
            {
                string folder = Path.Combine(webHostEnvironment.WebRootPath, "Images", "Cegek");
                string newFileName = ceg_id + Path.GetExtension(uploaded_file.FileName);

                path = Path.Combine("Images", "Cegek", newFileName);

                using (var fileStream = new FileStream(Path.Combine(folder, newFileName), FileMode.Create))
                {
                    await uploaded_file.CopyToAsync(fileStream);
                }
            }
            return path;
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
            string user_id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

            var szuperadmin = await _context.Felhasznalo.FindAsync(user_id);

            if (szuperadmin.jogosultsagi_szint != 4)
            {
                ModelState.AddModelError(nameof(szuperadmin.jogosultsagi_szint), "Nincs jogosultsága a parancs végrehajtásához.");
                return BadRequest(ModelState);
            }

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
            var cegadmin = await _context.Felhasznalo.Where(f => f.Id.Equals(ceg.CegadminId)).FirstAsync();
            cegadmin.jogosultsagi_szint = 1;
            if(ceg.ImagePath != null)
                if (System.IO.File.Exists(Path.Combine(webHostEnvironment.WebRootPath, ceg.ImagePath)))
                    System.IO.File.Delete(Path.Combine(webHostEnvironment.WebRootPath, ceg.ImagePath));

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
























        // DEADCODE -->











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
