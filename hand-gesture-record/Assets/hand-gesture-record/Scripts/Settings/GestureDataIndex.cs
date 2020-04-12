using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HandGestureRecord.Settings
{
    [System.Serializable]
    public sealed class GestureDataIndex : ScriptableObject
    {
        /// <summary>
        /// データの配置パス.
        /// </summary>
        [System.Serializable]
        public class DataPath
        {
            static readonly string Extension = ".asset";
            
            // TODO : 現在はとりあえずResourcesPathのみで運用.
            [SerializeField] string resourcesPath;
            public string ResourcesPath => resourcesPath;

            public DataPath(
                string path)
            {
                resourcesPath = path.Replace(Extension, "");
            }
        }


        static GestureDataIndex instance;
        static GestureDataIndex Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load("Settings/GestureDataIndex", typeof(GestureDataIndex)) as GestureDataIndex;
                }
                return instance;
            }
        }


        [SerializeField] List<DataPath> pathIndex;
        public IReadOnlyList<DataPath> PathIndex
        {
            get { return pathIndex; }
        }


        public static void AddData(string path)
        {

            int index = path.IndexOf("Resources", 0);
            path = path.Remove(0, index).Replace("Resources/", "");
            Instance.pathIndex.Add(new DataPath(path));
        }


#if UNITY_EDITOR    
        /// <summary>
        /// エディタ拡張部.
        /// </summary>
        [CustomEditor(typeof(GestureDataIndex))]
        public class GestureDataIndexEditor : Editor
        {
            
        }
        #endif
        
        
        
        
    }
}

    