using FluentNHibernate.Cfg.Db;
using NHibernate.Dialect;
using NHibernate.Driver;
using System;
using System.Collections.Generic;
using System.Text;


namespace Oragon.Contexts.FluentNHibernate.ForOracleDataAccess
{
    public class OracleDataClientConfiguration : PersistenceConfiguration<OracleDataClientConfiguration, OracleConnectionStringBuilder>
    {
        protected OracleDataClientConfiguration()
        {
            Driver<OracleDataClientDriver>();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDataClientConfiguration"/> class using the
        /// Oracle Data Provider (Oracle.DataAccess) library specifying the Oracle 11g dialect. 
        /// The Oracle.DataAccess library must be available to the calling application/library. 
        /// </summary>
        public static OracleDataClientConfiguration Oracle11
        {
            get { return new OracleDataClientConfiguration().Dialect<Oracle10gDialect>(); }
        }
    }
}
