using System.Collections.Generic;
using UnityEngine.SceneManagement;

using HandGestureRecord.Utility;
using UnityEngine;

namespace HandGestureRecord.Common
{
    /// <summary>
    /// ゲームマネージャ.
    /// </summary>
    public class ManagerProvider : SingletonMonoBehaviour<ManagerProvider>
    {


        Dictionary<string, RuntimeManagerBase> managerDic = new Dictionary<string, RuntimeManagerBase>();
        Player player;


        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
            SceneManager.activeSceneChanged += ActiveSceneChangeListener;
            SceneManager.sceneUnloaded += SceneUnLoadedListener;
        }


        /// <summary>
        /// Playerを取得.
        /// </summary>
        /// <returns></returns>
        public static Player GetPlayer() => Instance.player;


        /// <summary>
        /// Playerを登録.
        /// </summary>
        /// <param name="argPlayer"></param>
        public static void RegisterPlayer(Player argPlayer) => Instance.player = argPlayer;


        /// <summary>
        /// 登録済みのランタイムマネージャを取得.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRuntimeManager<T>() where T : class
        {
            if (!Instance.managerDic.ContainsKey(typeof(T).ToString())) return null;
            return Instance.managerDic[typeof(T).ToString()] as T;
        }


        /// <summary>
        /// ランタイムマネージャを登録.
        /// </summary>
        /// <param name="manager"></param>
        public static void RegisterRuntimeManager(
            RuntimeManagerBase manager)
        {
            string typeString = manager.ToString();
            if (Instance.managerDic.ContainsKey(typeString)) return;
            Instance.managerDic.Add(typeString, manager);
        }

        
        // シーンが破棄された故タイミングのリスナ.
        static void SceneUnLoadedListener(
            Scene scene)
        {
            // ランタイムマネージャはこのタイミングで破棄.
            Instance.managerDic?.Clear();
        }


        // シーン変更検知,Unityの仕様上beforeには何も入っていない,afterにロードされたシーンがある.
        static void ActiveSceneChangeListener(
            Scene before,
            Scene after)
        {
        }
        
    }
}