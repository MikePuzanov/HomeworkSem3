using System;

namespace Lazyy
{
    /// <summary>
    /// создает обьекты для работы либо в однопоточном, лио многопоточном режиме
    /// </summary>
    public static class LazyFactory
    {
        /// <summary>
        /// создает обьект в однопоточном режиме
        /// </summary>
        public static ILazy<T> CreateSingleLazy<T>(Func<T> supplier)
            => new LazySingle<T>(supplier);

        /// <summary>
        /// создает обьект в многопоточном режиме
        /// </summary>
        public static ILazy<T> CreateMultiLazy<T>(Func<T> supplier)
            => new LazyMulti<T>(supplier);
    }
}
