//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Globalization;

public class HR_PreBuildChecker : IPreprocessBuildWithReport {

    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report) {

        // Set the culture to InvariantCulture
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

    }

}
