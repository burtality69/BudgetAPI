using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Data.Entity;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using BudgeterAPI.Models;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Text;


namespace BudgeterAPI.Controllers
{
    public class ForecastController : ApiController 
    {
        private BudgeterEntities db = new BudgeterEntities();

        public List<Forecast_viewmodel> GetForecast(DateTime startdate, DateTime enddate,double startbal)
        {
            //Get the data 
            IEnumerable<Forecast_viewmodel> result = from p in db.getforecast(startdate, enddate)
                         select new Forecast_viewmodel
                         {
                             caldate = p.caldate,
                             total_payments = p.Total_payments,
                             payment_details = ParsePaydetails(p.payments_detail),
                             total_deductions = p.Total_deductions,
                             deduction_details = ParsePaydetails(p.deductions_detail),
                         };

            //Create a running total 
            List<Forecast_viewmodel> result2 = result.ToList();
            var sum = startbal;
            foreach (var p in result2)
            {
                sum += ((p.total_payments ?? 0) + ( p.total_deductions ?? 0));
                p.balance = sum;
            }

            return result2;
        }

        private string ParsePaydetails(string xmldoc = default(string))
        {

            if (xmldoc == "<details></details>") { return string.Empty; } //Short circuit

            XElement result = XElement.Parse(xmldoc);

            string csv = (from el in result.Descendants("transaction")
                          select
                             String.Format("{0}:{1}",
                                 (string)el.Element("name").Value,
                                 (string)el.Element("amount").Value,
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