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

            var root = _builder.XmlData.Elements().First(e => e.Name.LocalName == "TestRootElement");
            var node = root.Elements().First(e => e.Name.LocalName == "NodeUnderRoot");
            Assert.AreEqual("NodeUnderRoot",node.Name.LocalName);
        }

        [Test]
        public void Builder_Should_Be_Able_To_Add_With_Value_In_Element()
        {
            _builder.Add("NodeUnderRoot", "Node Value");

            var root = _builder.XmlData.Elements().First(e => e.Name.LocalName == "TestRootElement");
            var val = root.Elements().First(e => e.Name.LocalName == "NodeUnderRoot").Value;

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

            var root = _builder.XmlData.Elements().First(e => e.Name.LocalName == "TestRootElement");
            var node = root.Elements().First(e => e.Name.LocalName == "NodeUnderRoot");

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

            var root = _builder.XmlData.Elements().First(e => e.Name.LocalName == "TestRootElement");
            var node = root.Elements().First(e => e.Name.LocalName == "NodeUnderRoot");

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

            var root = _builder.XmlData.Elements().First(e => e.Name.LocalName == "TestRootElement");
            var nodeCount = root.Elements().Where(e => e.Name.LocalName == "NodeUnderRoot").ToList().Count;

            Assert.AreEqual(amount, nodeCount);
        }


        #endregion

        #region Add Range of elements (Basic)

        [Test]
        public void Builder_Should_Be_Able_To_Add_A_Range_Of_Elements()
        {
            var elements = new string[] {"ElementOne", "ElementTwo", "ElementThree", "ElementFour"};

            _builder.AddRange(elements);

            var root = _builder.XmlData.Elements().First(e => e.Name.LocalName == "TestRootElement");
            var eles = root.Elements().ToList();

            var i = 0;
            foreach (var ele in eles)
            {
                Assert.AreEqual(elements[i],ele.Name.LocalName);
                i++;
            }

            Assert.AreEqual(4,eles.Count);
        }

        [Test]
        public void Builder_Should_Be_Able_To_Add_A_Range_Of_Elements_With_Values()
        {
            var elements = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("ElementOne", "Element one's value"),
                new KeyValuePair<string, string>("ElementTwo", "Element two's value"),
                new KeyValuePair<string, string>("ElementThree", "Element three's value") 
            };

            _builder.AddRange(elements);

            var root = _builder.XmlData.Elements().First(e => e.Name.LocalName == "TestRootElement");
            var eles = root.Elements().ToList();

            var i = 0;
            foreach (var ele in eles)
            {
                Assert.AreEqual(elements[i].Key,ele.Name.LocalName);
                Assert.AreEqual(elements[i].Value, ele.Value);
                i++;
            }

            Assert.AreEqual(3,eles.Count);
        }

        [Test]
        public void Builder_Should_Be_Able_To_Add_A_Range_Of_Elements_And_Values_With_Matching_Attributes()
        {
            var elements = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("ElementOne", "Element one's value"),
                new KeyValuePair<string, string>("ElementTwo", "Element two's value"),
                new KeyValuePair<string, string>("ElementThree", "Element three's value")
            };

            var attrs = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("attrOne", "Attribute One"),
                new KeyValuePair<string, string>("attrTwo", "Attribute Two"),
                new KeyValuePair<string, string>("attrThree", "Attribute Three")
            };

            _builder.AddRange(elements, attrs);

            var root = _builder.XmlData.Elements().First(e => e.Name.LocalName == "TestRootElement");
            var eles = root.Elements().ToList();

            var i = 0;
            Assert.AreEqual(3, eles.Count);
            foreach (var ele in eles)
            {
                Assert.AreEqual(3, ele.Attributes().Count());
                var attributes = ele.Attributes().ToList();
                for(var a = 0; a < ele.Attributes().Count(); a++)
                {
                    Assert.AreEqual(attrs[a].Key, attributes[a].Name.LocalName);
                    Assert.AreEqual(attrs[a].Value, attributes[a].Value);
                }

                Assert.AreEqual(elements[i].Key,ele.Name.LocalName);
                Assert.AreEqual(elements[i].Value, ele.Value);
                i++;
            }


        }

        #endregion
    }
}
