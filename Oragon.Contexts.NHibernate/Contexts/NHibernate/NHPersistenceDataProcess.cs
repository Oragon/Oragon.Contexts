namespace Oragon.Contexts.NHibernate
{
	public class NHPersistenceDataProcess<T> : NHDataProcess
		where T : Business.Entity
    {
		#region Public Methods

		/// <summary>
		///     Realiza a exclusão da entidade do banco
		/// </summary>
		/// <param name="entity">Entidade a ser persistida</param>
		public virtual void Delete(T entity)
		{
			this.ObjectContext.Session.Delete(entity);
		}

		/// <summary>
		///     Realiza a operação de insert dos dados de uma entidade no banco de dados
		/// </summary>
		/// <param name="entity">Entidade a ser persistida</param>
		public virtual void Save(T entity)
		{
			this.ObjectContext.Session.Save(entity);
		}

		/// <summary>
		///     Tenta realizar um insert ou update para garantir o armazenamento do dado no banco
		/// </summary>
		/// <param name="entity">Entidade a ser persistida</param>
		public virtual void SaveOrUpdate(T entity)
		{
			this.ObjectContext.Session.SaveOrUpdate(entity);
		}

		/// <summary>
		///     Realiza uma operãção de update no banco
		/// </summary>
		/// <param name="entity">Entidade a ser persistida</param>
		public virtual void Update(T entity)
		{
			this.ObjectContext.Session.Update(entity);
		}

		#endregion Public Methods
	}
}