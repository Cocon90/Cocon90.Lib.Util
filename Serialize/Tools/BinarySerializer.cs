using Cocon90.Lib.Util.Serialize.Tools.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Cocon90.Lib.Util.Serialize.Tools
{
    public class BinarySerializer : ISerializer
    {
        private class Binder<T> : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                return typeof(T);
            }
        }
        private bool myEnforceTypeBinding;
        private static byte[] myNull = new byte[0];
        public BinarySerializer()
            : this(false)
        {
        }
        public BinarySerializer(bool enforceTypeBinding)
        {
            this.myEnforceTypeBinding = enforceTypeBinding;
        }
        public object Serialize<_T>(_T dataToSerialize)
        {
            object result;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    if (dataToSerialize == null)
                    {
                        result = BinarySerializer.myNull;
                    }
                    else
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        binaryFormatter.Serialize(memoryStream, dataToSerialize);
                        byte[] array = memoryStream.ToArray();
                        result = array;
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        public _T Deserialize<_T>(object serializedData)
        {
            _T result;
            byte[] array = (byte[])serializedData;
            if (array.Length == 0)
            {
                result = default(_T);
            }
            else
            {
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream(array))
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        if (this.myEnforceTypeBinding)
                        {
                            binaryFormatter.Binder = new BinarySerializer.Binder<_T>();
                        }
                        _T t = (_T)((object)binaryFormatter.Deserialize(memoryStream));
                        result = t;
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
