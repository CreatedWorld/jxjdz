using System.Text;
using LZR.Data.LtiJson;

namespace LZR.Data.TypeConversion
{
    public class FromBytes
    {
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string GetString(byte[] bytes,Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 获取JsonData对象
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static JsonData GetJson(byte[] bytes, Encoding encoding)
        {
            return JsonMapper.ToObject(GetString(bytes,encoding));
        }
    }
}
