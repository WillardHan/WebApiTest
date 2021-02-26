using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTest.Domain.Models
{
    public partial class Department
    {
        public Department()
        {

        }

        public Department(string code, string name)
        {
            this.Code = code;
            this.Name = name;
        }

        public void Update(string code, string name)
        {
            this.Code = code;
            this.Name = name;
        }
    }
}
