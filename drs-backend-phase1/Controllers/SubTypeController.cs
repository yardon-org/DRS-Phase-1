﻿using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Models;
using log4net;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    /// SubType Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/sub-type")]
    public class SubTypeController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private readonly DRSEntities _db;
        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Initializes a new instance of the <see cref="SubTypeController"/> class.
        /// </summary>
        public SubTypeController()
        {
            _db = new DRSEntities();
        }

        /// <summary>
        /// Creates a new SubType.
        /// </summary>
        /// <param name="newSubType">New type of the sub.</param>
        /// <returns>HttpActionResult</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateSubType([FromBody]SubType newSubType)
        {
            if (newSubType != null)
            {
                try
                {
                    _db.SubTypes.Add(newSubType);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error creating new SubType. The reason is as follows: {ex.Message} {ex.StackTrace}\n");
                    return BadRequest($"Error creating new SubType. The reason is as follows: {ex.Message}");

                }

                Log.DebugFormat("The new SubType record has been created successfully.\n");
                return Ok(true);

            }

            Log.DebugFormat(
                $"Error creating new SubType. SubType cannot be null\n");
            return BadRequest($"Error creating new SubType. SubType cannot be null");
        }

        /// <summary>
        /// Fetches all SubTypes.
        /// </summary>
        /// <returns>A list of SubTypes</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult FetchAllSubTypes()
        {   
            Log.DebugFormat("SubTypeController (ReadAllSubTypes)\n");

            try
            {
                var listOfSubTypes = _db.SubTypes.OrderBy(x=>x.name).ToList();
                Log.DebugFormat("Retrieval of SubTypes was successful.\n");
                return Ok(listOfSubTypes);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving SubTypes. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving SubTypes. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetches a SubType by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A SubType object</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult FetchSubTypeById(int id)
        {
            Log.DebugFormat("SubTypeController (ReadAllSubTypeById)\n");

            try
            {
                var subType = _db.SubTypes.SingleOrDefault(x => x.id==id);
                Log.DebugFormat("Retrieval of ReadAllSubTypeById was successful.\n");
                return Ok(subType);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving ReadAllSubTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving ReadAllSubTypeById. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a SubType.
        /// </summary>
        /// <param name="subTypeToUpdate">The sub type to update.</param>
        /// <returns>HttpActionResult</returns>
        [HttpPut]
        [Route("")]
        public IHttpActionResult UpdateSubType(SubType subTypeToUpdate)
        {
            Log.DebugFormat("SubTypeController (UpdateSubType)\n");

            if (subTypeToUpdate != null)
            {
                try
                {
                    _db.SubTypes.AddOrUpdate(subTypeToUpdate);
                    _db.SaveChanges();

                    Log.DebugFormat("Retrieval of UpdateSubType was successful.\n");
                    return Ok(true);
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error retrieving UpdateSubType. The reason is as follows: {ex.Message} {ex.StackTrace}");
                    return BadRequest($"Error retrieving UpdateSubType. The reason is as follows: {ex.Message}");
                }
            }

            Log.DebugFormat(
                $"Error updating SubType. SubType cannot be null\n");
            return BadRequest($"Error creating new SubType. SubType cannot be null");

        }

        /// <summary>
        /// Deletes a SubType by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HttpActionResult</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteSubTypeById(int id)
        {
            Log.DebugFormat("SubTypeController (DeleteSubTypeById)\n");

            try
            {
                var subType = _db.SubTypes.SingleOrDefault(x => x.id == id);

                if (subType != null)
                {
                    _db.SubTypes.Remove(subType);
                    _db.SaveChanges();
                }

                Log.DebugFormat("Retrieval of DeleteSubTypeById was successful.\n");
                return Ok(true);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving DeleteSubTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving DeleteSubTypeById. The reason is as follows: {ex.Message}");
            }
        }
    }
}