using NUnit.Framework;

namespace SweetVids.Tests
{
    [TestFixture]
    public abstract class ContextSpecification
    {

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            SetupFixtureContext();
        }



        private bool _hasExecuted;
        /// <summary>
        /// Runs before every test
        /// </summary>
        [SetUp]
        public void MainSetup()
        {
            SetContext();
            Because();

            if (!_hasExecuted)
            {
                BecauseOnce();
                _hasExecuted = true;
            }
        }

        /// <summary>
        /// Runs after every test
        /// </summary>
        [TearDown]
        protected void MainTeardown()
        {
            CleanUp();
        }

        /// <summary>
        /// SetupFixtureContext executes one time, before<see cref="Because"/>, <see cref="SetContext"/>, and <see cref="BecauseOnce"/>
        /// </summary>
        protected virtual void SetupFixtureContext()
        {

        }

        /// <summary>
        /// SetContext executes before every test, before <see cref="Because"/>
        /// </summary>
        protected virtual void SetContext() { }


        /// <summary>
        /// Cleanup executes after every test
        /// </summary>
        protected virtual void CleanUp() { }

        /// <summary>
        /// Because executes before every test, after <see cref="SetContext"/>
        /// </summary>
        protected virtual void Because() { }

        /// <summary>
        /// BecauseOnce executes once for all tests in a fixture, after <see cref="SetContext"/>
        /// </summary>
        protected virtual void BecauseOnce() { }


    }
}