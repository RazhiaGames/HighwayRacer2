using TMPro;
using UnityEngine;

namespace SweetSugar.Scripts.MapScripts
{
    public class StaticMapPlay : MonoBehaviour
    {
        public TextMeshProUGUI text;
        private int level;

        private void OnEnable()
        {
            level = LevelsMap.GetLastestReachedLevel();
            text.text = "Level" + " " + level;
        }

        public void PressPlay()
        {
            
        }
    }
}