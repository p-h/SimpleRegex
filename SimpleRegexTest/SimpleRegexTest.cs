using SimpleRegexLib;
using NUnit.Framework;

namespace SimpleRegexTest
{
    public class SimpleRegexTest
    {
        private StateMachineRegex regex;

        [OneTimeSetUp]
        public void SetUp()
        {
            regex = new StateMachineRegex();
        }

        [TestCase("foobar", "foobar", true)]
        [TestCase("fooba", "foobar", false)]
        [TestCase("foobar", "fooba", false)]
        [TestCase("foo*bar", "foobar", true)]
        [TestCase("foo*bar", "foobaz", false)]
        [TestCase("foo*bar", "foobarbar", true)]
        [TestCase("foo*bar", "foobazbarbar", true)]
        [TestCase("foo*.ar", "foobazbarbar", true)]
        [TestCase("foo.ar", "foobar", true)]
        [TestCase("...", "foo", true)]
        [TestCase("...", "fo", false)]
        [TestCase("foo*bar*baz", "foobarbaz", true)]
        [TestCase("foo*bar*baz", "foobarbarbarbaz", true)]
        [TestCase("foo*bar*baz", "foobarbarbarbazbar", false)]
        public void TestStateMachineSimpleRegex(string rule, string input, bool expected)
        {
            Assert.AreEqual(expected, regex.Matches(rule, input));
        }
    }
}