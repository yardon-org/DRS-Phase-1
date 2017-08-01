//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web;
//using System.Web.Http;
//using System.Web.Http.Description;
//using drs_backend_phase1.Models;
//using System.IO.Compression;
//using System.Reflection;
//using log4net;

//namespace drs_backend_phase1.Controllers
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <seealso cref="System.Web.Http.ApiController" />
//    public class ProfileDocumentsController : ApiController
//    {
//        /// <summary>
//        /// The log
//        /// </summary>
//        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

//        /// <summary>
//        /// The database
//        /// </summary>
//        private readonly DRSEntities db = new DRSEntities();

//        /// <summary>
//        /// Posts the specified filename.
//        /// </summary>
//        /// <param name="filename">The filename.</param>
//        /// <returns></returns>
//        public IHttpActionResult Post([FromUri]string filename)
//        {
//            var fileObj = new ProfileDocument();

//            //.pdf,.doc,.docx,.txt
//            string extension = Path.GetExtension(filename.Trim());
//            if (!string.IsNullOrEmpty(extension))
//            {
//                var tagList = new List<string> {"pdf","doc","docx","txt"};
//                var exists = tagList.Any(val => val.Contains(extension.ToLower()));

//                if (exists)
//                {
//                    fileObj.mimeType=extension.ToLower();
//                    fileObj.originalFileName = filename.ToLower().Trim();
//                }
//            }
//            else
//            {
//                Log.DebugFormat($"Missing or disallowed file extension");
//                var myError = new Error
//                {
//                    Code = "400",
//                    Message = "Missing or disallowed file extension",
//                    Data = null
//                };
//                return new ErrorResult(myError, Request);
//            }

//            // Read data here
//            var task = this.Request.Content.ReadAsStreamAsync();
//            task.Wait();
//            Stream requestStream = task.Result;

//            try
//            {
//                var byteArray = ReadFully(requestStream);
//                byte[] compress = Compress(byteArray);


//                fileObj.
             

//            }
//            catch (IOException)
//            {
//                //throw new HttpResponseException("A generic error occured. Please try again later.", HttpStatusCode.InternalServerError);
//            }

//            HttpResponseMessage response = new HttpResponseMessage {StatusCode = HttpStatusCode.Created};
//            return response;
//        }

//        /// <summary>
//        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
//        /// </summary>
//        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        /// <summary>
//        /// Profiles the document exists.
//        /// </summary>
//        /// <param name="id">The identifier.</param>
//        /// <returns></returns>
//        private bool ProfileDocumentExists(int id)
//        {
//            return db.ProfileDocuments.Count(e => e.id == id) > 0;
//        }

//        /// <summary>
//        /// Reads the fully.
//        /// </summary>
//        /// <param name="input">The input.</param>
//        /// <returns></returns>
//        private static byte[] ReadFully(Stream input)
//        {
//            byte[] buffer = new byte[16 * 1024];
//            using (MemoryStream ms = new MemoryStream())
//            {
//                int read;
//                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
//                {
//                    ms.Write(buffer, 0, read);
//                }
//                return ms.ToArray();
//            }
//        }

//        /// <summary>
//        /// Compresses the specified raw.
//        /// </summary>
//        /// <param name="raw">The raw.</param>
//        /// <returns></returns>
//        private static byte[] Compress(byte[] raw)
//        {
//            using (MemoryStream memory = new MemoryStream())
//            {
//                using (GZipStream gzip = new GZipStream(memory,
//                    CompressionMode.Compress, true))
//                {
//                    gzip.Write(raw, 0, raw.Length);
//                }
//                return memory.ToArray();
//            }
//        }
//    }
//}
 