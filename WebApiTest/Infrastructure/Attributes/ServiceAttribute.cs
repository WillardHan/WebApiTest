﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebApiTest.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;
        public ServiceAttribute(ServiceLifetime serviceLifetime)
        {
            Lifetime = serviceLifetime;
        }
    }
}
