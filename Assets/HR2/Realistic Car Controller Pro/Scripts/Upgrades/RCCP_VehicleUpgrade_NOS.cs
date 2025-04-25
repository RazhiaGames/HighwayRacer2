//----------------------------------------------
//        Realistic Car Controller Pro
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Upgrades NOS of the car controller.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller Pro/Customization/RCCP Vehicle Upgrade NOS")]
public class RCCP_VehicleUpgrade_NOS : RCCP_Component {

    private int _nosLevel = 0;

    /// <summary>
    /// Current NOS level. Maximum is 1.
    /// </summary>
    public int NOSLevel {
        get {
            return _nosLevel;
        }
        set {
            if (value <= 1)
                _nosLevel = value;
        }
    }

    /// <summary>
    /// Updates NOS and initializes it.
    /// </summary>
    public void Initialize() {

        if (!CarController.OtherAddonsManager) {

            Debug.LogError("OtherAddonsManager couldn't found in the vehicle. RCCP_VehicleUpgrade_NOS needs it to upgrade the NOS level");
            enabled = false;
            return;

        }

        if (!CarController.OtherAddonsManager.Nos) {

            Debug.LogError("NOS couldn't found in the vehicle. RCCP_VehicleUpgrade_NOS needs it to upgrade the NOS level");
            enabled = false;
            return;

        }

        CarController.OtherAddonsManager.Nos.enabled = NOSLevel == 1 ? true : false;

    }

    /// <summary>
    /// Updates NOS and save it.
    /// </summary>
    public void UpdateStats() {

        if (!CarController.OtherAddonsManager) {

            Debug.LogError("OtherAddonsManager couldn't found in the vehicle. RCCP_VehicleUpgrade_NOS needs it to upgrade the NOS level");
            enabled = false;
            return;

        }

        if (!CarController.OtherAddonsManager.Nos) {

            Debug.LogError("NOS couldn't found in the vehicle. RCCP_VehicleUpgrade_NOS needs it to upgrade the NOS level");
            enabled = false;
            return;

        }

        CarController.OtherAddonsManager.Nos.enabled = NOSLevel == 1 ? true : false;

    }

    public void Restore() {

        NOSLevel = 0;

        CarController.OtherAddonsManager.Nos.enabled = NOSLevel == 1 ? true : false;

    }

}
