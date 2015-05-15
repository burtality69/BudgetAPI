using System;
using System.Collections.Generic;
using System.Data;
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
    public class TransactionValuesController : ApiController
    {

        // GET: api/TransactionValues
        public IQueryable<vm_TransactionValue> GetTransactionValues()
        {

            using (Entities db = new Entities())
            {
                var result = db.TransactionValues.Select(tv => new vm_TransactionValue
                             {
                                 ID = tv.Id,
                                 Start_date = tv.Start_date,
                                 FrequencyID = tv.FrequencyID,
                                 FrequencyDescription = tv.Transaction_frequencies.Description,
                                 TransactionID = tv.TransactionID,
                                 Day = tv.Day,
                                 End_date = tv.End_date,
                                 Value = tv.Value
                             });

                return result;
            }
        }

        //GET: api/TransactionValuesbyTran/5 
        [ResponseType(typeof(vm_TransactionValue))]
        [HttpGet]
        [Route("api/TransactionValues/ByTran/{ID}")]
        public IQueryable<vm_TransactionValue> ByTran(int ID)
        {
            using (Entities db = new Entities())
            {
                var result = db.TransactionValues.Where(t => t.TransactionID == ID)
                             .Select(tv => new vm_TransactionValue
                             {
                                 ID = tv.Id,
                                 Start_date = tv.Start_date,
                                 FrequencyID = tv.FrequencyID,
                                 FrequencyDescription = tv.Transaction_frequencies.Description,
                                 TransactionID = tv.TransactionID,
                                 Day = tv.Day,
                                 End_date = tv.End_date,
                                 Value = tv.Value
                             });

                return result;
            };
        }

        // GET: api/TransactionValues/5

        [ResponseType(typeof(TransactionValue))]
        public async Task<IHttpActionResult> GetTransactionValue(int id)
        {
            using (Entities db = new Entities())
            {
                TransactionValue transactionValue = await db.TransactionValues.FindAsync(id);
                if (transactionValue == null)
                {
                    return NotFound();
                }

                return Ok(transactionValue);
            };
        }

        // PUT: api/TransactionValues/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTransactionValue(int id, TransactionValue transactionValue)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transactionValue.Id)
            {
                return BadRequest();
            }

            using (Entities db = new Entities())
            {
                db.Entry(transactionValue).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionValueExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return StatusCode(HttpStatusCode.NoContent);
            };
        }

        // POST: api/TransactionValues
        [ResponseType(typeof(TransactionValue))]
        public async Task<IHttpActionResult> PostTransactionValue(TransactionValue transactionValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (Entities db = new Entities())
            {
                db.TransactionValues.Add(transactionValue);
                await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = transactionValue.Id }, transactionValue);
            };
        }


        // DELETE: api/TransactionValues/5
        [ResponseType(typeof(TransactionValue))]
        public async Task<IHttpActionResult> DeleteTransactionValue(int id)
        {
            using (Entities db = new Entities())
            {

                TransactionValue transactionValue = await db.TransactionValues.Where(t => t.Id == id).SingleAsync();
                
                if (transactionValue == null)
                {
                    return NotFound();
                }

                db.TransactionValues.Remove(transactionValue);
                await db.SaveChangesAsync();

                return Ok(transactionValue);
            };
        }

        private bool TransactionValueExists(int id)
        {
            using (Entities db = new Entities())
            {
                return db.TransactionValues.Count(e => e.Id == id) > 0;
            };
        }
    }
}