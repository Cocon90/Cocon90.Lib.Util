using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Cocon90.Lib.Util.Serialize
{
    /// <summary>
    /// 二进制序列化辅助类
    /// </summary>
    public class binarySerialize
    {
        /// <summary>
        /// 将对像序列化为byte数组
        /// </summary>
        /// <param name="modelObject"></param>
        /// <returns></returns>
        public static byte[] Serialize(object modelObject)
        {
            IFormatter formatter = new BinaryFormatter();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            formatter.Serialize(ms, modelObject);//将对像序列化到ms中。
            var bys = ms.ToArray();
            ms.Close();
            return bys;
        }
        /// <summary>
        /// 将对像序列化为Base64字符串
        /// </summary>
        /// <param name="modelObject"></param>
        /// <returns></returns>
        public static string SerializeToString(object modelObject)
        {
            byte[] bys = Serialize(modelObject);
            return Convert.ToBase64String(bys, 0, bys.Length);
        }
        /// <summary>
        /// 将序列化后字节数组转为对像。
        /// </summary>
        /// <param name="serializeByte"></param>
        /// <returns></returns>
        public static object Deserialize(byte[] serializeByte)
        {
            IFormatter formatter = new BinaryFormatter();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(serializeByte);
            var obj = formatter.Deserialize(ms);
            ms.Close();
            return obj;
        }
        /// <summary>
        /// 将序列化后的BASE64编码字符串转为对像
        /// </summary>
        /// <param name="serializeString"></param>
        /// <returns></returns>
        public static object DeserializeFormString(string serializeString)
        {
            var bys = Convert.FromBase64String(serializeString);
            return Deserialize(bys);
        }
    }
}
