using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// KOZA - Chapter1 sahnesini kodla üretir: karanlık fabrika koridoru + fener + Selim Bey
public static class KozaSceneBuilder
{
    public static void BuildChapter1()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "Chapter1";

        // Atmosfer: loş ama görülebilir fabrika (zifiri karanlık yok) + sis
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.34f, 0.34f, 0.42f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogDensity = 0.035f;
        RenderSettings.skybox = null;

        // Zemin (fabrika betonu) - uzatıldı
        var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";
        floor.transform.localScale = new Vector3(3, 1, 18);
        floor.GetComponent<Renderer>().material.color = new Color(0.16f, 0.16f, 0.17f);

        // Duvarlar (uzun koridor) - yan odalara kapı boşluklu
        MakeWall("WallL1", new Vector3(-8, 2.5f, -31.5f), new Vector3(1, 5, 97));
        MakeWall("WallL2", new Vector3(-8, 2.5f, 61.5f), new Vector3(1, 5, 77));
        MakeWall("WallR1", new Vector3(8, 2.5f, -14), new Vector3(1, 5, 132));
        MakeWall("WallR2", new Vector3(8, 2.5f, 79), new Vector3(1, 5, 42));
        MakeWall("WallEnd", new Vector3(0, 2.5f, 88), new Vector3(17, 5, 1));
        // Kolonlar (harita devamı hissi)
        for (int i = 0; i < 6; i++)
        {
            MakeWall($"PillarL_{i}", new Vector3(-6, 2.5f, -20 + i * 20), new Vector3(1.2f, 5, 1.2f));
            MakeWall($"PillarR_{i}", new Vector3(6, 2.5f, -20 + i * 20), new Vector3(1.2f, 5, 1.2f));
        }
        // Yan odalar (keşif alanı)
        MakeRoom("RoomL", -14, 20);
        MakeRoom("RoomR", 14, 55);

        // Tavan lambaları: aralıklı kırmızımsı point light (atmosfer)
        for (int i = 0; i < 8; i++)
        {
            var lamp = new GameObject($"Lamp_{i}");
            lamp.transform.position = new Vector3(0, 4.4f, -36 + i * 16);
            var pl = lamp.AddComponent<Light>();
            pl.type = LightType.Point;
            pl.color = new Color(1f, 0.25f, 0.2f, 1f);
            pl.intensity = 1.2f;
            pl.range = 14f;
        }
        // Loş floresanlar (kırık ama çalışıyor): koridoru aydınlatır, üçte biri titrer
        for (int i = 0; i < 9; i++)
            AddFluorescent($"Fluorescent_{i}", new Vector3(i % 2 == 0 ? -3f : 3f, 4.6f, -40 + i * 14), i % 3 == 1);

        // Oyuncu: Ali Kayra (kapsül + fener)
        var player = new GameObject("AliKayra");
        player.transform.position = new Vector3(0, 1, -38);
        player.AddComponent<CharacterController>();
        var fps = player.AddComponent<KozaFPSController>();
        var camObj = new GameObject("CameraRig");
        camObj.transform.SetParent(player.transform);
        camObj.transform.localPosition = new Vector3(0, 1.6f, 0);
        var cam = camObj.AddComponent<Camera>();
        cam.backgroundColor = Color.black;
        fps.cameraRig = camObj.transform;
        var torch = new GameObject("Flashlight");
        torch.transform.SetParent(camObj.transform);
        torch.transform.localPosition = new Vector3(0.3f, -0.2f, 0.3f);
        var spot = torch.AddComponent<Light>();
        spot.type = LightType.Spot;
        spot.color = Color.white;
        spot.intensity = 4.5f;
        spot.range = 38f;
        spot.spotAngle = 80f;
        fps.flashlight = spot;
        AddBlockyHands(camObj.transform);
        // Joystick stub objesi (klavye yedeği)
        var joy = new GameObject("MoveJoystick");
        joy.transform.SetParent(player.transform);
        fps.moveJoystick = joy.AddComponent<Joystick>();
        // Mobil UI: dokunmatik joystick + duraklatma menüsü
        AddMobileUI(fps);

