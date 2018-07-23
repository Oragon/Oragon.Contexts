using NHibernate;
using System;

namespace Oragon.Extensions
{
	public static partial class OragonExtensions
	{
		#region Public Methods

		public static IQuery SetAnsiString(this IQuery query, string name, string val)
		{
			if (val != null)
			{
				return query.SetAnsiString(name, val);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.AnsiString);
			}
		}

		public static IQuery SetBinary(this IQuery query, string name, byte[] val)
		{
			if (val != null)
			{
				return query.SetBinary(name, val);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Binary);
			}
		}

		public static IQuery SetBoolean(this IQuery query, string name, bool? val)
		{
			if (val.HasValue)
			{
				return query.SetBoolean(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Boolean);
			}
		}

		public static IQuery SetByte(this IQuery query, string name, byte? val)
		{
			if (val.HasValue)
			{
				return query.SetByte(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Byte);
			}
		}

		public static IQuery SetCharacter(this IQuery query, string name, char? val)
		{
			if (val.HasValue)
			{
				return query.SetCharacter(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Character);
			}
		}

		public static IQuery SetDateTime(this IQuery query, string name, DateTime? val)
		{
			if (val.HasValue)
			{
				return query.SetDateTime(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.DateTime);
			}
		}

		public static IQuery SetDateTime2(this IQuery query, string name, DateTime? val)
		{
			if (val.HasValue)
			{
				return query.SetDateTime2(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.DateTime2);
			}
		}

		public static IQuery SetDateTimeOffset(this IQuery query, string name, DateTimeOffset? val)
		{
			if (val.HasValue)
			{
				return query.SetDateTimeOffset(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.DateTimeOffset);
			}
		}

		public static IQuery SetDecimal(this IQuery query, string name, decimal? val)
		{
			if (val.HasValue)
			{
				return query.SetDecimal(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Decimal);
			}
		}

		public static IQuery SetDouble(this IQuery query, string name, double? val)
		{
			if (val.HasValue)
			{
				return query.SetDouble(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Double);
			}
		}

		public static IQuery SetGuid(this IQuery query, string name, Guid? val)
		{
			if (val.HasValue)
			{
				return query.SetGuid(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Guid);
			}
		}

		public static IQuery SetInt16(this IQuery query, string name, short? val)
		{
			if (val.HasValue)
			{
				return query.SetInt16(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Int16);
			}
		}

		public static IQuery SetInt32(this IQuery query, string name, int? val)
		{
			if (val.HasValue)
			{
				return query.SetInt32(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Int32);
			}
		}

		public static IQuery SetInt64(this IQuery query, string name, long? val)
		{
			if (val.HasValue)
			{
				return query.SetInt64(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Int64);
			}
		}

		public static IQuery SetSingle(this IQuery query, string name, float? val)
		{
			if (val.HasValue)
			{
				return query.SetSingle(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Single);
			}
		}

		public static IQuery SetTime(this IQuery query, string name, DateTime? val)
		{
			if (val.HasValue)
			{
				return query.SetTime(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Time);
			}
		}

		public static IQuery SetTimeAsTimeSpan(this IQuery query, string name, TimeSpan? val)
		{
			if (val.HasValue)
			{
				return query.SetTimeAsTimeSpan(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.TimeAsTimeSpan);
			}
		}

		public static IQuery SetTimeSpan(this IQuery query, string name, TimeSpan? val)
		{
			if (val.HasValue)
			{
				return query.SetTimeSpan(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.TimeSpan);
			}
		}

		public static IQuery SetTimestamp(this IQuery query, string name, DateTime? val)
		{
			if (val.HasValue)
			{
				return query.SetTimestamp(name, val.Value);
			}
			else
			{
				return query.SetParameter(name, null, NHibernateUtil.Timestamp);
			}
		}

		#endregion Public Methods
	}
}