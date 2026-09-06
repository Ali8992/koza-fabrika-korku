using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        // Joystick stub objesi
        var joy = new GameObject("MoveJoystick");
        joy.transform.SetParent(player.transform);
        fps.moveJoystick = joy.AddComponent<Joystick>();

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

    static void MakeWall(string name, Vector3 pos, Vector3 scale)
    {
        var w = GameObject.CreatePrimitive(PrimitiveType.Cube);
        w.name = name;
        w.transform.position = pos;
        w.transform.localScale = scale;
        w.GetComponent<Renderer>().material.color = new Color(0.16f, 0.14f, 0.14f);
    }
}
