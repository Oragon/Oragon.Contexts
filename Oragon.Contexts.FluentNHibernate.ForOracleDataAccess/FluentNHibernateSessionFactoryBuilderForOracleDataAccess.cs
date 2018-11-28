using FluentNHibernate.Cfg.Db;
using Oragon.Contexts.NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Contexts.FluentNHibernate.ForOracleDataAccess
{
    public class FluentNHibernateSessionFactoryBuilderForOracleDataAccess : FluentNHibernateSessionFactoryBuilderForAnything<OracleDataClientConfiguration, OracleConnectionStringBuilder>
    {
        public override PersistenceConfiguration<OracleDataClientConfiguration, OracleConnectionStringBuilder> BaseConfiguration
        {
            get { return OracleDataClientConfiguration.Oracle11; }
            set { throw new InvalidOperationException("OracleFluentNHibernateSessionFactoryBuilder does not support setting of BaseConfiguration"); }
        }
    }
}
