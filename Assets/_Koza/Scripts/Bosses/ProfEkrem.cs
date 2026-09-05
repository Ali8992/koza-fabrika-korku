using UnityEngine;

// Final Boss: Prof. Ekrem - Elektrik kontrolu, hizli, dodge
public class ProfEkrem : BossBase
{
    public float power = 100f; // Elektrik gucu, salter kapaninca azalir
    public bool isPowerCut = false;
    public Transform bossChair; // arkasi donuk baslangic

    void OnEnable() => bossName = "Prof. Ekrem";

    void Update()
    {
        if (isPowerCut)
        {
            power -= 10f * Time.deltaTime;
            if (power < 30f)
            {
                // Yavasla ve sersemle
                Debug.Log("Prof gucunu kaybediyor!");
            }
        }
    }

    public void ElectricAttack()
    {
        if (power <= 0) return;
        Debug.Log("Prof elektrik dalgasi gonderdi!");
        // TODO: Instantiate elektrik efekti
    }

    public void DodgeAttack()
    {
        Debug.Log("Prof hafif dodge atti!");
        // TODO: animasyon
    }

    // Lanetli Ali'nin cagirdigi fonksiyon
    public void CutPower()
    {
        isPowerCut = true;
        Debug.Log("ANA SALTER KAPANDI! Prof zayifladi.");
    }
}
