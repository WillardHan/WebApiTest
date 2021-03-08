using AutoMapper;
using System;
using System.Reflection;

namespace WebApi.Infrastructure.Utility
{
	public static class ExceptionExtension
	{
		public static Exception GetLastInnerException(this Exception e)
		{
			while (e.InnerException != null) e = e.InnerException;
			return e;
		}
	}
}
