using UnityEngine;

namespace AppFw.Core
{
    /// <summary>
    /// ゲームの開始時,Awakeより前に一度だけ呼び出される処理.
    /// </summary>
    public static class ApplicationInitialize
    {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitializeApp()
        {
            // ここでGameMangerを生成する.
            var go = GameObject.Instantiate(Resources.Load("Prefabs/ManagerProvider")) as GameObject;
            go.name = go.name.Replace("(Clone)", "");
        }
    }
}