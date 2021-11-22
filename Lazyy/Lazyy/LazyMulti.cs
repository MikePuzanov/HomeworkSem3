using System;
using System.Threading;

namespace Lazyy
{
    /// <summary>
    /// реализация ILazy в многопоточном режиме
    /// </summary>
    public class LazyMulti<T> : ILazy<T>
    {
        private T _value;
        private bool _isGenerated = false;
        private Func<T> _supplier;
        private readonly Object _lockObject = new();

        /// <summary>
        /// создает обьект в многопоточном режиме
        /// </summary>
        public LazyMulti(Func<T> supplier)
            => _supplier = supplier ?? throw new ArgumentNullException();

        /// <summary>
        /// вызывает вычисление один раз и возвращает один и тот же обьект, полученный при вычислении
        /// </summary>
        public T Get()
        {
            if (_isGenerated)
            {
                return _value;
            }

            lock (this._lockObject)
            {
                if (Volatile.Read(ref _isGenerated))
                {
                    return _value;
                }

                _value = _supplier();
                Volatile.Write(ref _isGenerated, true);
                _supplier = null;
                return _value;
            }
        }
    }
}