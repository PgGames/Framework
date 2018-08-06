//===========================================================
/* 文本的有效格式 Text Key = Value;
 *      key 于 Value 形成键值对的格式
 *      key中不能有空格的出现
 *      key值只能有字母数字和下划线构成
 *      Value 值的前后不可有空格
 *      Value 值使用[]
 */
//===========================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PG.Manager
{
    public class LanguageManager : MonoBehaviour
    {


        public TextAsset m_Chinese;
        public TextAsset m_English;

        public enum LanguageType
        {
            Chinese,
            English,
        }
        public LanguageType m_Language;


        protected Dictionary<string, string> Dic_Chinese = new Dictionary<string, string>();
        protected Dictionary<string, string> Dic_English = new Dictionary<string, string>();

        private void Awake()
        {
            ReadText();
        }

        private void ReadText()
        {
            ReadTextAsset(m_Chinese, Dic_Chinese);
            ReadTextAsset(m_English, Dic_English);
        }
        /// <summary>
        /// 读取Txt文本
        /// </summary>
        /// <param name="varTextAsset"></param>
        /// <param name="varDic"></param>
        private void ReadTextAsset(TextAsset varTextAsset, Dictionary<string, string> varDic)
        {
            if (varTextAsset == null)
                return;
            string temp_Content = varTextAsset.text.Replace("\r\n", "\f");

            string[] varContent = temp_Content.Split('\f');
            if (varContent == null)
                return;
            List<string> LanguageList = RemovedNullString(varContent);
            if (LanguageList.Count == 0)
                return;
            for (int i = 0; i < LanguageList.Count; i++)
            {
                string Temp_Content = LanguageList[i];

                if (!Temp_Content.Contains("Text"))
                    continue;
                string Key;
                string Value = "";
                string[] TempLanguage = Temp_Content.Split('=');
                if (Temp_Content.Length < 2)
                    continue;
                Key = TempLanguage[0];
                if (!TestingKeyLegal(ref Key))
                    continue;
                for (int jx = 1; jx < TempLanguage.Length; jx++)
                {
                    if (jx == 1)
                        Value += TempLanguage[jx];
                    else
                        Value += "=" + TempLanguage[jx];
                }
                Value = Value.Trim();
                varDic.Add(Key, Value);
            }
        }
        /// <summary>
        /// 移除数组中无效的字符串
        /// </summary>
        /// <param name="varContent"></param>
        /// <returns></returns>
        private List<string> RemovedNullString(string[] varContent)
        {
            List<string> LanguageList = new List<string>();
            for (int i = 0; i < varContent.Length; i++)
            {
                if (string.IsNullOrEmpty(varContent[i]))
                    continue;
                LanguageList.Add(varContent[i]);
            }
            return LanguageList;
        }
        /// <summary>
        /// 检测Key的合法性
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        private bool TestingKeyLegal(ref string Key)
        {
            //string TempKey = Key;
            string[] content = Key.Split(' ');
            if (content.Length != 2)
                return false;
            if (content[0] != "Text")
                return false;
            Key = content[1];

            string TestingString = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_";
            char[] tempkey = Key.ToCharArray();
            for (int i = 0; i < tempkey.Length; i++)
            {
                char ms = tempkey[i];
                if (!TestingString.Contains(ms.ToString()))
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 检测Value值的合法性
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private bool TestingValueLegal(ref string Value)
        {
            //先移除两头无效的空格
            Value = Value.Trim();
            //当值中存在中括号时，移除两头的中括号
            if (Value.Contains("[") && Value.Contains("]"))
                Value.Trim('[', ']');
            return true;
        }

        public string GetValueToKey(string Key)
        {
            switch (m_Language)
            {
                case LanguageType.Chinese:
                    return GetValue(Key, Dic_Chinese);
                case LanguageType.English:
                    return GetValue(Key, Dic_English);
                default:
                    break;
            }
            return "";
        }
        private string GetValue(string Key, Dictionary<string, string> varDic)
        {
            if (!varDic.ContainsKey(Key))
                return Key;
            string Value = "";
            if (varDic.TryGetValue(Key, out Value))
                return Value;
            return Key;
        }
    }

}