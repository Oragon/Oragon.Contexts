using Newtonsoft.Json.Serialization;
using NHibernate.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Contexts.NHibernate
{
	public class NHibernateContractResolver : DefaultContractResolver
	{
		protected override List<MemberInfo> GetSerializableMembers(Type objectType)
		{
			if (typeof(INHibernateProxy).IsAssignableFrom(objectType))
			{
				return base.GetSerializableMembers(objectType.BaseType);
			} else
			{
				return base.GetSerializableMembers(objectType);
			}
		}
	}
}