        // Boss kapısı (FusePuzzle çözülmeden Selim'e ulaşılmaz)
        var door = GameObject.CreatePrimitive(PrimitiveType.Cube);
        door.name = "BossDoor";
        door.transform.position = new Vector3(0, 2.5f, 60);
        door.transform.localScale = new Vector3(16, 5, 1);
        door.GetComponent<Renderer>().material.color = new Color(0.35f, 0.08f, 0.08f);
        var fuse = new GameObject("FusePuzzle").AddComponent<FusePuzzle>();
        fuse.bossDoor = door;
        // 3 sigorta (sarı küpler) - uzatılmış hatta dağılmış
        for (int i = 0; i < 3; i++)
        {
            var f = GameObject.CreatePrimitive(PrimitiveType.Cube);
            f.name = $"Fuse_{i}";
            f.transform.position = new Vector3(i % 2 == 0 ? -5f : 5f, 1f, -30 + i * 16);
            f.transform.localScale = Vector3.one * 0.5f;
            f.GetComponent<Renderer>().material.color = Color.yellow;
            f.GetComponent<BoxCollider>().isTrigger = true;
            f.AddComponent<FusePickup>().puzzle = fuse;
        }

        // Boss: Selim Bey (koridor sonu)
        var selim = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        selim.name = "SelimBey";
        selim.transform.position = new Vector3(0, 1, 80);
        selim.GetComponent<Renderer>().material.color = Color.gray;
        selim.AddComponent<SelimBey>();

        // Çay+Simit noktası (girişte can yenileme)
        var heal = new GameObject("TeaSimitPoint");
        heal.transform.position = new Vector3(3, 1, -35);
        var halo = heal.AddComponent<Light>();
        halo.type = LightType.Point;
        halo.color = Color.yellow;
        halo.intensity = 1f;
        halo.range = 5f;

