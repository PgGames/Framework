
/*
 * 事件枚举：与事件管理器对应
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Manager.Enum
{
    public enum EventEnum :ulong
    {
        Null = 0,

        #region UI界面

        UI_Open_PromptAuto,                 //打开自动提示框

        #endregion

        #region 操作逻辑

        Operate_User_LoginSuccess,          //用户登陆成功

        #endregion
    }
}