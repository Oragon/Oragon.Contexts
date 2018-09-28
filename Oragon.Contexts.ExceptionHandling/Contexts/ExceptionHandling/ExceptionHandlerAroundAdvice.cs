using AopAlliance.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oragon.Business;
using Common.Logging;
using Oragon.Logging;
using LogLevel = Oragon.Logging.LogLevel;

namespace Oragon.Contexts.ExceptionHandling
{
    public class ExceptionHandlerAroundAdvice : IMethodInterceptor
    {
        public List<Type> BusinessExceptionTypes { get; set; }

        public ILogger Logger { get; set; }

        public string GenericErrorMessage { get; set; }

        public bool EnableDebug { get; set; }

        public object Invoke(IMethodInvocation invocation)
        {
            ExceptionHandlingAttribute currentAttribute = GetAttribute(invocation);
            string targetTypeFullName = string.Concat(invocation.TargetType.Namespace, ".", invocation.TargetType.Name);
            string targetMethod = string.Concat(targetTypeFullName, ".", invocation.Method);

            object returnValue = null;
            using (LogContext logContext = new LogContext(enlist: true))
            {
                logContext.SetValue("Type", targetTypeFullName);
                logContext.SetValue("Method", targetMethod);

                try
                {
                    if (this.EnableDebug)
                        this.Logger.Log(targetTypeFullName, string.Concat("Begin ", targetMethod), LogLevel.Debug, logContext.GetDictionary());

                    returnValue = invocation.Proceed();

                    if (this.EnableDebug)
                        this.Logger.Log(targetTypeFullName, string.Concat("End ", targetMethod), LogLevel.Debug, logContext.GetDictionary());
                }
                catch (UndefinedException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Type exceptionType = ex.GetType();

                    bool isBusinessException = (this.BusinessExceptionTypes.Any(it => it.IsAssignableFrom(exceptionType)));

                    string exceptionTypeKey = isBusinessException ? "BusinessException" : "ApplicationException";

                    logContext.SetValue(exceptionTypeKey, string.Concat(exceptionType.Namespace, ".", exceptionType.Name));

                    Action<LogLevel> logException = logLevel =>
                    {
                        this.Logger.Log(targetTypeFullName, ex.ToString(), logLevel, logContext.GetDictionary());
                    };

                    if (currentAttribute.Strategy.HasFlag(ExceptionHandlingStrategy.ContinueRunning)) //Supressão da Exception - Error
                    {
                        logException(LogLevel.Error);
                    }
                    else if (currentAttribute.Strategy.HasFlag(ExceptionHandlingStrategy.BreakOnException))
                    {
                        if (isBusinessException) //BusinessException - Warn
                        {
                            logException(LogLevel.Warn);
                            throw;
                        }
                        else //Outras exceptions - Fatal
                        {
                            logException(LogLevel.Fatal);
                            throw new UndefinedException(this.GenericErrorMessage);
                        }
                    }
                }
            }
            return returnValue;
        }

        private static ExceptionHandlingAttribute GetAttribute(IMethodInvocation invocation)
        {
            ExceptionHandlingAttribute attribute = invocation.GetAttibutes<ExceptionHandlingAttribute>().FirstOrDefault();
            if (attribute == null)
            {
                attribute = new ExceptionHandlingAttribute(ExceptionHandlingStrategy.BreakOnException);
            }

            if (attribute.Strategy.HasFlag(ExceptionHandlingStrategy.ContinueRunning) && (invocation.Method.ReturnType != typeof(void)))
            {
                throw new InvalidOperationException("Somente métodos com retorno void podem usar a estratégia de supressão de exceptions.");
            }
            return attribute;
        }
    }
}