using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgeterAPI.Models
{
  
    public class vm_Transaction
    {
        public vm_Transaction() 
        {
            this.TransactionValues = new List<vm_TransactionValue>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public byte TypeID { get; set; }
        public string TypeDescription { get; set; }
        public string UserID { get; set; }
        public List<vm_TransactionValue> TransactionValues { get; set; }

        public static implicit operator vm_Transaction(Transaction t)

        {
            var vm = new vm_Transaction
            {
                ID = t.ID,
                Name = t.Name,
                TypeID = t.TypeID,
                UserID = t.UserID
            };

            return vm; 
        }
    }

    public class vm_TransactionValue 
    {
        public int ID { get; set; }
        public int TransactionID { get; set; }
        public double Value { get; set; }
        public byte FrequencyID { get; set; }
        public string FrequencyDescription { get; set; }
        public Nullable<byte> Day { get; set; }
        public DateTime Start_date { get; set; }
        public Nullable<DateTime> End_date { get; set; }

        public static implicit operator vm_TransactionValue(TransactionValue tv)
        {
            var vm = new vm_TransactionValue
            {
                Start_date = tv.Start_date,
                FrequencyID = tv.FrequencyID,
                TransactionID = tv.TransactionID,
                Day = tv.Day,
                End_date = tv.End_date,
                ID = tv.Id,
                Value = tv.Value
            };

            return vm;
        }
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

    public class Budget_viewmodel
    {
        public Nullable<System.DateTime> Month { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Amount { get; set; }
    }
}