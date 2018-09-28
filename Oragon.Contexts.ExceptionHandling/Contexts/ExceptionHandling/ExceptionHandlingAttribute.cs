using System;

namespace Oragon.Contexts.ExceptionHandling
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ExceptionHandlingAttribute : Attribute
    {
        #region Public Constructors

        public ExceptionHandlingAttribute(ExceptionHandlingStrategy strategy)
        {
            this.Strategy = strategy;
        }

        #endregion Public Constructors

        #region Public Properties

        public ExceptionHandlingStrategy Strategy { get; set; }

        #endregion Public Properties
    }
}