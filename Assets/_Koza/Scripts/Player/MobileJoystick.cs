using UnityEngine;

// Gecici mobil joystick: Joystick Pack import edilene kadar klavye/ekran inputu ile calisir.
// Gercek Joystick Pack kurulunca bu dosya silinip paketteki Joystick kullanilabilir.
public class Joystick : MonoBehaviour
{
    public float Horizontal => Input.GetAxis("Horizontal");
    public float Vertical => Input.GetAxis("Vertical");
}
