using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Manager;
using PG.Manager.Enum;
using PG.UI;

namespace PG.Model
{
    public class MainModel : BaseModel
    {
        public override void Init()
        {
            GameObject Temp_Game = Resources.Load<GameObject>("UI/Prefabs/MainUI");

            WindowManager.GetManager().AddPath(WindowEnum.UIMain, Temp_Game);
            
            base.Init();
        }
        public override void Open()
        {

            GameObject Temp_View = WindowManager.GetManager().Open_Windows(WindowEnum.UIMain);
            this.m_View = Temp_View.GetComponent<MainView>();

            base.Open();
        }
        public override void Close()
        {
            base.Close();
        }
        protected override void AddListener()
        {
            //EventManager.
            base.AddListener();
        }

        protected void User_LoginSuccess(EventObjet eventObjet)
        {
        }
    }
}