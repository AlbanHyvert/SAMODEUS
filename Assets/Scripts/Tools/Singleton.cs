namespace Engine.Singleton
{
    using UnityEngine;

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Fields
        private static T _instance = null;
        #endregion Fields

        #region Properties
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        throw new System.Exception(typeof(T) + " Trying to access a nulled instance of a singleton. Exiting.");
                    }
                }
                return _instance;
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
        }
        #endregion Methods
    }
}

