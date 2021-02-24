using System;

namespace WebApiTest.Application.Models
{
    public class CompanyRequest
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class CompanyModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
