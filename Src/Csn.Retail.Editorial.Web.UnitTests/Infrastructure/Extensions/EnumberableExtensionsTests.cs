using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Infrastructure.Extensions
{
    class EnumberableExtensionsTests
    {
        [Test]
        public void NestedChildrenAreFlattened()
        {
            var test = new List<TestWithChildren>()
            {
                new TestWithChildren()
                {
                    SomeProp = "Test",
                    Children = new List<TestWithChildren>()
                    {
                        new TestWithChildren()
                        {
                            SomeProp = "TestChild",
                            Children = new List<TestWithChildren>()
                            {
                                new TestWithChildren(){SomeProp = "TestChildChild"}
                            }
                        }
                    }
                },
                new TestWithChildren()
                {
                    SomeProp = "Test2",
                    Children = new List<TestWithChildren>()
                    {
                        new TestWithChildren()
                        {
                            SomeProp = "TestChild2",
                            Children = new List<TestWithChildren>()
                            {
                                new TestWithChildren(){SomeProp = "TestChildChild2"}
                            }
                        }
                    }
                }
            };

            var flat = test.Flatten(t => t.Children).ToList();

            Assert.AreEqual(6, flat.Count);
        }

        public class TestWithChildren
        {
            public string SomeProp { get; set; }
            public List<TestWithChildren> Children { get; set; }
        }
    }
}
