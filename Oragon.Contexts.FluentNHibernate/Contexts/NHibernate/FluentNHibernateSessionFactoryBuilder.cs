using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using FluentNH = FluentNHibernate;
using NH = NHibernate;

namespace Oragon.Contexts.NHibernate
{
    /// <summary>
    ///     Responsável por inicializar a configuraçãio do NHibernate e disponibilizar um SessionFactory pra a aplicação
    /// </summary>
    public abstract class FluentNHibernateSessionFactoryBuilder : AbstractSessionFactoryBuilder
    {
        #region Public Constructors

        protected FluentNHibernateSessionFactoryBuilder()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        /// <summary>
        ///     Principal método privado, realiza a criação do SessionFactory e este não deve ser criado novamente até que o domínio de aplicação seja finalizado.
        /// </summary>
        /// <returns></returns>
        protected override NH.ISessionFactory BuildSessionFactoryInternal()
        {
            FluentNH.Cfg.Db.IPersistenceConfigurer databaseConfiguration = this.BuildPersistenceConfigurer();

            FluentNH.Cfg.FluentConfiguration configuration = FluentNH.Cfg.Fluently
                .Configure()
                .Database(databaseConfiguration)
                .Cache(it =>
                    it.UseQueryCache()
                    .ProviderClass<NH.Cache.HashtableCacheProvider>()
                )
                .Diagnostics(it =>
                    it.Enable(this.EnabledDiagnostics)
                    .OutputToConsole()
                );
                

            if (this.TypeNames == null)
            {
                throw new NullReferenceException("TypeNames is not set");
            }

            foreach (string typeName in this.TypeNames)
            {
                Type typeInfo = Type.GetType(typeName);
                if (typeInfo == null)
                {
                    throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Cannot load the Type '{0}', defined in TypeNames property of FluentNHibernateSessionFactoryBuilder", typeName));
                }

                configuration.Mappings(it =>
                {
                    it.FluentMappings.AddFromAssembly(typeInfo.Assembly);
                    it.HbmMappings.AddFromAssembly(typeInfo.Assembly);
                });
            }

            configuration = configuration.ExposeConfiguration(it =>
             {
                 NH.Cfg.Configuration config = it
                     .SetProperty("command_timeout", this.CommandTimeout.ToString(CultureInfo.InvariantCulture))
                     .SetProperty("adonet.batch_size", this.BatchSize.ToString(CultureInfo.InvariantCulture));

                 if (this.NHibernateRawConfigurationValues != null && this.NHibernateRawConfigurationValues.Count > 0)
                 {
                     foreach (System.Collections.Generic.KeyValuePair<string, string> rawConfigurationValue in this.NHibernateRawConfigurationValues)
                     {
                         config.SetProperty(rawConfigurationValue.Key, rawConfigurationValue.Value);
                     }
                 }

                 this.AddEventListeners(config);

                 if(this.OnExposeConfiguration!= null)
                    this.OnExposeConfiguration(it);
             });

            NH.ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory;
        }



