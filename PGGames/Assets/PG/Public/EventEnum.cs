
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

        UI_Open_PromptAuto,

        #endregion
    }
}