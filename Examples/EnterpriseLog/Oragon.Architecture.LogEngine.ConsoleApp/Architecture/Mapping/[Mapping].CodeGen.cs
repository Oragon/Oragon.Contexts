 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Oragon.Architecture.LogEngine.Business.Entity;
using FluentNHibernate.Mapping;

namespace Oragon.Architecture.LogEngine.Architecture.Mapping
{
	
	public partial class AlunoMapper : ClassMap<Oragon.Architecture.LogEngine.Business.Entity.Aluno>
	{

		partial void CompleteMappings();

		public AlunoMapper()
		{
			Table("[Aluno]");
			OptimisticLock.None();
			DynamicUpdate();
			Id(it => it.IdAluno, "[IdAluno]").GeneratedBy.Identity();
			HasManyToMany(x => x.Turmas)
				.ParentKeyColumns.Add("[IdAluno]")
				.Table("[AlunoTurma]")
				.ChildKeyColumns.Add("[IdTurma]")
				.LazyLoad()
				.Fetch.Select()
				.AsBag();
			Map(it => it.Nome, "[Nome]").Not.Nullable().CustomSqlType("nvarchar(300)").Length(300);
			Map(it => it.Descricao, "[Descricao]").Nullable().CustomSqlType("nvarchar(MAX)").Length(1073741823);
			this.CompleteMappings();
		}
		
	}
	
	public partial class TipoTurmaMapper : ClassMap<Oragon.Architecture.LogEngine.Business.Entity.TipoTurma>
	{

		partial void CompleteMappings();

		public TipoTurmaMapper()
		{
			Table("[TipoTurma]");
			OptimisticLock.None();
			DynamicUpdate();
			Id(it => it.IdTipoTurma, "[IdTipoTurma]").GeneratedBy.Assigned();
			HasMany(x => x.Turmas)
				.KeyColumns.Add("[TipoTurma]")
				.ForeignKeyConstraintName("[FK_Turma_TipoTurma]")
				.Inverse()
				.Cascade.Delete()				
				.LazyLoad()
				.Fetch.Select()
				.AsBag();
			Map(it => it.Nome, "[Nome]").Not.Nullable().CustomSqlType("nvarchar(300)").Length(300);
			Map(it => it.Descricao, "[Descricao]").Nullable().CustomSqlType("nvarchar(MAX)").Length(1073741823);
			this.CompleteMappings();
		}
		
	}
	
	public partial class TurmaMapper : ClassMap<Oragon.Architecture.LogEngine.Business.Entity.Turma>
	{

		partial void CompleteMappings();

		public TurmaMapper()
		{
			Table("[Turma]");
			OptimisticLock.None();
			DynamicUpdate();
			Id(it => it.IdTurma, "[IdTurma]").GeneratedBy.Identity();
			HasManyToMany(x => x.Alunos)
				.ParentKeyColumns.Add("[IdTurma]")
				.Table("[AlunoTurma]")
				.ChildKeyColumns.Add("[IdAluno]")
				.LazyLoad()
				.Fetch.Select()
				.AsBag();
			References(x => x.TipoTurma)
				.ForeignKey("[FK_Turma_TipoTurma]")
				.Columns("[TipoTurma]")
				.Fetch.Join()				
				.Cascade.None();
			Map(it => it.Nome, "[Nome]").Not.Nullable().CustomSqlType("nvarchar(300)").Length(300);
			Map(it => it.Descricao, "[Descricao]").Nullable().CustomSqlType("nvarchar(MAX)").Length(1073741823);
			this.CompleteMappings();
		}
		
	}
	
}
 




