using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FubuCore.Reflection;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Rhino.Mocks.Interfaces;
using Is = NUnit.Framework.Is;

namespace SweetVids.Tests
{
    public delegate void MethodThatThrows();

    public static class SpecificationExtensions
    {
       
        public static void ShouldHave<T>(this IEnumerable<T> values, Func<T, bool> func)
        {
            values.FirstOrDefault(func).ShouldNotBeNull();
        }

        public static void ShouldBeFalse(this bool condition)
        {
            Assert.IsFalse(condition);
        }

        public static void ShouldBeTrue(this bool condition)
        {
            Assert.IsTrue(condition);
        }

        public static void ShouldBeTrueBecause(this bool condition, string reason, params object[] args)
        {
            Assert.IsTrue(condition, reason, args);
        }

        public static object ShouldEqual(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
            return expected;
        }

        public static object ShouldEqual(this string actual, object expected)
        {
            Assert.AreEqual(expected.ToString(), actual);
            return expected;
        }

        public static void ShouldMatch(this string actual, string pattern)
        {
            Assert.That(actual, Is.StringMatching(pattern));
        }

        public static XmlElement AttributeShouldEqual(this XmlElement element, string attributeName, object expected)
        {
            Assert.IsNotNull(element, "The Element is null");

            string actual = element.GetAttribute(attributeName);
            Assert.AreEqual(expected, actual);
            return element;
        }

        public static object ShouldNotEqual(this object actual, object expected)
        {
            Assert.AreNotEqual(expected, actual);
            return expected;
        }

        public static void ShouldBeNull(this object anObject)
        {
            Assert.IsNull(anObject);
        }

        public static void ShouldNotBeNull(this object anObject)
        {
            Assert.IsNotNull(anObject);
        }

        public static object ShouldBeTheSameAs(this object actual, object expected)
        {
            Assert.AreSame(expected, actual);
            return expected;
        }

        public static object ShouldNotBeTheSameAs(this object actual, object expected)
        {
            Assert.AreNotSame(expected, actual);
            return expected;
        }

        public static T ShouldBeOfType<T>(this object actual)
        {
            actual.ShouldNotBeNull();
            actual.ShouldBeOfType(typeof(T));
            return (T)actual;
        }

        public static T As<T>(this object actual)
        {
            actual.ShouldNotBeNull();
            actual.ShouldBeOfType(typeof(T));
            return (T)actual;
        }

        public static object ShouldBeOfType(this object actual, Type expected)
        {
            Assert.IsAssignableFrom(expected, actual);
            return actual;
        }

        public static void ShouldNotBeOfType(this object actual, Type expected)
        {
            Assert.IsNotAssignableFrom(expected, actual);
        }

        public static void ShouldContain(this IList actual, object expected)
        {
            Assert.Contains(expected, actual);
        }

        public static void ShouldContain<T>(this IEnumerable<T> actual, T expected)
        {
            if (actual.Count(t => t.Equals(expected)) == 0)
            {
                Assert.Fail("The item was not found in the sequence.");
            }
        }

        public static void ShouldNotBeEmpty<T>(this IEnumerable<T> actual)
        {
            Assert.Greater(actual.Count(), 0, "The list should have at least one element");
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> actual, T expected)
        {
            if (actual.Count(t => t.Equals(expected)) > 0)
            {
                Assert.Fail("The item was found in the sequence it should not be in.");
            }
        }

        public static void ShouldHaveTheSameElementsAs(this IList actual, IList expected)
        {
            actual.ShouldNotBeNull();
            expected.ShouldNotBeNull();

            actual.Count.ShouldEqual(expected.Count);

            for (int i = 0; i < actual.Count; i++)
            {
                actual[i].ShouldEqual(expected[i]);
            }
        }

        public static void ShouldHaveTheSameElementsAs<T>(this IEnumerable<T> actual, params T[] expected)
        {
            ShouldHaveTheSameElementsAs(actual, (IEnumerable<T>)expected);
        }

