using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KEACanteenREST.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace KEACanteenREST.Controllers
{
    
    [Route("api/SensorDatas")]
    public class SensorDatasController : Controller
    {
        private ILogger<SensorDatasController> _logger;
        private readonly db_sysint_prodContext _context;

        public SensorDatasController(db_sysint_prodContext context, ILogger<SensorDatasController> logger)
        {
            _logger = logger;
            _context = context;
        }
        
        /// <summary>
        /// Sample URI and HTTP method: GET: api/SensorDatas
        /// </summary>
        /// <returns>A collection of measurements as a response payload</returns>
        [HttpGet]
        public IActionResult GetSensorDatas()
        {            
            var dataFromAzure = _context.SensorDatas;
            var modelToReturn = Mapper.Map<IEnumerable<RecordDto>>(dataFromAzure);
            return Ok(modelToReturn);                        
        }

        /// <summary>
        /// Sample URI and HTTP method: GET api/SensorDatas/32a483a6-3eb8-4cca-8f63-3394c95ecd0b
        /// </summary>
        /// <param name="id">The Guid Id of a single measurement resource</param>
        /// <returns>A single measurement record as a response payload</returns>
        [HttpGet("{id}", Name = "GetById")]
        public async Task<IActionResult> GetSensorDatas([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dataFromAzure = await _context.SensorDatas.SingleOrDefaultAsync(m => m.Id == id);

            if (dataFromAzure == null)
            {
                return NotFound();
            }

            var modelToReturn = Mapper.Map<RecordDto>(dataFromAzure);
            return Ok(modelToReturn);
        }

        /// <summary>
        /// Sample URI and HTTP method: PUT: api/SensorDatas/32a483a6-3eb8-4cca-8f63-3394c95ecd0b
        /// </summary>
        /// <param name="id">The Guid Id of a single measurement resource</param>
        /// <param name="record">A request payload Measurement object</param>
        /// <returns>Updates the collection by adding a new Measurement object from the request payload</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSensorDatas([FromRoute] Guid id, [FromBody] RecordForUpdateDto record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dataFromAzure = await _context.SensorDatas.SingleOrDefaultAsync(m => m.Id == id);
            if (dataFromAzure == null)
            {
                return NotFound();
            }

            var dataForAzure = Mapper.Map<SensorDatas>(record);
            
            _context.Entry(dataForAzure).State = EntityState.Modified;

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
                    // For the logger file
                    throw new Exception("Updating a record failed on save.");
                }
            }

            return NoContent();
        }
        
        /// <summary>
        /// Sample URI and HTTP method: POST: api/SensorDatas
        /// </summary>
        /// <param name="record">A measurement object as a request payload</param>
        /// <returns>Create a resource according to the Measurement object in the request payload</returns>
        [HttpPost]
        public async Task<IActionResult> PostSensorDatas([FromBody] RecordForCreationDto record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dataForAzure = Mapper.Map<SensorDatas>(record);

            _context.SensorDatas.Add(dataForAzure);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SensorDatasExists(dataForAzure.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    // For the logger file
                    throw new Exception("Creating a record failed on save.");
                }
            }

            var modelToReturn = Mapper.Map<RecordDto>(dataForAzure);

            // Status code 201 with the location (route) in the response header
            return CreatedAtRoute("GetById", new { id = modelToReturn.LocationIdentifier }, modelToReturn);
        }

        /// <summary>
        /// Sample URI and HTTP method:  DELETE: api/SensorDatas/32a483a6-3eb8-4cca-8f63-3394c95ecd0b
        /// </summary>
        /// <param name="id">The Guid Id of a single measurement resource</param>
        /// <returns>A collection of measurements as a response payload without the deleted record</returns>
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
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // For the logger file
                throw new Exception($"Deleting a record failed during the process. Trace: {e.StackTrace}");
            }            

            // Succesful status code 204 - No Content
            return NoContent();
        }

        //Helper method for sending 409 Conflict if Guid already exist
        private bool SensorDatasExists(Guid id)
        {
            return _context.SensorDatas.Any(e => e.Id == id);
        }
    }
}