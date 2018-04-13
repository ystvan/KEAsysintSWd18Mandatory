using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KEACanteenREST.Models;

namespace KEACanteenREST.Controllers
{
    [Produces("application/json")]
    [Route("api/SensorDatas")]
    public class SensorDatasController : Controller
    {
        private readonly db_sysint_prodContext _context;

        public SensorDatasController(db_sysint_prodContext context)
        {
            _context = context;
        }

        // GET: api/SensorDatas
        [HttpGet]
        public IEnumerable<SensorDatas> GetSensorDatas()
        {
            return _context.SensorDatas;
        }

        // GET: api/SensorDatas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSensorDatas([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sensorDatas = await _context.SensorDatas.SingleOrDefaultAsync(m => m.Id == id);

            if (sensorDatas == null)
            {
                return NotFound();
            }

            return Ok(sensorDatas);
        }

        // PUT: api/SensorDatas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSensorDatas([FromRoute] Guid id, [FromBody] SensorDatas sensorDatas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sensorDatas.Id)
            {
                return BadRequest();
            }

            _context.Entry(sensorDatas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorDatasExists(id))
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

        // POST: api/SensorDatas
        [HttpPost]
        public async Task<IActionResult> PostSensorDatas([FromBody] SensorDatas sensorDatas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SensorDatas.Add(sensorDatas);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SensorDatasExists(sensorDatas.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSensorDatas", new { id = sensorDatas.Id }, sensorDatas);
        }

        // DELETE: api/SensorDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensorDatas([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sensorDatas = await _context.SensorDatas.SingleOrDefaultAsync(m => m.Id == id);
            if (sensorDatas == null)
            {
                return NotFound();
            }

            _context.SensorDatas.Remove(sensorDatas);
            await _context.SaveChangesAsync();

            return Ok(sensorDatas);
        }

        private bool SensorDatasExists(Guid id)
        {
            return _context.SensorDatas.Any(e => e.Id == id);
        }
    }
}