using NHibernate.Linq;
using NH = NHibernate;

namespace Oragon.Contexts.NHibernate
{
	public abstract class NHDataProcess : AbstractDataProcess<NHContext, NHContextAttribute>
	{
		#region Public Methods

		/// <summary>
		///     Reanexa um objeto
		/// </summary>
		/// <param name="itemToAttach"></param>
		public virtual void Attach(Business.Entity itemToAttach)
		{
			this.ObjectContext.Session.Refresh(itemToAttach, NH.LockMode.None);
		}

		public virtual void Flush()
		{
			this.ObjectContext.Session.Flush();
		}

		public virtual bool IsAttached(object itemToAttach)
		{
			bool returnValue = this.ObjectContext.Session.Contains(itemToAttach);
			return returnValue;
		}

		#endregion Public Methods

		#region Protected Methods

		/// <summary>
		///     Obtém IQueryOver pronto para realizar consultas usando lambda expressions
		/// </summary>
		/// <returns></returns>
		protected virtual System.Linq.IQueryable<T> Query<T>()
			where T : Business.Entity
        {
			System.Linq.IQueryable<T> query = this.ObjectContext.Session.Query<T>();
			return query;
		}

		/// <summary>
		///     Obtém IQueryOver (ICriteria API) para que possa ser utilizado em consultas com lambda expressions
		/// </summary>
		/// <returns></returns>
		protected virtual NH.IQueryOver<T, T> QueryOver<T>()
			where T : Business.Entity
        {
			NH.IQueryOver<T, T> query = this.ObjectContext.Session.QueryOver<T>();
			return query;
		}

		#endregion Protected Methods
	}
}