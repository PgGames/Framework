/*
 * Model枚举：与Model管理器对应使用
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PG.Manager.Enum
{
    public enum ModelEnum :ulong
    {
        Null = 0,

        Login,          //登陆模块
        Public,         //公共模块
        Main,           //主模块
    }
}