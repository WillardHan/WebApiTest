using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Application.Models
{
    public class RedisStoreModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class MongoStoreModel
    {
        public string Content { get; set; }
    }
}
