using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTest.Domain.Models
{
    public partial class Computer
    {
        public Computer()
        {

        }

        public Computer(string code, string name, Guid? deparmentId)
        {
            this.Code = code;
            this.Name = name;
            this.DepartmentId = deparmentId;
        }

        public void Update(string code, string name)
        {
            this.Code = code;
            this.Name = name;
        }

        public void ChangeDepartment(Guid deparmentId)
        {
            this.DepartmentId = deparmentId;
        }

        public void ClearDepartment()
        {
            this.DepartmentId = null;
        }
    }
}