        public static void ShouldHaveTheSameElementsAs<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            IList actualList = (actual is IList) ? (IList)actual : actual.ToList();
            IList expectedList = (expected is IList) ? (IList)expected : expected.ToList();

            ShouldHaveTheSameElementsAs(actualList, expectedList);
        }

        public static void ShouldHaveTheSameElementKeysAs<ELEMENT, KEY>(this IEnumerable<ELEMENT> actual,
                                                                        IEnumerable expected,
                                                                        Func<ELEMENT, KEY> keySelector)
        {
            actual.ShouldNotBeNull();
            expected.ShouldNotBeNull();

            ELEMENT[] actualArray = actual.ToArray();
            object[] expectedArray = expected.Cast<object>().ToArray();

            actualArray.Length.ShouldEqual(expectedArray.Length);

            for (int i = 0; i < actual.Count(); i++)
            {
                keySelector(actualArray[i]).ShouldEqual(expectedArray[i]);
            }
        }

        public static IComparable ShouldBeGreaterThan(this IComparable arg1, IComparable arg2)
        {
            Assert.Greater(arg1, arg2);
            return arg2;
        }

        public static IComparable ShouldBeLessThan(this IComparable arg1, IComparable arg2)
        {
            Assert.Less(arg1, arg2);
            return arg2;
        }

        public static void ShouldBeEmpty(this ICollection collection)
        {
            Assert.IsEmpty(collection);
        }

        public static void ShouldBeEmpty(this string aString)
        {
            Assert.IsEmpty(aString);
        }

        public static void ShouldNotBeEmpty(this ICollection collection)
        {
            Assert.IsNotEmpty(collection);
        }

        public static void ShouldNotBeEmpty(this string aString)
        {
            Assert.IsNotEmpty(aString);
        }

        public static void ShouldContain(this string actual, string expected)
        {
            StringAssert.Contains(expected, actual);
        }

        public static void ShouldContainAllOf(this string actual, params string[] expectedItems)
        {
            expectedItems.Each(expected => actual.ShouldContain(expected));
        }

        public static void ShouldContain<T>(this IEnumerable<T> actual, Func<T, bool> expected)
        {
            actual.Count().ShouldBeGreaterThan(0);
            T result = actual.FirstOrDefault(expected);
            Assert.That(result, Is.Not.EqualTo(default(T)), "Expected item was not found in the actual sequence");
        }

        public static void ShouldNotContain(this string actual, string expected)
        {
            Assert.That(actual, new NotConstraint(new SubstringConstraint(expected)));
        }

        public static string ShouldBeEqualIgnoringCase(this string actual, string expected)
        {
            StringAssert.AreEqualIgnoringCase(expected, actual);
            return expected;
        }

        public static void ShouldEndWith(this string actual, string expected)
        {
            StringAssert.EndsWith(expected, actual);
        }

        public static void ShouldStartWith(this string actual, string expected)
        {
            StringAssert.StartsWith(expected, actual);
        }

        public static void ShouldContainErrorMessage(this Exception exception, string expected)
        {
            StringAssert.Contains(expected, exception.Message);
        }

        public static Exception ShouldBeThrownBy(this Type exceptionType, MethodThatThrows method)
        {
            return ShouldBeThrownBy(exceptionType, method, "");
        }

        public static Exception ShouldBeThrownBy(this Type exceptionType, MethodThatThrows method, string description)
        {
            Exception exception = null;

            try
            {
                method();
            }
            catch (Exception e)
            {
                e = (e is TargetInvocationException) ? e.InnerException : e;

                Assert.AreEqual(exceptionType, e.GetType(), e.ToString());
                exception = e;
            }

            if (exception == null)
            {
                Assert.Fail(String.Format("Expected {0} to be thrown{1}.", exceptionType.FullName, description == null ? "" : "by " + description));
            }

            return exception;
        }

        public static void ShouldEqualSqlDate(this DateTime actual, DateTime expected)
        {
            TimeSpan timeSpan = actual - expected;
            Assert.Less(Math.Abs(timeSpan.TotalMilliseconds), 3);
        }

