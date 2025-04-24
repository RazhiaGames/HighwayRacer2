//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Manages the main menu events, including creating and spawning vehicles, switching them, and enabling/disabling menus.
/// </summary>
public class HR_MainMenuManager : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_MainMenuManager instance;
    public static HR_MainMenuManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<HR_MainMenuManager>();
            }

            return instance;
        }
    }
    #endregion

    /// <summary>
    /// Spawn location of the cars.
    /// </summary>
    [Header("Spawn Location Of The Cars")]
    public Transform carSpawnLocation;

    /// <summary>
    /// Array to store all created cars.
    /// </summary>
    private GameObject[] createdCars;

    /// <summary>
    /// Current selected car.
    /// </summary>
    public HR_Player currentCar;

    /// <summary>
    /// Headlights of the car should be on?
    /// </summary>
    public bool headlightsOn = false;

    /// <summary>
    /// Current car index.
    /// </summary>
    public int carIndex = 0;

    /// <summary>
    /// All purchasable items in the cart (not purchased yet).
    /// </summary>
    public List<HR_CartItem> itemsInCart = new List<HR_CartItem>();

    /// <summary>
    /// AsyncOperation for scene loading.
    /// </summary>
    public AsyncOperation async;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake() {

        // Setting time scale, volume, unpause, and target frame rate.
        Time.timeScale = 1f;
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.pause = false;

        if (HR_UIOptionsManager.Instance)
            HR_UIOptionsManager.Instance.OnEnable();

        // Getting the money.
        if (!PlayerPrefs.HasKey("Currency"))
            PlayerPrefs.SetInt("Currency", HR_Settings.Instance.initialMoney);

        // Getting last selected car index.
        carIndex = PlayerPrefs.GetInt("SelectedPlayerCarIndex", 0);

        CreateCars(); // Creating all selectable cars at once.
        SpawnCar(); // Spawning only target car (carIndex).

    }

    /// <summary>
    /// Creates all spawnable cars at once.
    /// </summary>
    private void CreateCars() {

        // Creating a new array.
        createdCars = new GameObject[HR_PlayerCars.Instance.cars.Length];

        // Setting array elements.
        for (int i = 0; i < createdCars.Length; i++) {

            createdCars[i] = (RCCP.SpawnRCC(HR_PlayerCars.Instance.cars[i].playerCar.GetComponent<RCCP_CarController>(), carSpawnLocation.position, carSpawnLocation.rotation, false, false, false)).gameObject;
            createdCars[i].SetActive(false);

            if (createdCars[i].GetComponent<RCCP_CarController>().Lights != null)
                createdCars[i].GetComponent<RCCP_CarController>().Lights.lowBeamHeadlights = headlightsOn;

        }

    }

    /// <summary>
    /// Spawns the target car (carIndex).
    /// </summary>
    private void SpawnCar() {

        // If the price of the car is 0, or unlocked, save it as owned car.
        if (HR_PlayerCars.Instance.cars[carIndex].price <= 0 || HR_PlayerCars.Instance.cars[carIndex].unlocked)
            HR_API.UnlockVehicle(carIndex);

        // Disabling all cars at once. And then enabling only the target car (carIndex). And make sure spawned cars are always at spawn point.
        for (int i = 0; i < createdCars.Length; i++) {

            if (createdCars[i].activeInHierarchy)
                createdCars[i].SetActive(false);

        }

        // Enabling only the target car (carIndex).
        createdCars[carIndex].SetActive(true);
        RCCP.RegisterPlayerVehicle(createdCars[carIndex].GetComponent<RCCP_CarController>(), false, false);

        // Setting the current car.
        currentCar = createdCars[carIndex].GetComponent<HR_Player>();

        LoadCustomization();
        ApplyCustomization();

        HR_Events.Event_OnVehicleChanged(carIndex);

    }

    /// <summary>
    /// Purchases the current car.
    /// </summary>
    public void BuyCar() {

        // If we own the car, don't consume currency.
        if (HR_API.OwnedVehicle(carIndex)) {

            Debug.LogError("Car is already owned!");
            return;

        }

        // If the currency is enough, save it and consume currency. Otherwise, display the informer.
        if (HR_API.GetCurrency() >= HR_PlayerCars.Instance.cars[carIndex].price) {

            HR_API.ConsumeCurrency(HR_PlayerCars.Instance.cars[carIndex].price);

        } else {

            HR_UI_InfoDisplayer.Instance.ShowInfo("You have to earn " + (HR_PlayerCars.Instance.cars[carIndex].price - HR_API.GetCurrency()).ToString() + " more money to buy this vehicle");
            return;

        }

        // Saving the car.
        HR_API.UnlockVehicle(carIndex);

        // And spawning again to check modders of the car.
        SpawnCar();

    }

    /// <summary>
    /// Selects the current car with carIndex.
    /// </summary>
    public void SelectCar() {

        PlayerPrefs.SetInt("SelectedPlayerCarIndex", carIndex);

    }

    /// <summary>
    /// Switches to the next car.
    /// </summary>
    public void PositiveCarIndex() {

        carIndex++;

        if (carIndex >= createdCars.Length)
            carIndex = 0;

        SpawnCar();

    }

    /// <summary>
    /// Switches to the previous car.
    /// </summary>
    public void NegativeCarIndex() {

        carIndex--;

        if (carIndex < 0)
            carIndex = createdCars.Length - 1;

        SpawnCar();

    }

    /// <summary>
    /// Selects the scene with the specified name.
    /// </summary>
    /// <param name="levelName">The name of the scene to load.</param>
    public void SelectScene(string levelName) {

        PlayerPrefs.SetString("SelectedScene", levelName);

    }

    /// <summary>
    /// Selects the mode with the specified index.
    /// </summary>
    /// <param name="_modeIndex">The index of the mode to select.</param>
    public void SelectMode(int _modeIndex) {

        // Saving the selected mode, and enabling the scene selection menu.
        PlayerPrefs.SetInt("SelectedModeIndex", _modeIndex);

    }

    /// <summary>
    /// Starts the race.
    /// </summary>
    public void StartRace() {

        SelectCar();
        //SaveCustomization();

        async = SceneManager.LoadSceneAsync(PlayerPrefs.GetString("SelectedScene", ""));

    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame() {

        Application.Quit();

    }

    /// <summary>
    /// Adding money for testing purposes.
    /// </summary>
    public void Testing_AddMoney() {

        HR_API.AddCurrency(10000);

    }

    /// <summary>
    /// Unlocking all vehicles for testing purposes.
    /// </summary>
    public void Testing_UnlockAllCars() {

        HR_API.UnlockAllVehicles();

    }

    /// <summary>
    /// Deletes the save data and restarts the game for testing purposes.
    /// </summary>
    public void Testing_ResetSave() {

        HR_API.ResetGame();

    }

    /// <summary>
    /// Saves the current loadout.
    /// </summary>
    public void SaveCustomization() {

        HR_Player currentVehicle = currentCar;

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Save();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");

        //SGS_UI_Informer.Instance.Info("Customization saved!");

    }

    /// <summary>
    /// Loads the latest loadout.
    /// </summary>
    public void LoadCustomization() {

        HR_Player currentVehicle = currentCar;

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Load();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");

    }

    /// <summary>
    /// Applies the loaded loadout.
    /// </summary>
    public void ApplyCustomization() {

        HR_Player currentVehicle = currentCar;

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Initialize();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");

    }

    private void Reset() {

        GameObject carSpawnLocationGO = GameObject.Find("HR_SpawnLocation");

        if (carSpawnLocationGO)
            carSpawnLocation = carSpawnLocationGO.transform;

        if (carSpawnLocation != null)
            return;

        carSpawnLocation = new GameObject("HR_SpawnLocation").transform;
        carSpawnLocation.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

    }

    /// <summary>
    /// Adds a new item to the cart. Cart can't have items with same type.
    /// </summary>
    /// <param name="newItem"></param>
    public void AddItemToCart(HR_CartItem newItem) {

        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null) {

                if (Equals(itemsInCart[i].itemType, newItem.itemType))
                    itemsInCart.RemoveAt(i);

            }

        }

        if (!itemsInCart.Contains(newItem))
            itemsInCart.Add(newItem);

    }

    /// <summary>
    /// Removes an item from the cart. Cart can't have items with same type.
    /// </summary>
    /// <param name="newItem"></param>
    public void RemoveItemFromCart(HR_CartItem newItem) {

        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null) {

                if (Equals(itemsInCart[i].itemType, newItem.itemType))
                    itemsInCart.RemoveAt(i);

            }

        }

        if (itemsInCart.Contains(newItem))
            itemsInCart.Remove(newItem);

    }

    /// <summary>
    /// Clears the cart and restores the player vehicle back to the last loadout.
    /// </summary>
    public void ClearCart() {

        itemsInCart.Clear();

        LoadCustomization();
        ApplyCustomization();

        HR_Player currentVehicle = currentCar;

        RCCP_VehicleUpgrade_WheelManager wheelManager = currentVehicle.CarController.Customizer.WheelManager;

        if (wheelManager && currentVehicle.CarController.Customizer.WheelManager.wheelIndex == -1)
            currentVehicle.CarController.Customizer.WheelManager.Restore();

        RCCP_VehicleUpgrade_PaintManager paintManager = currentVehicle.CarController.Customizer.PaintManager;

        if (paintManager && currentVehicle.CarController.Customizer.PaintManager.color == new Color(1f, 1f, 1f, 0f))
            currentVehicle.CarController.Customizer.PaintManager.Restore();

        //  Updating all purchasable items in the scene.
        HR_UI_PurchaseItem[] uI_PurchaseItems = FindObjectsOfType<HR_UI_PurchaseItem>(true);

        for (int i = 0; i < uI_PurchaseItems.Length; i++)
            uI_PurchaseItems[i].OnEnable();

        //  Updating all upgradable items in the scene.
        HR_UI_PurchaseUpgrade[] uI_UpgradeItems = FindObjectsOfType<HR_UI_PurchaseUpgrade>(true);

        for (int i = 0; i < uI_UpgradeItems.Length; i++)
            uI_UpgradeItems[i].OnEnable();

    }

    /// <summary>
    /// Purchases all items in the cart and saves the player vehicle loadout..
    /// </summary>
    public void PurchaseCart() {

        //  Calculating the total price.
        int totalPrice = 0;

        //  Calculating the total price.
        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null)
                totalPrice += itemsInCart[i].price;

        }

        //  If player money is enough to purchase the cart, proceed. Otherwise return.
        if (HR_API.GetCurrency() < totalPrice) {

            //SGS_UI_Informer.Instance.Info("Not enough money to purchase the cart!");
            return;

        }

        //  Consuming the money.
        HR_API.ConsumeCurrency(totalPrice);

        //  Saving all purchased items.
        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null)
                PlayerPrefs.SetInt(itemsInCart[i].saveKey, 1);

        }

        //  Saving the loadout.
        SaveCustomization();

        //  Clearing the cart.
        itemsInCart.Clear();

        //  Updating all purchasable items in the scene.
        HR_UI_PurchaseItem[] uI_PurchaseItems = FindObjectsOfType<HR_UI_PurchaseItem>(true);

        for (int i = 0; i < uI_PurchaseItems.Length; i++)
            uI_PurchaseItems[i].CheckPurchase();

        //  Updating all upgradable items in the scene.
        HR_UI_PurchaseUpgrade[] uI_UpgradeItems = FindObjectsOfType<HR_UI_PurchaseUpgrade>(true);

        for (int i = 0; i < uI_UpgradeItems.Length; i++)
            uI_UpgradeItems[i].OnEnable();

    }

    /// <summary>
    /// Updates all items in the cart list.
    /// </summary>
    //public void UpdateCartItemsList() {

    //    //  Getting all items in the cart.
    //    SGS_UI_CartItem[] items = cartItemsContent.GetComponentsInChildren<SGS_UI_CartItem>(true);

    //    //  Destroying all items before instantiating them.
    //    foreach (SGS_UI_CartItem item in items) {

    //        if (!Equals(item.gameObject, cartItemReference.gameObject))
    //            Destroy(item.gameObject);
    //        else if (cartItemReference.gameObject.activeSelf)
    //            cartItemReference.gameObject.SetActive(false);

    //    }

    //    //  Instantiating all items in the cart and setting them.
    //    for (int i = 0; i < itemsInCart.Count; i++) {

    //        SGS_UI_CartItem cartItem = Instantiate(cartItemReference.gameObject, cartItemsContent.transform).GetComponent<SGS_UI_CartItem>();
    //        cartItem.gameObject.SetActive(true);
    //        cartItem.SetItem(itemsInCart[i]);

    //    }

    //    //  Calculating the total price.
    //    int totalPrice = 0;

    //    //  Calculating the total price.
    //    for (int i = 0; i < itemsInCart.Count; i++) {

    //        if (itemsInCart[i] != null)
    //            totalPrice += itemsInCart[i].price;

    //    }

    //    //  Enable the purchase button if total price is above 0. Disable otherwise.
    //    if (totalPrice > 0)
    //        purchaseCartButton.SetActive(true);
    //    else
    //        purchaseCartButton.SetActive(false);

    //    //  Set price text if purchase button is enabled.
    //    if (purchaseCartButton.activeSelf)
    //        purchaseCartButton.GetComponentInChildren<TextMeshProUGUI>().text = "Purchase For\n$ " + totalPrice.ToString("F0");

    //}

}
