using System;
using System.Threading.Tasks;
using Xunit;

namespace BigHelp.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void AsyncDeadLock()
        {
            int result = DoSomeWorkAsync().Result; // 1
        }

        private async Task<int> DoSomeWorkAsync()
        {
            await Task.Delay(5000).ConfigureAwait(true); //2
            return 1;
        }
    }
}
