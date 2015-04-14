using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BudgeterAPI.Models;

namespace BudgeterAPI.Controllers
{
    [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    public class TransactionsController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/Payments_deductions
        public IQueryable<vm_Transaction> GetTransactions()
        {

            string userid = RequestContext.Principal.Identity.GetUserId();

            var results = from p in db.Transactions
                          where p.UserID == userid
                          select new vm_Transaction
                          {
                              ID = p.ID,
                              Name = p.Name,
                              TypeID = p.TypeID,
                              UserID = p.UserID,
                              TypeDescription = p.Transaction_types.Description,
                              TransactionValues = from t in p.TransactionValues.OrderBy(t => t.Start_date)
                                                  select new vm_TransactionValue 
                                                  {
                                                      ID = t.Id,
                                                      TransactionID = t.TransactionID,
                                                      Start_date = t.Start_date,
                                                      End_date = t.End_date,
                                                      FrequencyID = t.FrequencyID, 
                                                      FrequencyDescription = t.Transaction_frequencies.Description,
                                                      Day = t.Day,
                                                      Value = t.Value 
                                                  }
                          };

            return results; 
                
        }

        // GET: api/Transactions/5
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> GetTransactions(int id)
        {
            Transaction payments_deductions = await db.Transactions.FindAsync(id);
            if (payments_deductions == null)
            {
                return NotFound();
            }

            return Ok(payments_deductions);
        }

        // PUT: api/Transactions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTransactions(int id, Transaction Transaction)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Transaction.ID)
            {
                return BadRequest();
            }

            db.Entry(Transaction).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST: api/Payments_deductions
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> PostTransactions(Transaction Transaction)
        {
            if (Transaction.UserID == null)
            {
                string userid = RequestContext.Principal.Identity.GetUserId();
                Transaction.UserID = userid;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transactions.Add(Transaction);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = Transaction.ID }, Transaction);
        }

        // DELETE: api/Payments_deductions/5
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> DeleteTransactions(int id)
        {
            Transaction Transaction = await db.Transactions.FindAsync(id);
            if (Transaction == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(Transaction);
            await db.SaveChangesAsync();

            return Ok(Transaction);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(int id)
        {
            return db.Transactions.Count(e => e.ID == id) > 0;
        }
    }
}
