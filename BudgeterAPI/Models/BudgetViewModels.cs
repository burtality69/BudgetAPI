using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgeterAPI.Models
{
  
    public class Payment_deduction_viewmodel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type {get; set; }
        public string Frequency { get; set; }
        public byte Day { get; set; }
        public double Amount { get; set; }
        public DateTime? Start_date { get; set; }
    }

    public class Forecast_viewmodel
    {
        public DateTime? caldate { get; set; }
        public string payment_details { get; set; }
        public double? total_payments { get; set; }
        public string deduction_details { get; set; }
        public double? total_deductions { get; set; }
        public double balance { get; set; }
    }
}