using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PG.Manager
{

    public class Manager<T> : MonoBehaviour where T:Component
    {
        protected static T manager;
        protected static GameObject m_Game;

        public static T GetManager()
        {
            if (manager == null)
            {
                manager = GameManager.Manager.GetGame().AddComponent<T>();
            }
            return manager;
        }

    }
    public class Managers<T>
    {
        protected static T manager;
        public static T GetManager
        {
            get
            {
                if (manager == null)
                {
                    manager = System.Activator.CreateInstance<T>(); ;
                }
                return manager;
            }
        }
    }
    public class Man
    { }
}