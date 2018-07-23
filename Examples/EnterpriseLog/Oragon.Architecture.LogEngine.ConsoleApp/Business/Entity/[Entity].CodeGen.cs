using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oragon.Architecture.LogEngine.Business.Entity
{

	public class EntityBase: Oragon.Business.Entity
	{
	
	}



	/// <summary>
	/// Classe Aluno.
	/// </summary>
	[Serializable]
	[DataContract(IsReference=true)]
	public partial class Aluno: EntityBase
	{
		#region "Propriedades"

		
		/// <summary>
		/// Define ou obtém um(a) Turmas da Aluno.
		/// </summary>
		[DataMember]
		public virtual IList<Turma> Turmas { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) IdAluno da Aluno.
		/// </summary>
		/// <remarks>Referencia Coluna Aluno.IdAluno int</remarks>
		[DataMember]
		public virtual int IdAluno { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Nome da Aluno.
		/// </summary>
		/// <remarks>Referencia Coluna Aluno.Nome nvarchar(300)</remarks>
		[DataMember]
		public virtual string Nome { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Descricao da Aluno.
		/// </summary>
		/// <remarks>Referencia Coluna Aluno.Descricao nvarchar(MAX)</remarks>
		[DataMember]
		public virtual string Descricao { get; set; }

		#endregion
		

		#region Equals/GetHashCode 


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Aluno))
				return false;
			if (Object.ReferenceEquals(this, obj))
				return true;
			Aluno objTyped = (Aluno)obj;
			bool returnValue = ((this.IdAluno.Equals(objTyped.IdAluno)));
			return returnValue;
		}

		public override int GetHashCode()
		{
			return (this.IdAluno.GetHashCode());
		}

		#endregion		

	}
	/// <summary>
	/// Classe TipoTurma.
	/// </summary>
	[Serializable]
	[DataContract(IsReference=true)]
	public partial class TipoTurma: EntityBase
	{
		#region "Propriedades"

		
		/// <summary>
		/// Define ou obtém um(a) Turmas da TipoTurma.
		/// </summary>
		[DataMember]
		public virtual IList<Turma> Turmas { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) IdTipoTurma da TipoTurma.
		/// </summary>
		/// <remarks>Referencia Coluna TipoTurma.IdTipoTurma int</remarks>
		[DataMember]
		public virtual int IdTipoTurma { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Nome da TipoTurma.
		/// </summary>
		/// <remarks>Referencia Coluna TipoTurma.Nome nvarchar(300)</remarks>
		[DataMember]
		public virtual string Nome { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Descricao da TipoTurma.
		/// </summary>
		/// <remarks>Referencia Coluna TipoTurma.Descricao nvarchar(MAX)</remarks>
		[DataMember]
		public virtual string Descricao { get; set; }

		#endregion
		

		#region Equals/GetHashCode 


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is TipoTurma))
				return false;
			if (Object.ReferenceEquals(this, obj))
				return true;
			TipoTurma objTyped = (TipoTurma)obj;
			bool returnValue = ((this.IdTipoTurma.Equals(objTyped.IdTipoTurma)));
			return returnValue;
		}

		public override int GetHashCode()
		{
			return (this.IdTipoTurma.GetHashCode());
		}

		#endregion		

	}
	/// <summary>
	/// Classe Turma.
	/// </summary>
	[Serializable]
	[DataContract(IsReference=true)]
	public partial class Turma: EntityBase
	{
		#region "Propriedades"

		
		/// <summary>
		/// Define ou obtém um(a) Alunos da Turma.
		/// </summary>
		[DataMember]
		public virtual IList<Aluno> Alunos { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) IdTurma da Turma.
		/// </summary>
		/// <remarks>Referencia Coluna Turma.IdTurma int</remarks>
		[DataMember]
		public virtual int IdTurma { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Nome da Turma.
		/// </summary>
		/// <remarks>Referencia Coluna Turma.Nome nvarchar(300)</remarks>
		[DataMember]
		public virtual string Nome { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Descricao da Turma.
		/// </summary>
		/// <remarks>Referencia Coluna Turma.Descricao nvarchar(MAX)</remarks>
		[DataMember]
		public virtual string Descricao { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) TipoTurma da Turma.
		/// </summary>
		[DataMember]
		public virtual TipoTurma TipoTurma { get; set; }

		#endregion
		

		#region Equals/GetHashCode 


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Turma))
				return false;
			if (Object.ReferenceEquals(this, obj))
				return true;
			Turma objTyped = (Turma)obj;
			bool returnValue = ((this.IdTurma.Equals(objTyped.IdTurma)));
			return returnValue;
		}

		public override int GetHashCode()
		{
			return (this.IdTurma.GetHashCode());
		}

		#endregion		

	}
	
}
 
