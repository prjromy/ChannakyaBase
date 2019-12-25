using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.Models
{
    public class ReturnBaseMessageModel
    {
        public ReturnBaseMessageModel()
        {
            Success = false;
            Msg = "Error";
        }
        public bool Success { get; set; }
        public string Msg { get; set; }
        public int ReturnId { get; set; }
        public decimal ValueOne { get; set; }
        public string TransactionType { get; set; }
        public string Value { get; set; }
        public bool BoolValue { get; set; }     
    }

    public class ReturnCheckInterestParam
    {
        public ReturnCheckInterestParam()
        {
            result = false;

        }

        public bool result { get; set; }
        public decimal OldInterestRate { get; set; }
    }

    public class DropdownModel
    {
        public string Text { get; set; }
        public  int Id { get; set; }
    }
    public class ReturnSingleValueModdel
    {
        public DateTime? DateValue { get; set; }
        public int IdValue { get; set; }
        public byte ByteValue { get; set; }
        public decimal AmountValue { get; set; }
    }

    public class CommonLoanWithdrawModel
    {
        public bool IsRevolving { get; set; }
        public bool TellerLimit { get; set; }
        public bool HasCustomisedSchedule { get; set; }
        public bool IsBoolValue { get; set; }
    }

    public class DropdownModelForSchemeName
    {
        public byte SDID { get; set; }
        public string SchemeName { get; set; }
       
    }
}
