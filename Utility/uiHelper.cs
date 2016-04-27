using Cocon90.Lib.Util.Parse;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cocon90.Lib.Util.Utility
{
    public class uiHelper<T>
    {
        /// <summary>
        /// 将界面上controlPanel容器内的控件值付给T实体并反回。
        /// 1、如果实器内的控件为Label则自动跳过不处理。
        /// 2、如果容器内的控件的Name和实体中的属性值相同(不区分大小写)，则自动取其Text属性并赋给实体；
        /// 3、如果容器内的控件Name是以：实体属性名+"控件属性名"名称，则自动取期该属性然后赋给实体；如果是常见8种数据类型和DateTime类型，则进行数据转换后再赋值，如果转换失败，则使用Tag中的值，若Tag中的值也转换失败，则使用0或DateTime.Now做为默认值
        /// </summary>
        public static T getModelFromUI(Control controlPanel)
        {
            var model = (T)Activator.CreateInstance(typeof(T), null);
            if (controlPanel == null) return model;
            foreach (Control item in controlPanel.Controls)
            {
                if (item is Label) continue;
                foreach (var prop in model.GetType().GetProperties())
                {
                    if (item.Name.ToLower() == (prop.Name + "").ToLower())
                    {//若是pName这种写法处理（默认类型为String，属性为Text）
                        var valueProp = item.GetType().GetProperty("Text");
                        if (valueProp != null && valueProp.CanRead)
                        {
                            if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(String))
                            {
                                var num = valueProp.GetValue(item, null) + "";
                                prop.SetValue(model, num, null);
                                break;
                            }
                        }
                    }
                    else if ((item.Name + "").ToLower().StartsWith(prop.Name.ToLower()))
                    {
                        //var param = item.Name.TrimStart(prop.Name.ToLower().ToCharArray());//.Net 4.5中不区分大小写。
                        var param = item.Name.Substring(prop.Name.Length);//适用于.Net4.5以下的版本。
                        //如pNameText;//取Text并转为String类型
                        var valueProp = item.GetType().GetProperty(param);
                        if (valueProp != null && valueProp.CanRead)
                        {
                            if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(String))
                            {
                                var pm = valueProp.GetValue(item, null);
                                if (pm != null && pm.GetType() == typeof(Color)) { prop.SetValue(model, parseHelper.toColor((Color)pm), null); }
                                else
                                {
                                    var num = pm + "";
                                    prop.SetValue(model, num, null);
                                }
                                break;
                            }
                            else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                            {
                                var num = parseHelper.toInt(valueProp.GetValue(item, null) + "", parseHelper.toInt(item.GetType().GetProperty("Tag").GetValue(item, null) + "", 0));
                                prop.SetValue(model, num, null);
                                break;
                            }
                            else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                            {
                                var num = parseHelper.toDouble(valueProp.GetValue(item, null) + "", parseHelper.toDouble(item.GetType().GetProperty("Tag").GetValue(item, null) + "", 0d));
                                prop.SetValue(model, num, null);
                                break;
                            }
                            else if (prop.PropertyType == typeof(float) || prop.PropertyType == typeof(float?))
                            {
                                var num = parseHelper.toFloat(valueProp.GetValue(item, null) + "", parseHelper.toFloat(item.GetType().GetProperty("Tag").GetValue(item, null) + "", 0f));
                                prop.SetValue(model, num, null);
                                break;
                            }
                            else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
                            {
                                var num = parseHelper.toLong(valueProp.GetValue(item, null) + "", parseHelper.toLong(item.GetType().GetProperty("Tag").GetValue(item, null) + "", 0l));
                                prop.SetValue(model, num, null);
                                break;
                            }
                            else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                            {
                                var num = parseHelper.toBool(valueProp.GetValue(item, null) + "");
                                prop.SetValue(model, num, null);
                                break;
                            }
                            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                            {
                                var num = parseHelper.toDateTime(valueProp.GetValue(item, null) + "", parseHelper.toDateTime(item.GetType().GetProperty("Tag").GetValue(item, null) + "", DateTime.Now));
                                prop.SetValue(model, num, null);
                                break;
                            }
                        }
                    }
                }
            }
            return model;
        }
        /// <summary>
        /// 将model的属性值付给界面上controlPanel容器内的控件。
        /// 1、如果实器内的控件为Label则自动跳过不处理。
        /// 2、如果容器内的控件的Name和实体中的属性值相同(不区分大小写)，则自动给其Text属性赋；
        /// 3、如果容器内的控件Name是以：实体属性名+"控件属性名"名称，则自动赋值给该属性；如果控件的属性值是常见8种数据类型和DateTime类型，则进行数据转换后再赋值，如果转换失败，则使用Tag中的值，若Tag中的值也转换失败，则使用0或DateTime.Now做为默认值
        /// </summary>
        public static void setModelToUI(Control controlPanel, T model)
        {
            if (model == null) return;
            foreach (var prop in model.GetType().GetProperties())
            {
                if (!prop.CanRead) continue;
                var value = prop.GetValue(model, null);
                foreach (Control item in controlPanel.Controls)
                {
                    if (item is Label) continue;
                    if (item.Name.ToLower() == prop.Name.ToLower())
                    {
                        item.Text = value + "";
                        break;
                    }
                    else if (item.Name.ToLower().StartsWith(prop.Name.ToLower()))
                    {
                        //var param = item.Name.TrimStart(prop.Name.ToLower().ToCharArray());//.Net 4.5中不区分大小写。
                        var param = item.Name.Substring(prop.Name.Length);//适用于.Net4.5以下的版本。
                        if ((param + "").Trim() != null)
                        {
                            var paramType = item.GetType().GetProperty(param);
                            if (paramType != null && paramType.PropertyType == typeof(string))
                            {
                                paramType.SetValue(item, prop.GetValue(model, null) + "", null);
                                break;
                            }
                            else if (paramType != null && (paramType.PropertyType == typeof(int) || paramType.PropertyType == typeof(int?)))
                            {
                                paramType.SetValue(item, parseHelper.toInt(prop.GetValue(model, null) + "", parseHelper.toInt(item.GetType().GetProperty("Tag").GetValue(item, null) + "", 0)), null);
                                break;
                            }
                            else if (paramType != null && (paramType.PropertyType == typeof(double) || paramType.PropertyType == typeof(double?)))
                            {
                                paramType.SetValue(item, parseHelper.toDouble(prop.GetValue(model, null) + "", parseHelper.toDouble(item.GetType().GetProperty("Tag").GetValue(item, null) + "", 0d)), null);
                                break;
                            }
                            else if (paramType != null && (paramType.PropertyType == typeof(float) || paramType.PropertyType == typeof(float?)))
                            {
                                paramType.SetValue(item, parseHelper.toFloat(prop.GetValue(model, null) + "", parseHelper.toFloat(item.GetType().GetProperty("Tag").GetValue(item, null) + "", 0f)), null);
                                break;
                            }
                            else if (paramType != null && (paramType.PropertyType == typeof(long) || paramType.PropertyType == typeof(long?)))
                            {
                                paramType.SetValue(item, parseHelper.toLong(prop.GetValue(model, null) + "", parseHelper.toLong(item.GetType().GetProperty("Tag").GetValue(item, null) + "", 0l)), null);
                                break;
                            }
                            else if (paramType != null && (paramType.PropertyType == typeof(decimal) || paramType.PropertyType == typeof(decimal?)))
                            {
                                paramType.SetValue(item, (decimal)parseHelper.toDouble(prop.GetValue(model, null) + "", parseHelper.toDouble(item.GetType().GetProperty("Tag").GetValue(item, null) + "", 0)), null);
                                break;
                            }
                            else if (paramType != null && (paramType.PropertyType == typeof(DateTime) || paramType.PropertyType == typeof(DateTime?)))
                            {
                                paramType.SetValue(item, parseHelper.toDateTime(prop.GetValue(model, null) + "", parseHelper.toDateTime(item.GetType().GetProperty("Tag").GetValue(item, null) + "", DateTime.Now)), null);
                                break;
                            }
                            else if (paramType != null && (paramType.PropertyType == typeof(Color) || paramType.PropertyType == typeof(Color?)))
                            {
                                var modelValue = prop.GetValue(model, null);
                                if (modelValue != null && (modelValue.GetType() == typeof(Color) || modelValue.GetType() == typeof(Color?)))
                                {
                                    paramType.SetValue(item, modelValue, null);
                                }
                                else if (modelValue != null && modelValue.GetType() == typeof(string))
                                {
                                    paramType.SetValue(item, parseHelper.toColor(modelValue + ""), null);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
