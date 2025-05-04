using System.Linq;
using UnityEditor;
using UnityEngine;

public class SameName : MonoBehaviour
{
    [MenuItem("Art Tools/Same Name", false, 0)]
    static void PerformRename()
    {
        var selectedObjects = Selection.objects.OfType<GameObject>().ToArray();
        var lastTf = selectedObjects.Last().transform;


        for (var i = 0; i < selectedObjects.Length - 1; i++)
        {
            var selectedObject = selectedObjects[i]; 
            var currentTf = selectedObject;
            Undo.RecordObject(currentTf, "Align transform " + currentTf.name + " with " + lastTf.name);
            currentTf.name = lastTf.gameObject.name;
        }
    }
}