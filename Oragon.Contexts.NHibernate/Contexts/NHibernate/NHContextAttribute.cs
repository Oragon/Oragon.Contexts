using System;

namespace Oragon.Contexts.NHibernate
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public sealed class NHContextAttribute : AbstractContextAttribute
	{
		#region Public Constructors

		public NHContextAttribute(string contextKey, bool isTransactional)
		{
			this.ContextKey = contextKey;
			this.IsTransactional = isTransactional;
		}

		#endregion Public Constructors

		#region Public Properties

		public bool? IsTransactional { get; set; }

		#endregion Public Properties

		#region Internal Properties

		internal ISessionFactoryBuilder SessionFactoryBuilder { get; set; }

		#endregion Internal Properties
	}
}