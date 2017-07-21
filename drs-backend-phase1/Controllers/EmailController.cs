using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using drs_backend_phase1.Models;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    /// Email Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    //[HMACAuthentication]
    [RoutePrefix("api/email")]
    public class EmailController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private readonly DRSEntities _db = new DRSEntities();

        // GET: api/Email
        /// <summary>
        /// Gets the emails.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Email> GetEmails()
        {
            return _db.Emails;
        }

        // GET: api/Email/5
        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
       [Authorize(Roles = "PERSONNEL")]
        [ResponseType(typeof(Email))]
        public IHttpActionResult GetEmail(int id)
        {
            Email email = _db.Emails.Find(id);
            if (email == null)
            {
                return NotFound();
            }

            return Ok(email);
        }

        // PUT: api/Email/5
        /// <summary>
        /// Puts the email.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
       [Authorize(Roles = "PERSONNEL")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmail(int id, Email email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != email.id)
            {
                return BadRequest();
            }

            _db.Entry(email).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Email
        /// <summary>
        /// Posts the email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
       [Authorize(Roles = "PERSONNEL")]
        [ResponseType(typeof(Email))]
        public IHttpActionResult PostEmail(Email email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Emails.Add(email);
            _db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new {email.id }, email);
        }

        // DELETE: api/Email/5
        /// <summary>
        /// Deletes the email.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
       [Authorize(Roles = "PERSONNEL")]
        [ResponseType(typeof(Email))]
        public IHttpActionResult DeleteEmail(int id)
        {
            Email email = _db.Emails.Find(id);
            if (email == null)
            {
                return NotFound();
            }

            _db.Emails.Remove(email);
            _db.SaveChanges();

            return Ok(email);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Emails the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool EmailExists(int id)
        {
            return _db.Emails.Count(e => e.id == id) > 0;
        }
    }
}