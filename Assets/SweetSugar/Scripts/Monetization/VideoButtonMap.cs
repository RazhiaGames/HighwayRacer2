using UnityEngine;
using UnityEngine.UI;

namespace SweetSugar.Scripts.Monetization
{
    public class VideoButtonMap : MonoBehaviour
    {
        public Animator anim;
        public Button button;

        private void OnEnable()
        {
            button.interactable = false;
            button.gameObject.SetActive(false);
            Invoke("Prepare",2);
        }

        private void Prepare()
        {
   
        }

        public void ShowVideoAds()
        {

        }

        private void ShowButton()
        {
            button.gameObject.SetActive(true);
            button.interactable = true;
            anim.SetTrigger("show");
        }

        public void Hide()
        {
            button.interactable = false;

        }
    }
}