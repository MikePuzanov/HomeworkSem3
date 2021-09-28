using System;
using NUnit.Framework;

namespace Lazyy.Test
{
    public class SingleThreadTest
    {
        [Test]
        public void NormalWorkForSingleTest()
        {
            var number = 2;
            var lazySingle = LazyFactory<int>.CreateSingleLazy(() =>
            {
                number += number;
                return number;
            });
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(4, lazySingle.Get());
            }
        }

        [Test]
        public void NullExceptionTest()
        {
            Assert.Throws<NullReferenceException>(() => LazyFactory<int>.CreateSingleLazy(null));
        }
    }
}