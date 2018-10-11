using Oragon.Spring.Objects.Factory.Attributes;
using System;
using System.Globalization;

namespace Oragon.Contexts
{
    public abstract class AbstractDataProcess<ContextType, AttributeType> : IDisposable
        where ContextType : AbstractContext<AttributeType>
        where AttributeType : AbstractContextAttribute
    {
        private bool disposed;
        #region Protected Properties

        protected virtual ContextType ObjectContext
        {
            get
            {
                ContextType returnValue = Spring.Threading.LogicalThreadContext.GetData(ObjectContextKey) as ContextType;
                if (returnValue == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "AbstractDataProcess cannot find context with key '{0}'", ObjectContextKey));
                }

                return returnValue;
            }
        }

        [Required]
        protected string ObjectContextKey { get; set; }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                /*
                 ...
                 */
                disposed = true;
            }
        }

        #endregion Protected Properties
    }
}