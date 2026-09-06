using UnityEngine;

// Sigorta parçası: Ali Kayra dokununca puzzle'a eklenir
public class FusePickup : MonoBehaviour
{
    public FusePuzzle puzzle;

    bool done = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<KozaFPSController>() == null && other.GetComponent<AliKayraController>() == null)
            return;
        CollectNow();
    }

    public void CollectNow()
    {
        if (done) return;
        done = true;
        puzzle?.CollectPart();
        Destroy(gameObject);
    }
}
