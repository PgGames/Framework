using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PG.Model;
using PG.Help;
using PG.Manager;

namespace PG.UI
{
    public class MainView : BaseView
    {
        public override void Init()
        {
            UserInit();
            base.Init();
        }

        private void Awake()
        {
            FindUserTran();
        }

        #region 用户信息

        private Text m_NickName;
        private Text m_Lv;
        private Scrollbar m_HP;
        private Scrollbar m_MP;
        private Scrollbar m_Exp;
        private Image m_Icon;
        private Image m_HPHandle;
        private Button m_Icon_Click;

        private void FindUserTran()
        {
            m_NickName = Helper.GetComponent<Text>(this.transform, "Root/LeftUP/UserInfo/Name");
            m_Lv = Helper.GetComponent<Text>(this.transform, "Root/LeftUP/UserInfo/LV/Value");
            m_HP = Helper.GetComponent<Scrollbar>(this.transform, "Root/LeftUP/UserInfo/bar/HP");
            m_HPHandle = Helper.GetComponent<Image>(this.transform, "Root/LeftUP/UserInfo/bar/HP/Sliding Area/Handle");
            m_MP = Helper.GetComponent<Scrollbar>(this.transform, "Root/LeftUP/UserInfo/bar/MP");
            m_Exp = Helper.GetComponent<Scrollbar>(this.transform, "Root/LeftUP/UserInfo/bar/EXP");
            m_Icon = Helper.GetComponent<Image>(this.transform, "Root/LeftUP/UserInfo/Icon");
            m_Icon_Click = Helper.GetComponent<Button>(this.transform, "Root/LeftUP/UserInfo/Icon/Image");

            m_Icon_Click.onClick.AddListener(Btn_Icon_OnClick);
        }

        private void UserInit()
        {
            SettingName(UserManager.GetManager.Player.NickName);
            SettingIcon(Helper.GetIconByIdx(UserManager.GetManager.Player.Icon));
            UpdateHP(UserManager.GetManager.Battle.CurrentHP, UserManager.GetManager.Battle.MaxHp);
            UpdateMP(UserManager.GetManager.Battle.CurrentMP, UserManager.GetManager.Battle.MaxMp);
            UpdateExp(UserManager.GetManager.Battle.CurrentExp, UserManager.GetManager.Battle.MaxExp);
            UpdateLV(UserManager.GetManager.Battle.Lv);
        }

        //设置名称
        private void SettingName(string varName)
        {
            if (m_NickName != null)
                m_NickName.text = varName;
        }
        //设置头像
        private void SettingIcon(Sprite varIcon)
        {
            if (m_Icon != null&&varIcon!=null)
                m_Icon.sprite = varIcon;
        }
        //更新生命值
        private void UpdateHP(long varCurrentHP, long varMaxHP)
        {
            if (varCurrentHP < 0 || varMaxHP <= 0)
                return;
            if (m_HP != null)
            {
                m_HP.value = varCurrentHP / (varMaxHP * 1.0f);
                if (m_HPHandle != null)
                {
                    float Value = m_HP.value * 100.0f;
                    float r = 1;
                    float g = 1;
                    float b = 1;
                    if (Value >= 75)
                    {
                        //由绿到蓝
                        r = 0;
                        b = 1 - (Value - 75) / 25.0f;
                    }
                    else if (Value > 50)
                    {
                        //由蓝到黄
                        r = 1 - (Value - 50) / 25.0f;
                        b = 1 - r;
                    }
                    else if (Value > 25)
                    {
                        //有黄到红
                        g = (Value - 25) / 25.0f;
                        b = 0;
                    }
                    else
                    {
                        //红
                        g = 0;
                        b = 0;
                    }
                    m_HPHandle.color = new Color(r, g, b);
                }
            }
        }
        //更新蓝量
        private void UpdateMP(long varCurrentMP, long varMaxMP)
        {
            if (varCurrentMP < 0 || varMaxMP <= 0)
                return;
            if (m_MP != null)
                m_MP.value = varCurrentMP / (varMaxMP * 1.0f);
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