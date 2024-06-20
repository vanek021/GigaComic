using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Infrastructure.Helpers
{
    public static class CallbackRetrier
    {
        private const int RETRIES_COUNT = 3;

        public static void ExecuteWithRetries(Action callback, int retriesCount = RETRIES_COUNT)
        {
            var initialException = default(Exception);

            var initialRetries = retriesCount;

            while (retriesCount > 0)
            {
                retriesCount--;

                try
                {
                    callback();
                    return;
                }
                catch (Exception ex)
                {
                    if (retriesCount == initialRetries - 1)
                    {
                        initialException = ex;
                    }

                    if (retriesCount < 1)
                        throw new Exception("Retrier reach limit", initialException) ?? ex;
                }
            }
        }
    }
}