        EditorSceneManager.SaveScene(scene, "Assets/_Koza/Scenes/Chapter1.unity");
        Debug.Log("KOZA: Chapter1 sahnesi olusturuldu.");
    }

    public static void BuildAllScenes()
    {
        BuildChapter0();
        BuildChapter1();
        BuildChapter3();
    }

    // Chapter0: Giriş hikayesi - Ali Kayra fabrikaya giriyor
    public static void BuildChapter0()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "Chapter0";
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogDensity = 0.04f;

        var camObj = new GameObject("IntroCamera");
        var cam = camObj.AddComponent<Camera>();
        cam.backgroundColor = Color.black;
        camObj.transform.position = new Vector3(0, 1.6f, -6);

        // Fabrika kapısı silüeti (karanlıkta iki dev kapı)
        MakeWall("GateL", new Vector3(-3, 2.5f, 6), new Vector3(4, 5, 1));
        MakeWall("GateR", new Vector3(3, 2.5f, 6), new Vector3(4, 5, 1));
        var eye = new GameObject("RedEye");
        eye.transform.position = new Vector3(0, 3.4f, 5.4f);
        var pl = eye.AddComponent<Light>();
        pl.type = LightType.Point;
        pl.color = new Color(1f, 0.15f, 0.15f);
        pl.intensity = 2f;
        pl.range = 12f;

        // UI tıklamaları için EventSystem şart (yoksa buton ölü kalır)
        var es = new GameObject("EventSystem");
        es.AddComponent<EventSystem>();
        es.AddComponent<StandaloneInputModule>();

        // Hikaye UI: StoryIntro (UGUI Canvas)
        var storyObj = new GameObject("StoryIntro");
        storyObj.AddComponent<StoryIntro>();

        EditorSceneManager.SaveScene(scene, "Assets/_Koza/Scenes/Chapter0.unity");
        Debug.Log("KOZA: Chapter0 (giriş hikayesi) olusturuldu.");
    }

    // Chapter3: Roket Motoru Burhan - robotlar + büyüklü küçüklü roketler + vana puzzle + şalter
    public static void BuildChapter3()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "Chapter3";
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.34f, 0.34f, 0.42f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogDensity = 0.03f;

        var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";
        floor.transform.localScale = new Vector3(4, 1, 6);
        floor.GetComponent<Renderer>().material.color = new Color(0.1f, 0.1f, 0.12f);
        MakeWall("WallBack", new Vector3(0, 2.5f, 28), new Vector3(40, 5, 1));
        MakeWall("Pillar3_1", new Vector3(-14, 2.5f, 0), new Vector3(1.5f, 5, 1.5f));
        MakeWall("Pillar3_2", new Vector3(14, 2.5f, 0), new Vector3(1.5f, 5, 1.5f));
        MakeWall("Pillar3_3", new Vector3(-14, 2.5f, 20), new Vector3(1.5f, 5, 1.5f));
        MakeWall("Pillar3_4", new Vector3(14, 2.5f, 20), new Vector3(1.5f, 5, 1.5f));

        // Oyuncu
        var player = new GameObject("AliKayra");
        player.transform.position = new Vector3(0, 1, -20);
        player.AddComponent<CharacterController>();
        var fps = player.AddComponent<KozaFPSController>();
        var camObj = new GameObject("CameraRig");
        camObj.transform.SetParent(player.transform);
        camObj.transform.localPosition = new Vector3(0, 1.6f, 0);
        var cam = camObj.AddComponent<Camera>();
        cam.backgroundColor = Color.black;
        fps.cameraRig = camObj.transform;
        // Geniş fener (Ch3'te yoktu, eklendi)
        var torch3 = new GameObject("Flashlight");
        torch3.transform.SetParent(camObj.transform);
        torch3.transform.localPosition = new Vector3(0.3f, -0.2f, 0.3f);
        var spot3 = torch3.AddComponent<Light>();
        spot3.type = LightType.Spot;
        spot3.color = Color.white;
        spot3.intensity = 4.5f;
        spot3.range = 38f;
        spot3.spotAngle = 80f;
        fps.flashlight = spot3;
        AddBlockyHands(camObj.transform);
        AddMobileUI(fps);
        // Loş floresanlar: arena çevresi
        for (int i = 0; i < 4; i++)
            AddFluorescent($"Fluorescent3_{i}", new Vector3(-12 + i * 8, 5f, 14), i % 2 == 0);

        // Burhan: sandalye + şalter arkası
        var chair = GameObject.CreatePrimitive(PrimitiveType.Cube);
        chair.name = "Chair";
        chair.transform.position = new Vector3(0, 0.5f, 22);
        chair.transform.localScale = new Vector3(2, 1, 2);
        var burhan = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        burhan.name = "RoketMotoruBurhan";
        burhan.transform.position = new Vector3(0, 1.5f, 22);
        burhan.GetComponent<Renderer>().material.color = Color.blue;
        var boss = burhan.AddComponent<RoketMotoruBurhan>();
        var lever = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lever.name = "Shalter";
        lever.transform.position = new Vector3(0, 1.5f, 23.5f);
        lever.transform.localScale = new Vector3(0.4f, 1.2f, 0.4f);
        lever.GetComponent<Renderer>().material.color = Color.red;

        // Robotlar (4 adet gri kub)
        for (int i = 0; i < 4; i++)
        {
            var r = GameObject.CreatePrimitive(PrimitiveType.Cube);
            r.name = $"Robot_{i}";
            r.transform.position = new Vector3(-9 + i * 6, 1f, 12);
            r.transform.localScale = new Vector3(1.5f, 2f, 1.5f);
            r.GetComponent<Renderer>().material.color = Color.gray;
            var eyeL = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            eyeL.transform.SetParent(r.transform);
            eyeL.transform.localPosition = new Vector3(0, 0.4f, 0.8f);
            eyeL.transform.localScale = Vector3.one * 0.25f;
            eyeL.GetComponent<Renderer>().material.color = Color.red;
            boss.robots.Add(r);
        }

        // Roketler: 2 büyük + 4 küçük (silindir + koni burun)
        for (int i = 0; i < 2; i++)
            boss.rocketsBig.Add(MakeRocket($"RocketBig_{i}", new Vector3(-12 + i * 24, 0, 18), 3.2f));
        for (int i = 0; i < 4; i++)
            boss.rocketsSmall.Add(MakeRocket($"RocketSmall_{i}", new Vector3(-12 + i * 8, 0, 6), 1.6f));

        // Vana puzzle: 3 vana (yeşil silindir)
        var valves = new GameObject("ValvePuzzle").AddComponent<ValvePuzzle>();
        boss.valvePuzzle = valves;
        for (int i = 0; i < 3; i++)
        {
            var v = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            v.name = $"Valve_{i}";
            v.transform.position = new Vector3(-8 + i * 8, 1f, -8);
            v.GetComponent<Renderer>().material.color = Color.green;
            v.GetComponent<Collider>().isTrigger = true;
            var pickup = v.AddComponent<ValvePickup>();
            pickup.puzzle = valves;
        }

        EditorSceneManager.SaveScene(scene, "Assets/_Koza/Scenes/Chapter3.unity");
        Debug.Log("KOZA: Chapter3 (Roket Motoru Burhan) olusturuldu.");
    }

    static GameObject MakeRocket(string name, Vector3 pos, float height)
    {
        var rocket = new GameObject(name);
        rocket.transform.position = pos;
        var body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        body.transform.SetParent(rocket.transform);
        body.transform.localPosition = new Vector3(0, height / 2, 0);
        body.transform.localScale = new Vector3(1, height / 2, 1);
        body.GetComponent<Renderer>().material.color = new Color(0.7f, 0.1f, 0.1f);
        var nose = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        nose.transform.SetParent(rocket.transform);
        nose.transform.localPosition = new Vector3(0, height + 0.3f, 0);
        nose.transform.localScale = new Vector3(1, 0.8f, 1);
        nose.GetComponent<Renderer>().material.color = Color.white;
        return rocket;
    }

    // Mobil UI: EventSystem + sol altta joystick + sağ üst duraklatma
    static void AddMobileUI(KozaFPSController fps)
    {
        var es = new GameObject("EventSystem");
        es.AddComponent<EventSystem>();
        es.AddComponent<StandaloneInputModule>();

        var canvasObj = new GameObject("MobileCanvas");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        // ŞART: dokunmalar UI'ya ulaşsın (yoksa joystick ölü kalır)
        canvasObj.AddComponent<GraphicRaycaster>();

        var joyObj = new GameObject("TouchJoystick");
        joyObj.transform.SetParent(canvasObj.transform, false);
        var rt = joyObj.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = Vector2.zero;
        rt.anchoredPosition = new Vector2(200, 200);
        fps.touchJoystick = joyObj.AddComponent<TouchJoystick>();

        // Sağ tarafta yuvarlak aksiyon butonları: FENER + EL (etkileşim) + ZIPLA
        AddRoundButton(canvasObj.transform, "BtnTorch", "FENER", new Vector2(-200, 500), new Color(1f, 0.85f, 0.2f, 0.55f), () => fps.ToggleFlashlight());
        AddRoundButton(canvasObj.transform, "BtnInteract", "EL", new Vector2(-200, 300), new Color(0.2f, 0.8f, 0.3f, 0.55f), () => fps.Interact());
        AddRoundButton(canvasObj.transform, "BtnJump", "ZIPLA", new Vector2(-420, 300), new Color(0.3f, 0.5f, 1f, 0.55f), () => fps.QueueJump());

        new GameObject("PauseMenu").AddComponent<PauseMenu>();
    }

    // Minecraft tarzı bloklu eller (kameraya sabit)
    static void AddBlockyHands(Transform camRig)
    {
        AddHand(camRig, "HandL", -0.38f);
        AddHand(camRig, "HandR", 0.38f);
    }

    static void AddHand(Transform camRig, string name, float x)
    {
        var hand = GameObject.CreatePrimitive(PrimitiveType.Cube);
        hand.name = name;
        hand.transform.SetParent(camRig, false);
        hand.transform.localPosition = new Vector3(x, -0.42f, 0.7f);
        hand.transform.localScale = new Vector3(0.22f, 0.22f, 0.5f);
        hand.GetComponent<Renderer>().material.color = new Color(0.87f, 0.66f, 0.5f); // ten
        var sleeve = GameObject.CreatePrimitive(PrimitiveType.Cube);
        sleeve.name = name + "_Sleeve";
        sleeve.transform.SetParent(camRig, false);
        sleeve.transform.localPosition = new Vector3(x, -0.42f, 0.35f);
        sleeve.transform.localScale = new Vector3(0.26f, 0.26f, 0.3f);
        sleeve.GetComponent<Renderer>().material.color = new Color(0.15f, 0.25f, 0.55f); // mont kolu
    }

    // Yan oda (keşif): küçük zemin + 3 duvar
    static void MakeRoom(string name, float cx, float cz)
    {
        var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = name + "_Floor";
        floor.transform.position = new Vector3(cx, 0.01f, cz);
        floor.transform.localScale = new Vector3(1.2f, 1, 1.2f);
        floor.GetComponent<Renderer>().material.color = new Color(0.14f, 0.14f, 0.15f);
        MakeWall(name + "_Back", new Vector3(cx, 2.5f, cz + 6), new Vector3(13, 5, 1));
        MakeWall(name + "_L", new Vector3(cx - 6, 2.5f, cz), new Vector3(1, 5, 13));
        MakeWall(name + "_R", new Vector3(cx + 6, 2.5f, cz), new Vector3(1, 5, 13));
        AddFluorescent(name + "_Tube", new Vector3(cx, 4.2f, cz), true);
    }

    // Sağ altta yuvarlak aksiyon butonu
    static void AddRoundButton(Transform parent, string name, string label, Vector2 anchoredPos, Color color, UnityEngine.Events.UnityAction onClick)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(1, 0);
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = new Vector2(150, 150);
        var img = go.AddComponent<Image>();
        img.sprite = UiSprites.Circle();
        img.color = color;
        var btn = go.AddComponent<Button>();
        btn.targetGraphic = img;
        btn.onClick.AddListener(onClick);
        var t = new GameObject("Label");
        t.transform.SetParent(go.transform, false);
        var trt = t.AddComponent<RectTransform>();
        trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
        trt.offsetMin = Vector2.zero; trt.offsetMax = Vector2.zero;
        var txt = t.AddComponent<Text>();
        txt.text = label;
        txt.fontSize = 30;
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.raycastTarget = false; // yazı tıklamayı yutmasın
    }

    // Loş floresan (kırık ama çalışıyor): tüp mesh + soğuk beyaz ışık, yarısı titrer
    static void AddFluorescent(string name, Vector3 pos, bool flickering)
    {
        var tube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tube.name = name;
        tube.transform.position = pos;
        tube.transform.localScale = new Vector3(2.4f, 0.15f, 0.15f);
        tube.GetComponent<Renderer>().material.color = new Color(0.9f, 0.95f, 1f);
        var lamp = new GameObject(name + "_Light");
        lamp.transform.position = pos + new Vector3(0, -0.5f, 0);
        var pl = lamp.AddComponent<Light>();
        pl.type = LightType.Point;
        pl.color = new Color(0.82f, 0.88f, 1f);
        pl.intensity = 1.1f;
        pl.range = 15f;
        if (flickering)
        {
            var fl = lamp.AddComponent<FlickerLight>();
            fl.flickerAmount = 0.4f;
            fl.flickerSpeed = 5f;
        }
    }

    static void MakeWall(string name, Vector3 pos, Vector3 scale)
    {
        var w = GameObject.CreatePrimitive(PrimitiveType.Cube);
        w.name = name;
        w.transform.position = pos;
        w.transform.localScale = scale;
        w.GetComponent<Renderer>().material.color = new Color(0.16f, 0.14f, 0.14f);
    }
}
