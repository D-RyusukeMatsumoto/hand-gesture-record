using UnityEngine;

namespace AppFw.Core
{
    /// <summary>
    /// シングルトンではない通常のマネージャクラスを作成するためのベースクラス.
    /// シーンに依存するマネージャやほかのシーンでは使う場面が全くないものなどに使う.
    /// Awakeで必ずGameManagerに登録すること.
    /// </summary>
    public class RuntimeManagerBase : MonoBehaviour
    {
        public override string ToString()
        {
            return GetType().ToString();
        }
    }
}