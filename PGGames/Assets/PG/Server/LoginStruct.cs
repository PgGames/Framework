using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGServer.Struct
{
    namespace Login
    {
        /// <summary>
        /// 登陆--请求
        /// </summary>
        public enum LoginSub_Request
        {
            Login = 0,     //登陆
            Signin,         //注册
            Userid,         //获取用户信息
        }
        /// <summary>
        /// 登陆--请求结果
        /// </summary>
        public enum LoginSub_Result
        {
            Login = 0,      //登陆结果
            Signin,         //注册结果
            Userid,         //获取用户信息
        }

        [ProtoContract]
        public class Request_Login          //登陆
        {
            [ProtoMember(1)]
            public string Account;      //账户
            [ProtoMember(2)]
            public string PassWord;     //密码
        }
        [ProtoContract]
        public class Request_Signin         //注册
        {
            [ProtoMember(1)]
            public string Account;          //账号
            [ProtoMember(2)]
            public string NickName;         //昵称
            [ProtoMember(3)]
            public byte Sex;                //性别
            [ProtoMember(4)]
            public string PassWord;         //密码
        }
        [ProtoContract]
        public class Request_GetUserInfo    //获取用户信息
        {
            [ProtoMember(1)]
            public uint UserID;         //用户ID（错误码为 0 时值正常）
        }


        [ProtoContract]
        public class Result_Login        //登陆结果
        {
            [ProtoMember(1)]
            public byte code;           //错误码(错误码为 0 其他数据有效)
            [ProtoMember(2)]
            public uint UserID;         //用户ID
        }
        [ProtoContract]
        public class Result_Signin       //注册结果
        {
            [ProtoMember(1)]
            public int code;           //错误码(错误码为 0 其他数据有效)
        }
        [ProtoContract]
        public class Result_User         //用户信息结果
        {
            [ProtoMember(1)]
            public byte code;           //错误码(错误码为 0 其他数据有效)
            [ProtoMember(2)]
            public uint UserID;         //用户ID
            [ProtoMember(3)]
            public string NickName;     //用户昵称
            [ProtoMember(4)]
            public byte Sex;            //用户性别
            [ProtoMember(5)]
            public byte IconIDx;         //用户头像
        }
    }
}