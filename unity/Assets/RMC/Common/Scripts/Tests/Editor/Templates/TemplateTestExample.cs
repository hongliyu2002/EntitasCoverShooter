using UnityEngine;
using NUnit.Framework;

namespace RMC.Common.Templates
{
    /*
    /// <summary>
    /// Replace me with description.
    /// </summary>
    [TestFixture]
    public class TemplateTestExample
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties

        // ------------------ Non-serialized fields

        // ------------------ Methods

        //CALLED EXACTLY ONCE BEFORE THE FIRST TEST
        [TestFixtureSetUp] 
        public void TestFixtureSetUp()
        { 
            Debug.Log ("TemplateTestFull.testFixtureSetUp()");
        }

        //CALLED EXACTLY ONCE AFTER THE LAST TEST
        [TestFixtureTearDown] 
        public void TestFixtureTearDown()
        {
            Debug.Log ("TemplateTestFull.testFixtureTearDown()");
        }


        //CALLED BEFORE EVERY 'TEST' METHOD IN THIS FIXTURE
        [SetUp] 
        public void SetUp()
        {
            Debug.Log ("  -TemplateTestFull.setup()");


        }


        //CALLED AFTER EVERY 'TEST' METHOD IN THIS FIXTURE
        [TearDown] 
        public void TearDown()
        {
            Debug.Log ("  -TemplateTestFull.tearDown()");
        }

        [Test]
        public void Test1 ()
        {
            Debug.Log ("     **TemplateTestFull.test1()");
            Assert.Pass();
        }

        [Test]
        public void Test2 ()
        {
            Debug.Log ("     **TemplateTestFull.test2()");
            Assert.Pass();
        }



        [Test]
        public void ExceptionTest ()
        {
            throw new Exception ("Exception throwing test");
        }

        [Test]
        [Ignore ("Ignored test")]
        public void IgnoredTest ()
        {
            throw new Exception ("Ignored this test");
        }

        [Test]
        [MaxTime (100)]
        public void SlowTest ()
        {
            Thread.Sleep (200);
        }

        [Test]
        public void FailingTest ()
        {
            Assert.Fail ();
        }

        [Test]
        public void InconclusiveTest ()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void PassingTest ()
        {
            Assert.Pass ();
        }

        [Test]
        public void ParameterizedTest ([Values (1, 2, 3)] int a)
        {
            Assert.Pass ();
        }

        [Test]
        public void RangeTest ( [Range (1, 10, 3)] int x )
        {
            Assert.Pass ();
        }

        [Test]
        [Culture ("pl-PL")]
        public void CultureSpecificTest ()
        {
        }

        [Test]
        [ExpectedException (typeof (ArgumentException), ExpectedMessage = "expected message")]
        public void ExpectedExceptionTest ()
        {
            throw new ArgumentException ("expected message");
        }

        [Datapoint]
        public double zero = 0;
        [Datapoint]
        public double positive = 1;
        [Datapoint]
        public double negative = -1;
        [Datapoint]
        public double max = double.MaxValue;
        [Datapoint]
        public double infinity = double.PositiveInfinity;

        [Theory]
        public void SquareRootDefinition ( double num )
        {
            Assume.That (num >= 0.0 && num < double.MaxValue);

            var sqrt = Math.Sqrt (num);

            Assert.That (sqrt >= 0.0);
            Assert.That (sqrt * sqrt, Is.EqualTo (num).Within (0.000001));
        }



    }
    */
}
