using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrugAPI.Models;
using DrugAPI.Attributes;

namespace DrugAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class DrugsController : ControllerBase
    {
        private readonly ApiContext _context;

        public DrugsController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/Drugs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drug>>> GetDrugsList(string filterOnCode = "", string filterOnLabel = "")
        {
            return await _context.DrugsList.Where(x => (filterOnCode == "" || x.Code.Contains(filterOnCode))
                                                && (filterOnLabel == "" || x.Label.Contains(filterOnLabel))).ToListAsync();
        }

        // GET: api/Drugs/5
        [HttpGet("{code}")]
        public async Task<ActionResult<Drug>> GetDrug(string code)
        {
            var drug = await _context.DrugsList.FindAsync(code);

            if (drug == null)
            {
                return NotFound();
            }

            return drug;
        }

        // PUT: api/Drugs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkcode=2123754
        [HttpPut("{code}")]
        public async Task<IActionResult> PutDrug(string code, Drug drug)
        {
            if (code != drug.Code)
            {
                return BadRequest("Error occured : code provided not the same as the one in the new drug definition");
            }

            //Error handling
            string errorMessage = "";
            if (!Drug.isCorrectDrug(drug, out errorMessage))
            {
                return BadRequest(errorMessage);
            }

            _context.Entry(drug).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DrugExists(code))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Drugs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkcode=2123754
        [HttpPost]
        public async Task<ActionResult<Drug>> PostDrug(Drug drug)
        {
            //Error handling
            string errorMessage = "";
            if (!Drug.isCorrectDrug(drug, out errorMessage))
            {
                return BadRequest(errorMessage);
            }


            try
            {
                _context.DrugsList.Add(drug);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (DrugExists(drug.Code))
                {
                    return BadRequest("Error Occured : Drug already present in database");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDrug", new { code = drug.Code }, drug);
        }

        // DELETE: api/Drugs/5
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteDrug(string code)
        {
            var drug = await _context.DrugsList.FindAsync(code);
            if (drug == null)
            {
                return NotFound();
            }

            _context.DrugsList.Remove(drug);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DrugExists(string code)
        {
            return _context.DrugsList.Any(e => e.Code == code);
        }
    }
}
