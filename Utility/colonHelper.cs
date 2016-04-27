using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Utility
{
    /// <summary>
    /// 克隆辅助类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class cloneHelper<T>
    {
        private cloneHelper() { }
        /// <summary>
        /// 克隆模型 取得新实例。
        /// </summary>
        /// <param name="sourceModel"></param>
        /// <returns></returns>
        public static T CloneModel(T sourceModel, bool isNullClone = true)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var newModel = (T)Activator.CreateInstance(type);
            foreach (var prop in properties)
            {
                if (prop.CanRead)
                {
                    var oldPropValue = prop.GetValue(sourceModel, null);
                    if (prop.CanWrite)
                    {
                        if (isNullClone)
                        {
                            prop.SetValue(newModel, oldPropValue, null);
                        }
                        else
                        {
                            if (oldPropValue != null) { prop.SetValue(newModel, oldPropValue, null); }
                        }
                    }
                }
            }
            return newModel;
        }
        /// <summary>
        /// 克隆到子类
        /// </summary>
        /// <param name="superModel"></param>
        /// <returns></returns>
        public static T CloneToSonModel(object superModel, bool isNullClone = true)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var newModel = (T)Activator.CreateInstance(type);
            foreach (var prop in properties)
            {
                foreach (var sunProp in superModel.GetType().GetProperties())
                {
                    if (sunProp.Name == prop.Name)
                    {
                        if (sunProp.CanRead && prop.CanWrite)
                        {
                            var val = sunProp.GetValue(superModel, null);
                            if (isNullClone)
                            {
                                prop.SetValue(newModel, val, null);
                            }
                            else
                            {
                                if (val != null)
                                {
                                    prop.SetValue(newModel, val, null);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return newModel;
        }
        /// <summary>
        /// 超类集合 克隆到 子类集合
        /// </summary>
        /// <returns></returns>
        public static List<T> CloneToSonModelList(IList superModelList, bool isNullClone = true)
        {
            List<T> lst = new List<T>();
            if (superModelList == null) { return lst; }
            foreach (var item in superModelList)
            {
                lst.Add(CloneToSonModel(item, isNullClone));
            }
            return lst;
        }
        /// <summary>
        /// 子类集合 克隆到 超类集合
        /// </summary>
        /// <returns></returns>
        public static List<T> CloneToSuperModelList(IList sonModelList, bool isNullClone = true)
        {
            List<T> lst = new List<T>();
            if (sonModelList == null) { return lst; }
            foreach (var item in sonModelList)
            {
                lst.Add(CloneToSuperModel(item, isNullClone));
            }
            return lst;
        }
        /// <summary>
        /// 克隆到超类
        /// </summary>
        /// <param name="sonModel"></param>
        /// <returns></returns>
        public static T CloneToSuperModel(object sonModel, bool isNullClone = true)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var newModel = (T)Activator.CreateInstance(type);
            foreach (var prop in properties)
            {
                foreach (var sunProp in sonModel.GetType().GetProperties())
                {
                    if (sunProp.Name == prop.Name)
                    {
                        if (sunProp.CanRead && prop.CanWrite)
                        {
                            var val = sunProp.GetValue(sonModel, null);
                            if (isNullClone)
                            {
                                prop.SetValue(newModel, val, null);
                            }
                            else
                            {
                                if (val != null)
                                {
                                    prop.SetValue(newModel, val, null);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return newModel;
        }
        /// <summary>
        /// 将fromModel中的可读可写属性值赋给toModel，第三参isNullClone表示是否将fromModel中的Null一块克隆过去，如果值为null则不克隆该属性
        /// </summary>
        /// <param name="fromModel"></param>
        /// <param name="toModel"></param>
        public static void Clone(T fromModel, T toModel, bool isNullClone = true)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            foreach (var prop in properties)
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    var value = prop.GetValue(fromModel, null);
                    if (isNullClone)
                    {
                        prop.SetValue(toModel, value, null);
                    }
                    else
                    {
                        if (value != null)
                        {
                            prop.SetValue(toModel, value, null);
                        }
                    }

                }
            }
        }
        /// <summary>
        /// 将modelList中的实体的指定属性复制到T类型的List集体中并返回。
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static List<T> ClonePropertyToList(IList modelList, string propertyName, HandlerPropertyDelegate handlerPropertyDelegate = null)
        {
            List<T> lst = new List<T>();
            if (modelList == null) return lst;
            foreach (var item in modelList)
            {
                var property = item.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    var val = property.GetValue(item, null);
                    if (handlerPropertyDelegate != null)
                    {
                        lst.Add(handlerPropertyDelegate((T)val));
                    }
                    else { lst.Add((T)val); }
                }
            }
            return lst;
        }
        /// <summary>
        /// 属性加工处理函数
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public delegate T HandlerPropertyDelegate(T propertyValue);
    }

    public class cloneHelper
    {
        /// <summary>
        /// 将fromModel中的可读可写属性值赋给toModel，第三参isNullClone表示是否将fromModel中的Null一块克隆过去，如果值为null则不克隆该属性
        /// </summary>
        /// <param name="fromModel"></param>
        /// <param name="toModel"></param>
        public static void Clone(object fromModel, object toModel, bool isNullClone = true)
        {
            var type = fromModel.GetType();
            var properties = type.GetProperties();
            foreach (var prop in properties)
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    var value = prop.GetValue(fromModel, null);
                    var propTarget = toModel.GetType().GetProperty(prop.Name);
                    if (propTarget == null || (!propTarget.CanWrite)) continue;
                    if (isNullClone)
                    {
                        propTarget.SetValue(toModel, value, null);
                    }
                    else
                    {
                        if (value != null)
                        {
                            propTarget.SetValue(toModel, value, null);
                        }
                    }

                }
            }
        }
    }
}
