namespace WebAPI_test.Controllers
{
    public class EventLogDTO
    {
        public string Source { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string MachineName { get; set; }
        public long InstanceId { get; set; }
        public string Category { get; set; }
    }
}