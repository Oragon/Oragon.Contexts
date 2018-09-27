using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Context.Tests.Schema
{
    public class DisposableDecorator : IDisposable
    {
        private readonly IDisposable _innerDisposable;

        public DisposableDecorator(IDisposable innerDisposable)
        {
            _innerDisposable = innerDisposable;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        ~DisposableDecorator()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _innerDisposable.Dispose();
        }
    }
}
