using UnityEngine;

// Her chapter'daki bulmacanın tabanı: 3 parça topla/kapat, boss kapısı açılsın
public abstract class PuzzleBase : MonoBehaviour
{
    [Header("Puzzle")]
    public string puzzleName = "Puzzle";
    public int requiredParts = 3;
    public int collectedParts = 0;

    public bool IsSolved() => collectedParts >= requiredParts;

    public virtual void CollectPart()
    {
        if (IsSolved()) return;
        collectedParts++;
        Debug.Log($"KOZA Puzzle [{puzzleName}]: {collectedParts}/{requiredParts}");
        if (IsSolved()) OnSolved();
    }

    protected virtual void OnSolved()
    {
        Debug.Log($"KOZA Puzzle [{puzzleName}] çözüldü! Boss kapısı açıldı.");
    }
}
