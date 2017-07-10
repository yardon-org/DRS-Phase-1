using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Models;
using log4net;

namespace drs_backend_phase1.Controllers
{
    [RoutePrefix("api/lookup")]
    public class LookupController : ApiController
    {
        private readonly DRSEntities _db;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    
        public LookupController()
        {
            _db = new DRSEntities();
        }

        [HttpPost]
        public IHttpActionResult CreateLookup(Lookup newLookup)
        {
            if (newLookup != null)
            {
                try
                {
                    _db.Lookups.Add(newLookup);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error creating new Lookup. The reason is as follows: {ex.Message} {ex.StackTrace}\n");
                    return BadRequest($"Error creating new Lookup. The reason is as follows: {ex.Message}");

                }

                Log.DebugFormat("The new Lookup record has been created successfully.\n");
                return Ok();

            }

            Log.DebugFormat(
                $"Error creating new Lookup. Lookup cannot be null\n");
            return BadRequest($"Error creating new Lookup. Lookup cannot be null");
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult FetchAllLookups()
        {   
            Log.DebugFormat("LookupController (FetchAllLookups)\n");

            try
            {
                var listOfJobTypes = _db.Lookups.OrderBy(x=>x.name).ToList();
                Log.DebugFormat("Retrieval of Lookups was successful.\n");
                return Ok(listOfJobTypes);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving Lookups. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving Lookups. The reason is as follows: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{bytypename}")]
        public IHttpActionResult FetchLookupByTypeName(string typename)
        {
            Log.DebugFormat("LookupController (FetchLookupByTypeName)\n");

            try
            {
                var lookupList = _db.Lookups.Where(x=>x.LookupType.name==typename).ToList();
                Log.DebugFormat("Retrieval of FetchLookupByTypeName was successful.\n");
                return Ok(lookupList);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving FetchLookupByTypeName. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchLookupByTypeName. The reason is as follows: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult FetchLookupById(int id)
        {
            Log.DebugFormat("LookupController (FetchLookupById)\n");

            try
            {
                var jobType = _db.Lookups.SingleOrDefault(x => x.id == id);
                Log.DebugFormat("Retrieval of FetchLookupById was successful.\n");
                return Ok(jobType);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving FetchLookupById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchLookupById. The reason is as follows: {ex.Message}");
            }
        }


        [HttpPut]
        public IHttpActionResult UpdateLookup(Lookup lookupToUpdate)
        {
            Log.DebugFormat("LookupController (UpdateLookup)\n");

            if (lookupToUpdate != null)
            {
                try
                {
                    _db.Lookups.AddOrUpdate(lookupToUpdate);
                    _db.SaveChanges();

                    Log.DebugFormat("Retrieval of UpdateLookup was successful.\n");
                    return Ok(lookupToUpdate);
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error retrieving UpdateLookup. The reason is as follows: {ex.Message} {ex.StackTrace}");
                    return BadRequest($"Error retrieving UpdateLookup. The reason is as follows: {ex.Message}");
                }
            }

            Log.DebugFormat(
                $"Error updating Lookup. Lookup cannot be null\n");
            return BadRequest($"Error creating new Lookup. Lookup cannot be null");

        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteLookupById(int id)
        {
            Log.DebugFormat("LookupController (DeleteLookupById)\n");

            try
            {
                var jobType = _db.Lookups.SingleOrDefault(x => x.id == id);

                if (jobType != null)
                {
                    _db.Lookups.Remove(jobType);
                    _db.SaveChanges();
                }

                Log.DebugFormat("Retrieval of DeleteLookupById was successful.\n");
                return Ok(jobType);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving DeleteLookupById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving DeleteLookupById. The reason is as follows: {ex.Message}");
            }
        }
    }
}
