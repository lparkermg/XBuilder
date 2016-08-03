using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Builder = BuildXDoc.Builder;

namespace BuilderTests
{
    [TestFixture]
    public class BuilderValidate
    {
        private Builder _builder;

        [SetUp]
        public void SetUp()
        {
            _builder = new Builder("TestRootElement");
        }

        [TearDown]
        public void TearDown()
        {
            _builder = null;
        }

        [Test]
        public void New_Builder_Should_Start_With_One_Root_Element()
        {
            var data = _builder.XmlData.Elements().ToList();

            Assert.AreEqual(1,data.Count());
        }

        [Test]
        public void New_Builder_Root_Element_Should_Have_Correct_LocalName()
        {
            var data = _builder.XmlData.Elements().First();

            Assert.AreEqual("TestRootElement",data.Name.LocalName);
        }

        [Test]
        public void New_Builder_Should_Support_Multiple_Attributes()
        {
            var attrs = new List<KeyValuePair<string, string>> ()
            {
                new KeyValuePair<string, string>("attrOne", "Value One"),
                new KeyValuePair<string, string>("attrTwo", "Another value")
            };
            _builder = new Builder("TestRootElement",attrs);

            var data = _builder.XmlData.Elements().First();
            var attrCount = data.Attributes().Count();

            Assert.AreEqual(2,attrCount);
        }
    }
}
