using LeTai.TrueShadow;
using RTLTMPro;
using UnityEngine;
using UnityEditor;
using TMPro;

public class ReplaceTMPWithPrefab : EditorWindow
{
    public GameObject prefab;

    [MenuItem("Tools/Replace TMP with Prefab")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceTMPWithPrefab>("Replace TMP with Prefab");
    }

    private void OnGUI()
    {
        GUILayout.Label("Replace TextMeshPro with Prefab", EditorStyles.boldLabel);

        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        if (GUILayout.Button("Replace Selected"))
        {
            if (prefab == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a prefab.", "OK");
                return;
            }

            ReplaceSelectedTextMeshPro();
        }
    }

    private void ReplaceSelectedTextMeshPro()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog("Error", "No GameObjects selected.", "OK");
            return;
        }

        foreach (GameObject obj in selectedObjects)
        {
            RTLTextMeshPro tmpComponent = obj.GetComponent<RTLTextMeshPro>();
            RectTransform tmpTf = obj.GetComponent<RectTransform>();
            TrueShadow tmpTrueShadow = obj.GetComponent<TrueShadow>();
            string tmpName = obj.name;
            if (tmpComponent == null)
            {
                Debug.LogWarning($"GameObject '{obj.name}' does not have a TextMeshPro component. Skipping...");
                continue;
            }

            // Instantiate the prefab
            GameObject newPrefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (newPrefabInstance == null)
            {
                Debug.LogError("Failed to instantiate prefab. Ensure the assigned prefab is valid.");
                return;
            }

            // Copy the TextMeshPro properties to the new prefab instance
            RTLTextMeshPro newTMP = newPrefabInstance.GetComponent<RTLTextMeshPro>();
            TrueShadow newTrueShadow = newPrefabInstance.GetComponent<TrueShadow>();
            if (newTMP == null)
            {
                Debug.LogError("The assigned prefab does not have a TextMeshPro component. Please assign a valid prefab.");
                DestroyImmediate(newPrefabInstance);
                return;
            }
            newPrefabInstance.transform.SetParent(obj.transform.parent);
            newPrefabInstance.transform.localPosition = obj.transform.localPosition;
            newPrefabInstance.transform.localRotation = obj.transform.localRotation;
            newPrefabInstance.transform.localScale = obj.transform.localScale;
            newTMP.gameObject.name = tmpName;

            CopyTMPProperties(tmpComponent, tmpTf, newTMP);
            if (tmpTrueShadow != null)
            {
                CopyTrueShadowProperties(tmpTrueShadow, newTrueShadow);
            }
            else
            {
                DestroyImmediate(newTrueShadow);
            }

            // Replace the original GameObject


            Undo.RegisterCreatedObjectUndo(newPrefabInstance, "Replace TMP with Prefab");
            Undo.DestroyObjectImmediate(obj);
        }
    }



    private void CopyTMPProperties(RTLTextMeshPro source, RectTransform sourceTf, RTLTextMeshPro target)
    {
        target.text = source.OriginalText;
        target.color = source.color;
        target.enableAutoSizing = source.enableAutoSizing;
        if (source.enableAutoSizing)
        {
            target.fontSizeMin = source.fontSizeMin;
            target.fontSizeMax = source.fontSizeMax;
        }
        target.fontSize = source.fontSize;
        target.alignment = source.alignment;
        target.textWrappingMode = source.textWrappingMode;
        target.richText = source.richText;
        target.characterSpacing = source.characterSpacing;
        target.lineSpacing = source.lineSpacing;
        target.paragraphSpacing = source.paragraphSpacing;
        target.fontStyle = source.fontStyle;
        target.overflowMode = source.overflowMode;
        target.margin = source.margin;
        // Copy other properties as needed.
        CopyRectTransform(sourceTf, target.rectTransform);
    }
    
    private void CopyRectTransform(RectTransform source, RectTransform target)
    {

        target.anchorMin = source.anchorMin;
        target.anchorMax = source.anchorMax;
        target.anchoredPosition = source.anchoredPosition;
        target.sizeDelta = source.sizeDelta;
        target.pivot = source.pivot;
        target.localRotation = source.localRotation;
        target.localScale = source.localScale;
        
        // target.localPosition = source.localPosition;
        // target.localRotation = source.localRotation;
        // target.localScale = source.localScale;

        target.anchoredPosition = source.anchoredPosition;
        target.localPosition = source.localPosition;
    }
    
    private void CopyTrueShadowProperties(TrueShadow source, TrueShadow target)
    {
        target.Algorithm = source.Algorithm;
        target.Inset = source.Inset;
        target.Size = source.Size;
        target.Spread = source.Spread;
        target.UseGlobalAngle = source.UseGlobalAngle;
        target.OffsetAngle = source.OffsetAngle;
        target.OffsetDistance = source.OffsetDistance;
        target.Color = source.Color;
        target.BlendMode = source.BlendMode;
    }
}
