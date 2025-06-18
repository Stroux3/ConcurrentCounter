using ConcurrentCounter.Core.Services;

namespace ConcurrentCounter.Tests
{
    public class ServerManualUnitTests
    {
        [Fact]
        public async Task MultipleReaders_ShouldReadSameValue()
        {
            ServerManual.Reset();
            ServerManual.AddToCount(100);

            var tasks = Enumerable.Range(0, 10)
                .Select(_ => Task.Run(() => ServerManual.GetCount()))
                .ToArray();

            var results = await Task.WhenAll(tasks);

            Assert.All(results, value => Assert.Equal(100, value));
        }

        [Fact]
        public async Task Writers_ShouldBeSequentialAndAccurate()
        {
            ServerManual.Reset();

            var tasks = Enumerable.Range(0, 10)
                .Select(_ => Task.Run(() => ServerManual.AddToCount(5)))
                .ToArray();

            await Task.WhenAll(tasks);

            Assert.Equal(50, ServerManual.GetCount());
        }

        [Fact]
        public async Task Readers_WaitDuringWrite()
        {
            ServerManual.Reset();
            ServerManual.AddToCount(3);

            var writer = Task.Run(() =>
            {
                ServerManual.AddToCount(7);
            });

            var reader = Task.Run(() => ServerManual.GetCount());

            await Task.WhenAll(writer, reader);

            int result = await reader;

            Assert.Equal(10, result);

        }
    }
}
