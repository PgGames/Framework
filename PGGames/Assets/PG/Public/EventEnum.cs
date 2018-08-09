
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

        User_Update_Player,             //更新用户信息
        User_Setting_UserID,            //设置用户ID

        #endregion
    }
}