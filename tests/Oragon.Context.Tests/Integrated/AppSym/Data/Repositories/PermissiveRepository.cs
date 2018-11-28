using Oragon.Context.Tests.Integrated.AppSym.Domain;
using Oragon.Contexts.NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Context.Tests.Integrated.AppSym.Data.Repositories
{
    public class PermissiveRepository: NHDataProcess
    {
        public NHibernate.ISession GetSession() => this.ObjectContext.Session;
    }
}
