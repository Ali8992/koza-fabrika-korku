using UnityEngine;
using UnityEngine.SceneManagement;

// Chapter0: Ali Kayra'nın fabrikaya girişi - hikaye ekranı + FABRİKAYA GİR butonu
// (UGUI paketsiz: yerleşik IMGUI ile çizilir)
public class StoryIntro : MonoBehaviour
{
    GUIStyle titleStyle;
    GUIStyle storyStyle;
    GUIStyle buttonStyle;
    bool stylesReady = false;

    void MakeStyles()
    {
        titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 64;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.normal.textColor = Color.white;
        titleStyle.fontStyle = FontStyle.Bold;

        storyStyle = new GUIStyle(GUI.skin.label);
        storyStyle.fontSize = 26;
        storyStyle.alignment = TextAnchor.MiddleCenter;
        storyStyle.normal.textColor = new Color(0.85f, 0.85f, 0.85f);
        storyStyle.wordWrap = true;

        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 34;
        buttonStyle.normal.textColor = Color.white;
        stylesReady = true;
    }

    void OnGUI()
    {
        if (!stylesReady) MakeStyles();
        float w = Screen.width, h = Screen.height;
        GUI.Label(new Rect(0, h * 0.08f, w, 100), "KOZA FABRİKASI", titleStyle);
        GUI.Label(new Rect(w * 0.1f, h * 0.28f, w * 0.8f, h * 0.35f),
            "Gece yarısı. Yağmur yağıyor.\n\nAli Kayra, Koza Fabrikası'nın paslı kapısında duruyor. " +
            "İçeriden makine sesleri ve fısıltılar geliyor. Görevi belli: 5 mini-boss'u geçip " +
            "en derinde bekleyen Prof. Ekrem ile yüzleşmek.\n\nEl fenerini yakıyor... içeri giriyor.",
            storyStyle);
        if (GUI.Button(new Rect(w / 2 - 250, h * 0.78f, 500, 90), "FABRİKAYA GİR", buttonStyle))
            SceneManager.LoadScene("Chapter1");
    }
}
