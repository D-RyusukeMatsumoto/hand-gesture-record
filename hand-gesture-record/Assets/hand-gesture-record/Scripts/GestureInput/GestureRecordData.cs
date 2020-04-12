using UnityEngine;

#if UNITY_EDITOR
using HandGestureRecord.Settings;
using UnityEditor;
#endif


namespace HandGestureRecord.GestureInput
{
    /// <summary>
    /// 記録したジェスチャーデータ.
    /// </summary>
    [System.Serializable]
    public class GestureRecordData : ScriptableObject
    {
        [SerializeField] HandDataBase.FingerStraightInfo data;

        public HandDataBase.FingerStraightInfo Data => data;
        public string GestureName => name;
        
        public GestureRecordData(
            HandDataBase.FingerStraightInfo argData)
        {
            data = argData;
        }

        public void SetData(
            HandDataBase.FingerStraightInfo argData)
        {
            data = argData;
        }
        
        
        #if UNITY_EDITOR
        /// <summary>
        /// エディタ拡張.
        /// </summary>
        [CustomEditor(typeof(GestureRecordData))]
        public class GestureRecordDataEditor : Editor
        {
            string path;
            
            void OnEnable()
            {
                path = AssetDatabase.GetAssetPath(target);
                GestureDataIndex.AddData(path);
            }


            void OnDestroy()
            {
                if (!Application.isPlaying)
                {
                    if (!target)
                    {
                        // 削除時.
                        Debug.Log("jejeje : " + path);
                    }
                    else
                    {
                        // フォーカスが外れた時.
                        Debug.Log("hazureta : " + path);
                    }
                }
            }
        }

        #endif

    }
}