using Oragon.Configuration;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Oragon.Contexts.NHibernate
{
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
            const string nameSpace = "urn:nhibernate-configuration-2.2";
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



        #endregion Private Methods
    }
}