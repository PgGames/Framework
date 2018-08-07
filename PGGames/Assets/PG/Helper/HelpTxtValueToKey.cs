using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Manager;

namespace PG.Help
{
    public class HelpTxtValueToKey : MonoBehaviour
    {
        public string Key;
        public Text m_Content;

        public void Awake()
        {
            LanguageManager.GetManager.Add(GetValueToKey);
            if (m_Content == null)
                m_Content = Helper.GetComponent<Text>(this.transform, null);
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