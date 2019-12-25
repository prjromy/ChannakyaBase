using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.ViewModel
{
    public class TaskVerificationList
    {
        public TaskVerificationList()
        {
            this.VerifierList = new List<TaskVerificationList>();
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public bool IsMultiVerifier { get; set; }
        public bool IsSelected { get; set; }
        public string Message { get; set; }
        public bool IsExceedAmount { get; set; }
        public int EventId { get; set; }
        public List<TaskVerificationList> VerifierList { get; set; }
       // public List<Task> TaskList { get; set; }
    }
    public class TaskViewModel
    {
        
        public int Task1Id { get; set; }
        public System.DateTime RaisedOn { get; set; }
        public Nullable<int> RaisedBy { get; set; }
        public int EventId { get; set; }
        public Nullable<Int64> EventValue { get; set; }
        public string Message { get; set; }
        public int TaskTo { get; set; }
        public Nullable<System.DateTime> VerifiedOn { get; set; }
        public Nullable<System.DateTime> SeenOn { get; set; }
        public Nullable<System.DateTime> RejectedOn { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public DateTime vdate { get; set; }
        public int CountOfTask { get; set; }
        public bool IsVerified { get; set; }
        public IPagedList<TaskViewModel> TaskDetailWithIPageList { get; set; }
        public List<TaskViewModel> TaskDetailList { get; set; }
        public int TotalCount { get; set; }
        public string Verifier { get; set; }
        public bool IsRejected { get; set; }
        public string TaskMessage { get; set; }



    }
    public class TaskDetailsMOdel
    {
        public int Task2Id { get; set; }
        public int Task1Id { get; set; }
        public int TaskTo { get; set; }
        public DateTime VerifiedOn { get; set; }
        public virtual Task Task { get; set; }
    }
    
    public class MessageInfoViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set;}
        public int Id { get; set; }
        public int EventValue { get; set; }
        public byte Idtype { get; set; }
        public int OpenFormEventid { get; set; }
        [Required]
        [Display(Name ="Event")]
        public byte Eventid { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime Fdate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime Tdate { get; set; }
        [Required(ErrorMessage ="Message is Required")]
        public string Mdesc { get; set; }
        public IPagedList<MessageInfoViewModel> MessageWithIPageList { get; set; }
        public List<MessageInfoViewModel> MessageInfoList { get; set;}
    }
    
}
