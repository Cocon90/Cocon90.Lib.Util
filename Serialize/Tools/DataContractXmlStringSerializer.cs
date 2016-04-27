using Cocon90.Lib.Util.Serialize.Tools.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Cocon90.Lib.Util.Serialize.Tools
{
    public class DataContractXmlStringSerializer : ISerializer
    {
        private Func<Type, XmlObjectSerializer> myDataContractFactoryMethod;
        public DataContractXmlStringSerializer()
            : this((Type x) => new DataContractSerializer(x))
        {
        }
        public DataContractXmlStringSerializer(Func<Type, XmlObjectSerializer> dataContractFactoryMethod)
        {
            this.myDataContractFactoryMethod = dataContractFactoryMethod;
        }
        public object Serialize<_T>(_T dataToSerialize)
        {
            object result;
            {
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
                            XmlObjectSerializer xmlObjectSerializer = this.myDataContractFactoryMethod(typeof(_T));
                            xmlObjectSerializer.WriteObject(xmlWriter, dataToSerialize);
                        }
                        text = stringWriter.ToString();
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
                result = text;
            }
            return result;
        }
        public _T Deserialize<_T>(object serializedData)
        {
            _T result;
            {
                string s = (string)serializedData;
                try
                {
                    using (StringReader stringReader = new StringReader(s))
                    {
                        using (XmlReader xmlReader = XmlReader.Create(stringReader))
                        {
                            XmlObjectSerializer xmlObjectSerializer = this.myDataContractFactoryMethod(typeof(_T));
                            result = (_T)((object)xmlObjectSerializer.ReadObject(xmlReader));
                        }
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
            return result;
        }
    }
}
