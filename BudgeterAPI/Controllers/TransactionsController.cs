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
using System.Data.Entity.SqlServer;

namespace BudgeterAPI.Controllers
{
    public class TransactionsController : ApiController
    {

        // GET: api/Transactions
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IQueryable<vm_Transaction> GetTransactions()
        {
            Entities db = new Entities(); 
            string userid = RequestContext.Principal.Identity.GetUserId();

            var results = db.Transactions.Where(p => p.UserID == userid)
                .Select(p => new vm_Transaction
                {
                    ID = p.ID,
                    Name = p.Name,
                    TypeID = p.TypeID,
                    UserID = p.UserID,
                    TypeDescription = p.Transaction_types.Description,
                    TransactionValues = p.TransactionValues.OrderBy(t => t.Start_date)
                    .Select(t => new vm_TransactionValue
                    {
                        ID = t.Id,
                        TransactionID = t.TransactionID,
                        Start_date = t.Start_date,
                        End_date = t.End_date,
                        FrequencyID = t.FrequencyID,
                        FrequencyDescription = t.Transaction_frequencies.Description,
                        Day = t.Day,
                        Value = t.Value
                    }).ToList()
                });

            return results;
        }
                

        // GET: API/TRANSACTIONS/5 
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> GetTransactions(int id)
        {
            
            using (var db = new Entities())
            {
                Transaction payments_deductions = await db.Transactions.FindAsync(id);
            
                if (payments_deductions == null)
                {
                    return NotFound();
                }

                return Ok(payments_deductions);
            }
        }

        // PUT: API/TRANSACTIONS/5 
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTransactions(int id, Transaction Transaction)
        {
            Entities db = new Entities();

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

        // POST: api/Transactions
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> PostTransactions(Transaction Trans)
        {

            String Userid = RequestContext.Principal.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            Transaction t = new Transaction {
                Name = Trans.Name,
                TypeID = Trans.TypeID,
                UserID = Userid
            };

            var _tv = Trans.TransactionValues.First();

            TransactionValue tv = new TransactionValue
            {
                Start_date = _tv.Start_date,
                End_date = _tv.End_date,
                Value = _tv.Value,
                Day = _tv.Day,
                Transactions = t,
                FrequencyID = _tv.FrequencyID
            };

            using (Entities db = new Entities())
            {

                    t.TransactionValues.Add(tv);
                    db.Transactions.Add(t);
                    
                    await db.SaveChangesAsync();

                    vm_TransactionValue v = new vm_TransactionValue
                        {
                            ID = tv.Id,
                            Day = tv.Day,
                            Start_date = tv.Start_date,
                            End_date = tv.End_date,
                            FrequencyID = tv.FrequencyID,
                            TransactionID = t.ID,
                            Value = tv.Value
                        };

                    var returnModel = new vm_Transaction
                        {
                            ID = t.ID,
                            Name = t.Name,
                            TypeID = t.TypeID,
                            UserID = t.UserID,
                        };

                    try
                    {
                        returnModel.TransactionValues.Add(v);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("FaILURE");
                    }

                    
                    return CreatedAtRoute("DefaultApi", new { id = t.ID }, returnModel);
            };
        }

        // DELETE: api/Payments_deductions/5
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> DeleteTransactions(int id)
        {

            using (Entities db = new Entities()) {

                Transaction Transaction = await db.Transactions.FindAsync(id);
                if (Transaction == null)
                {
                    return NotFound();
                }

                db.Transactions.Remove(Transaction);
                await db.SaveChangesAsync();

                return Ok(Transaction);
            }
        }


        //Transaction Existence Check 
        private bool TransactionExists(int id)
        {
            using (Entities db = new Entities()) { 
            return db.Transactions.Count(e => e.ID == id) > 0;
            };
        }
    }
}
