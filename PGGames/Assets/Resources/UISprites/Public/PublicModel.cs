using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Manager;
using PG.Manager.Enum;
using PG.UI;

namespace PG.Model {

    public class PublicModel : BaseModel {

        public override void Init()
        {
            GameObject PromptAuto = Resources.Load<GameObject>("UI/Prefabs/PromptAuto");
            WindowManager.GetManager().AddPath(WindowEnum.UIPromptAuto, PromptAuto);

            AddListener();
            base.Init();
        }
        public override void Open()
        {
            base.Open();
        }
        public override void Close()
        {
            base.Close();
        }

        protected void AddListener()
        {
            EventManager.GetManager().AddListener(EventEnum.UI_Open_PromptAuto, PromptAutoWindows);
        }
        protected void PromptAutoWindows(EventObjet eventMonth)
        {
            string content = null;
            EventManager.GetValue(eventMonth, out content);
            GameObject promptview = WindowManager.GetManager().Open_Windows(WindowEnum.UIPromptAuto, false);
            PromptAutoView UIView = promptview.GetComponent<PromptAutoView>();
            if (UIView != null)
            {
                UIView.Init();
                UIView.OpenUI(content);
            }
        }
    }
}