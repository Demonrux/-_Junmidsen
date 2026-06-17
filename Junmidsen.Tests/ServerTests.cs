using Junmidsen.Core;

namespace Junmidsen.Tests
{
    public class ServerTests
    {
        public ServerTests()
        {
            Server.Reset();
        }

        [Fact]
        public void GetCount_ReturnsCurrentValue()
        {
            Assert.Equal(0, Server.GetCount());
            Server.AddToCount(10);
            Assert.Equal(10, Server.GetCount());
        }

        [Fact]
        public async Task AddToCount_ConcurrentWrites_AreSerialized()
        {
            const int iterations = 1000;
            var tasks = new Task[iterations];
            for (int i = 0; i < iterations; i++)
                tasks[i] = Task.Run(() => Server.AddToCount(1));

            await Task.WhenAll(tasks);
            Assert.Equal(iterations, Server.GetCount());
        }
    }
}