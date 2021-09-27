using System;

namespace Lazyy
{
    public class LazyFactory<T>
    {
        public static LazySingle<T> CreateSingleLazy(Func<T> supplier)
            => new LazySingle<T>(supplier);

        public static LazyMulti<T> CreateMultiLazy(Func<T> supplier)
            => new LazyMulti<T>(supplier);
    }
}