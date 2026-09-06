using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Sağ üst duraklatma butonu: Ayarlar (hassasiyet + fener) / Yeniden Başlat / Oyundan Çık
// Kendi UI'ını kurar (boş GameObject'e ekle yeter)
public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;

    GameObject panel;
    GameObject settingsPanel;
    Slider sensSlider;
    KozaFPSController player;

    void Awake()
    {
        IsPaused = false;
        player = FindObjectOfType<KozaFPSController>();

        var canvasObj = new GameObject("PauseCanvas");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        var scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Sağ üst duraklatma butonu
        var pauseBtn = NewButton("PauseBtn", canvasObj.transform, "II", 40, Color.white, new Color(0, 0, 0, 0.5f));
        var prt = pauseBtn.GetComponent<RectTransform>();
        prt.anchorMin = prt.anchorMax = new Vector2(1, 1);
        prt.anchoredPosition = new Vector2(-90, -90);
        prt.sizeDelta = new Vector2(110, 110);
        var pimg0 = pauseBtn.GetComponent<Image>();
        pimg0.sprite = UiSprites.Circle();
        pauseBtn.GetComponent<Button>().onClick.AddListener(Toggle);

        // Ana panel (ortada)
        panel = new GameObject("Panel");
        panel.transform.SetParent(canvasObj.transform, false);
        var pr = panel.AddComponent<RectTransform>();
        pr.anchorMin = pr.anchorMax = new Vector2(0.5f, 0.5f);
        pr.sizeDelta = new Vector2(700, 650);
        var pimg = panel.AddComponent<Image>();
        pimg.color = new Color(0.05f, 0.05f, 0.05f, 0.95f);

        var resume = NewButton("Resume", panel.transform, "DEVAM ET", 34, Color.white, new Color(0.15f, 0.5f, 0.15f));
        Place(resume, new Vector2(0, 180));
        resume.GetComponent<Button>().onClick.AddListener(Toggle);

        var settings = NewButton("Settings", panel.transform, "AYARLAR", 34, Color.white, new Color(0.2f, 0.2f, 0.5f));
        Place(settings, new Vector2(0, 40));
        settings.GetComponent<Button>().onClick.AddListener(() => settingsPanel.SetActive(!settingsPanel.activeSelf));

        var restart = NewButton("Restart", panel.transform, "YENİDEN BAŞLAT", 34, Color.white, new Color(0.6f, 0.4f, 0.1f));
        Place(restart, new Vector2(0, -100));
        restart.GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            IsPaused = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        var quit = NewButton("Quit", panel.transform, "OYUNDAN ÇIK", 34, Color.white, new Color(0.6f, 0.1f, 0.1f));
        Place(quit, new Vector2(0, -240));
        quit.GetComponent<Button>().onClick.AddListener(() => Application.Quit());

        // Ayarlar alt paneli: hassasiyet + fener
        settingsPanel = new GameObject("SettingsPanel");
        settingsPanel.transform.SetParent(panel.transform, false);
        var sr = settingsPanel.AddComponent<RectTransform>();
        sr.anchoredPosition = new Vector2(480, 40);
        sr.sizeDelta = new Vector2(420, 300);
        var simg = settingsPanel.AddComponent<Image>();
        simg.color = new Color(0.12f, 0.12f, 0.18f, 1f);

        var sensLabel = NewText(settingsPanel.transform, "Kamera Hassasiyeti", 26, Color.white);
        sensLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 90);

        var sliderObj = new GameObject("SensSlider");
        sliderObj.transform.SetParent(settingsPanel.transform, false);
        var slr = sliderObj.AddComponent<RectTransform>();
        slr.anchoredPosition = new Vector2(0, 20);
        slr.sizeDelta = new Vector2(340, 50);
        sensSlider = sliderObj.AddComponent<Slider>();
        sensSlider.minValue = 0.5f;
        sensSlider.maxValue = 5f;
        sensSlider.value = player != null ? player.lookSensitivity : 2f;
        sensSlider.onValueChanged.AddListener(v => { if (player) player.lookSensitivity = v; });

        var torch = NewButton("Torch", settingsPanel.transform, "FENER AÇ/KAPA", 24, Color.white, new Color(0.3f, 0.3f, 0.1f));
        torch.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -80);
        torch.GetComponent<RectTransform>().sizeDelta = new Vector2(340, 80);
        torch.GetComponent<Button>().onClick.AddListener(() => { if (player) player.ToggleFlashlight(); });

        settingsPanel.SetActive(false);
        panel.SetActive(false);
    }

    public void Toggle()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
        if (panel) panel.SetActive(IsPaused);
        if (!IsPaused && settingsPanel) settingsPanel.SetActive(false);
    }

    static void Place(GameObject btn, Vector2 pos)
    {
        var rt = btn.GetComponent<RectTransform>();
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(560, 100);
    }

    static GameObject NewButton(string name, Transform parent, string text, int fontSize, Color tc, Color bg)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>();
        var img = go.AddComponent<Image>();
        img.color = bg;
        var btn = go.AddComponent<Button>();
        btn.targetGraphic = img;
        var t = new GameObject("Label");
        t.transform.SetParent(go.transform, false);
        var trt = t.AddComponent<RectTransform>();
        trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
        trt.offsetMin = Vector2.zero; trt.offsetMax = Vector2.zero;
        var txt = t.AddComponent<Text>();
        txt.text = text;
        txt.fontSize = fontSize;
        txt.color = tc;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        return go;
    }

    static GameObject NewText(Transform parent, string content, int size, Color color)
    {
        var go = new GameObject("Label");
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>().sizeDelta = new Vector2(400, 60);
        var t = go.AddComponent<Text>();
        t.text = content;
        t.fontSize = size;
        t.color = color;
        t.alignment = TextAnchor.MiddleCenter;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        return go;
    }
}
