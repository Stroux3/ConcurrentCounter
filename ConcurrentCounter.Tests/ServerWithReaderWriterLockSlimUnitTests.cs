using ConcurrentCounter.Core.Services;

namespace ConcurrentCounter.Tests
{
    public class ServerWithReaderWriterLockSlimUnitTests
    {
        [Fact]
        public async Task MultipleReaders_ShouldReadSameValue()
        {
            ServerWithReaderWriterLockSlim.Reset();
            ServerWithReaderWriterLockSlim.AddToCount(42);

            var tasks = Enumerable.Range(0, 10)
                .Select(_ => Task.Run(() => ServerWithReaderWriterLockSlim.GetCount()))
                .ToArray();

            var results = await Task.WhenAll(tasks);

            Assert.All(results, value => Assert.Equal(42, value));
        }

        [Fact]
        public async Task Writers_ShouldAccumulateCorrectly()
        {
            ServerWithReaderWriterLockSlim.Reset();

            var tasks = Enumerable.Range(0, 10)
                .Select(_ => Task.Run(() => ServerWithReaderWriterLockSlim.AddToCount(1)))
                .ToArray();

            await Task.WhenAll(tasks);

            Assert.Equal(10, ServerWithReaderWriterLockSlim.GetCount());
        }

        [Fact]
        public async Task ReadersWaitDuringWrite()
        {
            ServerWithReaderWriterLockSlim.Reset();
            ServerWithReaderWriterLockSlim.AddToCount(1);

            var writeTask = Task.Run(() =>
            {
                ServerWithReaderWriterLockSlim.AddToCount(2);
            });

            var readTask = Task.Run(() =>
            {
                return ServerWithReaderWriterLockSlim.GetCount();
            });

            await Task.WhenAll(writeTask, readTask);

            int result = await readTask; 
            Assert.Equal(3, result);
        }

    }
}
