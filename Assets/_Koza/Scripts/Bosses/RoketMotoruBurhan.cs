using UnityEngine;
using System.Collections.Generic;

// 3. Boss: Roket Motoru Burhan (eski ad: Burhan Efendi)
// Robot sürüsü + BÜYÜKLÜ KÜÇÜKLÜ ROKETLERİ kontrol eder.
// Zayıf Yön: sandalyenin arkasındaki şalter + vana puzzle (roketleri durdurur)
public class RoketMotoruBurhan : BossBase
{
    [Header("Ordu")]
    public List<GameObject> robots = new List<GameObject>();
    public List<GameObject> rocketsBig = new List<GameObject>();
    public List<GameObject> rocketsSmall = new List<GameObject>();
    public bool switchPulled = false;
    public ValvePuzzle valvePuzzle; // 3 vana kapanınca roketler susar

    void OnEnable() => bossName = "Roket Motoru Burhan";

    void Update()
    {
        bool rocketsOff = valvePuzzle != null && valvePuzzle.IsSolved();
        if (switchPulled)
        {
            // Her şey kapalı: savunmasız
            SetArmyActive(false);
        }
        else if (rocketsOff)
        {
            // Roketler sustu ama robotlar saldırıyor
            SetRocketsActive(false);
            SetRobotsActive(true);
        }
    }

    void SetArmyActive(bool on)
    {
        SetRobotsActive(on);
        SetRocketsActive(on);
    }

    void SetRobotsActive(bool on)
    {
        foreach (var r in robots) if (r) r.SetActive(on);
    }

    void SetRocketsActive(bool on)
    {
        foreach (var r in rocketsBig) if (r) r.SetActive(on);
        foreach (var r in rocketsSmall) if (r) r.SetActive(on);
    }

    // Etkileşim butonuyla şalter çekilir
    public void PullSwitch()
    {
        if (switchPulled) return;
        switchPulled = true;
        SetArmyActive(false);
        Debug.Log("KOZA: Şalter çekildi! Robotlar ve roketler durdu, Burhan savunmasız.");
    }

    public override void TakeDamage(int dmg)
    {
        // Şalter çekilmedikçe robotlar hasarı yutar
        if (!switchPulled) dmg = Mathf.Max(1, dmg / 5);
        base.TakeDamage(dmg);
    }
}
