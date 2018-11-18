
using FluentNHibernate.Cfg.Db;
using System;
using FluentNH = FluentNHibernate;

namespace Oragon.Contexts.NHibernate
{
    public class FluentNHibernateSessionFactoryBuilderForSQLite : FluentNHibernateSessionFactoryBuilderForAnything<SQLiteConfiguration, ConnectionStringBuilder>
    {

        public bool InMemory { get; set; } = false;

        public string FileName { get; set; }

        public string Password { get; set; }

        public override PersistenceConfiguration<SQLiteConfiguration, ConnectionStringBuilder> BaseConfiguration
        {
            get
            {
                if (this.InMemory == false && (string.IsNullOrWhiteSpace(this.FileName) || (System.IO.File.Exists(this.FileName) == false)))
                {
                    throw new InvalidOperationException("FluentNHibernateSessionFactoryBuilderForSQLite require or InMemory (set property InMemory=true) or set a valid FileName ");
                }

                if (this.InMemory)
                {
                    return FluentNH.Cfg.Db.SQLiteConfiguration.Standard.InMemory();
                }
                else if (string.IsNullOrWhiteSpace(this.Password))
                {
                    return FluentNH.Cfg.Db.SQLiteConfiguration.Standard.UsingFile(this.FileName);
                }
                else
                {
                    return FluentNH.Cfg.Db.SQLiteConfiguration.Standard.UsingFileWithPassword(this.FileName, this.Password);
                }
            }
            set { throw new InvalidOperationException("FluentNHibernateSessionFactoryBuilderForSQLite does not support setting of BaseConfiguration"); }
        }

    }
}
