using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Manager;

namespace PG.Help
{
    public class HelpTxtValueToKey : MonoBehaviour
    {
        [SerializeField]
        private string Key;
        //private string privateKey;
        public Text m_Content;

        public void Awake()
        {
            LanguageManager.GetManager.Add(GetValueToKey);
            if (m_Content == null)
                m_Content = Helper.GetComponent<Text>(this.transform, null);
            //privateKey = Key;
            GetValueToKey();
        }
        //private void Update()
        //{
        //    if (privateKey != Key)
        //    {
        //        GetValueToKey();
        //        privateKey = Key;
        //    }
        //}
        public void SettingKey(string varKey)
        {
            Key = varKey;
            GetValueToKey();
        }
        public void GetValueToKey()
        {
            string Value = LanguageManager.GetManager.GetValueToKey(Key);
            if (m_Content != null)
                m_Content.text = Value;
        }
    }
}