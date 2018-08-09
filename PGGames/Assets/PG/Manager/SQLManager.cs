using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.SQLite;
using System.IO;
using PG.Manager;


namespace PG.Manager
{
    public class SQLManager : Manager<SQLManager>
    {
        private SQLiteDate m_SQLData;
        //数据库地址
        private string m_SQL_Path;
        //数据库名称
        private string m_SQL_Name = "/Const.db";
        
        public void Init()
        {
            m_SQL_Path = Application.streamingAssetsPath;
            if (!Directory.Exists(m_SQL_Path))
                Directory.CreateDirectory(m_SQL_Path);
            m_SQLData = new SQLiteDate("data source=" + m_SQL_Path + m_SQL_Name);
        }

        /// <summary>
        /// 获取错误码信息
        /// </summary>
        /// <param name="ErrorCode"></param>
        /// <returns></returns>
        public string GetErrorCode(int ErrorCode)
        {
            try
            {
                string TempErrorMsg = null;
                string TempEnglishMsg = null;

                if (m_SQLData.ExistsTable("ErrorCode"))
                {
                    m_SQLData.SelectInfo("ErrorCode", (sql) =>
                    {
                        while (sql.Read())
                        {
                            TempErrorMsg = sql["Msg"].ToString();
                            TempEnglishMsg = sql["English"].ToString();
                        }
                    }, "Msg", "CodeID=" + ErrorCode);
                }
                if (!string.IsNullOrEmpty(TempErrorMsg))
                    return TempErrorMsg;
                else
                    return "错误码不存在" + ErrorCode.ToString();
            }
            catch(System.Exception ex)
            {
                return "数据库操作异常" + ex.Message;
            }
        }
        

    }
}