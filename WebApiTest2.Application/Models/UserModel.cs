using System;

namespace WebApiTest2.Application.Models
{
    public class UserRequest
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class UserModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
