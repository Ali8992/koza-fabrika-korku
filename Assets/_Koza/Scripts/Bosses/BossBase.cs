using UnityEngine;

public abstract class BossBase : MonoBehaviour
{
    public string bossName;
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDefeated = false;

    protected virtual void Start() => currentHealth = maxHealth;

    public virtual void TakeDamage(int dmg)
    {
        if (isDefeated) return;
        currentHealth -= dmg;
        if (currentHealth <= 0) OnDefeated();
    }

    protected virtual void OnDefeated()
    {
        isDefeated = true;
        Debug.Log($"{bossName} yenildi!");
        ChapterManager.Instance.OnBossDefeated(bossName);
    }
}
