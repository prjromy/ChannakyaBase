using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaBase.Model.ViewModel
{
    public class HomeTransactionsViewModel
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public short Currid { get; set; }
        public int Brhid { get; set; }
        public int FUserid { get; set; }
        public byte TType { get; set; }
        public System.DateTime TDate { get; set; }
        public string Tdesc { get; set; }
        public long TNO { get; set; }
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}",ApplyFormatInEditMode =true)]
        public decimal Dramt { get; set; }
        public decimal Cramt { get; set; }
        public Nullable<System.DateTime> AcceptedOn { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<decimal> Vno { get; set; }
        public bool UserState { get; set; }
        public Nullable<decimal> CurrentBalance { get; set; }
        public int DesignationId { get; set; }
        public string DGName { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public Nullable<decimal> MyBalance { get; set; }
        public int DepartmentId { get; set; }
        public string DeptName { get; set; }
        public int PId { get; set; }
        public int CTId { get; set; }
        public SelectList RecieverList { get; set; }
        public SelectList CurrencyList { get; set; }
        public List<CurrencyModel> AmountWithCurrency { get; set; }
        public List<HomeTransactionsViewModel> HomeTransactionsList { get; set; }
        public int operationType { get; set; }
        public string ActionType { get; set; }
        public int DegOrder { get; set; }
    }

    public class CashFlowTypeDictionary
    {
        public int Id { get; set; }
        public  string Text { get; set; }
        public int FDeg { get; set; }
        public int TDeg { get; set; }
    }
}
