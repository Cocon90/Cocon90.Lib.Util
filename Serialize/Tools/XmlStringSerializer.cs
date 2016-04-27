using Cocon90.Lib.Util.Serialize.Tools.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Cocon90.Lib.Util.Serialize.Tools
{

    public class XmlStringSerializer : ISerializer
    {
        private Func<Type, XmlSerializer> myXmlSerializerFactoryMethod;
        public XmlStringSerializer()
            : this((Type x) => new XmlSerializer(x))
        {
        }
        public XmlStringSerializer(Func<Type, XmlSerializer> xmlSerializerFactoryMethod)
        {

            this.myXmlSerializerFactoryMethod = xmlSerializerFactoryMethod;

        }
        public object Serialize<_T>(_T dataToSerialize)
        {
            object result;

            string text = "";
            try
            {
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
                    {
                        OmitXmlDeclaration = true
                    }))
                    {
                        XmlSerializer xmlSerializer = this.myXmlSerializerFactoryMethod(typeof(_T));
                        xmlSerializer.Serialize(xmlWriter, dataToSerialize);
                    }
                    text = stringWriter.ToString();
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            result = text;

            return result;
        }
        public _T Deserialize<_T>(object serializedData)
        {
            _T result;

            string s = (string)serializedData;
            try
            {
                using (StringReader stringReader = new StringReader(s))
                {
                    XmlSerializer xmlSerializer = this.myXmlSerializerFactoryMethod(typeof(_T));
                    result = (_T)((object)xmlSerializer.Deserialize(stringReader));
                }
            }
            catch (Exception err)
            {
                throw err;
            }

            return result;
        }
    }
}
