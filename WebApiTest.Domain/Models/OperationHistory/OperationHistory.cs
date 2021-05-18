using System;

namespace WebApiTest.Domain.Models
{
    public partial class OperationHistory
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
