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
    public class Payments_deductionsController : ApiController
    {
        private BudgeterEntities db = new BudgeterEntities();

        // GET: api/Payments_deductions
        public IQueryable<Payment_deduction_viewmodel> GetPayments_deductions()
        {
            var results = from p in db.Payments_deductions
                          select new Payment_deduction_viewmodel
                          {
                              ID = p.ID,
                              Amount = p.Amount,
                              Day = p.Day,
                              Frequency = p.frequency.Frequency_description,
                              Name = p.Name,
                              Type = p.payment_deduction_types.Type_description,
                              Start_date = p.Start_date
                          };

            return results;
                
        }

        // GET: api/Payments_deductions/5
        [ResponseType(typeof(Payments_deductions))]
        public async Task<IHttpActionResult> GetPayments_deductions(int id)
        {
            Payments_deductions payments_deductions = await db.Payments_deductions.FindAsync(id);
            if (payments_deductions == null)
            {
                return NotFound();
            }

            return Ok(payments_deductions);
        }

        // PUT: api/Payments_deductions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPayments_deductions(int id, Payments_deductions payments_deductions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payments_deductions.ID)
            {
                return BadRequest();
            }

            db.Entry(payments_deductions).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Payments_deductionsExists(id))
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
        [ResponseType(typeof(Payments_deductions))]
        public async Task<IHttpActionResult> PostPayments_deductions(Payments_deductions payments_deductions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Payments_deductions.Add(payments_deductions);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = payments_deductions.ID }, payments_deductions);
        }

        // DELETE: api/Payments_deductions/5
        [ResponseType(typeof(Payments_deductions))]
        public async Task<IHttpActionResult> DeletePayments_deductions(int id)
        {
            Payments_deductions payments_deductions = await db.Payments_deductions.FindAsync(id);
            if (payments_deductions == null)
            {
                return NotFound();
            }

            db.Payments_deductions.Remove(payments_deductions);
            await db.SaveChangesAsync();

            return Ok(payments_deductions);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Payments_deductionsExists(int id)
        {
            return db.Payments_deductions.Count(e => e.ID == id) > 0;
        }
    }
}