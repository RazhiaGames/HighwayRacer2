using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.Runtime.InteropServices;

public static class SaveDirectory
{
    [MenuItem("Tools/Razhia/Open Save Directory")]
    static void OpenSaveDirectory()
    {
        // string savePath = Application.persistentDataPath;
        var savePath = $"{Application.persistentDataPath}/StreamingAssets/";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            System.Diagnostics.Process.Start("open", $"-R \"{savePath}\"");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Use System.Diagnostics.Process to open the folder in Windows Explorer
            Process.Start("explorer.exe", savePath.Replace("/", "\\"));
        }
    }
}