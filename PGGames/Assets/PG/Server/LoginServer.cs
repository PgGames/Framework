using PG.SQLite;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using PG.Help;
using PGServer.Struct;
using PGServer.Struct.Login;
using PG.Manager;
using System;

namespace PGServer
{
    public class LoginServer
    {
        private SQLiteDate m_SQLData;
        //数据库地址
        private string m_SQL_Path;
        //数据库名称
        private string m_SQL_Name = "/LoginServer.db";

        public void Init()
        {
            m_SQL_Path = Application.streamingAssetsPath;
            if (!Directory.Exists(m_SQL_Path))
                Directory.CreateDirectory(m_SQL_Path);
            m_SQLData = new SQLiteDate("data source=" + m_SQL_Path + m_SQL_Name);

            CreateTable();
        }


        #region 创建表格

        /// <summary>
        /// 创建表格
        /// </summary>
        private void CreateTable()
        {
            CreateUserTable();
        }

        /// <summary>
        /// 创建用户基本信息表
        /// </summary>
        protected void CreateUserTable()
        {
            if (!m_SQLData.ExistsTable("UserInfo"))
            {
                //创建用户信息表
                //表的结构
                /*
                 * 表的字段      |  字段含义          字段类型
                 * ---------------------------------------
                 * UserID        |  用户标识    |       int(表的主键，自增)
                 * UserSex       |  用户型别    |       int(默认为0，)
                 * IconIdx       |  用户头像    |       int(默认为0，)
                 * NickName      |  用户昵称    |     string
                 * Account       |  登陆账号    |     string
                 * PassWord      |  账户密码    |     string
                 * SigninTime    |  注册时间    |      long
                 */
                m_SQLData.CreateTable("UserInfo", new string[] {
                    "UserID",
                    "UserSex",
                    "IconIdx",
                    "NickName",
                    "Account",
                    "PassWord",
                    "SigninTime",
                }, new string[] {
                    "INTEGER NOT NULL PRIMARY KEY",
                    "INTEGER DEFAULT 0",
                    "INTEGER DEFAULT 0",
                    "TEXT NOT NULL",
                    "TEXT NOT NULL",
                    "TEXT NOT NULL",
                    "INTEGER",
                });
            }
        }


        #endregion

        #region 用户信息表的调用

