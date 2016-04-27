using Cocon90.Lib.Util.Serialize.Tools.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Cocon90.Lib.Util.Serialize.Tools
{
    public class RsaDigitalSignatureSerializer : ISerializer
    {
        private ISerializer myUnderlyingSerializer;
        private EncoderDecoder myEncoderDecoder = new EncoderDecoder();
        private byte[] myPublicCertificate;
        private X509Certificate2 mySignerCertificate;
        private Func<X509Certificate2, bool> myVerifySignerCertificate;
        private string TracedObject
        {
            get
            {
                return base.GetType().Name + ' ';
            }
        }
        public RsaDigitalSignatureSerializer(X509Certificate2 signerCertificate)
            : this(signerCertificate, null, new XmlStringSerializer())
        {
        }
        public RsaDigitalSignatureSerializer(X509Certificate2 signerCertificate, Func<X509Certificate2, bool> verifySignerCertificate, ISerializer underlyingSerializer)
        {
            if (signerCertificate != null && !(signerCertificate.PrivateKey is RSACryptoServiceProvider))
            {
                throw new ArgumentException("The input parameter X509Certificate2 does not contain the private key of type RSACryptoServiceProvider.");
            }
            this.mySignerCertificate = signerCertificate;
            this.myPublicCertificate = signerCertificate.Export(X509ContentType.Cert);
            this.myVerifySignerCertificate = ((verifySignerCertificate == null) ? new Func<X509Certificate2, bool>(this.VerifySignerCertificate) : verifySignerCertificate);
            this.myUnderlyingSerializer = underlyingSerializer;
        }
        public object Serialize<_T>(_T dataToSerialize)
        {
            object result;
            {
                if (this.mySignerCertificate == null)
                {
                    throw new InvalidOperationException(this.TracedObject + "failed to serialize data. The signer certificate is null and thus the serializer can be used only for deserialization.");
                }
                byte[][] array = new byte[3][];
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    object dataToEncode = this.myUnderlyingSerializer.Serialize<_T>(dataToSerialize);
                    this.myEncoderDecoder.Encode(memoryStream, dataToEncode);
                    array[0] = memoryStream.ToArray();
                }
                SHA1Managed sHA1Managed = new SHA1Managed();
                byte[] rgbHash = sHA1Managed.ComputeHash(array[0]);
                RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)this.mySignerCertificate.PrivateKey;
                array[2] = rSACryptoServiceProvider.SignHash(rgbHash, CryptoConfig.MapNameToOID("SHA1"));
                array[1] = this.myPublicCertificate;
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
                X509Certificate2 x509Certificate = new X509Certificate2(array[1]);
                if (!this.myVerifySignerCertificate(x509Certificate))
                {
                    throw new InvalidOperationException(this.TracedObject + "failed to deserialize data because the verification of signer certificate failed.");
                }
                SHA1Managed sHA1Managed = new SHA1Managed();
                byte[] rgbHash = sHA1Managed.ComputeHash(array[0]);
                RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)x509Certificate.PublicKey.Key;
                if (!rSACryptoServiceProvider.VerifyHash(rgbHash, CryptoConfig.MapNameToOID("SHA1"), array[2]))
                {
                    throw new InvalidOperationException(this.TracedObject + "failed to deserialize data because the signature verification failed.");
                }
                using (MemoryStream memoryStream = new MemoryStream(array[0], 0, array[0].Length))
                {
                    object serializedData2 = this.myEncoderDecoder.Decode(memoryStream);
                    _T t = this.myUnderlyingSerializer.Deserialize<_T>(serializedData2);
                    result = t;
                }
            }
            return result;
        }
        private bool VerifySignerCertificate(X509Certificate2 signerCertificate)
        {
            return signerCertificate.Verify();
        }
    }
}
