using FluentNHibernate.Cfg.Db;
using System;
using System.Configuration;
using System.Globalization;
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
                .ExposeConfiguration(it =>
                    configureWithRawProperties(
                        it
                        .SetProperty("command_timeout", this.CommandTimeout.ToString(CultureInfo.InvariantCulture))
                        .SetProperty("adonet.batch_size", this.BatchSize.ToString(CultureInfo.InvariantCulture))
                    )
                );

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