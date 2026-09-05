using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    void Awake() { Instance = this; }

    public void GameOver() => Debug.Log("Game Over - Yeniden dene");
    public void WinGame() => Debug.Log("Kazandin - Koza kurtuldu");
}
