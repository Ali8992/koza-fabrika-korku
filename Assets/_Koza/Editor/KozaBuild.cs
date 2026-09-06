using UnityEditor;
using UnityEngine;

// KOZA - Tek tıkla Android APK build (Unity -> KOZA menüsü)
public static class KozaBuild
{
    [MenuItem("KOZA/Android Ayarlarını Uygula")]
    public static void ApplyAndroidSettings()
    {
        PlayerSettings.applicationIdentifier = "com.koza.horror";
        PlayerSettings.productName = "KOZA";
        PlayerSettings.companyName = "KozaFabrika";
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        EditorUtility.DisplayDialog("KOZA", "Android ayarları uygulandı: com.koza.horror, ARM64, IL2CPP", "Tamam");
    }

    public static void FullPipeline()
    {
        KozaSetup.Configure();
        BuildApk();
    }

    [MenuItem("KOZA/APK Build Al (Release)")]
    public static void BuildApk()
    {
        ApplyAndroidSettings();
        string outPath = System.IO.Path.Combine(Application.dataPath, "../../koza-unity.apk");
        outPath = System.IO.Path.GetFullPath(outPath);
        var report = BuildPipeline.BuildPlayer(
            new[] { "Assets/_Koza/Scenes/Chapter1.unity" },
            outPath,
            BuildTarget.Android,
            BuildOptions.None);
        Debug.Log($"KOZA APK build sonucu: {report.summary.result} -> {outPath}");
    }
}
