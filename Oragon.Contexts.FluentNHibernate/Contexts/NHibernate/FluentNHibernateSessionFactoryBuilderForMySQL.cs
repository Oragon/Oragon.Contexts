using FluentNHibernate.Cfg.Db;
using System;
using System.Configuration;
using System.Globalization;
using FluentNH = FluentNHibernate;
using NH = NHibernate;


namespace Oragon.Contexts.NHibernate
{
    public class FluentNHibernateSessionFactoryBuilderForMySQL : FluentNHibernateSessionFactoryBuilderForAnything<MySQLConfiguration, MySQLConnectionStringBuilder>
    {

        public override PersistenceConfiguration<MySQLConfiguration, MySQLConnectionStringBuilder> BaseConfiguration
        {
            get { return FluentNH.Cfg.Db.MySQLConfiguration.Standard; }
            set { throw new InvalidOperationException("FluentNHibernateSessionFactoryBuilderForMySQL does not support setting of BaseConfiguration"); }
        }

    }
}
