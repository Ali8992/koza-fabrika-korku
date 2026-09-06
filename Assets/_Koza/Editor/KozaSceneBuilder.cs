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

        // Atmosfer: gece + sis
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogDensity = 0.06f;
        RenderSettings.skybox = null;

        // Zemin (fabrika betonu)
        var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";
        floor.transform.localScale = new Vector3(3, 1, 10);
        floor.GetComponent<Renderer>().material.color = new Color(0.12f, 0.12f, 0.12f);

        // Duvarlar (koridor)
        MakeWall("WallL", new Vector3(-8, 2.5f, 0), new Vector3(1, 5, 100));
        MakeWall("WallR", new Vector3(8, 2.5f, 0), new Vector3(1, 5, 100));
        MakeWall("WallEnd", new Vector3(0, 2.5f, 48), new Vector3(17, 5, 1));

        // Tavan lambaları: titreyen floresan hissi için aralıklı kırmızımsı point light
        for (int i = 0; i < 5; i++)
        {
            var lamp = new GameObject($"Lamp_{i}");
            lamp.transform.position = new Vector3(0, 4.4f, -30 + i * 18);
            var pl = lamp.AddComponent<Light>();
            pl.type = LightType.Point;
            pl.color = new Color(1f, 0.25f, 0.2f, 1f);
            pl.intensity = 1.2f;
            pl.range = 14f;
        }

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
        spot.intensity = 3f;
        spot.range = 25f;
        spot.spotAngle = 55f;
        fps.flashlight = spot;
        // Joystick stub objesi (klavye yedeği)
        var joy = new GameObject("MoveJoystick");
        joy.transform.SetParent(player.transform);
        fps.moveJoystick = joy.AddComponent<Joystick>();
        // Mobil UI: dokunmatik joystick + duraklatma menüsü
        AddMobileUI(fps);

        // Boss kapısı (FusePuzzle çözülmeden Selim'e ulaşılmaz)
        var door = GameObject.CreatePrimitive(PrimitiveType.Cube);
        door.name = "BossDoor";
        door.transform.position = new Vector3(0, 2.5f, 30);
        door.transform.localScale = new Vector3(16, 5, 1);
        door.GetComponent<Renderer>().material.color = new Color(0.35f, 0.08f, 0.08f);
        var fuse = new GameObject("FusePuzzle").AddComponent<FusePuzzle>();
        fuse.bossDoor = door;
        // 3 sigorta (sarı küpler)
        for (int i = 0; i < 3; i++)
        {
            var f = GameObject.CreatePrimitive(PrimitiveType.Cube);
            f.name = $"Fuse_{i}";
            f.transform.position = new Vector3(-5 + i * 5, 1f, -20 + i * 8);
            f.transform.localScale = Vector3.one * 0.5f;
            f.GetComponent<Renderer>().material.color = Color.yellow;
            f.GetComponent<BoxCollider>().isTrigger = true;
            f.AddComponent<FusePickup>().puzzle = fuse;
        }

        // Boss: Selim Bey (koridor sonu)
        var selim = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        selim.name = "SelimBey";
        selim.transform.position = new Vector3(0, 1, 40);
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
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogDensity = 0.05f;

        var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";
        floor.transform.localScale = new Vector3(4, 1, 6);
        floor.GetComponent<Renderer>().material.color = new Color(0.1f, 0.1f, 0.12f);
        MakeWall("WallBack", new Vector3(0, 2.5f, 28), new Vector3(40, 5, 1));

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
        AddMobileUI(fps);

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

        var joyObj = new GameObject("TouchJoystick");
        joyObj.transform.SetParent(canvasObj.transform, false);
        var rt = joyObj.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = Vector2.zero;
        rt.anchoredPosition = new Vector2(200, 200);
        fps.touchJoystick = joyObj.AddComponent<TouchJoystick>();

        new GameObject("PauseMenu").AddComponent<PauseMenu>();
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
