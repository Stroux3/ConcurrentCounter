namespace ConcurrentCounter.Core.Services
{
    /// <summary>
    /// Класс, реализующий потокобезопасный счётчик с поддержкой параллельного чтения и последовательной записи.
    /// </summary>
    public static class Server
    {
        private static readonly int count = 0;

        private static readonly ReaderWriterLockSlim rwLock = new();

        /// <summary>
        /// Возвращает текущее значение счётчика.
        /// Множественные вызовы могут выполняться параллельно.
        /// </summary>
        public static int GetCount()
        {
            rwLock.EnterReadLock();
            try
            {
                return count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }
    }
}
