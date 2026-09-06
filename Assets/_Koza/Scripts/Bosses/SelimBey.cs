using UnityEngine;

// 1. Boss: Selim Bey (NERF'li) - Saldirini kaydeder ama 2 vuruşta 1 counter atar
// Zayif Yon: 2sn hareket etmemek -> 3sn sersemler, hafızası silinir
// NERF: counter hasarı 18->10, counter sıklığı yarıya, can 100->80
public class SelimBey : BossBase
{
    [Header("NERF Ayarları")]
    public int counterDamage = 10;
    public float stunToTrigger = 2f;   // eskiden 3sn idi
    public float stunDuration = 3f;    // eskiden 2sn idi

    private string lastPlayerAttack = "";
    private float stillTime = 0f;
    private bool counterReady = false; // 2 vuruşta 1 true olur
    private float stunTimer = 0f;

    void OnEnable()
    {
        bossName = "Selim Bey";
        maxHealth = 80; // NERF: 100 -> 80
    }

    public void RecordPlayerAttack(string attackType)
    {
        lastPlayerAttack = attackType;
        counterReady = !counterReady; // NERF: her 2 kayıtta 1 counter hakkı
        Debug.Log($"Selim Bey kaydetti: {attackType} (counter hazır: {counterReady})");
    }

    // Gerçek counter hasarı: hazır değilse zayıf tokat
    public int GetCounterDamage()
    {
        if (stunTimer > 0) return 0;
        if (counterReady)
        {
            counterReady = false;
            Debug.Log($"Selim Bey counter: {lastPlayerAttack} -{counterDamage}");
            return counterDamage;
        }
        Debug.Log("Selim Bey yorgun, zayıf vurdu -6");
        return 6;
    }

    void Update()
    {
        if (stunTimer > 0) { stunTimer -= Time.deltaTime; return; }

        // Oyuncu hareketsiz mi kontrolu
        if (AliKayraStill())
        {
            stillTime += Time.deltaTime;
            if (stillTime > stunToTrigger)
            {
                Stun(stunDuration);
                lastPlayerAttack = ""; // hafiza silindi
                counterReady = false;
                stillTime = 0f;
            }
        }
        else stillTime = 0f;
    }

    bool AliKayraStill()
    {
        var player = FindObjectOfType<AliKayraController>();
        return player != null && player.GetComponent<CharacterController>().velocity.magnitude < 0.1f;
    }

    void Stun(float duration)
    {
        stunTimer = duration;
        Debug.Log($"Selim Bey {duration}sn sersemledi!");
    }
}