        private void AddEventListeners(NH.Cfg.Configuration config)
        {
            if (this.LoadEventListeners != null && this.LoadEventListeners.Any()) config.EventListeners.LoadEventListeners = this.LoadEventListeners;
            if (this.SaveOrUpdateEventListeners != null && this.SaveOrUpdateEventListeners.Any()) config.EventListeners.SaveOrUpdateEventListeners = this.SaveOrUpdateEventListeners;
            if (this.MergeEventListeners != null && this.MergeEventListeners.Any()) config.EventListeners.MergeEventListeners = this.MergeEventListeners;
            if (this.PersistEventListeners != null && this.PersistEventListeners.Any()) config.EventListeners.PersistEventListeners = this.PersistEventListeners;
            if (this.PersistOnFlushEventListeners != null && this.PersistOnFlushEventListeners.Any()) config.EventListeners.PersistOnFlushEventListeners = this.PersistOnFlushEventListeners;
            if (this.ReplicateEventListeners != null && this.ReplicateEventListeners.Any()) config.EventListeners.ReplicateEventListeners = this.ReplicateEventListeners;
            if (this.DeleteEventListeners != null && this.DeleteEventListeners.Any()) config.EventListeners.DeleteEventListeners = this.DeleteEventListeners;
            if (this.AutoFlushEventListeners != null && this.AutoFlushEventListeners.Any()) config.EventListeners.AutoFlushEventListeners = this.AutoFlushEventListeners;
            if (this.DirtyCheckEventListeners != null && this.DirtyCheckEventListeners.Any()) config.EventListeners.DirtyCheckEventListeners = this.DirtyCheckEventListeners;
            if (this.FlushEventListeners != null && this.FlushEventListeners.Any()) config.EventListeners.FlushEventListeners = this.FlushEventListeners;
            if (this.EvictEventListeners != null && this.EvictEventListeners.Any()) config.EventListeners.EvictEventListeners = this.EvictEventListeners;
            if (this.LockEventListeners != null && this.LockEventListeners.Any()) config.EventListeners.LockEventListeners = this.LockEventListeners;
            if (this.RefreshEventListeners != null && this.RefreshEventListeners.Any()) config.EventListeners.RefreshEventListeners = this.RefreshEventListeners;
            if (this.FlushEntityEventListeners != null && this.FlushEntityEventListeners.Any()) config.EventListeners.FlushEntityEventListeners = this.FlushEntityEventListeners;
            if (this.InitializeCollectionEventListeners != null && this.InitializeCollectionEventListeners.Any()) config.EventListeners.InitializeCollectionEventListeners = this.InitializeCollectionEventListeners;
            if (this.PostLoadEventListeners != null && this.PostLoadEventListeners.Any()) config.EventListeners.PostLoadEventListeners = this.PostLoadEventListeners;
            if (this.PreLoadEventListeners != null && this.PreLoadEventListeners.Any()) config.EventListeners.PreLoadEventListeners = this.PreLoadEventListeners;
            if (this.PreDeleteEventListeners != null && this.PreDeleteEventListeners.Any()) config.EventListeners.PreDeleteEventListeners = this.PreDeleteEventListeners;
            if (this.PreUpdateEventListeners != null && this.PreUpdateEventListeners.Any()) config.EventListeners.PreUpdateEventListeners = this.PreUpdateEventListeners;
            if (this.PreInsertEventListeners != null && this.PreInsertEventListeners.Any()) config.EventListeners.PreInsertEventListeners = this.PreInsertEventListeners;
            if (this.PostDeleteEventListeners != null && this.PostDeleteEventListeners.Any()) config.EventListeners.PostDeleteEventListeners = this.PostDeleteEventListeners;
            if (this.PostUpdateEventListeners != null && this.PostUpdateEventListeners.Any()) config.EventListeners.PostUpdateEventListeners = this.PostUpdateEventListeners;
            if (this.PostInsertEventListeners != null && this.PostInsertEventListeners.Any()) config.EventListeners.PostInsertEventListeners = this.PostInsertEventListeners;
            if (this.PostCommitDeleteEventListeners != null && this.PostCommitDeleteEventListeners.Any()) config.EventListeners.PostCommitDeleteEventListeners = this.PostCommitDeleteEventListeners;
            if (this.PostCommitUpdateEventListeners != null && this.PostCommitUpdateEventListeners.Any()) config.EventListeners.PostCommitUpdateEventListeners = this.PostCommitUpdateEventListeners;
            if (this.PostCommitInsertEventListeners != null && this.PostCommitInsertEventListeners.Any()) config.EventListeners.PostCommitInsertEventListeners = this.PostCommitInsertEventListeners;
            if (this.SaveEventListeners != null && this.SaveEventListeners.Any()) config.EventListeners.SaveEventListeners = this.SaveEventListeners;
            if (this.UpdateEventListeners != null && this.UpdateEventListeners.Any()) config.EventListeners.UpdateEventListeners = this.UpdateEventListeners;
            if (this.PreCollectionRecreateEventListeners != null && this.PreCollectionRecreateEventListeners.Any()) config.EventListeners.PreCollectionRecreateEventListeners = this.PreCollectionRecreateEventListeners;
            if (this.PostCollectionRecreateEventListeners != null && this.PostCollectionRecreateEventListeners.Any()) config.EventListeners.PostCollectionRecreateEventListeners = this.PostCollectionRecreateEventListeners;
            if (this.PreCollectionRemoveEventListeners != null && this.PreCollectionRemoveEventListeners.Any()) config.EventListeners.PreCollectionRemoveEventListeners = this.PreCollectionRemoveEventListeners;
            if (this.PostCollectionRemoveEventListeners != null && this.PostCollectionRemoveEventListeners.Any()) config.EventListeners.PostCollectionRemoveEventListeners = this.PostCollectionRemoveEventListeners;
            if (this.PreCollectionUpdateEventListeners != null && this.PreCollectionUpdateEventListeners.Any()) config.EventListeners.PreCollectionUpdateEventListeners = this.PreCollectionUpdateEventListeners;
            if (this.PostCollectionUpdateEventListeners != null && this.PostCollectionUpdateEventListeners.Any()) config.EventListeners.PostCollectionUpdateEventListeners = this.PostCollectionUpdateEventListeners;
        }

        protected abstract FluentNH.Cfg.Db.IPersistenceConfigurer BuildPersistenceConfigurer();

        public Action<NH.Cfg.Configuration> OnExposeConfiguration;

        #endregion Protected Methods

    }
}