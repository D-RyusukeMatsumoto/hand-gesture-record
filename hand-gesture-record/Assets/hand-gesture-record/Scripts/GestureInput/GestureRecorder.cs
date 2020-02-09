using HandGestureRecord.Common;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace HandGestureRecord.GestureInput
{
    /// <summary>
    /// ジェスチャーレコードクラス.
    /// </summary>
    public class GestureRecorder : MonoBehaviour
    {
#if UNITY_EDITOR
        GestureManager manager;

        [SerializeField] KeyCode keyCode = KeyCode.Space;
        
        void Start()
        {
            manager = ManagerProvider.GetRuntimeManager<GestureManager>();
        }

        
        void FixedUpdate()
        {
        }



        /// <summary>
        /// レコード部分インスペクタ拡張.
        /// </summary>
        [CustomEditor(typeof(GestureRecorder))]
        public class GestureRecorderInspector : Editor
        {
            bool isRecoredStarted = false;
            
            public override void OnInspectorGUI()
            {
                var component = target as GestureRecorder;

                component.keyCode = (KeyCode) EditorGUILayout.EnumPopup("録画操作キーコード", component.keyCode);
            
                
                if (isRecoredStarted)
                {
                    if (GUILayout.Button("録画終了"))
                    {
                        // パスを指定するウィンドウを表示してScriptableObjectで保存.
                        isRecoredStarted = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("録画開始") && EditorApplication.isPlaying)
                    {
                        isRecoredStarted = true;
                    }
                }
            }
            
            
            
            
        }


#endif
    }
}