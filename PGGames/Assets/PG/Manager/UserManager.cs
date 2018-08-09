using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Manager.Enum;

namespace PG.Manager
{
    public class UserManager : Managers<UserManager>
    {
        private Players m_Player = new Players();
        private Combat m_ComBat = new Combat();

        public Players Player { get { return m_Player; } }
        public Combat Battle { get { return m_ComBat; } }




        public void Init()
        {
            EventManager.GetManager().AddListener(EventEnum.User_Update_Player, UpDate_User_Player);
            EventManager.GetManager().AddListener(EventEnum.User_Setting_UserID, User_Setting_UserID);
        }


        protected void User_Setting_UserID(EventObjet eventObjet)
        {
            uint ID = 0;
            EventManager.GetValue(eventObjet, out ID);
            m_Player.UserID = ID;
        }
        protected void UpDate_User_Player(EventObjet eventObjet)
        {
            Players players;
            EventManager.GetValue(eventObjet, out players);
            m_Player.NickName = players.NickName;
            m_Player.Sex = players.Sex;
            m_Player.Icon = players.Icon;
        }












    }

    /// <summary>
    /// 用户的基础属性
    /// </summary>
    public struct Players
    {
        public string NickName { set; get; }        //昵称
        public byte Sex { set; get; }               //性别
        public byte Icon { set; get; }              //头像
        public uint UserID { set; get; }            //ID
    }
    /// <summary>
    /// 用户的战斗属性
    /// </summary>
    public struct Combat
    {
        public int Lv { set; get; }                 //等级

        public long ATK { set; get; }               //攻击力
        public long DEF { set; get; }               //防御力

        public long MaxHp { set; get; }             //最大生命值
        public long MaxMp { set; get; }             //最大蓝量
        public long MaxExp { set; get; }            //最大经验值
        public long CurrentHP { set; get; }         //当前生命值
        public long CurrentMP { set; get; }         //当前蓝量
        public long CurrentExp { set; get; }        //当前经验值
    }
}