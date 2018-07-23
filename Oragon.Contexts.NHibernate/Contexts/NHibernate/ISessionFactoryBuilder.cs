using NH = NHibernate;

namespace Oragon.Contexts.NHibernate
{
	public interface ISessionFactoryBuilder
	{
		#region Public Properties

		NH.FlushMode DefaultFlushMode { get; }

		System.Data.IsolationLevel DefaultIsolationLevel { get; }

		string ObjectContextKey { get; }

		NH.FlushMode TransactionFlushMode { get; }

		System.Data.IsolationLevel TransactionIsolationLevel { get; }

		#endregion Public Properties

		#region Public Methods

		NH.ISessionFactory BuildSessionFactory();

		#endregion Public Methods
	}
}