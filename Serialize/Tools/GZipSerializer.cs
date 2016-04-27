using Cocon90.Lib.Util.Serialize.Tools.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Serialize.Tools
{
    public class GZipSerializer : ISerializer
    {
        private ISerializer myUnderlyingSerializer;
        private EncoderDecoder myEncoderDecoder = new EncoderDecoder();
        private string TracedObject
        {
            get
            {
                return base.GetType().Name + ' ';
            }
        }
        public GZipSerializer()
            : this(new XmlStringSerializer())
        {
        }
        public GZipSerializer(ISerializer underlyingSerializer)
        {
            this.myUnderlyingSerializer = underlyingSerializer;
        }
        public object Serialize<_T>(_T dataToSerialize)
        {
            object result;
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                    {
                        object dataToEncode = this.myUnderlyingSerializer.Serialize<_T>(dataToSerialize);
                        this.myEncoderDecoder.Encode(gZipStream, dataToEncode);
                    }
                    byte[] array = memoryStream.ToArray();
                    result = array;
                }
            }
            return result;
        }
        public _T Deserialize<_T>(object serializedData)
        {
            _T result;
            {
                byte[] buffer = (byte[])serializedData;
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        object serializedData2 = this.myEncoderDecoder.Decode(gZipStream);
                        _T t = this.myUnderlyingSerializer.Deserialize<_T>(serializedData2);
                        result = t;
                    }
                }
            }
            return result;
        }
    }
}
