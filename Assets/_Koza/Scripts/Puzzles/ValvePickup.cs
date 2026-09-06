using UnityEngine;

// Vana: Ali Kayra dokununca kapanır, roket puzzle'ına eklenir
public class ValvePickup : MonoBehaviour
{
    public ValvePuzzle puzzle;
    private bool closed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<KozaFPSController>() == null && other.GetComponent<AliKayraController>() == null)
            return;
        CloseNow();
    }

    public void CloseNow()
    {
        if (closed) return;
        closed = true;
        GetComponent<Renderer>().material.color = Color.gray;
        puzzle?.CollectPart();
    }
}
