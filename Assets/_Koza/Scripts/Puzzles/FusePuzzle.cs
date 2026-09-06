using UnityEngine;

// Chapter1 puzzle: 3 sigortayı topla, Selim Bey'in kapısı açılsın
public class FusePuzzle : PuzzleBase
{
    public GameObject bossDoor; // kapı: çözülünce kapanır/kaybolur

    void Awake() => puzzleName = "Sigortalar";

    protected override void OnSolved()
    {
        base.OnSolved();
        if (bossDoor) bossDoor.SetActive(false);
    }
}
