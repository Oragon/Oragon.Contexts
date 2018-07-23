using FluentNHibernate.Cfg.Db;
using System;
using System.Configuration;
using System.Globalization;
using FluentNH = FluentNHibernate;
using NH = NHibernate;
 
namespace Oragon.Contexts.NHibernate
{
    public class FluentNHibernateSessionFactoryBuilderForSQLServer : FluentNHibernateSessionFactoryBuilderForAnything<MsSqlConfiguration, MsSqlConnectionStringBuilder>
    {

        public override PersistenceConfiguration<MsSqlConfiguration, MsSqlConnectionStringBuilder> BaseConfiguration
        {
            get { return FluentNH.Cfg.Db.MsSqlConfiguration.MsSql2012; }
            set { throw new InvalidOperationException("FluentNHibernateSessionFactoryBuilderForSQLServer does not support setting of BaseConfiguration"); }
        }

    }
}
