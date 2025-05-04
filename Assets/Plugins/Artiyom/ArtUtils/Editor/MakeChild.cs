using System.Linq;
using UnityEditor;
using UnityEngine;

public class MakeChild : EditorWindow
{
    [MenuItem("Art Tools/Make Child", false, 0)]
    static void PerformMakeChild()
    {
        var selectedObjects = Selection.objects.OfType<GameObject>().ToArray();
        var lastTf = selectedObjects.Last().transform;


        for (var i = 0; i < selectedObjects.Length - 1; i++)
        {
            var selectedObject = selectedObjects[i];
            var currentTf = selectedObject.transform;
            
            // Record the changes to the selected object and the last object.
            Undo.SetTransformParent(currentTf, lastTf, "Make Child " + currentTf.name + " with " + lastTf.name);

            currentTf.SetParent(lastTf);
            

            EditorUtility.SetDirty(selectedObject);
        }
    }
}