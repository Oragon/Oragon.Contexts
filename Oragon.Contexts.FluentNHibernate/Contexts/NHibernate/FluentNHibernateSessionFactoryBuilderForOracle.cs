using FluentNHibernate.Cfg.Db;
using System;
using System.Configuration;
using System.Globalization;
using FluentNH = FluentNHibernate;
using NH = NHibernate;

namespace Oragon.Contexts.NHibernate
{
    public class FluentNHibernateSessionFactoryBuilderForOracle : FluentNHibernateSessionFactoryBuilderForAnything<OracleDataClientConfiguration, OracleConnectionStringBuilder>
    {

        public override PersistenceConfiguration<OracleDataClientConfiguration, OracleConnectionStringBuilder> BaseConfiguration
        {
            get { return FluentNH.Cfg.Db.OracleDataClientConfiguration.Oracle10; }
            set { throw new InvalidOperationException("FluentNHibernateSessionFactoryBuilderForOracle does not support setting of BaseConfiguration"); }
        }

    }
}

