using System;
using MyNUnit;

namespace ClassLibrary1
{
    public class Test1
    {
        private int nonStaticCounter = 0;

        [Before]
        public void NonStaticIncrement() => nonStaticCounter++;

        [Test(null)]
        public void TestWithoutExpected()
        {

        }

        [Test(null, "yes")]
        public void TestShouldBeIgnored()
        {

        }

        [Test(typeof(ArgumentException))]
        public void TestWithExpectedException()
        {
            throw new ArgumentException();
        }


        [Test(null)]
        public void TestBefore()
        {
            if (nonStaticCounter != 1)
            {
                throw new ArgumentException();
            }
        }
        
        [Test(null)]
        public void NullExpectedButThrowException()
        {
            throw new ArgumentException();
        }

        [Test(typeof(ArgumentException))]
        public void ExceptionExpectedButWasNull()
        {

        }

        [Test(typeof(ArgumentException))]
        public void OneExceptionExpectedButWasAnother()
        {
            throw new AggregateException();
        }

        [BeforeClass]
        public void NonStaticBeforeClass()
        {

        }

        [AfterClass]
        public static void ExceptionInAfterClass()
        {
            throw new AggregateException();
        }

        [After]
        [Test(null)]
        public void TestWithIncompatibleAttributes()
        {

        }
    }
}