using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using WebApiTest.Domain.Models;

namespace WebApiTest.Domain.Mappings
{
    public class OperationHistoryMapping
    {
        public static void Configure()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(OperationHistory)))
            {
                BsonClassMap.RegisterClassMap<OperationHistory>(map =>
                {
                    map.MapIdProperty(m => m.Id)
                       .SetIdGenerator(CombGuidGenerator.Instance)
                       .SetIsRequired(true)
                       .SetOrder(1);

                    map.MapProperty(m => m.Content)
                       .SetIsRequired(true)
                       .SetOrder(2);

                    map.MapProperty(m => m.CreateTime)
                       .SetDefaultValue(DateTime.Now)
                       .SetIsRequired(true)
                       .SetOrder(3);
                });
            }
        }
    }
}
