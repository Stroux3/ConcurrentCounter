namespace ConcurrentCounter.Core.Services
{
    /// <summary>
    /// Класс, реализующий потокобезопасный счётчик с поддержкой параллельного чтения и последовательной записи.
    /// </summary>
    public static class ServerWithReaderWriterLockSlim
    {
        private static int count = 0;

        private static readonly ReaderWriterLockSlim rwLock = new();

        #region Получение значения счётчика
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
        #endregion

        #region Добавление значения к счётчику
        /// <summary>
        /// Прибавляет значение к счётчику.
        /// Запись происходит эксклюзивно, блокируя других писателей и читателей.
        /// </summary>
        public static void AddToCount(int value)
        {
            rwLock.EnterWriteLock();
            try
            {
                count += value;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
        #endregion

        public static void Reset()
        {
            count = 0;
        }
    }
}
