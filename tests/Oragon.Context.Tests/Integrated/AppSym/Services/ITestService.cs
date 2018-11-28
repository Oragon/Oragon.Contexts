using Oragon.Context.Tests.Integrated.AppSym.Domain;
using System.Collections.Generic;

namespace Oragon.Context.Tests.Integrated.AppSym.Services
{
    public interface ITestService
    {
        void Create1();

        void Create2();

        List<DomainEntity> RetrieveAll();

        void Update(Student student);

        void Delete(DomainEntity domainEntity);
    }
}