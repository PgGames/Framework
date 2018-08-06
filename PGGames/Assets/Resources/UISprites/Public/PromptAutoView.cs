using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Model;
using PG.Help;
using PG.Manager;

namespace PG.UI
{
    public class PromptAutoView : BaseView
    {
        Text m_Prompt_Text;
        public override void Init()
        {
            FindTran();
            base.Init();
        }
        protected void FindTran()
        {
            m_Prompt_Text = Helper.GetComponent<Text>(this.transform, "Root/Title");
        }
        public void OpenUI(string text)
        {

            if (m_Prompt_Text != null)
                m_Prompt_Text.text = text;
            m_Start = true;
        }
        bool m_Start = false;
        float m_Times = 0;
        private void Update()
        {
            if (m_Start)
            {
                m_Times += Time.deltaTime;
                if (m_Times >= Constant.Time_PromptAuto)
                {
                    CloseUI();
                }
            }
        }
        public void CloseUI()
        {
            m_Start = false;
            m_Times = 0;
            WindowManager.GetManager().Close_Windows(this.transform);
        }
    }
}