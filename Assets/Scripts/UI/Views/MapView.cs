using System;
using Joyixir.GameManager.UI;
using RTLTMPro;
using SweetSugar.Scripts.MapScripts;
using UnityEngine;
using UnityEngine.UI;

public class MapView : View
{
    public GameObject playOnEffect;
    public GameObject playOffEffect;
    public RTLTextMeshPro prizeText;
    public ButtonElement goToGameButton;
    public ButtonElement goToGarageButton;


    private void OnEnable()
    {
        goToGameButton.onClick.AddListener(GoToGameButtonClicked);
        goToGarageButton.onClick.AddListener(GoToGarageButtonClicked);
        LevelsMap.MapItemClicked += OnMapItemClicked;
    }



    private void OnDisable()
    {
        goToGameButton.onClick.RemoveListener(GoToGameButtonClicked);
        goToGarageButton.onClick.RemoveListener(GoToGarageButtonClicked);
        LevelsMap.MapItemClicked -= OnMapItemClicked;
    }

    private void GoToGameButtonClicked()
    {
        var selectedLevelConfig = LevelsMap.Instance.selectedItem.config;
        Debug.Log($"Go To Game Button: {selectedLevelConfig.levelType.ToString()}");
        //GameManager.Instance.GoToLevel(selectedLevelConfig)
    }
    
    private void GoToGarageButtonClicked()
    {
        GarageManager.Instance.ShowGarage();
    }

    private void OnMapItemClicked(object sender, LevelReachedEventArgs e)
    {
        Initialize();
    }


    public void Initialize()
    {
        var level = LevelsMap.Instance.currentClickedItem;
        prizeText.text = level.config.levelPrize.ToString();
        playOffEffect.SetActive(level.IsLocked);
        playOnEffect.SetActive(!level.IsLocked);
    }


    protected override void OnBackBtn()
    {
    }
}