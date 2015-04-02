using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using BudgeterAPI.Models;

namespace BudgeterAPI.Controllers

{
    public class AdminController : ApiController
    {
        private BudgeterEntities db = new BudgeterEntities();

        //GET Frequencies 

        [HttpGet]
        [Route("api/admin/transactionfrequencies")]
        public IQueryable<Frequency_viewmodel> TransactionFrequencies()
        {
            IQueryable<Frequency_viewmodel> result = from p in db.frequencies
                                select new Frequency_viewmodel
                                {
                                    ID = p.ID,
                                    Description = p.Frequency_description
                                };
                
                return result; 
        }

        //GET TransactionTypes
        [HttpGet]
        [Route("api/admin/transactiontypes")]
        public IQueryable<PayDedType_viewmodel> TransactionTypes()
        {
            IQueryable<PayDedType_viewmodel> result = from p in db.payment_deduction_types
                                                      select new PayDedType_viewmodel
                                                     {
                                                         ID = p.ID,
                                                         Description = p.Type_description
                                                     };

            return result;
        }

    }
}
