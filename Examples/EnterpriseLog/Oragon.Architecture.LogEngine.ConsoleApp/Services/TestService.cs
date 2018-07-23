using Oragon.Architecture.LogEngine.Business.Entity;
using Oragon.Architecture.LogEngine.Data.Process;
using Oragon.Contexts.NHibernate;

namespace Oragon.Architecture.LogEngine.ConsoleApp.Services
{
    public class TestService : ITestService
    {
        internal PersistenceDataProcess WriteDataProcess { get; set; }

        internal AlunoDataProcess AlunoDataProcess { get; set; }


        [NHContext("APP_DB_PRINCIPAL_CONTEXT", true)]
        public void Test()
        {
            //this.WriteDataProcess.Save(new Aluno() { Nome = "Luiz Carlos Faria", Descricao = "Tete" });

            //this.WriteDataProcess.SaveOrUpdate(new Aluno() { IdAluno=1, Nome = "Luiz Carlos Faria", Descricao = "Teste" });

            //this.WriteDataProcess.Delete(new Aluno() { IdAluno = 1, Nome = "a" });
        }
    }
}
