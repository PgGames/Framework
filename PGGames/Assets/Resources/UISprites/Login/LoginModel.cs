using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Model;
using PG.Manager;
using PG.Manager.Enum;
using PG.UI;
using PG.Help;
using PGServer.Struct.Login;

public class LoginModel : BaseModel {
    

    public override void Init()
    {
        GameObject TempLogin = Resources.Load<GameObject>("UI/Prefabs/Login");
        WindowManager.GetManager().AddPath(WindowEnum.UILogin, TempLogin);

        base.Init();
    }
    public override void Open()
    {
        GameObject Temp_View = WindowManager.GetManager().Open_Windows(WindowEnum.UILogin);
        this.m_View = Temp_View.GetComponent<LoginView>();

        base.Open();
    }
    public override void Close()
    {
        WindowManager.GetManager().Close_Windows(WindowEnum.UILogin);
        base.Close();
    }
    protected override void AddListener()
    {
        NetWorkManager.GetManager().AddListen(NetWorkManager.ServerType.Login, (int)CmdCommand.Login, (int)LoginSub_Result.Login, Login_Result);
        NetWorkManager.GetManager().AddListen(NetWorkManager.ServerType.Login, (int)CmdCommand.Login, (int)LoginSub_Result.Signin, Signin_Result);
        NetWorkManager.GetManager().AddListen(NetWorkManager.ServerType.Login, (int)CmdCommand.Login, (int)LoginSub_Result.Userid, UserID_Result);

        base.AddListener();
    }
    protected void Login_Result(byte[] date)
    {
        Result_Login TempDate = Helper.Get<Result_Login>(date);
        if (TempDate.code != 0)
        {
            ShowErrorPrompt(TempDate.code);
            return;
        }
        else
        {
            //Debug.LogError("登陆成功");
            NetWorkManager.GetManager().SendLoginMessage(new NetWorkHead {
                MainCommand = (int) CmdCommand.Login,
                SubCommand = (int)LoginSub_Request.Userid,
                Date = Helper.Set(new Request_GetUserInfo() {
                    UserID = TempDate.UserID,
                }),
            });
        }
    }
    protected void Signin_Result(byte[] date)
    {
        Result_Signin Temp_Data = Helper.Get<Result_Signin>(date);
        if (Temp_Data.code != 0)
            return;
        else
            ShowErrorPrompt("UI_Login_22");
    }
    protected void UserID_Result(byte[] date)
    {
        Result_User TempDate = Helper.Get<Result_User>(date);
        if (TempDate.code != 0)
            ShowErrorPrompt(TempDate.code);
        else
        {
            EventManager.Broadcast(EventEnum.User_Setting_UserID, TempDate.UserID);
            EventManager.Broadcast(EventEnum.User_Update_Player, new Players {
                Icon = TempDate.IconIDx,
                Sex = TempDate.Sex,
                NickName = TempDate.NickName,
            });
            //UserManager
            //打开主模块
            ModelManager.GetManager().Open(ModelEnum.Main);
            Close();
        }
    }



    public void Login_Detection(string varAccount, string varPassWord)
    {
        if (Helper.IsNullOrEmpty(varAccount))
        {
            ShowErrorPrompt("UI_Login_23");
            return;
        }
        if (Helper.IsNullOrEmpty(varPassWord))
        {
            ShowErrorPrompt("UI_Login_24");
            return;
        }

        Request_Login temp_Date = new Request_Login
        {
            Account = varAccount,
            PassWord = varPassWord,
        };

        NetWorkManager.GetManager().SendLoginMessage(new NetWorkHead {
            MainCommand = (int)CmdCommand.Login,
            SubCommand = (int)LoginSub_Request.Login,
            Date = Helper.Set(temp_Date),
        });
    }
    public void SignIn_Detection(string varAccount, string varPassWord,string Temp_ConfirmPassWord, byte varSex, string varNickName)
    {
        if (varPassWord != Temp_ConfirmPassWord)
        {
            ShowErrorPrompt("UI_Login_21");
            return;
        }
        if (Helper.IsNullOrEmpty(varAccount))
        {
            ShowErrorPrompt("UI_Login_23");
            return;
        }
        if (Helper.IsNullOrEmpty(varPassWord))
        {
            ShowErrorPrompt("UI_Login_24");
            return;
        }
        if (Helper.IsNullOrEmpty(varNickName))
        {
            ShowErrorPrompt("UI_Login_25");
            return;
        }

        Request_Signin Temp_Date = new Request_Signin {
            Account = varAccount,
            NickName = varNickName,
            PassWord = varPassWord,
            Sex =varSex,
        };

        NetWorkManager.GetManager().SendLoginMessage(new NetWorkHead {
            MainCommand = (int)CmdCommand.Login,
            SubCommand = (int)LoginSub_Request.Signin,
            Date = Helper.Set(Temp_Date),
        });
    }



    /// <summary>
    /// 显示错误信息
    /// </summary>
    /// <param name="error"></param>
    protected void ShowErrorPrompt(int errorCode)
    {
        string msg = SQLManager.GetManager().GetErrorCode(errorCode);
        EventManager.Broadcast(EventEnum.UI_Open_PromptAuto, msg);
    }
    /// <summary>
    /// 显示错误信息
    /// </summary>
    /// <param name="error"></param>
    protected void ShowErrorPrompt(string error)
    {
        //string msg = SQLManager.GetManager().GetErrorCode(errorCode);
        EventManager.Broadcast(EventEnum.UI_Open_PromptAuto, error);
    }
    
}
