using UnityEngine;

// Chapter3 puzzle: Roket Motoru Burhan'ın roketlerini durdurmak için 3 vanayı kapat
public class ValvePuzzle : PuzzleBase
{
    void Awake() => puzzleName = "Roket Vanaları";

    protected override void OnSolved()
    {
        base.OnSolved();
        Debug.Log("KOZA: Roketler sustu! Robotlar hâlâ aktif, şalteri bul!");
    }
}
