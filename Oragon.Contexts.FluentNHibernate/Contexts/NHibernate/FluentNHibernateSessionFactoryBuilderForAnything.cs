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
            

            TThisConfiguration configSqlClient = this.BaseConfiguration
                               .MaxFetchDepth(this.MaxFetchDepth)
                               .IsolationLevel(this.DefaultIsolationLevel);

            if (this.ConnectionStringDiscoverer != null)
            {
                configSqlClient = configSqlClient.ConnectionString(this.ConnectionStringDiscoverer.GetConfiguration());
            }

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
