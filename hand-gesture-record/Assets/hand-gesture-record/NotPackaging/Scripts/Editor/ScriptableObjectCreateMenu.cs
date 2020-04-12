using UnityEditor;
using UnityEngine;

namespace NoPackaging
{
    public static class ScriptableObjectCreateMenu
    {

        const string MenuText = "Assets/Create/ScriptableObject";
        const string PanelTitle = "CreateScriptableAsset";
        const string DefaultFileName = "ScriptableAsset";
        const string Extension = "asset";


        // コンテキストメニュー.
        [MenuItem(MenuText, false, 0)]
        static void CreateScriptableObject()
        {
            var script = Selection.activeObject as MonoScript;
            string filePath = EditorUtility.SaveFilePanelInProject(PanelTitle, DefaultFileName, Extension, "");
            OutputFile(script.GetClass(), filePath);            
        }


        // ファイルの生成.
        static void OutputFile(
            System.Type type, 
            string outputPath)
        {
            ScriptableObject scriptableObject = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(scriptableObject, outputPath);
            AssetDatabase.Refresh();
            ProjectWindowUtil.ShowCreatedAsset(scriptableObject);
        }


        // 選択しているスクリプトがScriptableObjectか否か判定.
        [MenuItem(MenuText, true)]
        static bool ValidateCreateAsset()
        {
            var script = Selection.activeObject as MonoScript;
            if (script == null) return false;
            return script.GetClass().IsSubclassOf(typeof(ScriptableObject));
        }

    }
}
