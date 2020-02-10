﻿namespace Engine.Singleton
{
    using UnityEngine;

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Fields
        private static T _instance = null;
        private static object _lock = new object();
        #endregion Fields

        #region Properties
        public static T Instance
        {
            get
            {
                lock(_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        /*if (_instance == null)
                        {
                            var singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString() + "(Singleton)";

                            DontDestroyOnLoad(singletonObject);
                        }*/
                    }
                    return _instance;
                }
            }
        }
        #endregion Properties

        #region Methods
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
            _lock = null;
        }
        #endregion Methods
    }
}

