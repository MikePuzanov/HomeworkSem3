using System;

namespace Lazyy
{
    /// <summary>
    /// реализация ILazy в однопоточном режиме
    /// </summary>
    public class LazySingle<T> : ILazy<T>
    {
        private T _value;
        private Func<T> _supplier;
        private bool _isGenerated = false;

        /// <summary>
        /// создает обьект в однопоточном режиме
        /// </summary>
        public LazySingle(Func<T> supplier)
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

            _isGenerated = true;
            _value = _supplier();
            _supplier = null;
            return _value;
        }
    }
}