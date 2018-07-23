using FluentNHibernate.Cfg.Db;
using System;
using System.Configuration;
using System.Globalization;
using FluentNH = FluentNHibernate;
using NH = NHibernate;


namespace Oragon.Contexts.NHibernate
{
    public class FluentNHibernateSessionFactoryBuilderForDB2 : FluentNHibernateSessionFactoryBuilderForAnything<DB2Configuration, DB2ConnectionStringBuilder>
    {

        public override PersistenceConfiguration<DB2Configuration, DB2ConnectionStringBuilder> BaseConfiguration
        {
            get { return FluentNH.Cfg.Db.DB2Configuration.Standard; }
            set { throw new InvalidOperationException("FluentNHibernateSessionFactoryBuilderForDB2 does not support setting of BaseConfiguration"); }
        }

    }
}
