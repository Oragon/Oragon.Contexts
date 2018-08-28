using AopAlliance.Intercept;
using Oragon.Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using NH = NHibernate;

namespace Oragon.Contexts.NHibernate
{
	public class NHContextAroundAdvice : AbstractContextAroundAdvice<NHContext, NHContextAttribute>
	{
		#region Dependence Injection

		private bool ElevateToSystemTransactionsIfRequired { get; set; }

		private NH.IInterceptor Interceptor { get; set; }

		[Required]
		private List<ISessionFactoryBuilder> SessionFactoryBuilders { get; set; }

		#endregion Dependence Injection

		#region Protected Properties

		protected override Func<NHContextAttribute, bool> AttributeQueryFilter
		{
			get
			{
				return (it =>
				{
					it.SessionFactoryBuilder = this.SessionFactoryBuilders.FirstOrDefault(sfb => sfb.ObjectContextKey == it.ContextKey);
					return (it.SessionFactoryBuilder != null);
				});
			}
		}

		protected override string ContextStackListKey
		{
			get { return "Oragon.Architecture.Aop.Data.NHContextAroundAdvice.Contexts"; }
		}

		#endregion Protected Properties

		#region Protected Methods

		protected override object Invoke(IMethodInvocation invocation, IEnumerable<NHContextAttribute> contextAttributes)
		{
			object returnValue = null;

			if (this.ElevateToSystemTransactionsIfRequired && contextAttributes.Count(it => it.IsTransactional.HasValue && it.IsTransactional.Value) > 1)
			{
				using (var scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required))
				{
					returnValue = this.InvokeUsingContext(invocation, new Stack<NHContextAttribute>(contextAttributes));
					scope.Complete();
				}
			}
			else
			{
				returnValue = this.InvokeUsingContext(invocation, new Stack<NHContextAttribute>(contextAttributes));
			}
			return returnValue;
		}

		#endregion Protected Methods

		#region Private Methods

		private object InvokeUsingContext(IMethodInvocation invocation, Stack<NHContextAttribute> contextAttributesStack)
		{
			//Este método é chamado recursivamente, removendo o item do Stack sempre que houver um. Até que não haja nenhum. Quando não houver nenhum item mais, ele efetivamente
			//manda executar a chamada ao método de destino.
			//Esse controle é necessário pois as os "Usings" de Contexto, Sessão e Transação precisam ser encadeados
			object returnValue = null;
			if (contextAttributesStack.Count == 0)
			{
				returnValue = invocation.Proceed();
			}
			else
			{
				//Obtendo o primeiro primeiro último RequiredPersistenceContextAttribute da stack, removendo-o.
				NHContextAttribute currentContextAttribute = contextAttributesStack.Pop();
				//Criando o contexto
				using (NHContext currentContext = new NHContext(currentContextAttribute, this.Interceptor, this.ContextStack))
				{
					returnValue = this.InvokeUsingContext(invocation, contextAttributesStack);
					currentContext.Complete();
				}
			}
			return returnValue;
		}

		#endregion Private Methods
	}
}