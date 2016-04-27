using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Serialize.Tools.Base
{
    internal class EncoderDecoder
    {
        private readonly byte STRING_UTF8_ID = 10;
        private readonly byte STRING_UTF16_LE_ID = 20;
        private readonly byte STRING_UTF16_BE_ID = 30;
        private readonly byte BYTES_ID = 40;
        public void Encode(Stream writer, object dataToEncode)
        {

            if (dataToEncode is string)
            {
                byte[] bytes = Encoding.UTF8.GetBytes((string)dataToEncode);
                writer.WriteByte(this.STRING_UTF8_ID);
                writer.Write(bytes, 0, bytes.Length);
            }
            else
            {
                if (!(dataToEncode is byte[]))
                {
                    string message = "Encoding of data failed because the underlying serializer does not serialize to String or byte[].";
                    throw new InvalidOperationException(message);
                }
                writer.WriteByte(this.BYTES_ID);
                writer.Write((byte[])dataToEncode, 0, ((byte[])dataToEncode).Length);
            }

        }
        public object Decode(Stream reader)
        {
            object result;

            int num = reader.ReadByte();
            if (num == -1)
            {
                string message = "Decoding of serialized data failed because of unexpected end of stream.";
                throw new InvalidOperationException(message);
            }
            if (num != (int)this.STRING_UTF8_ID && num != (int)this.STRING_UTF16_LE_ID && num != (int)this.STRING_UTF16_BE_ID && num != (int)this.BYTES_ID)
            {
                string message2 = "Decoding of serialized data failed because of incorrect data fromat.";
                throw new InvalidOperationException(message2);
            }
            byte[] array2;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] array = new byte[32000];
                int count;
                while ((count = reader.Read(array, 0, array.Length)) > 0)
                {
                    memoryStream.Write(array, 0, count);
                }
                array2 = memoryStream.ToArray();
            }
            object obj;
            if (num == (int)this.STRING_UTF8_ID)
            {
                Encoding uTF = Encoding.UTF8;
                obj = uTF.GetString(array2, 0, array2.Length);
            }
            else
            {
                if (num == (int)this.STRING_UTF16_LE_ID)
                {
                    Encoding unicode = Encoding.Unicode;
                    obj = unicode.GetString(array2, 0, array2.Length);
                }
                else
                {
                    if (num == (int)this.STRING_UTF16_BE_ID)
                    {
                        Encoding bigEndianUnicode = Encoding.BigEndianUnicode;
                        obj = bigEndianUnicode.GetString(array2, 0, array2.Length);
                    }
                    else
                    {
                        if (num != (int)this.BYTES_ID)
                        {
                            string message3 = "Decoding of serialized data failed because of incorrect data fromat.";
                            throw new InvalidOperationException(message3);
                        }
                        obj = array2;
                    }
                }
            }
            result = obj;

            return result;
        }
    }
}
