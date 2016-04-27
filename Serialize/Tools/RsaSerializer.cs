using Cocon90.Lib.Util.Serialize.Tools.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cocon90.Lib.Util.Serialize.Tools
{
    public class RsaSerializer : ISerializer
    {
        private ISerializer myUnderlyingSerializer;
        private int myAesBitSize;
        private RSAParameters myPrivateKey;
        private RSAParameters myPublicKey;
        public RsaSerializer(RSAParameters publicKey, RSAParameters privateKey)
            : this(publicKey, privateKey, 128, new XmlStringSerializer())
        {
        }
        public RsaSerializer(RSAParameters publicKey, RSAParameters privateKey, int aesBitSize, ISerializer underlyingSerializer)
        {
            {
                this.myPublicKey = publicKey;
                this.myPrivateKey = privateKey;
                this.myAesBitSize = aesBitSize;
                this.myUnderlyingSerializer = underlyingSerializer;
            }
        }
        public object Serialize<_T>(_T dataToSerialize)
        {
            object result;
            {
                byte[][] array = new byte[3][];
                AesManaged aesManaged = new AesManaged();
                aesManaged.KeySize = this.myAesBitSize;
                aesManaged.GenerateIV();
                aesManaged.GenerateKey();
                AesSerializer aesSerializer = new AesSerializer(aesManaged.Key, aesManaged.IV, this.myUnderlyingSerializer);
                array[2] = (byte[])aesSerializer.Serialize<_T>(dataToSerialize);
                RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
                rSACryptoServiceProvider.ImportParameters(this.myPublicKey);
                array[0] = rSACryptoServiceProvider.Encrypt(aesManaged.Key, false);
                array[1] = rSACryptoServiceProvider.Encrypt(aesManaged.IV, false);
                object obj = this.myUnderlyingSerializer.Serialize<byte[][]>(array);
                result = obj;
            }
            return result;
        }
        public _T Deserialize<_T>(object serializedData)
        {
            _T result;
            {
                byte[][] array = this.myUnderlyingSerializer.Deserialize<byte[][]>(serializedData);
                RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
                rSACryptoServiceProvider.ImportParameters(this.myPrivateKey);
                byte[] key = rSACryptoServiceProvider.Decrypt(array[0], false);
                byte[] iv = rSACryptoServiceProvider.Decrypt(array[1], false);
                AesSerializer aesSerializer = new AesSerializer(key, iv, this.myUnderlyingSerializer);
                _T t = aesSerializer.Deserialize<_T>(array[2]);
                result = t;
            }
            return result;
        }
    }
}
