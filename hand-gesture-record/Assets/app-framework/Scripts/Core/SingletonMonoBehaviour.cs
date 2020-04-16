using UnityEngine;
using System;


namespace AppFw.Core
{
    /// <summary>
    /// Singletonにするコンポーネントのベースクラス.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {

        static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null) 
                {
                    Type t = typeof(T);

                    instance = (T)FindObjectOfType(t);
                    if (instance == null) 
                    {
                        Debug.LogError (t + " をアタッチしているGameObjectはありません");
                    }
                }
                return instance;
            }
        }


        virtual protected void Awake()
        {
            CheckInstance();
        }


        // インスタンスが生成されているかチェックを掛ける.
        protected bool CheckInstance()
        {
            if (instance == null) 
            {
                instance = this as T;
                return true;
            }
            else if (Instance == this) 
            {
                return true;
            }
            Destroy (this);
            return false;
        }
    }
}