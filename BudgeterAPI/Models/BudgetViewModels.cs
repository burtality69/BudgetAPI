using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgeterAPI.Models
{
  
    public class vm_Transaction
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public byte TypeID { get; set; }
        public string TypeDescription { get; set; }
        public string UserID { get; set; }
        public IEnumerable<vm_TransactionValue> TransactionValues { get; set; } 
    }

    public class vm_TransactionValue 
    {
        public int ID { get; set; }
        public int TransactionID { get; set; }
        public double Value { get; set; }
        public byte FrequencyID { get; set; }
        public string FrequencyDescription { get; set; }
        public Nullable<byte> Day { get; set; }
        public System.DateTime Start_date { get; set; }
        public Nullable<System.DateTime> End_date { get; set; }
    }

    public class Forecast_viewmodel
    {
        public DateTime? caldate { get; set; }
        public string payment_details { get; set; }
        public decimal? total_payments { get; set; }
        public string deduction_details { get; set; }
        public decimal? total_deductions { get; set; }
        public string savings_details {get; set;}
        public decimal? total_savings { get; set; }
        public decimal balance { get; set; }
        public decimal savings { get; set; }
    }
}