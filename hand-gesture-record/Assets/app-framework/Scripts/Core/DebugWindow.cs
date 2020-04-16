using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;


namespace AppFw.Core
{
    /// <summary>
    /// デバッグウィンドウ.
    /// </summary>
    public class DebugWindow : SingletonMonoBehaviour<DebugWindow>
    {

        
        [Header("Parameters")] 
        [SerializeField] Text infoText;

        GameObject mainPanel;
        Transform scrollViewContents;
        Dictionary<string, string> infoDictionary;
        StringBuilder stringBuilder;
        
        
 
        
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
            infoDictionary = new Dictionary<string, string>();
            stringBuilder = new StringBuilder(1024);
            mainPanel = transform.Find("MainPanel").gameObject;
        }


        void Start()
        {
            SetDisplay(true);
        }


        void Update()
        {
            UpdateMainText();            
        }


        void UpdateMainText()
        {
            foreach (var key in infoDictionary.Keys)
            {
                stringBuilder.AppendLine(infoDictionary[key]);
            }
            infoText.text = stringBuilder.ToString();
            stringBuilder.Clear();
        }

        
        /// <summary>
        /// デバッグ情報をセット.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="info"></param>
        public static void SetDebugInfo(
            string key,
            string info)
        {
            if (!Instance.infoDictionary.ContainsKey(key))
            {
                Instance.infoDictionary.Add(key, info);
            }
            else
            {
                Instance.infoDictionary[key] = info;
            }
        }

        
        /// <summary>
        /// デバッグ情報を削除.
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveDebugInfo(
            string key)
        {
            if (!Instance.infoDictionary.ContainsKey(key)) return;
            Instance.infoDictionary.Remove(key);
        }


        /// <summary>
        /// 表示非表示切り替え.
        /// </summary>
        /// <param name="isDisplay"></param>
        public static void SetDisplay(
            bool isDisplay)
        {
            Instance.mainPanel.SetActive(isDisplay);
        }
    }

}
