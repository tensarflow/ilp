///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System.Xml.Linq;
using System.Xml.XPath;
using com.espertech.esper.client;
using com.espertech.esper.compat.container;
using com.espertech.esper.core.support;
using com.espertech.esper.events.xml;
using com.espertech.esper.supportunit.events;
using com.espertech.esper.supportunit.util;
using NUnit.Framework;

namespace com.espertech.esper.events.xelement
{
    [TestFixture]
    public class TestSimpleXEventType
    {
        private const string Xml = "<simpleEvent>\n" +
                                   "\t<nested1 attr1=\"SAMPLE_ATTR1\">\n" +
                                   "\t\t<prop1>SAMPLE_V1</prop1>\n" +
                                   "\t\t<prop2>true</prop2>\n" +
                                   "\t\t<nested2>\n" +
                                   "\t\t\t<prop3>3</prop3>\n" +
                                   "\t\t\t<prop3>4</prop3>\n" +
                                   "\t\t\t<prop3>5</prop3>\n" +
                                   "\t\t</nested2>\n" +
                                   "\t</nested1>\n" +
                                   "\t<prop4 attr2=\"true\">SAMPLE_V6</prop4>\n" +
                                   "\t<nested3>\n" +
                                   "\t\t<nested4 id=\"a\">\n" +
                                   "\t\t\t<prop5>SAMPLE_V7</prop5>\n" +
                                   "\t\t\t<prop5>SAMPLE_V8</prop5>\n" +
                                   "\t\t</nested4>\n" +
                                   "\t\t<nested4 id=\"b\">\n" +
                                   "\t\t\t<prop5>SAMPLE_V9</prop5>\n" +
                                   "\t\t</nested4>\n" +
                                   "\t\t<nested4 id=\"c\">\n" +
                                   "\t\t\t<prop5>SAMPLE_V10</prop5>\n" +
                                   "\t\t\t<prop5>SAMPLE_V11</prop5>\n" +
                                   "\t\t</nested4>\n" +
                                   "\t</nested3>\n" +
                                   "</simpleEvent>";

        private EventBean _event;
        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            _container = SupportContainer.Reset();

            var simpleDoc = XDocument.Parse(Xml);

            var config = new ConfigurationEventTypeXMLDOM();
            config.RootElementName = "simpleEvent";
            config.AddXPathProperty("customProp", "count(/simpleEvent/nested3/nested4)", XPathResultType.Number);

            var eventType = new SimpleXElementType(null, 1, config, _container.Resolve<EventAdapterService>(), _container.LockManager());
            _event = new XEventBean(simpleDoc.Root, eventType);
        }

        [Test]
        public void TestCustomProperty()
        {
            Assert.AreEqual(typeof(double?), _event.EventType.GetPropertyType("customProp"));
            Assert.AreEqual(3.0d, _event.Get("customProp"));
        }

        [Test]
        public void TestIndexedProperties()
        {
            Assert.AreEqual("5", _event.Get("nested1.nested2.prop3[2]"));
            Assert.AreEqual(typeof(string), _event.EventType.GetPropertyType("nested1.nested2.prop3[2]"));
        }

        [Test]
        public void TestMappedProperties()
        {
            Assert.AreEqual("SAMPLE_V8", _event.Get("nested3.nested4('a').prop5[1]"));
            Assert.AreEqual("SAMPLE_V10", _event.Get("nested3.nested4('c').prop5[0]"));
        }

        [Test]
        public void TestNestedProperties()
        {
            Assert.AreEqual("true", _event.Get("nested1.prop2"));
        }

        [Test]
        public void TestSimpleProperies()
        {
            Assert.AreEqual("SAMPLE_V6", _event.Get("prop4"));
        }
    }
}
