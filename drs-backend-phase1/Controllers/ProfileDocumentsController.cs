using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Results;
using drs_backend_phase1.Extensions;
using drs_backend_phase1.Models;
using drs_backend_phase1.Models.DTOs;
using log4net;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/filemanager")]
    public class ProfileDocumentsController : ApiController
    {
        /// <summary>
        ///     The log
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     The database
        /// </summary>
        private readonly DRSEntities db = new DRSEntities();

        #region Get_Endpoints

        /// <summary>
        /// Fetches the list of all files by profile identifier.
        /// </summary>
        /// <param name="profileId">ProfileId</param>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchProfileDocMetaByProfId")]
        public IHttpActionResult FetchListOfProfileDocMetaByProfileId(int profileId, bool includeDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("ProfileDocumentsController (FetchListOfAllFilesByProfileId)\n");
            try
            {
                var profs = db.ProfileDocuments
                    .Where(p => p.isDeleted == false || includeDeleted && p.isDeleted)
                    .Where(p=>p.profileId==profileId)
                    .OrderBy(x => x.id)
                    .ToPagedList(page, pageSize).ToMappedPagedList<ProfileDocument, ProfileDocumentDTO>();

                return Ok(new { metaData = profs.GetMetaData(), items = profs });

            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving ProfileDocuments. The reason is as follows: {ex.Message} {ex.StackTrace}");

                var myError = new Error
                {
                    Code = "400",
                    Message = "Error retrieving ProfileDocuments",
                    Data = new object[] { ex.Message, ex.StackTrace }
                };
                return new ErrorResult(myError, Request);
            }
        }

        /// <summary>
        /// Fetches a file by profile document identifier.
        /// </summary>
        /// <param name="profileDocumentId">The profile document identifier.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchFileByProfileDocId")]
        public IHttpActionResult FetchFileByProfileDocId([FromUri]int profileDocumentId)
        {
            var profileDoc = db.ProfileDocuments.SingleOrDefault(x => x.profileId == profileDocumentId);

            if (profileDoc==null)
            {
                Log.DebugFormat($"File not found.");

                var myError = new Error
                {
                    Code = "400",
                    Message = "File not found",
                    Data = null
                };
                return new ErrorResult(myError, Request);
            }

            try
            {
                if (profileDoc.documentData == null)
                {
                    Log.DebugFormat("No file data found in the database.");

                    var myError = new Error
                    {
                        Code = "400",
                        Message = "No file data found in the database",
                        Data = null
                    };

                    return new ErrorResult(myError, Request);
                }

                var fileBytes = profileDoc.documentData;

                fileBytes =Decompress(fileBytes);


                MemoryStream stream = new MemoryStream(fileBytes);
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(stream)
                };
                httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = profileDoc.originalFileName
                };


                // Change mediatype by file extension
                switch (profileDoc.mimeType)
                {
                    case "txt":
                        httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                        break;

                    case "pdf":
                        httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                        break;

                    case "doc":
                    case "docx":
                        httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/msword");
                        break;
                }

                ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
                return responseMessageResult;
            }
            catch (IOException ex)
            {
                Log.DebugFormat($"Error retrieving ProfileDocument. The reason is as follows: {ex.Message} {ex.StackTrace}");
                var myError = new Error
                {
                    Code = "400",
                    Message = "Error retrieving ProfileDocument",
                    Data = new object[] { ex.Message, ex.StackTrace }
                };
                return new ErrorResult(myError, Request);
            }
        }
        #endregion

        #region Post_Endpoints

        /// <summary>
        ///     Posts the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="profileId">The profile identifier.</param>
        /// <param name="docmentTypeId">DocumentTypeId</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(string filename, int profileId, int docmentTypeId)
        {
            var fileObj = new ProfileDocument();
            var thisProfile = db.Profiles.SingleOrDefault(x => x.id == profileId);

            if (thisProfile == null)
            {
                Log.DebugFormat(
                    "Error retrieving Profile. The profile cannot be found");

                var myError = new Error
                {
                    Code = "400",
                    Message = "Error retrieving Profile. The profile cannot be found",
                    Data = null
                };

                return new ErrorResult(myError, Request);
            }

            // Check for valid file types
            var extension = Path.GetExtension(filename.Trim());
            if (!string.IsNullOrEmpty(extension))
            {
                extension = extension.TrimStart('.'); //remove full stop
                var tagList = new List<string> {"pdf", "doc", "docx", "txt"};
                var exists = tagList.Any(val => val.Contains(extension.ToLower()));

                if (exists)
                {
                    fileObj.mimeType = extension.ToLower();
                    fileObj.originalFileName = filename.ToLower().Trim();
                }
            }
            else
            {
                Log.DebugFormat("Missing or disallowed file extension");
                var myError = new Error
                {
                    Code = "400",
                    Message = "Missing or disallowed file extension",
                    Data = null
                };
                return new ErrorResult(myError, Request);
            }

            // Read data here
            var task = Request.Content.ReadAsStreamAsync();
            task.Wait();
            var requestStream = task.Result;

            try
            {
                var byteArray = ReadFully(requestStream);
                var compress = Compress(byteArray);

                fileObj.documentData = compress;
                fileObj.dateObtained = DateTime.Now;
                fileObj.profileId = profileId;
                fileObj.documentTypeId = docmentTypeId;

                db.ProfileDocuments.Add(fileObj);
                db.SaveChanges();
            }
            catch (IOException ex)
            {
                Log.DebugFormat(
                    $"Error storing ProfileDocument. The reason is as follows: {ex.Message} {ex.StackTrace}");
                var myError = new Error
                {
                    Code = "400",
                    Message = "Error storing ProfileDocument",
                    Data = new object[] {ex.Message, ex.StackTrace}
                };
                return new ErrorResult(myError, Request);
            }

            return Ok();
        }

        #endregion

        #region Delete_Endpoints

        /// <summary>
        ///     Deletes a ProfileDocument by ProfileDocumentId.
        /// </summary>
        /// <param name="productDocumentId"></param>
        /// <returns>HttpActionResult</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpDelete]
        [Route("{id}")]
        public virtual IHttpActionResult DeleteProfileDocumentById(int productDocumentId)
        {
            Log.DebugFormat("ProfileDocController (DeleteProfileDocumentById)\n");

            try
            {
                var profileDoc = db.ProfileDocuments.SingleOrDefault(x => x.id == productDocumentId);

                if (profileDoc != null)
                {
                    profileDoc.isDeleted = true;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        var myError = new Error
                        {
                            Code = "400",
                            Message = "The entity being updated has already been updated by another user...",
                            Data = null
                        };
                        return new ErrorResult(myError, Request);
                    }
                }

                Log.DebugFormat("Retrieval of DeleteProfileDocumentById was successful.\n");
                return Ok(true);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving DeleteProfileDocumentById. The reason is as follows: {ex.Message} {ex.StackTrace}");

                var myError = new Error
                {
                    Code = "400",
                    Message = "Error retrieving DeleteProfileDocumentById",
                    Data = new object[] { ex.Message, ex.StackTrace }
                };
                return new ErrorResult(myError, Request);
            }
        }

        #endregion

        #region Disposing

        /// <summary>
        ///     Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region Stream_Utilities

        /// <summary>
        ///     Reads the fully.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     Compresses the specified raw.
        /// </summary>
        /// <param name="raw">The raw.</param>
        /// <returns></returns>
        private byte[] Compress(byte[] raw)
        {
            using (var memory = new MemoryStream())
            {
                using (var gzip = new GZipStream(memory,
                    CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }

        static byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip),CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    } while (count > 0);
                    return memory.ToArray();
                }
            }
        }


        #endregion
        }
}