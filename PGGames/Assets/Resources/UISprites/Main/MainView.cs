using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Model;
using PG.Help;

namespace PG.UI
{
    public class MainView : BaseView
    {
        public override void Init()
        {
            base.Init();
        }

        private void Awake()
        {
            FindTran();
        }

        private void FindTran()
        {

        }
        #region 用户信息

        private Text m_NickName;
        private Text m_Lv;
        private Scrollbar m_HP;
        private Scrollbar m_MP;
        private Scrollbar m_Exp;
        private Image m_Icon;


        //设置名称
        private void SettingName(string varName)
        {
            if (m_NickName != null)
                m_NickName.text = varName;
        }
        //设置头像
        private void SettingIcon(Sprite varIcon)
        {
            if (m_Icon != null)
                m_Icon.sprite = varIcon;
        }
        //更新生命值
        private void UpdateHP(long varCurrentHP, long varMaxHP)
        {
            if (varCurrentHP < 0 || varMaxHP <= 0)
                return;
            if (m_Exp != null)
                m_Exp.value = varCurrentHP / (varMaxHP * 1.0f);
        }
        //更新蓝量
        private void UpdateMP(long varCurrentMP, long varMaxMP)
        {
            if (varCurrentMP < 0 || varMaxMP <= 0)
                return;
            if (m_Exp != null)
                m_Exp.value = varCurrentMP / (varMaxMP * 1.0f);
        }
        //更新经验值
        private void UpdateExp(long varCurrentExp, long varMaxExp)
        {
            if (varCurrentExp < 0 || varMaxExp <= 0)
                return;
            if (m_Exp != null)
                m_Exp.value = varCurrentExp / (varMaxExp * 1.0f);
        }
        //更新等级
        private void UpdateLV(int varLv)
        {
            if (varLv < 1)
                return;
            if (m_Lv != null)
                m_Lv.text = varLv.ToString();
        }
        //头像的点击事件
        private void Btn_Icon_OnClick()
        {
        }

        #endregion
    }
}