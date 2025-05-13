using UnityEngine;

namespace Highway_Racer.Scripts.UI_Scripts.Upgrade
{
    public class HrUIMod : MonoBehaviour  
    {
        [SerializeField] protected  BuyButton _buyButton;

        public virtual void Upgrade()
        {
            _buyButton.SetActiveBuyer(this);
        }
        public virtual void Buy()
        {
        }
    
        public virtual void CheckPurchase() 
        {
        }
    }
}