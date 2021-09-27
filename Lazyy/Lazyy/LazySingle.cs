using System;

namespace Lazyy
{
    public class LazySingle<T> : ILazy<T>
    {
        private T Value;
        private Func<T> Supplier;
        private bool IsGenerate = false;
        
        
        public LazySingle(Func<T> supplierNew)
        {
            Supplier = supplierNew ?? throw new NullReferenceException();
        }

        public T Get()
        {
            if (IsGenerate)
            {
                return Value;
            }

            IsGenerate = true;
            Value = Supplier();
            Supplier = null;
            return Value;
        }
    }
}