        /// <summary>
        /// 登陆检测
        /// </summary>
        /// <param name="varAccount">账号</param>
        /// <param name="varPassWord">密码</param>
        /// <returns>0-成功</returns>
        public byte User_Detection_AccountAndPassWord(string varAccount, string varPassWord, out uint varUserID)
        {
            varUserID = 0;
            try
            {
                long TempUserID = -1;
                m_SQLData.SelectInfo("UserInfo", (sql) =>
                {
                    while (sql.Read())
                    {
                        string pass = sql["PassWord"].ToString();
                        if (pass != varPassWord)
                            break;
                        TempUserID = uint.Parse(sql["UserID"].ToString());
                        break;
                    }
                }, "*", "Account='" + varAccount + "'");
                if (TempUserID < 0)
                    return 16;          //账号不存在或密码错误
                varUserID = (uint)TempUserID;
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// 账号检测
        /// </summary>
        /// <param name="varAccount"></param>
        /// <returns>3-账号不存在  1-数据库异常  2-账号已存在</returns>
        public byte User_Detection_Account(string varAccount)
        {
            try
            {
                long TempUserID = -1;
                m_SQLData.SelectInfo("UserInfo", (sql) =>
                {
                    while (sql.Read())
                    {
                        TempUserID = 0;
                        break;
                    }
                }, "*", "Account='" + varAccount + "'");
                if (TempUserID >= 0)
                    return 2;       //账号已存在
                else
                    return 3;       //账号不存在
            }
            catch
            {
                return 1;           //数据库异常
            }
            //return 2;               //账号不存在
        }
        /// <summary>
        /// 昵称检测
        /// </summary>
        /// <param name="varName"></param>
        /// <returns>15-昵称不存在  1-数据库异常  4-昵称已存在</returns>
        public byte User_Detection_NickName(string varName)
        {
            try
            {
                if (string.IsNullOrEmpty(varName))
                    return 10;  //昵称不能为空
                long TempUserID = -1;
                m_SQLData.SelectInfo("UserInfo", (sql) =>
                {
                    while (sql.Read())
                    {
                        TempUserID = 0;
                        break;
                    }
                }, "*", "NickName='" + varName + "'");
                if (TempUserID < 0)
                    return 15;  //昵称不存在
                else
                    return 4;   //昵称已存在
            }
            catch
            {
                return 1;       //数据库异常
            }
        }
        /// <summary>
        /// 注册检测
        /// </summary>
        /// <param name="varAccount">账号</param>
        /// <param name="varPassWord">密码</param>
        /// <param name="varSex">性别</param>
        /// <param name="varNickName">昵称</param>
        /// <returns>0-注册成功</returns>
        public byte User_Detection_Sigin(string varAccount, string varPassWord, byte varSex, string varNickName)
        {
            try
            {
                byte xcode = 0;

                xcode = User_Detection_Account(varAccount);
                if (xcode != 3)
                    return xcode;
                xcode = User_Detection_NickName(varNickName);
                if (xcode != 15)
                    return xcode;
                //插入用户基本信息
                string content = string.Format("(UserSex,Account,PassWord,NickName) VALUES ({0},'{1}','{2}','{3}')", varSex, varAccount, varPassWord, varNickName);
                m_SQLData.InsertInfo("UserInfo", content);
                //获取表的数据数量
                int counts = 0;
                m_SQLData.ExecuteQuery("Select count(*) AS Count From UserInfo ", (sql) => {
                    while (sql.Read())
                    {
                        counts = int.Parse(sql["Count"].ToString());
                    }
                });
                //插入用户战斗信息
                m_SQLData.InsertInfo("UserCombatInfo", "(UserID) VALUES (" + counts + ")");
            }
            catch
            {
                return 1;   //数据库异常
            }
            return 0;       //注册成功
        }
        /// <summary>
        /// 获取登陆用户信息
        /// </summary>
        /// <param name="userID">用户id</param>
        /// <param name="NickName">用户昵称</param>
        /// <param name="Icon">用户头像</param>
        /// <param name="Sex">用户性别</param>
        /// <returns></returns>
        public byte GetLoginUserInfo(uint userID, out string NickName, out byte Icon, out byte Sex)
        {
            NickName = null;
            Icon = 1;
            Sex = 0;
            try
            {
                string tempname = "";
                byte tempIcon = 0;
                byte tempsex = 1;
                m_SQLData.SelectInfo("UserInfo", (sql) => {
                    while (sql.Read())
                    {
                        tempname = sql["NickName"].ToString();
                        tempIcon = byte.Parse(sql["IconIdx"].ToString());
                        tempsex = byte.Parse(sql["UserSex"].ToString());
                    }
                }, "*", "UserID=" + userID);
                if (Helper.IsNullOrEmpty(tempname))
                    return 3;
                NickName = tempname;
                Icon = tempIcon;
                Sex = tempsex;
            }
            catch(Exception ex)
            {
                Debug.LogError(ex.Message);
                return 1;   //数据库异常
            }
            return 0;       //注册成功
        }

        #endregion

        public void Login_Receive(int SubCommand,byte[] Date)
        {
            switch ((LoginSub_Request)SubCommand)
            {
                case LoginSub_Request.Login:
                    LoginInfo(Date);
                    break;
                case LoginSub_Request.Signin:
                    SigninInfo(Date);
                    break;
                case LoginSub_Request.Userid:
                    GetUserInfo(Date);
                    break;
                default:
                    break;
            }
        }
        //用户登陆
        protected void LoginInfo(byte[] Data)
        {
            Request_Login login = Helper.Get<Request_Login>(Data);
            //uint Userid;

            Result_Login Date = new Result_Login();

            if (Helper.IsNullOrEmpty(login.Account))
                Date.code = 8;
            else if (Helper.IsNullOrEmpty(login.PassWord))
                Date.code = 9;
            else
                Date.code = User_Detection_AccountAndPassWord(login.Account, login.PassWord, out Date.UserID);

            NetWorkManager.GetManager().NotifyLoginListen(new NetWorkHead {
                MainCommand = (int)CmdCommand.Login,
                SubCommand = (int)LoginSub_Result.Login,
                Date = Helper.Set(Date)
            });
        }
        //用户注册
        protected void SigninInfo(byte[] Data)
        {
            Request_Signin temp_Signin = Helper.Get<Request_Signin>(Data);

            int Code = Account_Detection(temp_Signin.Account);
            if (Code == 0)
            {
                Code = PassWord_Detection(temp_Signin.PassWord);
                if (Code == 0)
                {
                    Code = NickName_Detection(temp_Signin.NickName);
                    if (Code == 0)
                    {
                        Code = User_Detection_Sigin(temp_Signin.Account, temp_Signin.PassWord, temp_Signin.Sex, temp_Signin.NickName);
                    }
                }
            }
            Result_Signin TempDate = new Result_Signin
            {
                code = Code,
            };
            NetWorkManager.GetManager().NotifyLoginListen(new NetWorkHead
            {
                MainCommand = (int)CmdCommand.Login,
                SubCommand = (int)LoginSub_Result.Login,
                Date = Helper.Set(TempDate)
            });
        }
        //获取用户信息
        protected void GetUserInfo(byte[] date)
        {
            Request_GetUserInfo tempDate = Helper.Get<Request_GetUserInfo>(date);

            Result_User Temp = new Result_User();

            Temp.code = GetLoginUserInfo(tempDate.UserID, out Temp.NickName,out Temp.IconIDx,out Temp.Sex);

            NetWorkManager.GetManager().NotifyLoginListen(new NetWorkHead {
                MainCommand = (int)CmdCommand.Login,
                SubCommand = (int)LoginSub_Result.Userid,
                Date = Helper.Set(Temp)
            });
        }

        /// <summary>
        /// 账号检测
        /// 账号长度必须大于10，且只能有字母、数字构成
        /// </summary>
        /// <param name="varAccount"></param>
        /// <returns></returns>
        protected byte Account_Detection(string varAccount)
        {
            if (string.IsNullOrEmpty(varAccount))
                return 8;           //账号不能为空
            if (varAccount.Length < 5)
                return 11;          //账号的长度不足
            char[] TempAccount = varAccount.ToCharArray();

            string DetectionStr = "1234567890ABCDEFGHIJKLMNOPQLSTUVWXYZabcdefghijklmnopqlstuvwxyz";

            for (int i = 0; i < TempAccount.Length; i++)
            {
                char varchar = TempAccount[i];
                if (DetectionStr.Contains(varchar.ToString()))
                    continue;
                return 12;          //账号只能有数字和字母构成
            }
            return 0;
        }
        /// <summary>
        /// 密码检测
        /// 密码长度必须大于6，且只能有字母、数字构成
        /// </summary>
        /// <param name="varPassWord"></param>
        /// <returns></returns>
        protected byte PassWord_Detection(string varPassWord)
        {
            if (string.IsNullOrEmpty(varPassWord))
                return 9;           //密码不能为空
            if (varPassWord.Length < 6)
                return 13;          //密码长度不足
            string DetectionStr = "1234567890ABCDEFGHIJKLMNOPQLSTUVWXYZabcdefghijklmnopqlstuvwxyz";
            for (int i = 0; i < varPassWord.Length; i++)
            {
                char varchar = varPassWord[i];
                if (DetectionStr.Contains(varchar.ToString()))
                    continue;
                return 14;          //密码只能有数字和字母构成
            }
            return 0;
        }
        /// <summary>
        /// 昵称检测
        /// 长度不能超过7个汉字或14个字母
        /// </summary>
        /// <param name="varNickName"></param>
        /// <returns></returns>
        protected int NickName_Detection(string varNickName)
        {
            if (varNickName.Length > 14)
                return 17;
            int idx = 0;
            for (int i = 0; i < varNickName.Length; i++)
            {
                char temp = varNickName[i];
                if ((int)temp < 127)
                    idx++;
                else
                    idx += 2;
            }
            if (idx > 14)
                return 17;
            for (int i = 0; i < illegality.Length; i++)
            {
                if (varNickName.Contains(illegality[i]))
                    return 18;
            }
            return 0;
        }

        //非法字符集
        string[] illegality = new string[] {
            "操","逼","妈","爸","爷","奶","性"
        };
    }
}