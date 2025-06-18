
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

        #region Получение значения счётчика
        /// <summary>
        /// Получает значение счётчика. Разрешается параллельное чтение.
        /// </summary>
        public static int GetCount()
        {
            BeginRead();

            try
            {
                return count;
            }
            finally
            {
                EndRead();
            }
        }
        #endregion

        #region Добавление значения к счётчику
        /// <summary>
        /// Добавляет значение к счётчику. Писатели работают эксклюзивно.
        /// </summary>
        public static void AddToCount(int value)
        {
            writerSemaphore.Wait();

            try
            {
                WaitForReadersToFinish();

                count += value;
            }
            finally
            {
                writerSemaphore.Release();
            }
        }
        #endregion

        private static void WaitForReadersToFinish()
        {
            while (true)
            {
                lock (readerLock)
                {
                    if (readers == 0)
                        break;
                }
                Thread.Sleep(1);
            }
        }


    }
}
