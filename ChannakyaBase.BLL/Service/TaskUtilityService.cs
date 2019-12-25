using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public static class BaseTaskUtilityService
    {
        private static BaseTaskVerificationService taskService = null;
        static BaseTaskUtilityService()
        {
            taskService = new BaseTaskVerificationService();
        }
        public static string GetTaskNameByEventId(int EventId = 0)
                {
            var taskname = taskService.GetEventByEventId(EventId).FirstOrDefault();
            return taskname.EventName.ToString();
        }
        public static string VerifiedBy(int task1Id = 0)
        {
            string verifier = taskService.VerifiedBy(task1Id).ToString();
            return verifier;
        }
        public static SelectList GetAllEventList(int? eventid = 0)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var eventList = uow.Repository<Event>().GetAll().ToList();
                if (eventid == 4)
                {
                    var deleteitem = uow.Repository<Event>().FindBy(x => x.EventId == eventid).FirstOrDefault();
                    eventList.Remove(deleteitem);
                }
                return new SelectList(eventList, "EventId", "EventName");



            }
        }


        public static int TaskIdByEventIdAndEventValue(Int64 eventId, Int64 eventVAlue)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var taskId = uow.Repository<ChannakyaBase.DAL.DatabaseModel.Task>().FindBy(x => x.EventId == eventId && x.EventValue == eventVAlue).Select(x => x.Task1Id).FirstOrDefault();
                //if()
                return taskId;
            }
        }

    }
}
