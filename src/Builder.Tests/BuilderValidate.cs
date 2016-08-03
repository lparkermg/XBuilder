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

        #region Builder Init
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
        #endregion

        #region Adding Elements (Basic)

        [Test]
        public void Builder_Should_Be_Able_To_Add_Without_Value_In_Element()
        {
            _builder.Add("NodeUnderRoot");

            var ele = _builder.XmlData.Elements().First(e => e.Name.LocalName == "TestRootElement");
            var node = ele.Elements().First(e => e.Name.LocalName == "NodeUnderRoot");
            Assert.AreEqual("NodeUnderRoot",node.Name.LocalName);
        }

        [Test]
        public void Builder_Should_Be_Able_To_Add_With_Value_In_Element()
        {
            _builder.Add("NodeUnderRoot", "Node Value");

            var ele = _builder.XmlData.Elements().First(e => e.Name.LocalName == "TestRootElement");
            var val = ele.Elements().First(e => e.Name.LocalName == "NodeUnderRoot").Value;

            Assert.AreEqual("Node Value", val);
        }

        [Test]
        public void Builder_Should_Be_Able_To_Add_With_Multiple_Attributes_In_Element()
        {
            var attrs = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("attrOne", "Attribute Value One."),
                new KeyValuePair<string, string>("attrTwo", "Attribute Value Two")
            };

            _builder.Add("NodeUnderRoot", attrs);

            var ele = _builder.XmlData.Elements().First(e => e.Name.LocalName == "TestRootElement");
            var node = ele.Elements().First(e => e.Name.LocalName == "NodeUnderRoot");

            var attrCount = node.Attributes().Count();

            Assert.AreEqual(2,attrCount);
        }

        [Test]
        public void Builder_Should_Be_Able_To_Add_With_Value_And_Attributes_In_Element()
        {
            var attrs = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("attrOne", "Attribute Value One."),
                new KeyValuePair<string, string>("attrTwo", "Attribute Value Two")
            };

            _builder.Add("NodeUnderRoot", "Node Value", attrs);

            var ele = _builder.XmlData.Elements().First(e => e.Name.LocalName == "TestRootElement");
            var node = ele.Elements().First(e => e.Name.LocalName == "NodeUnderRoot");

            var val = node.Value;
            var attrCount = node.Attributes().Count();

            Assert.AreEqual("Node Value",val);
            Assert.AreEqual(2, attrCount);
        }

        [Test]
        public void Builder_Should_Be_Able_To_Add_X_Amount_Of_Duplicates_In_Element()
        {
            var amount = 10;

            _builder.Add("NodeUnderRoot", amount);

            var ele = _builder.XmlData.Elements().First(e => e.Name.LocalName == "TestRootElement");
            var nodeCount = ele.Elements().Where(e => e.Name.LocalName == "NodeUnderRoot").ToList().Count;

            Assert.AreEqual(amount, nodeCount);
        }


        #endregion
    }
}
