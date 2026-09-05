using UnityEngine;

// 1. Boss: Selim Bey - Saldirini kaydeder, 2. saldirida geri kullanir
// Zayif Yon: Hareket etmemek
public class SelimBey : BossBase
{
    private string lastPlayerAttack = "";
    private float stillTime = 0f;

    void OnEnable() => bossName = "Selim Bey";

    public void RecordPlayerAttack(string attackType)
    {
        lastPlayerAttack = attackType;
        Debug.Log($"Selim Bey kaydetti: {attackType}");
    }

    public void UseRecordedAttack()
    {
        if (string.IsNullOrEmpty(lastPlayerAttack)) return;
        // Ayni saldiriyi oyuncuya uygula
        Debug.Log($"Selim Bey karsi saldiri: {lastPlayerAttack}");
        // TODO: animasyon tetikle
    }

    void Update()
    {
        // Oyuncu hareketsiz mi kontrolu
        if (AliKayraStill())
        {
            stillTime += Time.deltaTime;
            if (stillTime > 3f)
            {
                // Zayif yon tetiklendi - sersemle
                Stun(2f);
                lastPlayerAttack = ""; // hafiza silindi
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

    void Stun(float duration) => Debug.Log($"Selim Bey {duration}sn sersemledi!");
}
