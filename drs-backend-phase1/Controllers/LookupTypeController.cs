using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Models;
using log4net;

namespace drs_backend_phase1.Controllers
{
    [RoutePrefix("api/lookuptype")]
    public class LookupTypeController : ApiController
    {
        private readonly DRSEntities _db;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    
        public LookupTypeController()
        {
            _db = new DRSEntities();
        }

        [HttpPost]
        public IHttpActionResult CreateLookupType(LookupType newLookupType)
        {
            if (newLookupType != null)
            {
                try
                {
                    _db.LookupTypes.Add(newLookupType);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error creating new LookupType. The reason is as follows: {ex.Message} {ex.StackTrace}\n");
                    return BadRequest($"Error creating new LookupType. The reason is as follows: {ex.Message}");

                }

                Log.DebugFormat("The new LookupType record has been created successfully.\n");
                return Ok();

            }

            Log.DebugFormat(
                $"Error creating new LookupType. LookupType cannot be null\n");
            return BadRequest($"Error creating new LookupType. LookupType cannot be null");
        }

        [HttpGet]
        public IHttpActionResult FetchAllLookupTypes()
        {   
            Log.DebugFormat("LookupTypeController (ReadAllLookupTypes)\n");

            try
            {
                var listOfLookupTypes = _db.LookupTypes.OrderBy(x=>x.name).ToList();
                Log.DebugFormat("Retrieval of LookupTypes was successful.\n");
                return Ok(listOfLookupTypes);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving LookupTypes. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving LookupTypes. The reason is as follows: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult FetchLookupTypeById(int id)
        {
            Log.DebugFormat("LookupTypeController (FetchLookupTypeById)\n");

            try
            {
                var lookupType = _db.LookupTypes.SingleOrDefault(x => x.id==id);
                Log.DebugFormat("Retrieval of FetchLookupTypeById was successful.\n");
                return Ok(lookupType);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving FetchLookupTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchLookupTypeById. The reason is as follows: {ex.Message}");
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateLookupType(LookupType lookupTypeToUpdate)
        {
            Log.DebugFormat("LookupTypeController (UpdateLookupType)\n");

            if (lookupTypeToUpdate != null)
            {
                try
                {
                    _db.LookupTypes.AddOrUpdate(lookupTypeToUpdate);
                    _db.SaveChanges();

                    Log.DebugFormat("Retrieval of UpdateLookupType was successful.\n");
                    return Ok(lookupTypeToUpdate);
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error retrieving UpdateLookupType. The reason is as follows: {ex.Message} {ex.StackTrace}");
                    return BadRequest($"Error retrieving UpdateLookupType. The reason is as follows: {ex.Message}");
                }
            }

            Log.DebugFormat(
                $"Error updating LookupType. LookupType cannot be null\n");
            return BadRequest($"Error creating new LookupType. LookupType cannot be null");

        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteLookupTypeById(int id)
        {
            Log.DebugFormat("LookupTypeController (DeleteLookupTypeById)\n");

            try
            {
                var lookupType = _db.LookupTypes.SingleOrDefault(x => x.id == id);

                if (lookupType != null)
                {
                    _db.LookupTypes.Remove(lookupType);
                    _db.SaveChanges();
                }

                Log.DebugFormat("Retrieval of DeleteLookupTypeById was successful.\n");
                return Ok(lookupType);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving DeleteLookupTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving DeleteLookupTypeById. The reason is as follows: {ex.Message}");
            }
        }
    }
}
