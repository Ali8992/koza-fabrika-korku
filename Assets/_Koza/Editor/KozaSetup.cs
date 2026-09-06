using UnityEditor;
using UnityEngine;

// KOZA - Batchmode kurulum: Android SDK/NDK/JDK yolları + player ayarları
public static class KozaSetup
{
    public static void Configure()
    {
        UnityEditor.Android.AndroidExternalToolsSettings.sdkRootPath = @"C:\Android\Sdk";
        UnityEditor.Android.AndroidExternalToolsSettings.jdkRootPath = @"C:\Program Files\Microsoft\jdk-11.0.32.101-hotspot";
        UnityEditor.Android.AndroidExternalToolsSettings.ndkRootPath = @"C:\Android\Sdk\ndk\23.1.7779620";
        Debug.Log("KOZA: SDK/NDK/JDK yollari ayarlandi.");
        PlayerSettings.applicationIdentifier = "com.koza.horror";
        PlayerSettings.productName = "KOZA";
        PlayerSettings.companyName = "KozaFabrika";
        Debug.Log("KOZA: Configure tamam.");
    }
}
