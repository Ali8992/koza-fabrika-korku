using UnityEngine;

// Ali Kayra - Ana Karakter Kontrolcusu (Mobil Joystick Uyumlu)
public class AliKayraController : MonoBehaviour
{
    [Header("Hareket")]
    public float moveSpeed = 4f;
    public float dodgeForce = 6f;
    public Joystick joystick; // Unity Joystick Pack
    public CharacterController controller;

    [Header("Can Sistemi")]
    public int maxHealth = 100;
    public int currentHealth;
    public bool hasESP32 = false; // Lanetlenmis Ali'den alinir, Emir'i gecmek icin

    private Vector3 moveDir;
    private bool isDodging;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Mobil joystick input
        float h = joystick != null ? joystick.Horizontal : Input.GetAxis("Horizontal");
        float v = joystick != null ? joystick.Vertical : Input.GetAxis("Vertical");
        moveDir = new Vector3(h, 0, v).normalized;

        if (!isDodging)
            controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // Fener ac/kapa vs burada
    }

    public void Dodge()
    {
        if (isDodging) return;
        isDodging = true;
        controller.Move(transform.forward * dodgeForce * Time.deltaTime);
        Invoke(nameof(ResetDodge), 0.5f);
    }

    void ResetDodge() => isDodging = false;

    public void HealWithTeaAndSimit(int amount = 30)
    {
        // Her kat arasi cagirilir
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        Debug.Log($"Simit+Cay ile can yenilendi: {currentHealth}");
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0) GameManager.Instance.GameOver();
    }
}
