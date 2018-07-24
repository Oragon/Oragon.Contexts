using Oragon.Architecture.LogEngine.Business.Entity;
using Oragon.Architecture.LogEngine.Data.Process;
using Oragon.Contexts.NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Oragon.Architecture.LogEngine.ConsoleApp.Services
{
    public class TestService : ITestService
    {
        internal PersistenceDataProcess WriteDataProcess { get; set; }

        internal AlunoDataProcess AlunoDataProcess { get; set; }
        internal TipoTurmaDataProcess TipoTurmaDataProcess { get; set; }
        internal TurmaDataProcess TurmaDataProcess { get; set; }



        [NHContext("APP_DB_PRINCIPAL_CONTEXT", true)]
        public void Test1Create()
        {
            Aluno aluno1 = new Aluno() { Nome = "Luiz Carlos Faria 1", Descricao = "1" };
            Aluno aluno2 = new Aluno() { Nome = "Luiz Carlos Faria 2", Descricao = "2" };

            TipoTurma tipoTurmaManha = new TipoTurma() { IdTipoTurma = 1, Nome = "Diurna", Descricao = "1" };
            TipoTurma tipoTurmaNoite = new TipoTurma() { IdTipoTurma = 2, Nome = "Noturna", Descricao = "2" };

            Turma turma1 = new Turma() { Nome = "Turma 1", Descricao = "1", TipoTurma = tipoTurmaManha };
            Turma turma2 = new Turma() { Nome = "Turma 2", Descricao = "2", TipoTurma = tipoTurmaNoite };

            EntityBase[] entitiesToSave = new EntityBase[] { aluno1, aluno2, tipoTurmaManha, tipoTurmaNoite, turma1, turma2 };

            foreach (EntityBase entity in entitiesToSave)
            {
                this.WriteDataProcess.Save(entity);
            }

            aluno1.Turmas = new List<Turma>() { turma1 };

            turma2.Alunos = new List<Aluno>() { aluno2 };


            this.WriteDataProcess.Save(aluno1);
            this.WriteDataProcess.Save(turma2);

        }

        [NHContext("APP_DB_PRINCIPAL_CONTEXT", true)]
        public void Test2Compare()
        {
            if (this.AlunoDataProcess.GetListBy(it => it.Descricao == "1").Single().Nome != "Luiz Carlos Faria 1")
            {
                throw new System.Exception();
            }

            if (this.AlunoDataProcess.GetListBy(it => it.Descricao == "2").Single().Nome != "Luiz Carlos Faria 2")
            {
                throw new System.Exception();
            }
        }

        [NHContext("APP_DB_PRINCIPAL_CONTEXT", true)]
        public void Test3Change()
        {
            Aluno aluno1 = this.AlunoDataProcess.GetListBy(it => it.Descricao == "1").Single();

            aluno1.Nome = "Não é Luiz Carlos Faria";

            this.WriteDataProcess.Update(aluno1);
        }

        [NHContext("APP_DB_PRINCIPAL_CONTEXT", true)]
        public void Test4Compare()
        {
            if (this.AlunoDataProcess.GetListBy(it => it.Descricao == "1").Single().Nome != "Não é Luiz Carlos Faria")
            {
                throw new System.Exception();
            }
            
        }


        [NHContext("APP_DB_PRINCIPAL_CONTEXT", true)]
        public void Test5Delete()
        {
            this.AlunoDataProcess.GetListBy().ToList().ForEach(it =>
            {
                this.WriteDataProcess.Delete(it);
            });

            this.TurmaDataProcess.GetListBy().ToList().ForEach(it =>
            {
                this.WriteDataProcess.Delete(it);
            });

            this.TipoTurmaDataProcess.GetListBy().ToList().ForEach(it =>
            {
                this.WriteDataProcess.Delete(it);
            });

        }
    }
}
