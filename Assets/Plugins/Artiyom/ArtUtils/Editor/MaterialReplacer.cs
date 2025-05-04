using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

public class MaterialReplacer : EditorWindow
{
    private Material material;
    private string materialName;
    private List<Object> objects;


    [MenuItem("Art Tools/Material Replacer")]
    static void CreateReplacer()
    {
        GetWindow<MaterialReplacer>();
    }

    public void Replace()
    {
        var activeObjects = Selection.objects.OfType<GameObject>().ToArray();

        foreach (var activeObject in activeObjects)
        {
            var meshRenderer = activeObject.GetComponentsInChildren<MeshRenderer>();
            Undo.RecordObject(activeObject, "Change Material " + activeObject.name);
            foreach (var renderer in meshRenderer)
            {
                var Mats = renderer.sharedMaterials.ToList();

                for (var i = 0; i < Mats.Count; i++)
                {
                    var mat = Mats[i];
                    if (mat.name == materialName)
                        Mats[i] = material;
                }

                renderer.sharedMaterials = Mats.ToArray();
            }
        }
    }



    private void OnGUI()
    {
        material = (Material)EditorGUILayout.ObjectField("material", material, typeof(Material));
        materialName = EditorGUILayout.TextField("Material Name", materialName);
        if (GUILayout.Button("Replace Material"))
        {
            Replace();
            OnGUI();
            GUI.enabled = false;
        }
    }

}