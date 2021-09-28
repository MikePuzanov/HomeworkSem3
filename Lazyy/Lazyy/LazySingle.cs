using System;

namespace Lazyy
{
    public class LazySingle<T> : ILazy<T>
    {
        private T _value;
        private Func<T> _supplier;
        private bool _isGenerate = false;
        
        
        public LazySingle(Func<T> supplierNew)
        {
            _supplier = supplierNew ?? throw new NullReferenceException();
        }

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