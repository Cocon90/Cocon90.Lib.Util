using Cocon90.Lib.Util.Serialize.Tools.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cocon90.Lib.Util.Serialize.Tools
{
    public class RijndaelSerializer : ISerializer
    {
        private CryptoSerializerProvider myCryptoSerializer;
        public RijndaelSerializer(string password)
            : this(new XmlStringSerializer(), new Rfc2898DeriveBytes(password, new byte[]
		{
			1,
			80,
			5,
			10,
			15,
			254,
			9,
			18,
			43,
			180
		}), 128)
        {
        }
        public RijndaelSerializer(string password, ISerializer underlyingSerializer)
            : this(underlyingSerializer, new Rfc2898DeriveBytes(password, new byte[]
		{
			1,
			80,
			5,
			10,
			15,
			254,
			9,
			18,
			43,
			180
		}), 128)
        {
        }
        public RijndaelSerializer(string password, byte[] salt, ISerializer underlyingSerializer)
            : this(underlyingSerializer, new Rfc2898DeriveBytes(password, salt), 128)
        {
        }
        public RijndaelSerializer(string password, byte[] salt, ISerializer underlyingSerializer, int keyBitSize)
            : this(underlyingSerializer, new Rfc2898DeriveBytes(password, salt), keyBitSize)
        {
        }
        public RijndaelSerializer(ISerializer underlyingSerializer, DeriveBytes passwordBasedKeyGenerator, int keyBitSize)
        {
            this.myCryptoSerializer = new CryptoSerializerProvider(underlyingSerializer, passwordBasedKeyGenerator, keyBitSize);
        }
        public object Serialize<_T>(_T dataToSerialize)
        {
            object result;
            result = this.myCryptoSerializer.Serialize<_T>(dataToSerialize, new RijndaelManaged());
            return result;
        }
        public _T Deserialize<_T>(object serializedData)
        {
            _T result;
            _T t = this.myCryptoSerializer.Deserialize<_T>(serializedData, new RijndaelManaged());
            result = t;
            return result;
        }
    }
}
