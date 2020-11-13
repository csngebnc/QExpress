using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QExpress.Data;
using QExpress.Models;
using QExpress.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FelhasznaloController : ControllerBase
    {
        private readonly QExpressDbContext _context;
        public FelhasznaloController(QExpressDbContext context)
        {
            _context = context;
        }

        /*
         * Aktualisan bejelentkezett felhasznalo lekerese.
         * api/Felhasznalo/GetCurrentFelhasznalo
         */
        [HttpGet]
        [Route("GetCurrentFelhasznalo")]
        public async Task<ActionResult<FelhasznaloDTO>> GetCurrentFelhasznalo()
        {
            var user = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
            if(user == null)
            {
                return new FelhasznaloDTO{ jogosultsagi_szint = 0, Email= "None", Id = "", UserName = "None" };
            }
            var currentUser = await _context.Felhasznalo.FindAsync(user.Value);
            return new FelhasznaloDTO(currentUser);
        }

        /*
         * Osszes felhasznalo lekerese
         * api/Felhasznalo/GetFelhasznalok
         */
        [HttpGet]
        [Route("GetFelhasznalok")]
        public async Task<ActionResult<IEnumerable<FelhasznaloDTO>>> GetFelhasznalok()
        {
            var felhasznalok = await _context.Felhasznalo.ToListAsync();
            var dto = new List<FelhasznaloDTO>();
            foreach (var f in felhasznalok)
            {
                dto.Add(new FelhasznaloDTO(f));
            }
            return dto;
        }

        /*
         * Egy felhasznalo lekerese megadott ID alapjan.
         * api/Felhasznalo/GetFelhasznalo/{id}
         */
        [HttpGet("GetFelhasznalo/{id}")]
        public async Task<ActionResult<FelhasznaloDTO>> GetFelhasznalo([FromRoute] String id)
        {
            if (!FelhasznaloExists(id))
            {
                ModelState.AddModelError(nameof(id), "A megadott azonosítóhoz nem tartozik felhasználó.");
                return BadRequest(ModelState);
            }

            var felhasznalo = await _context.Felhasznalo.FindAsync(id);

            return new FelhasznaloDTO(felhasznalo);
        }

        /*
         * Egy felhasznalo lekerese megadott email alapjan.
         * api/Felhasznalo/GetFelhasznaloByEmail/{email}
         */
        [HttpGet("GetFelhasznaloByEmail/{email}")]
        public async Task<ActionResult<FelhasznaloDTO>> GetFelhasznaloByEmail([FromRoute] String email)
        {
            if(!_context.Felhasznalo.Any(f => f.Email.Equals(email)))
            {
                ModelState.AddModelError("Email", "A megadott e-mail címhez nem tartozik felhasználó.");
                return BadRequest(ModelState);
            }

            var felhasznalo = await _context.Felhasznalo.Where(f => f.Email.Equals(email)).FirstAsync();
            if (felhasznalo == null)
            {
                ModelState.AddModelError(nameof(email), "A megadott e-mail címhez nem tartozik felhasználó.");
                return BadRequest(ModelState);
            }
            return new FelhasznaloDTO(felhasznalo);
        }

        /*
         * A bejelentkezett felhasznalo aktiv sorszamainak lekerese
         * api/Felhasznalo/AktivSorszamok
         */
        [HttpGet]
        [Route("AktivSorszamok")]
        public async Task<ActionResult<IEnumerable<SorszamDTO>>> GetFelhasznaloSorszamai()
        {
            var user = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
            if (user == null)
            {
                ModelState.AddModelError("", "Nem vagy bejelentkezve.");
                return BadRequest(ModelState);
            }
            string id = user.Value;
            var sorszamok = await _context.Sorszam.Where(s => s.UgyfelId.Equals(id) && s.Allapot.Equals("Aktív")).ToListAsync();

            var dto = new List<SorszamDTO>();
            foreach (var s in sorszamok)
            {
                var ujDTO = new SorszamDTO(s);
                var cegnev = s.Telephely.Ceg.nev;
                var telephelyCim = s.Telephely.Cim;
                var kategoriaNeve = s.Kategoria.Megnevezes;
                var sorbanAllok = (await _context.Sorszam.Where(ssz => ssz.TelephelyId == s.TelephelyId && ssz.Allapot.Equals("Aktív")).ToListAsync()).Count;

                ujDTO.Ceg = cegnev;
                ujDTO.Telephely = telephelyCim;
                ujDTO.Kategoria = kategoriaNeve;
                ujDTO.SorbanAllokSzama = sorbanAllok - 1;
                dto.Add(ujDTO);
            }
            return dto;
        }

        /*
         * A bejelentkezett felhasznalo mar korabban behivott sorszamainak lekerese
         * api/Felhasznalo/KorabbiSorszamok
         */
        [HttpGet]
        [Route("KorabbiSorszamok")]
        public async Task<ActionResult<IEnumerable<SorszamDTO>>> GetKorabbiSorszamok()
        {
            var user = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
            if (user == null)
            {
                ModelState.AddModelError("", "Nem vagy bejelentkezve.");
                return BadRequest(ModelState);
            }
            string id = user.Value;
            var sorszamok = await _context.Sorszam.Where(s => s.UgyfelId.Equals(id) && s.Allapot.Equals("Behívott")).ToListAsync();
            
            var dto = new List<SorszamDTO>();
            foreach (var s in sorszamok)
            {
                var ujDTO = new SorszamDTO(s);
                var cegnev = s.Telephely.Ceg.nev;
                var telephelyCim = s.Telephely.Cim;
                var kategoriaNeve = s.Kategoria.Megnevezes;
                var sorbanAllok = (await _context.Sorszam
                    .Where(ssz => 
                    ssz.TelephelyId == s.TelephelyId && 
                    ssz.Allapot.Equals("Aktív") &&
                    ssz.KategoriaId == s.KategoriaId &&
                    ssz.Idopont < s.Idopont
                    ).ToListAsync()).Count;

                ujDTO.Ceg = cegnev.ToString();
                ujDTO.Telephely = telephelyCim.ToString();
                ujDTO.Kategoria = kategoriaNeve.ToString();
                ujDTO.SorbanAllokSzama = sorbanAllok - 1;
                dto.Add(ujDTO);
            }
            return dto;
        }

        [HttpGet]
        [Route("GetTelephelySorszamai")]
        public async Task<ActionResult<IEnumerable<SorszamDTO>>> GetTelephelySorszamai()
        {
            var user = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
            if(user == null)
            {
                ModelState.AddModelError("", "Nem vagy bejelentkezve.");
                return BadRequest(ModelState);
            }
            string user_id = user.Value;
            var telephelyHozzarendeles = await _context.FelhasznaloTelephely.Where(ft => ft.FelhasznaloId.Equals(user_id)).FirstAsync();
            var telephely = await _context.Telephely.FindAsync(telephelyHozzarendeles.TelephelyId);

            var sorszamok = await _context.Sorszam.Where(s => s.TelephelyId == telephely.Id && s.Allapot.Equals("Aktív")).ToListAsync();

            var dto = new List<SorszamDTO>();
            foreach (var s in sorszamok)
            {
                var ujDTO = new SorszamDTO(s);
                var ugyfelNeve = s.Ugyfel.UserName.ToString();
                var kategoriaNeve = s.Kategoria.Megnevezes.ToString();
                var sorbanAllok = (await _context.Sorszam.Where(ssz => ssz.TelephelyId == s.TelephelyId && ssz.Allapot.Equals("Aktív")).ToListAsync()).Count;

                ujDTO.Ugyfel = ugyfelNeve;
                ujDTO.Kategoria = kategoriaNeve;
                ujDTO.SorbanAllokSzama = sorbanAllok;
                dto.Add(ujDTO);
            }

            return dto;
        }

        /*
         * A bejelentkezett felhasznalo e-mail cimenek megvaltoztatasa
         * api/Felhasznalo/NewEmail
         * param: uj email
         */
        [HttpPut]
        [Route("NewEmail")]
        public async Task<IActionResult> EditFelhasznaloEmail([FromBody] String uj_email)
        {
            if(User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier) == null)
            {
                ModelState.AddModelError("Bejelentkezés", "Nincs bejelentkezve. A művelet végrehajtásához jelentkezzen be.");
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(uj_email))
            {
                ModelState.AddModelError("Email", "A megadott e-mail cím nem lehet üres.");
                return BadRequest(ModelState);
            }


            string id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            Felhasznalo felh = _context.Felhasznalo.Where(f => f.Id.Equals(id)).First();

            felh.Email = uj_email;
            await _context.SaveChangesAsync();

            var dto = new FelhasznaloDTO(felh);

            return CreatedAtAction(nameof(GetFelhasznalo), new { id = felh.Id }, dto);
        }

        /*
         * A bejelentkezett felhasznalo jelszavanak megvaltoztatasa
         * api/Felhasznalo/NewPassword
         * params: regi_jelszo, uj_jelszo
         */
        [HttpPut]
        [Route("NewPassword")]
        public async Task<IActionResult> EditFelhasznaloJelszo([FromBody] String[] jelszavak)
        {

            if (User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier) == null)
            {
                ModelState.AddModelError("Bejelentkezés", "Nincs bejelentkezve. A művelet végrehajtásához jelentkezzen be.");
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(jelszavak[0]) || string.IsNullOrEmpty(jelszavak[1]))
            {
                ModelState.AddModelError("Jelszó", "Minden jelszó mező kitöltése kötelező.");
                return BadRequest(ModelState);
            }

            string id = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            Felhasznalo felh = await _context.Felhasznalo.FindAsync(id);


            if (!felh.PasswordHash.Equals(jelszavak[0]))
            {
                ModelState.AddModelError("Rossz jelszó", "A megadott jelszó nem egyezik a jelenlegi jelszóval.");
                return BadRequest(ModelState);
            }


            felh.PasswordHash = jelszavak[1];
            await _context.SaveChangesAsync();

            var dto = new FelhasznaloDTO(felh);

            return CreatedAtAction(nameof(GetFelhasznalo), new { id = felh.Id }, dto);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<FelhasznaloDTO>> DeleteFelhasznalo([FromRoute] String id)
        {
            if (!FelhasznaloExists(id))
            {
                ModelState.AddModelError(nameof(id), "A megadott azonosítóhoz nem tartozik felhasználó.");
                return BadRequest(ModelState);
            }

            var felhasznalo = await _context.Felhasznalo.FindAsync(id);
            var ceg = await _context.Ceg.Where(c => c.CegadminId.Equals(felhasznalo.Id)).FirstAsync();

            var felhasznalo_sorszamai = await _context.Sorszam.Where(s => s.UgyfelId.Equals(felhasznalo.Id)).ToListAsync();
            _context.Sorszam.RemoveRange(felhasznalo_sorszamai);
            var felhasznalo_panaszai = await _context.UgyfLevelek.Where(s => s.PanaszoloId.Equals(felhasznalo.Id)).ToListAsync();
            _context.UgyfLevelek.RemoveRange(felhasznalo_panaszai);
            if (_context.FelhasznaloTelephely.Any(ft => ft.FelhasznaloId.Equals(felhasznalo.Id)))
                _context.FelhasznaloTelephely.Remove(await _context.FelhasznaloTelephely.Where(ft => ft.FelhasznaloId.Equals(felhasznalo.Id)).FirstAsync());

            if (ceg != null)
            {
                var ugyfLevelek = await _context.UgyfLevelek.Where(uf => uf.CegId == ceg.Id).ToListAsync();
                var telephelyek = await _context.Telephely.Where(t => t.Ceg_id == ceg.Id).ToListAsync();
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
                var kategoriak = await _context.Kategoria.Where(k => k.CegId == ceg.Id).ToListAsync();

                _context.UgyfLevelek.RemoveRange(ugyfLevelek);
                _context.Sorszam.RemoveRange(sorszamok);
                _context.FelhasznaloTelephely.RemoveRange(felhasznalo_telephely);
                _context.Telephely.RemoveRange(telephelyek);
                _context.Kategoria.RemoveRange(kategoriak);
                _context.Ceg.Remove(ceg);

            }
            _context.Felhasznalo.Remove(felhasznalo);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /*
         * Megadott ID-val rendelkezo felhasznalo hozzarendelese a megadott telephelyhez.
         * api/Felhasznalo/SetTelephely
         * param: FelhasznaloTelephelyDTO --> FelhasznaloId, TelephelyId
         */
        [HttpPost]
        [Route("SetTelephely")]
        public async Task<IActionResult> SetTelephely([FromBody] FelhasznaloTelephelyDTO felhasznaloTelephely)
        {
            if (!FelhasznaloExists(felhasznaloTelephely.FelhasznaloId))
            {
                ModelState.AddModelError(nameof(felhasznaloTelephely.FelhasznaloId), "A megadott azonosítóhoz nem tartozik felhasználó.");
                return BadRequest(ModelState);
            }
            if (!_context.Telephely.Any(e => e.Id == felhasznaloTelephely.TelephelyId))
            {
                ModelState.AddModelError(nameof(felhasznaloTelephely.TelephelyId), "A megadott azonosítóhoz nem tartozik telephely.");
                return BadRequest(ModelState);
            }
            if (_context.FelhasznaloTelephely.Any(ft => ft.FelhasznaloId.Equals(felhasznaloTelephely.FelhasznaloId)))
            {
                ModelState.AddModelError(nameof(felhasznaloTelephely.FelhasznaloId), "A megadott e-mail cím már egy másik telephelyhez regisztrálva van.");
                return BadRequest(ModelState);
            }


            Felhasznalo felh = _context.Felhasznalo.Where(f => f.Id.Equals(felhasznaloTelephely.FelhasznaloId)).First();
            _context.FelhasznaloTelephely.Add(new FelhasznaloTelephely { FelhasznaloId = felhasznaloTelephely.FelhasznaloId, TelephelyId = felhasznaloTelephely.TelephelyId });
            felh.jogosultsagi_szint = 2;

            await _context.SaveChangesAsync();

            var dto = new FelhasznaloDTO(felh);

            return CreatedAtAction(nameof(GetFelhasznalo), new { id = felh.Id }, dto);
        }

        [HttpPut]
        [Route("UpdateUgyintezoTelephely")]
        public async Task<IActionResult> UpdateUgyintezoTelephely([FromBody] FelhasznaloTelephelyDTO felhasznaloTelephely)
        {
            
            if (!FelhasznaloExists(felhasznaloTelephely.FelhasznaloId))
            {
                ModelState.AddModelError(nameof(felhasznaloTelephely.FelhasznaloId), "A megadott azonosítóhoz nem tartozik felhasználó.");
                return BadRequest(ModelState);
            }
            if (!_context.Telephely.Any(e => e.Id == felhasznaloTelephely.TelephelyId))
            {
                ModelState.AddModelError(nameof(felhasznaloTelephely.TelephelyId), "A megadott azonosítóhoz nem tartozik telephely.");
                return BadRequest(ModelState);
            }
            if (!_context.FelhasznaloTelephely.Any(ft => ft.FelhasznaloId.Equals(felhasznaloTelephely.FelhasznaloId)))
            {
                ModelState.AddModelError(nameof(felhasznaloTelephely.FelhasznaloId), "A megadott felhasználó nem tartozik telephelyhez.");
                return BadRequest(ModelState);
            }

            var felhTelep = await _context.FelhasznaloTelephely.Where(f => f.FelhasznaloId.Equals(felhasznaloTelephely.FelhasznaloId)).FirstAsync();
            felhTelep.TelephelyId = felhasznaloTelephely.TelephelyId;
            await _context.SaveChangesAsync();


            return Ok();
        }

        /*
         * A megadott felhasznalo eltavolitasa egy megadott telephely alkalmazotti listajabol.
         * api/Felhasznalo/DelFromTelephely
         * param: FelhasznaloTelephelyDTO --> FelhasznaloId, TelephelyId
         */
        [HttpDelete]
        [Route("DelFromTelephely/{user_id}")]
        public async Task<IActionResult> DelFromTelephely([FromRoute] String user_id)
        {
            if (!FelhasznaloExists(user_id))
            {
                ModelState.AddModelError(nameof(user_id), "A megadott azonosítóhoz nem tartozik felhasználó.");
                return BadRequest(ModelState);
            }

            if (!_context.FelhasznaloTelephely.Any(ft => ft.FelhasznaloId.Equals(user_id)))
            {
                ModelState.AddModelError(nameof(user_id), "A megadott felhasználó nem tartozik telephelyhez.");
                return BadRequest(ModelState);
            }

            var felhasznalo = await _context.Felhasznalo.FindAsync(user_id);
            var hozzarendeles = await _context.FelhasznaloTelephely.Where(ft => ft.FelhasznaloId.Equals(user_id)).FirstAsync();

            _context.FelhasznaloTelephely.Remove(hozzarendeles);
            felhasznalo.jogosultsagi_szint = 1;
            await _context.SaveChangesAsync();
           

            return Ok();
        }

        /*
         * Ellenorzes, hogy a parameterkent kapott ID-val letezik-e felhasznalo.
         */
        private bool FelhasznaloExists(String id)
        {
            return _context.Felhasznalo.Any(e => e.Id == id);
        }
    }
}
