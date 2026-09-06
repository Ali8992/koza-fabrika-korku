using UnityEngine;

// Sigorta parçası: Ali Kayra dokununca puzzle'a eklenir
public class FusePickup : MonoBehaviour
{
    public FusePuzzle puzzle;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<KozaFPSController>() == null && other.GetComponent<AliKayraController>() == null)
            return;
        puzzle?.CollectPart();
        Destroy(gameObject);
    }
}
