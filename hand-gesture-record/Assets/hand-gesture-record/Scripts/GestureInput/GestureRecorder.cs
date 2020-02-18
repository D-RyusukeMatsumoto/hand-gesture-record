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

        [SerializeField] HandId handId;
        [SerializeField] float threshold;
        
        
        void Start()
        {
            manager = ManagerProvider.GetRuntimeManager<GestureManager>();
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

                component.handId = (HandId)EditorGUILayout.EnumPopup("HandId", component.handId);
                component.threshold = EditorGUILayout.Slider("Threshold", component.threshold, 0.1f, 1f);
                
                if (isRecoredStarted)
                {
                    if (GUILayout.Button("録画終了"))
                    {
                        // パスを指定するウィンドウを表示してScriptableObjectで保存.

                        // TODO : 一定期間での指の動作でのジェスチャ作成の時までは録画ボタンを押下した瞬間の指の状態だけをとることとする.
                        //isRecoredStarted = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("録画開始") && EditorApplication.isPlaying)
                    {
                        // ここで保存.
                        this.SaveFile(component);

                        //isRecoredStarted = true;
                    }
                }
            }
            

            /// <summary>
            /// ジェスチャファイルの保存.
            /// </summary>
            void SaveFile(
                GestureRecorder component)
            {
                // TODO : 保存先パスは固定にする?.
                var filePath = EditorUtility.SaveFilePanel("SaveGestureFile", Application.dataPath, "anyFile", "asset");
                if (string.IsNullOrEmpty(filePath))
                    return;

                var fingerStraightInfo = new HandDataBase.FingerStraightInfo
                {
                    thumb = component.manager.IsFingerStraight(component.handId, FingerId.Thumb, component.threshold),
                    index = component.manager.IsFingerStraight(component.handId, FingerId.Index, component.threshold),
                    middle = component.manager.IsFingerStraight(component.handId, FingerId.Middle, component.threshold),
                    ring = component.manager.IsFingerStraight(component.handId, FingerId.Ring, component.threshold),
                    pinky = component.manager.IsFingerStraight(component.handId, FingerId.Pinky, component.threshold)
                };
                var instance = ScriptableObject.CreateInstance<GestureRecordData>();
                EditorUtility.SetDirty(instance);
                instance.SetData(fingerStraightInfo);

                AssetDatabase.CreateAsset(instance, filePath.Replace(Application.dataPath, "Assets"));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            
        }

#endif
    }
}