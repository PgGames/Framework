using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PG.Manager
{
    public class GameManager : MonoBehaviour
    {
        private static GameObject m_Game;
        protected static GameManager m_Manager;
        public static GameManager Manager
        {
            get
            {
                if (m_Manager == null)
                {
                    if (m_Game == null)
                    {
                        m_Game = new GameObject("Manager");
                        GameObject.DontDestroyOnLoad(m_Game);
                    }
                    m_Manager = m_Game.AddComponent<GameManager>();
                }
                return m_Manager;
            }
        }
        private void Awake()
        {
            if (m_Manager == null)
                m_Manager = this;
            if (m_Game == null)
            {
                m_Game = this.gameObject;
                GameObject.DontDestroyOnLoad(m_Game);
            }
            Init();
        }
        /// <summary>
        /// 获取管理器预制
        /// </summary>
        /// <returns></returns>
        public GameObject GetGame()
        {
            return m_Game;
        }

        [Header("游戏中使用的文字")]
        public TextAsset m_Chinese;
        public TextAsset m_English;

        /// <summary>
        /// 信息初始化
        /// </summary>
        public void Init()
        {
            //初始化---窗口管理器
            WindowManager.GetManager().Init();
            //初始化---数据库管理器
            SQLManager.GetManager().Init();
            //初始化---模块管理器
            ModelManager.GetManager().Init();
            //初始化---语言信息-中文
            LanguageManager.GetManager.m_Chinese = m_Chinese;
            //初始化---语言信息-英文
            LanguageManager.GetManager.m_English = m_English;
            //初始化---语言信息
            LanguageManager.GetManager.Init();

            //打开登陆模块
            ModelManager.GetManager().Open(Enum.ModelEnum.Login);
        }
    }
}
