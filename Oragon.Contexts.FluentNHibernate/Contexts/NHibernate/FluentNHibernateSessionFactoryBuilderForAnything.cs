using FluentNHibernate.Cfg.Db;
using System;
using System.Configuration;
using System.Globalization;
using FluentNH = FluentNHibernate;
using NH = NHibernate;

namespace Oragon.Contexts.NHibernate
{



    public class FluentNHibernateSessionFactoryBuilderForAnything<TThisConfiguration, TConnectionString> : FluentNHibernateSessionFactoryBuilder
            where TThisConfiguration : PersistenceConfiguration<TThisConfiguration, TConnectionString>
            where TConnectionString : ConnectionStringBuilder, new()
    {
        public virtual PersistenceConfiguration<TThisConfiguration, TConnectionString> BaseConfiguration { get; set; }

        protected override IPersistenceConfigurer BuildPersistenceConfigurer()
        {
            var configSqlClient = this.BaseConfiguration
                               .ConnectionString(this.ConnectionStringDiscoverer.GetConfiguration())
                               .MaxFetchDepth(this.MaxFetchDepth)
                               .IsolationLevel(this.DefaultIsolationLevel);
            if (this.EnabledDiagnostics)
                configSqlClient = configSqlClient.ShowSql().FormatSql();
            FluentNH.Cfg.Db.IPersistenceConfigurer returnValue = configSqlClient;
            return returnValue;
        }
    }
}
