using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using SweetSugar.Scripts.MapScripts;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class UIManager : Singleton<UIManager>
{


    private Dictionary<string, GameObject> _activeViews = new();
    private Dictionary<string, GameObject> _viewPrefabs = new();

    [PropertyTooltip("3 Different Layers for UI Views")] [FoldoutGroup("UI Containers")]
    public List<Transform> containers;

    [Button]
    public async void ShowMapView()
    {
        MapView mapView = await ShowViewAsync<MapView>("View-Map");
        mapView.Initialize(LevelsMap.Instance.selectedLevel.config);
        mapView.AnimateUp();
    }
    [Button]

    public  void HideMapView()
    {
         HideView("View-Map");
    }
    
    #region AddressablesBoilerPlate
    
    public async UniTask<T> ShowViewAsync<T>(string addressKey, ViewPriority order = ViewPriority.Low, CancellationToken ct = default)
        where T : Component
    {
        if (_activeViews.TryGetValue(addressKey, out var existingView))
            return existingView.GetComponent<T>();

        GameObject prefab;
        if (!_viewPrefabs.TryGetValue(addressKey, out prefab))
        {
            prefab = await Addressables.LoadAssetAsync<GameObject>(addressKey).ToUniTask(cancellationToken: ct);
            _viewPrefabs[addressKey] = prefab;
        }

        GameObject instance = Instantiate(prefab, containers[(int)order]);
        instance.name = prefab.name; // Optional for clarity
        _activeViews[addressKey] = instance;

        return instance.GetComponent<T>();
    }


    public void HideView(string addressKey)
    {
        if (_activeViews.TryGetValue(addressKey, out var view))
        {
            Destroy(view);
            _activeViews.Remove(addressKey);
        }
    }

    public async UniTask HideAndReleaseViewAsync(string addressKey)
    {
        if (_activeViews.TryGetValue(addressKey, out var view))
        {
            Destroy(view);
            _activeViews.Remove(addressKey);
        }

        if (_viewPrefabs.TryGetValue(addressKey, out var prefab))
        {
            Addressables.Release(prefab);
            _viewPrefabs.Remove(addressKey);
        }

        await UniTask.Yield(); // optional: delay if needed for frame sync
    }
    

    #endregion
    
    public enum ViewPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
}