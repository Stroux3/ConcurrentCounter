
namespace ConcurrentCounter.Core.Services
{
    /// <summary>
    /// Потокобезопасный счётчик без использования ReaderWriterLockSlim.
    /// Поддерживает параллельное чтение и эксклюзивную запись.
    /// </summary>
    public static class ServerManual
    {
        private static int count = 0;

        private static int readers = 0;
        private static readonly Lock readerLock = new(); // защита счётчика читателей
        private static readonly SemaphoreSlim writerSemaphore = new(1, 1); // эксклюзивная запись
        private static void BeginRead()
        {
            writerSemaphore.Wait();
            lock (readerLock)
            {
                readers++;
            }
            writerSemaphore.Release();
        }
        private static void EndRead()
        {
            lock (readerLock)
            {
                readers--;
            }
        }

        
    }
}
