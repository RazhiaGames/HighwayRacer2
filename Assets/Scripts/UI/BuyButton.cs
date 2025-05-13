using System;
using System.Collections;
using System.Collections.Generic;
using Highway_Racer.Scripts.UI_Scripts.Upgrade;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    private HrUIMod _uiMod;

    private void Awake()
    {
        _button.onClick.AddListener(ButtonPressed);
    }

    private void ButtonPressed()
    {
        _uiMod.Buy();
        _uiMod.CheckPurchase();
    }

    public void SetActiveBuyer(HrUIMod activeHrMod)
    {
        _uiMod = activeHrMod;
    }

    public void EnableItself()
    {
        gameObject.SetActive(true);
    }
    
    public void DisableItself()
    {
        gameObject.SetActive(false);
    }
}
