/*
 * 窗口枚举：与窗口管理器对应
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.Manager.Enum
{
    /// <summary>
    /// 窗口枚举
    /// </summary>
    public enum WindowEnum:ulong
    {
        NULL = 0,
        
        UILogin,                //登陆界面
        UIPromptAuto,           //提示框-文本提示无按钮自动关闭
    }
}
