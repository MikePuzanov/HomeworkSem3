using System;

namespace Lazyy
{
    /// <summary>
    /// реализация однопоточного режима
    /// </summary>
    public class LazySingle<T> : ILazy<T>
    {
        private T _value;
        private Func<T> _supplier;
        private bool _isGenerate = false;
        
        /// <summary>
        /// создает обьект в однопоточном режиме
        /// </summary>
        public LazySingle(Func<T> supplierNew)
            => _supplier = supplierNew ?? throw new NullReferenceException();
        
        /// <summary>
        /// вызывает вычисление один раз и возвращает один и тот же обьект, полученный при вычислении
        /// </summary>
        public T Get()
        {
            if (_isGenerate)
            {
                return _value;
            }

            _isGenerate = true;
            _value = _supplier();
            _supplier = null;
            return _value;
        }
    }
}