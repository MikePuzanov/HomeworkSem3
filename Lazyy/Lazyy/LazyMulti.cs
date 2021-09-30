using System;

namespace Lazyy
{
    /// <summary>
    /// реализация многопоточного режима
    /// </summary>
    public class LazyMulti<T> : ILazy<T>
    {
        private T _value;
        private bool _isGenerate = false;
        private Func<T> _supplier;
        private readonly Object _lockObject = new();
        
        /// <summary>
        /// создает обьект в многопоточном режиме
        /// </summary>
        public LazyMulti(Func<T> supplierNew) 
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
            lock (this._lockObject)
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
}