        public static void ShouldBe<T>(this Expression<Func<T, object>> actual, Expression<Func<T, object>> expected)
        {
            string expectedMethod = ReflectionHelper.GetMethod(expected).Name;
            string actualMethod = ReflectionHelper.GetMethod(actual).Name;
            actualMethod.ShouldEqual(expectedMethod);
        }

        public static IEnumerable<T> ShouldHaveCount<T>(this IEnumerable<T> actual, int expected)
        {
            actual.Count().ShouldEqual(expected);
            return actual;
        }

        public static void Matches(this List<string> list, params string[] values)
        {
            AssertListMatches(list, values);
        }

        public static void AssertListMatches(IList actualList, IList expectedList)
        {
            var actual = new ArrayList(actualList);
            var expected = new ArrayList(expectedList);

            foreach (object item in actual.ToArray())
            {
                actual.Remove(item);
                expected.Remove(item);
            }

            if (actual.Count == 0 && expected.Count == 0) return;

            string message = "";
            actual.Each(o => message += string.Format("Extra:  {0}\n", (object)o));
            expected.Each(o => message += string.Format("Missing:  {0}\n", o));

            Assert.Fail(message);
        }


        public static CapturingConstraint CaptureArgumentsFor<MOCK>(this MOCK mock,
                                                                    Expression<Action<MOCK>> methodExpression)
            where MOCK : class
        {
            return CaptureArgumentsFor(mock, methodExpression, o => { });
        }

        public static CapturingConstraint CaptureArgumentsFor<MOCK>(this MOCK mock,
                                                                    Expression<Action<MOCK>> methodExpression,
                                                                    Action<IMethodOptions<RhinoMocksExtensions.VoidType>> optionsAction)
            where MOCK : class
        {
            return CaptureArgumentsFor(mock,
                                       methodExpression,
                                       m => m.Expect(methodExpression.Compile()),
                                       optionsAction);
        }

        public static CapturingConstraint CaptureArgumentsFor<MOCK>(this MOCK mock,
                                                                    Expression<Function<MOCK, object>> methodExpression)
            where MOCK : class
        {
            return CaptureArgumentsFor(mock, methodExpression, o => { });
        }

        public static CapturingConstraint CaptureArgumentsFor<MOCK, RESULT>(this MOCK mock,
                                                                            Expression<Function<MOCK, RESULT>> methodExpression,
                                                                            Action<IMethodOptions<RESULT>> optionsAction)
            where MOCK : class
        {
            return CaptureArgumentsFor(mock,
                                       methodExpression,
                                       m => m.Stub(methodExpression.Compile()),
                                       optionsAction);
        }

        public static CapturingConstraint CaptureArgumentsFor<MOCK, DELEGATETYPE, OPTIONTYPE>(MOCK mock,
                                                                                              Expression<DELEGATETYPE> methodExpression,
                                                                                              Func<MOCK, IMethodOptions<OPTIONTYPE>> expectAction,
                                                                                              Action<IMethodOptions<OPTIONTYPE>> optionsAction)
            where MOCK : class
        {
            var method = ReflectionHelper.GetMethod(methodExpression);
            var constraint = new CapturingConstraint();
            var constraints = new List<AbstractConstraint>();

            method.GetParameters().Each(p => constraints.Add(constraint));

            var expectation = expectAction(mock).Constraints(constraints.ToArray()).Repeat.Any();
            optionsAction(expectation);

            return constraint;
        }

        #region Nested type: CapturingConstraint

        public class CapturingConstraint : AbstractConstraint
        {
            private readonly ArrayList argList = new ArrayList();

            public override string Message
            {
                get { return ""; }
            }

            public void Clear()
            {
                argList.Clear();
            }

            public override bool Eval(object obj)
            {
                argList.Add(obj);
                return true;
            }

            public T First<T>()
            {
                return ArgumentAt<T>(0);
            }

            public T ArgumentAt<T>(int pos)
            {
                return (T)argList[pos];
            }

            public T Second<T>()
            {
                return ArgumentAt<T>(1);
            }
        }

        #endregion
    }
}