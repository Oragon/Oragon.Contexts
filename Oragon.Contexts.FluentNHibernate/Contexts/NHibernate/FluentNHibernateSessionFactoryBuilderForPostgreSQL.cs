using FluentNHibernate.Cfg.Db;
using System;
using System.Configuration;
using System.Globalization;
using FluentNH = FluentNHibernate;
using NH = NHibernate;


namespace Oragon.Contexts.NHibernate
{
    public class FluentNHibernateSessionFactoryBuilderForPostgreSQL : FluentNHibernateSessionFactoryBuilderForAnything<PostgreSQLConfiguration, PostgreSQLConnectionStringBuilder>
    {

        public override PersistenceConfiguration<PostgreSQLConfiguration, PostgreSQLConnectionStringBuilder> BaseConfiguration
        {
            get { return FluentNH.Cfg.Db.PostgreSQLConfiguration.PostgreSQL82; }
            set { throw new InvalidOperationException("FluentNHibernateSessionFactoryBuilderForPostgreSQL does not support setting of BaseConfiguration"); }
        }

    }
}
