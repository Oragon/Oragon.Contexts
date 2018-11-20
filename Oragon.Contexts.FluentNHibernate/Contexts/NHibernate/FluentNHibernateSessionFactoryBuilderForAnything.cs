using FluentNHibernate.Cfg.Db;
using System;
using FluentNH = FluentNHibernate;

namespace Oragon.Contexts.NHibernate
{



    public class FluentNHibernateSessionFactoryBuilderForAnything<TThisConfiguration, TConnectionString> : FluentNHibernateSessionFactoryBuilder
            where TThisConfiguration : PersistenceConfiguration<TThisConfiguration, TConnectionString>
            where TConnectionString : ConnectionStringBuilder, new()
    {
        public virtual PersistenceConfiguration<TThisConfiguration, TConnectionString> BaseConfiguration { get; set; }

        protected override IPersistenceConfigurer BuildPersistenceConfigurer()
        {
            if (this.ConnectionStringDiscoverer == null)
            {
                throw new NullReferenceException("ConnectionStringDiscoverer is not set");
            }

            TThisConfiguration configSqlClient = this.BaseConfiguration
                               .ConnectionString(this.ConnectionStringDiscoverer.GetConfiguration())
                               .MaxFetchDepth(this.MaxFetchDepth)
                               .IsolationLevel(this.DefaultIsolationLevel);
            if (this.EnabledDiagnostics)
            {
                configSqlClient = configSqlClient.ShowSql().FormatSql();
            }

            if (!string.IsNullOrWhiteSpace(this.DefaultSchema))
            {
                configSqlClient = configSqlClient.DefaultSchema(this.DefaultSchema);
            }

            FluentNH.Cfg.Db.IPersistenceConfigurer returnValue = configSqlClient;
            return returnValue;
        }
    }
}
