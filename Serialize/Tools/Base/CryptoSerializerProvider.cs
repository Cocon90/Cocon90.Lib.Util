using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cocon90.Lib.Util.Serialize.Tools.Base
{
    internal class CryptoSerializerProvider
    {
        private byte[] mySecretKey;
        private byte[] myInitializeVector;
        private ISerializer myUnderlyingSerializer;
        private EncoderDecoder myEncoderDecoder = new EncoderDecoder();
        private string TracedObject
        {
            get
            {
                return base.GetType().Name + ' ';
            }
        }
        public CryptoSerializerProvider(ISerializer underlyingSerializer, DeriveBytes passwordBasedKeyGenerator, int keyBitSize)
            : this(underlyingSerializer, passwordBasedKeyGenerator.GetBytes(keyBitSize / 8), passwordBasedKeyGenerator.GetBytes(16))
        {
        }
        public CryptoSerializerProvider(ISerializer underlyingSerializer, byte[] key, byte[] iv)
        {

            this.myUnderlyingSerializer = underlyingSerializer;
            this.mySecretKey = key;
            this.myInitializeVector = iv;

        }
        public object Serialize<_T>(_T dataToSerialize, SymmetricAlgorithm algorithm)
        {
            object result;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                algorithm.Key = this.mySecretKey;
                algorithm.IV = this.myInitializeVector;
                ICryptoTransform transform = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                {
                    object dataToEncode = this.myUnderlyingSerializer.Serialize<_T>(dataToSerialize);
                    this.myEncoderDecoder.Encode(cryptoStream, dataToEncode);
                }
                byte[] array = memoryStream.ToArray();
                result = array;
            }

            return result;
        }
        public _T Deserialize<_T>(object serializedData, SymmetricAlgorithm algorithm)
        {
            _T result;

            byte[] buffer = (byte[])serializedData;
            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                algorithm.Key = this.mySecretKey;
                algorithm.IV = this.myInitializeVector;
                ICryptoTransform transform = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
                {
                    object serializedData2 = this.myEncoderDecoder.Decode(cryptoStream);
                    _T t = this.myUnderlyingSerializer.Deserialize<_T>(serializedData2);
                    result = t;
                }
            }

            return result;
        }
    }
}
