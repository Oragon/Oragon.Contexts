using Oragon.Architecture.LogEngine.Business.Entity;
using Oragon.Architecture.LogEngine.Data.Process;
using Oragon.Contexts.NHibernate;
using System;

namespace Oragon.Architecture.LogEngine.ConsoleApp.Services
{
    public class TestService : ITestService
    {
        internal PersistenceDataProcess WriteDataProcess { get; set; }

        internal AlunoDataProcess AlunoDataProcess { get; set; }


        [NHContext("APP_DB_PRINCIPAL_CONTEXT", true)]
        public void Test()
        {
            this.WriteDataProcess.Save(new Aluno() { Nome = "Luiz Carlos Faria", Descricao = "Tete" });
        }
    }
}
