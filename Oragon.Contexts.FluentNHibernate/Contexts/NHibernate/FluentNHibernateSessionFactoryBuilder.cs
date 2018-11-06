using FluentNHibernate.Cfg.Db;
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

            Func<NH.Cfg.Configuration, NH.Cfg.Configuration> configureWithRawProperties = (config) => {
                if (this.NHibernateRawConfigurationValues != null && this.NHibernateRawConfigurationValues.Count > 0)
                {
                    foreach (var rawConfigurationValue in this.NHibernateRawConfigurationValues)
                    {
                        config.SetProperty(rawConfigurationValue.Key, rawConfigurationValue.Value);
                    }
                }
                return config;
            };


            


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
                )
                .ExposeConfiguration(it => {
                    configureWithRawProperties(
                        it
                        .SetProperty("command_timeout", this.CommandTimeout.ToString(CultureInfo.InvariantCulture))
                        .SetProperty("adonet.batch_size", this.BatchSize.ToString(CultureInfo.InvariantCulture))
                    );

                    if (this.LoadEventListeners != null && this.LoadEventListeners.Any()) it.EventListeners.LoadEventListeners = this.LoadEventListeners;
                    if (this.SaveOrUpdateEventListeners != null && this.SaveOrUpdateEventListeners.Any()) it.EventListeners.SaveOrUpdateEventListeners = this.SaveOrUpdateEventListeners;
                    if (this.MergeEventListeners != null && this.MergeEventListeners.Any()) it.EventListeners.MergeEventListeners = this.MergeEventListeners;
                    if (this.PersistEventListeners != null && this.PersistEventListeners.Any()) it.EventListeners.PersistEventListeners = this.PersistEventListeners;
                    if (this.PersistOnFlushEventListeners != null && this.PersistOnFlushEventListeners.Any()) it.EventListeners.PersistOnFlushEventListeners = this.PersistOnFlushEventListeners;
                    if (this.ReplicateEventListeners != null && this.ReplicateEventListeners.Any()) it.EventListeners.ReplicateEventListeners = this.ReplicateEventListeners;
                    if (this.DeleteEventListeners != null && this.DeleteEventListeners.Any()) it.EventListeners.DeleteEventListeners = this.DeleteEventListeners;
                    if (this.AutoFlushEventListeners != null && this.AutoFlushEventListeners.Any()) it.EventListeners.AutoFlushEventListeners = this.AutoFlushEventListeners;
                    if (this.DirtyCheckEventListeners != null && this.DirtyCheckEventListeners.Any()) it.EventListeners.DirtyCheckEventListeners = this.DirtyCheckEventListeners;
                    if (this.FlushEventListeners != null && this.FlushEventListeners.Any()) it.EventListeners.FlushEventListeners = this.FlushEventListeners;
                    if (this.EvictEventListeners != null && this.EvictEventListeners.Any()) it.EventListeners.EvictEventListeners = this.EvictEventListeners;
                    if (this.LockEventListeners != null && this.LockEventListeners.Any()) it.EventListeners.LockEventListeners = this.LockEventListeners;
                    if (this.RefreshEventListeners != null && this.RefreshEventListeners.Any()) it.EventListeners.RefreshEventListeners = this.RefreshEventListeners;
                    if (this.FlushEntityEventListeners != null && this.FlushEntityEventListeners.Any()) it.EventListeners.FlushEntityEventListeners = this.FlushEntityEventListeners;
                    if (this.InitializeCollectionEventListeners != null && this.InitializeCollectionEventListeners.Any()) it.EventListeners.InitializeCollectionEventListeners = this.InitializeCollectionEventListeners;
                    if (this.PostLoadEventListeners != null && this.PostLoadEventListeners.Any()) it.EventListeners.PostLoadEventListeners = this.PostLoadEventListeners;
                    if (this.PreLoadEventListeners != null && this.PreLoadEventListeners.Any()) it.EventListeners.PreLoadEventListeners = this.PreLoadEventListeners;
                    if (this.PreDeleteEventListeners != null && this.PreDeleteEventListeners.Any()) it.EventListeners.PreDeleteEventListeners = this.PreDeleteEventListeners;
                    if (this.PreUpdateEventListeners != null && this.PreUpdateEventListeners.Any()) it.EventListeners.PreUpdateEventListeners = this.PreUpdateEventListeners;
                    if (this.PreInsertEventListeners != null && this.PreInsertEventListeners.Any()) it.EventListeners.PreInsertEventListeners = this.PreInsertEventListeners;
                    if (this.PostDeleteEventListeners != null && this.PostDeleteEventListeners.Any()) it.EventListeners.PostDeleteEventListeners = this.PostDeleteEventListeners;
                    if (this.PostUpdateEventListeners != null && this.PostUpdateEventListeners.Any()) it.EventListeners.PostUpdateEventListeners = this.PostUpdateEventListeners;
                    if (this.PostInsertEventListeners != null && this.PostInsertEventListeners.Any()) it.EventListeners.PostInsertEventListeners = this.PostInsertEventListeners;
                    if (this.PostCommitDeleteEventListeners != null && this.PostCommitDeleteEventListeners.Any()) it.EventListeners.PostCommitDeleteEventListeners = this.PostCommitDeleteEventListeners;
                    if (this.PostCommitUpdateEventListeners != null && this.PostCommitUpdateEventListeners.Any()) it.EventListeners.PostCommitUpdateEventListeners = this.PostCommitUpdateEventListeners;
                    if (this.PostCommitInsertEventListeners != null && this.PostCommitInsertEventListeners.Any()) it.EventListeners.PostCommitInsertEventListeners = this.PostCommitInsertEventListeners;
                    if (this.SaveEventListeners != null && this.SaveEventListeners.Any()) it.EventListeners.SaveEventListeners = this.SaveEventListeners;
                    if (this.UpdateEventListeners != null && this.UpdateEventListeners.Any()) it.EventListeners.UpdateEventListeners = this.UpdateEventListeners;
                    if (this.PreCollectionRecreateEventListeners != null && this.PreCollectionRecreateEventListeners.Any()) it.EventListeners.PreCollectionRecreateEventListeners = this.PreCollectionRecreateEventListeners;
                    if (this.PostCollectionRecreateEventListeners != null && this.PostCollectionRecreateEventListeners.Any()) it.EventListeners.PostCollectionRecreateEventListeners = this.PostCollectionRecreateEventListeners;
                    if (this.PreCollectionRemoveEventListeners != null && this.PreCollectionRemoveEventListeners.Any()) it.EventListeners.PreCollectionRemoveEventListeners = this.PreCollectionRemoveEventListeners;
                    if (this.PostCollectionRemoveEventListeners != null && this.PostCollectionRemoveEventListeners.Any()) it.EventListeners.PostCollectionRemoveEventListeners = this.PostCollectionRemoveEventListeners;
                    if (this.PreCollectionUpdateEventListeners != null && this.PreCollectionUpdateEventListeners.Any()) it.EventListeners.PreCollectionUpdateEventListeners = this.PreCollectionUpdateEventListeners;
                    if (this.PostCollectionUpdateEventListeners != null && this.PostCollectionUpdateEventListeners.Any()) it.EventListeners.PostCollectionUpdateEventListeners = this.PostCollectionUpdateEventListeners;



                });

            foreach (string typeName in this.TypeNames)
            {
                Type typeInfo = Type.GetType(typeName);
                if (typeInfo == null)
                    throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Cannot load the Type '{0}', defined in TypeNames property of FluentNHibernateSessionFactoryBuilder", typeName));
                configuration.Mappings(it =>
                {
                    it.FluentMappings.AddFromAssembly(typeInfo.Assembly);
                    it.HbmMappings.AddFromAssembly(typeInfo.Assembly);
                });
            }

            NH.ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory;
        }

        protected abstract FluentNH.Cfg.Db.IPersistenceConfigurer BuildPersistenceConfigurer();

        #endregion Protected Methods

    }
}