using UnityEditor;
using UnityEngine;

public class ReplaceAssetContextMenu
{
    [MenuItem("Assets/Replace Asset With...")]
    private static void ReplaceAsset()
    {
        // Open a file dialog to select the new asset
        string newPath = EditorUtility.OpenFilePanel("Select New Asset", "", "");
        if (!string.IsNullOrEmpty(newPath))
        {
            // Load the new asset
            Object newAsset = AssetDatabase.LoadAssetAtPath(newPath, typeof(Object));

            // Replace the selected asset(s) with the new asset
            foreach (Object selectedObject in Selection.objects)
            {
                string oldPath = AssetDatabase.GetAssetPath(selectedObject);
                if (!string.IsNullOrEmpty(oldPath))
                {
                    // AssetDatabase.DeleteAsset(oldPath);
                    // AssetDatabase.CopyAsset(newPath, oldPath);
                    Debug.Log(oldPath);
                    Debug.Log(newPath);
                    
                    FileUtil.ReplaceFile(newPath, oldPath);
                }
            }

            AssetDatabase.Refresh();
        }
    }
}
