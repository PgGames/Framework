using PG.SQLite;
using PGServer;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



namespace PG.Manager
{
    public enum CmdCommand
    {
        Commond =0,    //通用命令
        Login =1,      //登陆命令（链接验证）
        User  =2,       //用户命令
    }
    /// <summary>
    /// 用于单机版游戏模拟服务器端处理游戏逻辑
    /// </summary>
    public class ServerManager : Manager<ServerManager>
    {

        private LoginServer m_LoginServer;
        private GamesServer m_GamesServer;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (m_LoginServer == null)
                m_LoginServer = new LoginServer();
            if (m_GamesServer == null)
                m_GamesServer = new GamesServer();
            //登陆服务器初始化
            m_LoginServer.Init();
            //游戏服务器初始化
            m_GamesServer.Init();
        }

        /// <summary>
        /// 接受登陆信息
        /// </summary>
        /// <param name="CMDCommand"></param>
        /// <param name="SubCommand"></param>
        /// <param name="Data"></param>
        public void ReceiveLogin(int CMDCommand,int SubCommand,byte[] Data)
        {
            switch ((CmdCommand)CMDCommand)
            {
                case CmdCommand.Login:
                    m_LoginServer.Login_Receive(SubCommand, Data);
                    break;
                case CmdCommand.User:
                    break;
                case CmdCommand.Commond:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 接受游戏消息
        /// </summary>
        /// <param name="CMDCommand"></param>
        /// <param name="SubCommand"></param>
        /// <param name="Data"></param>
        public void ReceiveGames(int CMDCommand, int SubCommand, byte[] Data)
        {
        }
    }
}