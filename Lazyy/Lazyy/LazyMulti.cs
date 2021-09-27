using System;

namespace Lazyy
{
    public class LazyMulti<T> : ILazy<T>
    {
        private T Value;
        private bool IsGenerate = false;
        private Func<T> Supplier;
        private Object lockObject = new Object();
        
        public LazyMulti(Func<T> supplierNew)
        {
            Supplier = supplierNew ?? throw new NullReferenceException();
        }
        
        public T Get()
        {
            if (IsGenerate)
            {
                return Value;
            }

            lock (this.lockObject)
            {
                IsGenerate = true;
                Value = Supplier();
                Supplier = null;
                return Value;
            }
        }
    }
}