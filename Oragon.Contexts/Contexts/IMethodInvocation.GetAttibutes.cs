using AopAlliance.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oragon.Contexts
{
	public static partial class OragonExtensions
	{
		#region Public Methods

		public static T[] GetAttibutes<T>(this IMethodInvocation invocation, Func<T, bool> predicate = null)
			where T : Attribute
		{
			if (predicate == null)
				predicate = (it => true);

            //Recupera os atributos do método
            T[] returnValue = invocation.Method.GetCustomAttributes(typeof(T), true)
            .Cast<T>()
            .Where(predicate)
            .ToArray();
			return returnValue;
		}

		#endregion Public Methods
	}
}