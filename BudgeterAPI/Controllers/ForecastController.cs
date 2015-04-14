using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Xml.Linq;
using System.Data.Entity;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using BudgeterAPI.Models;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Text;


namespace BudgeterAPI.Controllers
{
    public class ForecastController : ApiController 
    {
        private Entities db = new Entities();

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public List<Forecast_viewmodel> GetForecast(DateTime startdate, DateTime enddate,decimal startbal)
        {
            //Get the data 

            string userid = RequestContext.Principal.Identity.GetUserId();

            IEnumerable<Forecast_viewmodel> result = from p in db.getforecast(startdate, enddate,userid)
                         select new Forecast_viewmodel
                         {
                             caldate = p.CALDATE,
                             total_payments = p.TOTAL_PAYMENTS,
                             payment_details = ParsePaydetails(p.PAYMENTS_DETAIL),
                             total_deductions = p.TOTAL_DEDUCTIONS,
                             deduction_details = ParsePaydetails(p.DEDUCTIONS_DETAIL),
                             total_savings = p.TOTAL_SAVINGS,
                             savings_details = p.SAVINGS_DETAIL

                         };

            //Create a running total 
            List<Forecast_viewmodel> result2 = result.ToList();
            decimal sum = startbal;
            decimal sav = 0; 

            foreach (var p in result2)
            {
                sum += ((p.total_payments ?? 0) + (p.total_deductions ?? 0) + (p.total_savings ?? 0));
                p.balance = sum;

                sav += (p.total_savings * -1) ?? 0;
                p.total_savings = sav;
            }

            return result2;
        }

        private string ParsePaydetails(string xmldoc = default(string))
        {

            if (xmldoc == "<details></details>" || xmldoc ==null) { return string.Empty; } //Short circuit

            XElement result = XElement.Parse(xmldoc);

            string csv = (from el in result.Elements("Transaction")
                          select
                             String.Format("{0}:{1}",
                                 (string)el.Element("DESCRIPT").Value,
                                 (string)el.Element("Amount").Value,
                                 "\n") 
                        ).Aggregate(
                        new StringBuilder(),
                        (sb, s) => sb.Append(s),
                        sb => sb.ToString()
                            );

            return csv;

        }

}
}