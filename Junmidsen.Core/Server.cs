namespace Junmidsen.Core
{
    /// <summary>
    /// A static server that holds an integer counter
    /// </summary>
    public static class Server
    {
        private static int _count = 0;
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        /// <summary>
        /// Returns the current counter value
        /// </summary>
        public static int GetCount()
        {
            _lock.EnterReadLock();
            try
            {
                return _count;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Adds a value to the counter
        /// </summary>
        public static void AddToCount(int value)
        {
            _lock.EnterWriteLock();
            try
{
                _count += value;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public static void Reset() => _count = 0;
    }
}