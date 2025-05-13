using System;
using UnityEngine;

public class GarageManager : Singleton<GarageManager>
{
    public GameObject garageCamera;
    public GameObject mapObject;


    protected override void Awake()
    {
        base.Awake();
        mapObject.SetActive(false);
    }

    public void ShowMap()
    {
        garageCamera.SetActive(false);
        mapObject.SetActive(true);
        UIManager.Instance.ShowMapView();
        UIManager.Instance.HideMainMenuView();
    }

    public void ShowGarage()
    {
        UIManager.Instance.HideMapView();
        garageCamera.SetActive(true);
        mapObject.SetActive(false);
        UIManager.Instance.ShowMainMenuView();
        UIManager.Instance.HideMapView();

    }
    
    
}
