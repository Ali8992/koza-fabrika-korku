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
        PlayerSettings.bundleVersion = "1.13";
        PlayerSettings.Android.bundleVersionCode = 13;
        // Yatay ekran (mobil korku: landscape)
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        // Kırmızı kelebek ikon
        var iconTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_Koza/Icon/koza-icon.png");
        if (iconTex != null)
        {
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, new[] { iconTex });
            Debug.Log("KOZA: ikon + yatay ekran ayarlandi.");
        }
        else Debug.LogWarning("KOZA: ikon bulunamadı!");
        Debug.Log("KOZA: Configure tamam.");
    }
}
