using System;

namespace Lazyy
{
    /// <summary>
    /// реаилизация многопоточного режима
    /// </summary>
    public class LazyMulti<T> : ILazy<T>
    {
        private T _value;
        private bool _isGenerate = false;
        private Func<T> _supplier;
        private readonly Object _lockObject = new Object();
        
        public LazyMulti(Func<T> supplierNew)
        {
            _supplier = supplierNew ?? throw new NullReferenceException();
        }
        
        public T Get()
        {
            if (_isGenerate)
            {
                return _value;
            }

            lock (this._lockObject)
            {
                _isGenerate = true;
                _value = _supplier();
                _supplier = null;
                return _value;
            }
        }
    }
}