using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PG.Manager
{

    public delegate void NetWorkMethod(byte[] Date);

    /// <summary>
    /// 网络通信管理
    /// </summary>
    public class NetWorkManager : Manager<NetWorkManager>
    {
        public enum ServerType
        {
            Login,          //登陆服务器
            Game,           //游戏服务器
        }
        Dictionary<ServerType, Dictionary<int, Dictionary<int, NetWorkMethod>>> Dic_All_Listen = new Dictionary<ServerType, Dictionary<int, Dictionary<int, NetWorkMethod>>>();


        /// <summary>
        /// 添加网络事件监听
        /// </summary>
        /// <param name="varServerType">服务器类型</param>
        /// <param name="varMainCommand">主命令</param>
        /// <param name="varSubCommand">子命令</param>
        /// <param name="method">回调方法</param>
        public void AddListen(ServerType varServerType,int varMainCommand,int varSubCommand, NetWorkMethod method)
        {
            if (method == null)
            {
                Debug.LogError("回调方法不能为空");
                return;
            }
            if (!Dic_All_Listen.ContainsKey(varServerType))
                Dic_All_Listen.Add(varServerType, new Dictionary<int, Dictionary<int, NetWorkMethod>>());
            Dictionary<int, Dictionary<int, NetWorkMethod>> TempTypeDic = null;
            if (!Dic_All_Listen.TryGetValue(varServerType, out TempTypeDic))
                return;
            if (!TempTypeDic.ContainsKey(varMainCommand))
                TempTypeDic.Add(varMainCommand, new Dictionary<int, NetWorkMethod>());
            Dictionary<int, NetWorkMethod> TempCommand = null;
            if (!TempTypeDic.TryGetValue(varMainCommand, out TempCommand))
                return;
            if (TempCommand.ContainsKey(varSubCommand))
            {
                Debug.LogError("该网络监听已存在");
                return;
            }
            TempCommand.Add(varSubCommand, method);
        }
        /// <summary>
        /// 网络事件广播（网络游戏由Socket调起，即服务器调起）
        /// </summary>
        /// <param name="Date"></param>
        public void NotifyLoginListen(NetWorkHead Date)
        {
            if (!Dic_All_Listen.ContainsKey(ServerType.Login))
            {
                Debug.LogError("没有登陆服务器的监听");
                return;
            }
            Dictionary<int, Dictionary<int, NetWorkMethod>> TempTypeDic = null;
            if (!Dic_All_Listen.TryGetValue(ServerType.Login, out TempTypeDic))
                return;
            //if()
        }
        /// <summary>
        /// 网络事件广播（网络游戏由Socket调起，即服务器调起）
        /// </summary>
        /// <param name="Date"></param>
        public void NotifyGameListen(NetWorkHead Date)
        {
        }
    }
    /// <summary>
    /// 网络消息包
    /// </summary>
    public class NetWorkHead
    {
        public int MainCommand;
        public int SubCommand;
        public byte[] Date;
    }
}