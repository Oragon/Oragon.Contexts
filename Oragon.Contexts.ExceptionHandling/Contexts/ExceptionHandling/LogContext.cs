using Oragon.Extensions;
using System;
using System.Collections.Generic;

namespace Oragon.Contexts.ExceptionHandling
{
	public class LogContext : IDisposable
	{
        const string LogContextKey = nameof(LogContext);

        protected Dictionary<string, object> LogTags { get; private set; }

		public LogContext Parent { get; private set; }

		public bool Enlist { get; private set; }

		public LogContext(bool enlist = true)
		{
            this.LogTags = new Dictionary<string, object>
            {
                { "LogContextID", Guid.NewGuid().ToString("D") },
                { "ParentLogContextID", string.Empty }
            };

            this.Enlist = enlist;
			if (this.Enlist)
			{
				this.Parent = Spring.Threading.LogicalThreadContext.GetData(LogContextKey) as LogContext;
				if (this.Parent != null)
				{
					this.LogTags["ParentLogContextID"] = this.Parent.LogTags["LogContextID"];
				}
				Spring.Threading.LogicalThreadContext.SetData(LogContextKey, this);
			}
		}

		public void SetValue(string key, object value)
		{
			this.LogTags.AddOrUpdate(key, value);
		}

		public void Remove(string key)
		{
			this.LogTags.Remove(key);
		}

		public Dictionary<string, object> GetDictionary()
		{
			Dictionary<string, object> returnValue = new Dictionary<string, object>();
			foreach (var item in this.LogTags)
				returnValue.Add(item.Key, item.Value);
            return returnValue;
		}

		public void Dispose()
		{
			if (this.Enlist)
			{
				LogContext itemToSet = this.Parent ?? null;
				Spring.Threading.LogicalThreadContext.SetData(LogContextKey, itemToSet);
			}
            GC.SuppressFinalize(this);
		}

		public static LogContext Current
		{
			get
			{
				LogContext logContext = Spring.Threading.LogicalThreadContext.GetData(LogContextKey) as LogContext ?? new LogContext();
				return logContext;
			}
		}
	}
}