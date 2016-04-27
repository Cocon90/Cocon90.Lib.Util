using Cocon90.Lib.Util.Serialize.Tools.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cocon90.Lib.Util.Serialize.Tools
{
    public class AesSerializer : ISerializer
    {
        private CryptoSerializerProvider myCryptoSerializer;
        public AesSerializer(string password)
            : this(new XmlStringSerializer(), new Rfc2898DeriveBytes(password, new byte[]
		{
			1,
			3,
			5,
			8,
			15,
			254,
			9,
			189,
			43,
			129
		}), 128)
        {
        }
        public AesSerializer(string password, ISerializer underlyingSerializer)
            : this(underlyingSerializer, new Rfc2898DeriveBytes(password, new byte[]
		{
			1,
			3,
			5,
			8,
			15,
			254,
			9,
			189,
			43,
			129
		}), 128)
        {
        }
        public AesSerializer(string password, byte[] salt, ISerializer underlyingSerializer)
            : this(underlyingSerializer, new Rfc2898DeriveBytes(password, salt), 128)
        {
        }
        public AesSerializer(string password, byte[] salt, ISerializer underlyingSerializer, int keyBitSize)
            : this(underlyingSerializer, new Rfc2898DeriveBytes(password, salt), keyBitSize)
        {
        }
        public AesSerializer(ISerializer underlyingSerializer, DeriveBytes passwordBasedKeyGenerator, int keyBitSize)
        {

            this.myCryptoSerializer = new CryptoSerializerProvider(underlyingSerializer, passwordBasedKeyGenerator, keyBitSize);

        }
        public AesSerializer(byte[] key, byte[] iv, ISerializer underlyingSerializer)
        {

            this.myCryptoSerializer = new CryptoSerializerProvider(underlyingSerializer, key, iv);

        }
        public object Serialize<_T>(_T dataToSerialize)
        {
            object result;
            result = this.myCryptoSerializer.Serialize<_T>(dataToSerialize, new AesManaged());
            return result;
        }
        public _T Deserialize<_T>(object serializedData)
        {
            _T result;
            _T t = this.myCryptoSerializer.Deserialize<_T>(serializedData, new AesManaged());
            result = t;
            return result;
        }
    }
}
