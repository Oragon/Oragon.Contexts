using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Oragon.Architecture.Data.Process;


namespace Oragon.Architecture.LogEngine.Data.Process
{
	internal sealed class PersistenceDataProcess : PersistenceDataProcessBase<Oragon.Architecture.LogEngine.Business.Entity.EntityBase> { }

	internal partial class AlunoDataProcess : QueryDataProcess<Oragon.Architecture.LogEngine.Business.Entity.Aluno>
	{
		

	}
	internal partial class TipoTurmaDataProcess : QueryDataProcess<Oragon.Architecture.LogEngine.Business.Entity.TipoTurma>
	{
		

	}
	internal partial class TurmaDataProcess : QueryDataProcess<Oragon.Architecture.LogEngine.Business.Entity.Turma>
	{
		

	}
	
}

