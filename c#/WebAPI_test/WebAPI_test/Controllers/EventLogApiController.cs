using System;
using System.Collections.Generic;
using System.Diagnostics;  //使用EventLog要加入的
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI_test.Controllers
{
    public class EventLogApiController : ApiController
    {
        // GET: api/EventLogApi
        //沒裝 OData時用 IEnumerable  最後 return result.Take(10);
        //有裝時就可改成IQueryable  改成return result.AsQueryable().Take(10);
        [Queryable]
        public IQueryable<EventLogDTO> Get()
        {
            //這裡面就是你要做的事  EventLog
            EventLog eventLog = new EventLog();
            eventLog.Log = "application";

            var result = from EventLogEntry entry in eventLog.Entries
                         select new EventLogDTO{
                             Source = entry.Source,
                             UserName = entry.UserName,
                             Message = entry.Message,
                             MachineName = entry.MachineName,
                             InstanceId = entry.InstanceId,
                             Category = entry.Category
                         };

            return result.AsQueryable().Take(10); //拿個10筆回來               


        }

        // GET: api/EventLogApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/EventLogApi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/EventLogApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/EventLogApi/5
        public void Delete(int id)
        {
        }
    }
}
