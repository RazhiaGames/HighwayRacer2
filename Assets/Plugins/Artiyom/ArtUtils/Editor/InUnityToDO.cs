using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TODO List", menuName = "ScriptableObjects/TODO List", order = 1)]
public class TODO : ScriptableObject
{
    public List<string> whatToDo;

    public List<string> done;
}