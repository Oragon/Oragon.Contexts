using Oragon.Contexts.NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Architecture.Data.Process
{
    public class PersistenceDataProcessBase<T>: NHPersistenceDataProcess<T>
        where T : Oragon.Business.Entity
    {
    }
}
