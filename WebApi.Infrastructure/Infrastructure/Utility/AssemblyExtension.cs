using AutoMapper;
using System;
using System.Reflection;

namespace WebApi.Infrastructure.Utility
{
    public static class AssemblyExtension
    {
        public static Assembly[] GetAllAssemblies()
        {
            var refAssembyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            foreach (var asslembyNames in refAssembyNames)
            {
                Assembly.Load(asslembyNames);
            }
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
