using UnityEngine;

// Kırık ama hâlâ çalışan floresan: loş titreme + ara sıra göz kırpma
[RequireComponent(typeof(Light))]
public class FlickerLight : MonoBehaviour
{
    [Header("Titreme")]
    public float flickerAmount = 0.35f;
    public float flickerSpeed = 6f;

    Light lamp;
    float baseIntensity;
    float seed;

    void Awake()
    {
        lamp = GetComponent<Light>();
        baseIntensity = lamp.intensity;
        seed = Random.Range(0f, 100f);
    }

    void Update()
    {
        float n = Mathf.PerlinNoise(Time.time * flickerSpeed, seed);
        float drop = n > 0.92f ? 0.4f : 1f; // ara sıra kırp
        lamp.intensity = baseIntensity * (1f - n * flickerAmount) * drop;
    }
}
