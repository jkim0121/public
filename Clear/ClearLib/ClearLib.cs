using System;
using System.Collections.Generic;
using System.Linq;

namespace Clear
{
    public class Measure : IDisposable
    {
        public IEnumerable<string> FizzBuzz(int upperbound)
        {
            if (upperbound <= 0)
            {
                throw new ArgumentException("The upperbound shall be positive.");
            }

            for (var i = 1; i <= upperbound; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                {
                    yield return "fizz buzz";
                }
                else if (i % 3 == 0)
                {
                    yield return "fizz";
                }
                else if (i % 5 == 0)
                {
                    yield return "buzz";
                }
                else
                {
                    yield return Convert.ToString(i);
                }
            }
        }

        public IEnumerable<string> FizzBuzz(int upperbound, params Func<int, (bool, string)>[] conditions)
        {
            if (upperbound <= 0)
            {
                throw new ArgumentException("The upperbound shall be positive.");
            }

            for (var i = 1; i <= upperbound; i++)
            {
                var result = default(bool);
                var message = default(string);

                var match = conditions.FirstOrDefault(c =>
                {
                    (result, message) = c(i);
                    return result;
                });

                if (match != null)
                {
                    (result, message) = match(i);
                    yield return message;
                }
                else
                {
                    yield return Convert.ToString(i);
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Measure()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
