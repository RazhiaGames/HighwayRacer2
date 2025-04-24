//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HR_UI_MainmenuPanel : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_UI_MainmenuPanel instance;
    public static HR_UI_MainmenuPanel Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<HR_UI_MainmenuPanel>();
            }
            return instance;
        }
    }
    #endregion

    /// <summary>
    /// Mainmenu manager instance.
    /// </summary>
    private HR_MainMenuManager mainmenuManager;
    public HR_MainMenuManager MainMenuManager {
        get {
            if (mainmenuManager == null)
                mainmenuManager = HR_MainMenuManager.Instance;

            return mainmenuManager;
        }
    }

    /// <summary>
    /// Welcome menu panel GameObject.
    /// </summary>
    public GameObject welcomeMenu;

    /// <summary>
    /// Main menu panel GameObject.
    /// </summary>
    public GameObject mainMenu;

    /// <summary>
    /// Car selection menu GameObject.
    /// </summary>
    public GameObject carSelectionMenu;

    /// <summary>
    /// Modifications selection menu GameObject.
    /// </summary>
    public GameObject customizationSelectionMenu;

    /// <summary>
    /// Modifications selection menu GameObject.
    /// </summary>
    public GameObject modsSelectionMenu;

    /// <summary>
    /// Scene selection menu GameObject.
    /// </summary>
    public GameObject sceneSelectionMenu;

    /// <summary>
    /// Car stats panel GameObject.
    /// </summary>
    public GameObject carStatsPanel;

    /// <summary>
    /// Options menu GameObject.
    /// </summary>
    public GameObject optionsMenu;

    [Header("UI Loading Section")]
    /// <summary>
    /// Loading screen GameObject.
    /// </summary>
    public GameObject loadingScreen;

    /// <summary>
    /// Loading bar slider.
    /// </summary>
    public Slider loadingBar;

    [Header("Buttons")]
    /// <summary>
    /// Button for buying a car.
    /// </summary>
    public GameObject buyCarButton;

    /// <summary>
    /// Button for selecting a car.
    /// </summary>
    public GameObject selectCarButton;

    [Header("Texts")]
    /// <summary>
    /// Text component for displaying the player name.
    /// </summary>
    public TextMeshProUGUI playerName;

    /// <summary>
    /// Text component for displaying currency.
    /// </summary>
    public TextMeshProUGUI currency;

    /// <summary>
    /// Text component for displaying current panel's title.
    /// </summary>
    public TextMeshProUGUI panelTitleText;

    [Header("InputTexts")]
    /// <summary>
    /// Text component for displaying the player name.
    /// </summary>
    public TMP_InputField playerNameInputField;

    [Header("Best Score Texts")]
    /// <summary>
    /// Text component for displaying the current vehicle name.
    /// </summary>
    public TextMeshProUGUI vehicleNameText;

    /// <summary>
    /// Text component for displaying the best score in one way mode.
    /// </summary>
    public TextMeshProUGUI bestScoreOneWay;

    /// <summary>
    /// Text component for displaying the best score in two way mode.
    /// </summary>
    public TextMeshProUGUI bestScoreTwoWay;

    /// <summary>
    /// Text component for displaying the best score with time left.
    /// </summary>
    public TextMeshProUGUI bestScoreTimeLeft;

    /// <summary>
    /// Text component for displaying the best score in bomb mode.
    /// </summary>
    public TextMeshProUGUI bestScoreBomb;

    /// <summary>
    /// UI sliders for the vehicle stats.
    /// </summary>
    [Header("UI Sliders For Vehicle Stats")]
    public Image vehicleStats_Engine;
    public Image vehicleStats_Handling;
    public Image vehicleStats_Speed;

    /// <summary>
    /// UI sliders for the upgraded vehicle stats.
    /// </summary>
    [Space()] public Image vehicleStats_Engine_Upgraded;
    public Image vehicleStats_Handling_Upgraded;
    public Image vehicleStats_Speed_Upgraded;

    /// <summary>
    /// UI cart panel GameObject.
    /// </summary>
    [Header("Cart")]
    public GameObject cartPanel;

    /// <summary>
    /// Button for purchasing cart items.
    /// </summary>
    public GameObject purchaseCartButton;

    /// <summary>
    /// Target content GameObject where all cart items will be located.
    /// </summary>
    public GameObject cartItemsContent;

    /// <summary>
    /// Prefab or GameObject to instantiate new cart items.
    /// </summary>
    public HR_UI_CartItem cartItemReference;
    private HR_UI_PurchaseItem[] itemPurchaseButtons;

    /// <summary>
    /// List of all purchasable items in the cart.
    /// </summary>
    public List<HR_CartItem> itemsInCart {
        get {
            return MainMenuManager.itemsInCart;
        }
    }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake() {

        bool firstPlay = HR_API.IsFirstGameplay();

        if (!firstPlay) {

            EnableMenu(mainMenu);

        } else {

            playerNameInputField.SetTextWithoutNotify("New Player " + Random.Range(0, 999).ToString());
            EnableMenu(welcomeMenu);

        }

    }

    /// <summary>
    /// Enables the target menu and disables all other menus.
    /// </summary>
    /// <param name="activeMenu">The menu to activate.</param>
    public void EnableMenu(GameObject activeMenu) {

        welcomeMenu.SetActive(false);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        carSelectionMenu.SetActive(false);
        customizationSelectionMenu.SetActive(false);
        modsSelectionMenu.SetActive(false);
        sceneSelectionMenu.SetActive(false);
        loadingScreen.SetActive(false);

        activeMenu.SetActive(true);

        if (activeMenu == modsSelectionMenu)
            BestScores();

    }

    private void Start() {

        HR_API_OnPlayerNameChanged();
        HR_API_OnPlayerMoneyChanged();

    }

    /// <summary>
    /// Subscribes to the vehicle changed event when the object is enabled.
    /// </summary>
    private void OnEnable() {

        HR_Events.OnVehicleChanged += HR_Events_OnVehicleChanged;
        HR_API.OnPlayerNameChanged += HR_API_OnPlayerNameChanged;
        HR_API.OnPlayerMoneyChanged += HR_API_OnPlayerMoneyChanged;

    }

    private void HR_API_OnPlayerMoneyChanged() {

        currency.text = "$ " + HR_API.GetCurrency().ToString("F0");

    }

    private void HR_API_OnPlayerNameChanged() {

        playerName.text = HR_API.GetPlayerName();

    }

    /// <summary>
    /// Unsubscribes from the vehicle changed event when the object is disabled.
    /// </summary>
    private void OnDisable() {

        HR_Events.OnVehicleChanged -= HR_Events_OnVehicleChanged;
        HR_API.OnPlayerNameChanged -= HR_API_OnPlayerNameChanged;
        HR_API.OnPlayerMoneyChanged -= HR_API_OnPlayerMoneyChanged;

    }

    /// <summary>
    /// Updates the UI when the vehicle is changed.
    /// </summary>
    /// <param name="carIndex">The index of the selected car.</param>
    private void HR_Events_OnVehicleChanged(int carIndex) {

        CheckCurrentVehicle(carIndex);

    }

    /// <summary>
    /// Checks the current vehicle and updates the UI accordingly.
    /// </summary>
    /// <param name="carIndex">The index of the selected car.</param>
    private void CheckCurrentVehicle(int carIndex) {

        if (vehicleNameText)
            vehicleNameText.text = HR_PlayerCars.Instance.cars[carIndex].vehicleName;

        if (HR_API.OwnedVehicle(carIndex)) {

            if (buyCarButton.GetComponentInChildren<TextMeshProUGUI>())
                buyCarButton.GetComponentInChildren<TextMeshProUGUI>().text = "";

            buyCarButton.SetActive(false);
            selectCarButton.SetActive(true);

        } else {

            if (buyCarButton.GetComponentInChildren<TextMeshProUGUI>())
                buyCarButton.GetComponentInChildren<TextMeshProUGUI>().text = "$ " + HR_PlayerCars.Instance.cars[carIndex].price.ToString("F0");

            selectCarButton.SetActive(false);
            buyCarButton.SetActive(true);

        }

    }

    /// <summary>
    /// Adds money for testing purposes.
    /// </summary>
    public void Testing_AddMoney() {

        MainMenuManager.Testing_AddMoney();

    }

    /// <summary>
    /// Unlocks all vehicles for testing purposes.
    /// </summary>
    public void Testing_UnlockAllCars() {

        MainMenuManager.Testing_UnlockAllCars();

    }

    /// <summary>
    /// Deletes the save data and restarts the game for testing purposes.
    /// </summary>
    public void Testing_ResetSave() {

        MainMenuManager.Testing_ResetSave();

    }

    /// <summary>
    /// Sets the panel title text.
    /// </summary>
    /// <param name="title">The title to set.</param>
    public void SetPanelTitleText(string title) {

        panelTitleText.text = title;

    }

    /// <summary>
    /// Sets the mods panel state.
    /// </summary>
    /// <param name="state">The state to set.</param>
    public void SetModsPanel(bool state) {

        carStatsPanel.SetActive(state);

    }

    /// <summary>
    /// Purchases the current car.
    /// </summary>
    public void BuyCar() {

        MainMenuManager.BuyCar();

    }

    /// <summary>
    /// Selects the current car.
    /// </summary>
    public void SelectCar() {

        MainMenuManager.SelectCar();

    }

    /// <summary>
    /// Switches to the next car.
    /// </summary>
    public void PositiveCarIndex() {

        MainMenuManager.PositiveCarIndex();

    }

    /// <summary>
    /// Switches to the previous car.
    /// </summary>
    public void NegativeCarIndex() {

        MainMenuManager.NegativeCarIndex();

    }

    /// <summary>
    /// Selects the scene with the specified name.
    /// </summary>
    /// <param name="levelName">The name of the scene to load.</param>
    public void SelectScene(string levelName) {

        MainMenuManager.SelectScene(levelName);

    }

    /// <summary>
    /// Selects the mode with the specified index.
    /// </summary>
    /// <param name="_modeIndex">The index of the mode to select.</param>
    public void SelectMode(int _modeIndex) {

        MainMenuManager.SelectMode(_modeIndex);

    }

    /// <summary>
    /// Starts the race.
    /// </summary>
    public void StartRace() {

        MainMenuManager.StartRace();

    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame() {

        MainMenuManager.QuitGame();

    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update() {

        HR_Player currentVehicle = MainMenuManager.currentCar;

        if (currentVehicle) {

            RCCP_Engine engine = currentVehicle.CarController.Engine;
            RCCP_Stability stability = currentVehicle.CarController.Stability;
            RCCP_Differential differential = currentVehicle.CarController.Differential;
            RCCP_Customizer customizer = currentVehicle.CarController.Customizer;

            RCCP_VehicleUpgrade_UpgradeManager upgradeManager = null;
            RCCP_VehicleUpgrade_Engine upgradeEngine = null;
            RCCP_VehicleUpgrade_Handling upgradeHandling = null;
            RCCP_VehicleUpgrade_Speed upgradeSpeed = null;

            if (customizer)
                upgradeManager = customizer.UpgradeManager;

            if (upgradeManager) {

                upgradeEngine = upgradeManager.Engine;
                upgradeHandling = upgradeManager.Handling;
                upgradeSpeed = upgradeManager.Speed;

            }

            if (vehicleStats_Engine && engine)
                vehicleStats_Engine.fillAmount = Mathf.InverseLerp(-400f, 800f, engine.maximumTorqueAsNM);

            if (vehicleStats_Handling && stability)
                vehicleStats_Handling.fillAmount = Mathf.InverseLerp(0f, .5f, (stability.tractionHelperStrength) * 1f);

            if (vehicleStats_Speed && differential)
                vehicleStats_Speed.fillAmount = 1f - Mathf.InverseLerp(2.8f, 5.31f, differential.finalDriveRatio);

            if (vehicleStats_Engine_Upgraded && customizer && upgradeManager && upgradeEngine)
                vehicleStats_Engine_Upgraded.fillAmount = Mathf.InverseLerp(-400f, 800f, upgradeEngine.defEngine * upgradeEngine.efficiency);
            else if (vehicleStats_Engine_Upgraded)
                vehicleStats_Engine_Upgraded.fillAmount = 0f;

            if (vehicleStats_Handling_Upgraded && customizer && upgradeManager && upgradeHandling)
                vehicleStats_Handling_Upgraded.fillAmount = Mathf.InverseLerp(0f, .5f, upgradeHandling.defHandling * upgradeHandling.efficiency);
            else if (vehicleStats_Handling_Upgraded)
                vehicleStats_Handling_Upgraded.fillAmount = 0f;

            if (vehicleStats_Speed_Upgraded && customizer && upgradeManager && upgradeSpeed)
                vehicleStats_Speed_Upgraded.fillAmount = 1f - Mathf.InverseLerp(2.8f, 5.31f, Mathf.Lerp(upgradeSpeed.defRatio, upgradeSpeed.defRatio * .6f, upgradeSpeed.efficiency - 1f));
            else if (vehicleStats_Speed_Upgraded)
                vehicleStats_Speed_Upgraded.fillAmount = 0f;

            if (cartPanel.activeInHierarchy)
                UpdateCartItemsList();

        }

        if (MainMenuManager) {

            if (MainMenuManager.async != null && !MainMenuManager.async.isDone)
                loadingBar.value = MainMenuManager.async.progress;

        }

    }

    /// <summary>
    /// Checks if the upgrade item is purchased and updates the cart accordingly.
    /// </summary>
    /// <param name="newItem">The upgrade item to check.</param>
    public void CheckUpgradePurchased(HR_CartItem newItem) {

        HR_Player currentVehicle = MainMenuManager.currentCar;

        if (!currentVehicle.CarController.Customizer) {

            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");
            return;

        }

        if (PlayerPrefs.HasKey(currentVehicle.CarController.Customizer.saveFileName + newItem.saveKey))
            RemoveItemFromCart(newItem);
        else
            AddItemToCart(newItem);

    }

    /// <summary>
    /// Checks if the purchasable item is in the cart and updates the cart accordingly.
    /// </summary>
    /// <param name="newItem">The item to check.</param>
    public void CheckItemPurchased(HR_CartItem newItem) {

        if (PlayerPrefs.HasKey(newItem.saveKey))
            RemoveItemFromCart(newItem);
        else
            AddItemToCart(newItem);

        if (newItem.itemType == HR_CartItem.CartItemType.Customization)
            HR_UI_InfoDisplayer.Instance.ShowInfo("Added Unlocker To The Cart, Purchase It To Use Customization");

    }

    /// <summary>
    /// Adds a new item to the cart.
    /// </summary>
    /// <param name="newItem">The item to add.</param>
    public void AddItemToCart(HR_CartItem newItem) {

        MainMenuManager.AddItemToCart(newItem);

    }

    /// <summary>
    /// Removes an item from the cart.
    /// </summary>
    /// <param name="newItem">The item to remove.</param>
    public void RemoveItemFromCart(HR_CartItem newItem) {

        MainMenuManager.RemoveItemFromCart(newItem);

    }

    /// <summary>
    /// Clears the cart and restores the player's vehicle to the last loadout.
    /// </summary>
    public void ClearCart() {

        MainMenuManager.ClearCart();

        HR_UI_PurchaseItem[] uI_PurchaseItems = FindObjectsOfType<HR_UI_PurchaseItem>(true);

        for (int i = 0; i < uI_PurchaseItems.Length; i++)
            uI_PurchaseItems[i].OnEnable();

        HR_UI_PurchaseUpgrade[] uI_UpgradeItems = FindObjectsOfType<HR_UI_PurchaseUpgrade>(true);

        for (int i = 0; i < uI_UpgradeItems.Length; i++)
            uI_UpgradeItems[i].OnEnable();

    }

    /// <summary>
    /// Purchases all items in the cart and saves the player's vehicle loadout.
    /// </summary>
    public void PurchaseCart() {

        MainMenuManager.PurchaseCart();

        HR_UI_PurchaseItem[] uI_PurchaseItems = FindObjectsOfType<HR_UI_PurchaseItem>(true);

        for (int i = 0; i < uI_PurchaseItems.Length; i++)
            uI_PurchaseItems[i].CheckPurchase();

        HR_UI_PurchaseUpgrade[] uI_UpgradeItems = FindObjectsOfType<HR_UI_PurchaseUpgrade>(true);

        for (int i = 0; i < uI_UpgradeItems.Length; i++)
            uI_UpgradeItems[i].OnEnable();

    }

    /// <summary>
    /// Updates all items in the cart list.
    /// </summary>
    public void UpdateCartItemsList() {

        HR_UI_CartItem[] items = cartItemsContent.GetComponentsInChildren<HR_UI_CartItem>(true);

        foreach (HR_UI_CartItem item in items) {

            if (!Equals(item.gameObject, cartItemReference.gameObject))
                Destroy(item.gameObject);
            else if (cartItemReference.gameObject.activeSelf)
                cartItemReference.gameObject.SetActive(false);

        }

        for (int i = 0; i < itemsInCart.Count; i++) {

            HR_UI_CartItem cartItem = Instantiate(cartItemReference.gameObject, cartItemsContent.transform).GetComponent<HR_UI_CartItem>();
            cartItem.gameObject.SetActive(true);
            cartItem.SetItem(itemsInCart[i]);

        }

        int totalPrice = 0;

        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null)
                totalPrice += itemsInCart[i].price;

        }

        if (totalPrice > 0)
            purchaseCartButton.SetActive(true);
        else
            purchaseCartButton.SetActive(false);

        if (purchaseCartButton.activeSelf)
            purchaseCartButton.GetComponentInChildren<TextMeshProUGUI>().text = "Purchase For\n$ " + totalPrice.ToString("F0");

    }

    /// <summary>
    /// Saves the current vehicle customization.
    /// </summary>
    public void SaveCustomization() {

        MainMenuManager.SaveCustomization();

    }

    /// <summary>
    /// Loads the latest saved vehicle customization.
    /// </summary>
    public void LoadCustomization() {

        MainMenuManager.LoadCustomization();

    }

    /// <summary>
    /// Applies the loaded vehicle customization.
    /// </summary>
    public void ApplyCustomization() {

        MainMenuManager.ApplyCustomization();

    }

    public void EnterPlayerName() {

        HR_API.SetPlayerName(playerNameInputField.text);
        HR_UI_InfoDisplayer.Instance.ShowInfo("Welcome " + HR_API.GetPlayerName() + "!");
        EnableMenu(mainMenu);

    }

    /// <summary>
    /// Displays the best scores of all four modes.
    /// </summary>
    private void BestScores() {

        int[] scores = HR_API.GetHighScores();

        bestScoreOneWay.text = "BEST SCORE\n" + scores[0];
        bestScoreTwoWay.text = "BEST SCORE\n" + scores[1];
        bestScoreTimeLeft.text = "BEST SCORE\n" + scores[2];
        bestScoreBomb.text = "BEST SCORE\n" + scores[3];

    }

}
