using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Models;
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
        private static byte[] Compress(byte[] raw)
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

        #endregion
    }
}