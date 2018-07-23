using Oragon.Configuration;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Oragon.Contexts.NHibernate
{
    //TODO: Falta suporte a DB2
    public class NHConfigFileConnectionStringDiscoverer : IConfigurationResolver
    {
        #region Public Properties

        public string FileName { get; set; }

        #endregion Public Properties

        #region Public Methods

        public string GetConfiguration()
        {
            return BuildDic()["connection.connection_string"];
        }

        #endregion Public Methods

        #region Private Methods

        private Dictionary<string, string> BuildDic()
        {
            Dictionary<string, string> valueDic = new Dictionary<string, string>();
            string nameSpace = "urn:nhibernate-configuration-2.2";
            XDocument xDoc = XDocument.Load(FileName);
            XElement hibernateConfiguration = xDoc.Element(XName.Get("hibernate-configuration", nameSpace));
            XElement sessionFactory = hibernateConfiguration.Element(XName.Get("session-factory", nameSpace));
            IEnumerable<XElement> propertyList = sessionFactory.Elements(XName.Get("property", nameSpace));
            foreach (XElement currentProperty in propertyList)
            {
                valueDic.Add(currentProperty.Attribute("name").Value, currentProperty.Value);
            }

            return valueDic;
        }

        private string GetProviderName(string driver_class)
        {
            if (driver_class.ToLower() == "nhibernate.driver.sqlclientdriver")
            {
                return "System.Data.SqlClient";
            }
            else if (driver_class.ToLower() == "nhibernate.driver.mysqldatadriver")
            {
                return "MySql.Data.MySqlClient";
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Private Methods
    }
}