using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Chapter0: Ali Kayra'nın fabrikaya girişi - hikaye ekranı + FABRİKAYA GİR butonu
// UGUI Canvas bir kez kurulur, sahne başına maliyet sıfırdır (kasmasın diye OnGUI yok).
public class StoryIntro : MonoBehaviour
{
    [Header("Otomatik geçiş")]
    public float autoEnterAfter = 6f; // butona gerek yok, kendi girer
    float timer = 0f;
    bool entering = false;

    void Update()
    {
        if (entering) return;
        timer += Time.deltaTime;
        // Süre dolduysa VEYA ekrana dokunulduysa direkt gir
        if (timer >= autoEnterAfter || Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            entering = true;
            SceneManager.LoadScene("Chapter1");
        }
    }

    void Awake()
    {
        var canvasObj = new GameObject("StoryCanvas");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObj.AddComponent<GraphicRaycaster>();

        var title = NewText("Title", canvasObj.transform, "KOZA FABRİKASI", 64, Color.white);
        title.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 220);

        var story = NewText("Story", canvasObj.transform,
            "Gece yarısı. Yağmur yağıyor.\n\nAli Kayra, Koza Fabrikası'nın paslı kapısında duruyor. " +
            "İçeriden makine sesleri ve fısıltılar geliyor. Görevi belli: 5 mini-boss'u geçip " +
            "en derinde bekleyen Prof. Ekrem ile yüzleşmek.\n\nEl fenerini yakıyor... içeri giriyor.",
            30, new Color(0.85f, 0.85f, 0.85f));
        var storyRt = story.GetComponent<RectTransform>();
        storyRt.sizeDelta = new Vector2(1500, 500);
        storyRt.anchoredPosition = new Vector2(0, -20);

        var btnObj = new GameObject("EnterButton");
        btnObj.transform.SetParent(canvasObj.transform);
        var btnRt = btnObj.AddComponent<RectTransform>();
        btnRt.anchoredPosition = new Vector2(0, -330);
        btnRt.sizeDelta = new Vector2(560, 110);
        var btnImg = btnObj.AddComponent<Image>();
        btnImg.color = new Color(0.6f, 0.1f, 0.1f);
        var btn = btnObj.AddComponent<Button>();
        btn.targetGraphic = btnImg;
        var label = NewText("Label", btnObj.transform, "FABRİKAYA GİR", 36, Color.white);
        label.GetComponent<RectTransform>().sizeDelta = new Vector2(560, 110);
        btn.onClick.AddListener(() => SceneManager.LoadScene("Chapter1"));
    }

    static Text NewText(string name, Transform parent, string content, int size, Color color)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>().sizeDelta = new Vector2(1500, 200);
        var t = go.AddComponent<Text>();
        t.text = content;
        t.fontSize = size;
        t.color = color;
        t.alignment = TextAnchor.MiddleCenter;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.raycastTarget = false;
        return t;
    }
}
