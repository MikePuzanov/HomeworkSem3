namespace Lazyy
{
    /// <summary>
    /// интерфейс ленивого вычисления
    /// </summary>
    public interface ILazy<out T>
    {
        /// <summary>
        /// вызывает вычисление один раз и возвращает один и тот же обьект, полученный при вычислении
        /// </summary>
        public T Get();
    }
}