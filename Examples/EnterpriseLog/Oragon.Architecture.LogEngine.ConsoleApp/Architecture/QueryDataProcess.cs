using Oragon.Contexts.NHibernate;

namespace Oragon.Architecture.Data.Process
{
    public class QueryDataProcess<T> : NHQueryDataProcess<T>
        where T : Oragon.Business.Entity
    {
    }
}
