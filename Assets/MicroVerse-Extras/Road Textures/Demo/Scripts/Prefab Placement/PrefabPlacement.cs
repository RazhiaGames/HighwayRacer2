using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rowlan.PrefabPlacement
{
    public class PrefabPlacement : MonoBehaviour
    {
        public string namePrefix = "Cell ";
        public GameObject prefab;

        public float size = 1;
        public float margin = 0.5f;

        public int columnCount = 4;

        public string pathFilter;

        public int fontSize = 12;
        public float textOffsetZ = 0f;

        public int headerFontSize = 40;
        public float headerOffsetX = 0f;
        public float headerOffsetZ = 0f;
    }
}