using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager Instance;
    public int currentChapter = 1; // 1-5 mini boss, 6 final
    public string[] chapterNames = { "Selim Bey", "Kerem", "Burhan Efendi", "Lanetlenmis Ali", "Emir", "Prof. Ekrem" };

    void Awake() { Instance = this; DontDestroyOnLoad(gameObject); }

    public void OnBossDefeated(string bossName)
    {
        Debug.Log($"Chapter {currentChapter} bitti: {bossName}");
        // Cay+simit ile can yenile
        FindObjectOfType<AliKayraController>()?.HealWithTeaAndSimit(30);
        currentChapter++;
        if (currentChapter <= 6)
            SceneManager.LoadScene($"Chapter{currentChapter}");
        else
            Debug.Log("OYUN BITTI - Prof tutuklandi, Ali intihar etti... devam edecek");
    }
}
