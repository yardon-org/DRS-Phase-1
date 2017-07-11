﻿using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Models;
using log4net;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    /// Lookup Controller
    /// </summary>
    /// <seealso cref="ApiController" />
    [RoutePrefix("api/lookup")]
    public class LookupController : ApiController
    {
        private readonly DRSEntities _db;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Initializes a new instance of the <see cref="LookupController"/> class.
        /// </summary>
        public LookupController()
        {
            _db = new DRSEntities();
        }

        /// <summary>
        /// Adds a new Lookup object to the database.
        /// </summary>
        /// <param name="newLookup">The new Lookup object.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateLookup([FromBody]Lookup newLookup)
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

        /// <summary>
        /// Fetches all Lookup objects in the database.
        /// </summary>
        /// <returns>A list of Lookup objects</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult FetchAllLookups()
        {   
            Log.DebugFormat("LookupController (FetchAllLookups)\n");

            try
            {
                var listOfJobTypes = _db.Lookups.OrderBy(x=>x.name)
                    .Include(x=>x.LookupType)
                    .ToList();
                Log.DebugFormat("Retrieval of Lookups was successful.\n");
                return Ok(listOfJobTypes);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving Lookups. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving Lookups. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetches a list of Lookup objects based on their typename.
        /// </summary>
        /// <param name="typename">The typename.</param>
        /// <returns>A list of Lookup objects</returns>
        [HttpGet]
        [Route("bytypename")]
        public IHttpActionResult FetchLookupByTypeName(string typename)
        {
            Log.DebugFormat("LookupController (FetchLookupByTypeName)\n");

            try
            {
                var lookupList = _db.Lookups
                    .Include(x => x.LookupType)
                    .Where(x => x.LookupType.name == typename)
                    .ToList();
                Log.DebugFormat("Retrieval of FetchLookupByTypeName was successful.\n");
                return Ok(lookupList);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving FetchLookupByTypeName. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchLookupByTypeName. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetches a Lookup object by identifier.
        /// </summary>
        /// <param name="id">Lookup identifier.</param>
        /// <returns>A Lookup object</returns>
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


        /// <summary>
        /// Updates a Lookup object in the database.
        /// </summary>
        /// <param name="lookupToUpdate">The updated Lookup object.</param>
        /// <returns>HttpActionResult</returns>
        [HttpPut]
        [Route("")]
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
                    return Ok(true);
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

        /// <summary>
        /// Deletes a Lookup by identifier.
        /// </summary>
        /// <param name="id">The Lookup identifier.</param>
        /// <returns>HttpActionResult</returns>
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
                return Ok(true);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving DeleteLookupById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving DeleteLookupById. The reason is as follows: {ex.Message}");
            }
        }
    }
}