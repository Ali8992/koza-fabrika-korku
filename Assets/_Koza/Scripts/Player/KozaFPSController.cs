using UnityEngine;

// KOZA 3D - Poppy Playtime tarzı mobil FPS kontrol (sol joystick + sağ swipe kamera)
[RequireComponent(typeof(CharacterController))]
public class KozaFPSController : MonoBehaviour
{
    [Header("Hareket")]
    public float walkSpeed = 3.5f;
    public float runSpeed = 5.5f;
    public Joystick moveJoystick; // Unity Joystick Pack
    public float lookSensitivity = 1.5f;

    [Header("Fener (Poppy atmosferi)")]
    public Light flashlight;
    public Transform cameraRig;

    CharacterController cc;
    float yaw, pitch;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        yaw = transform.eulerAngles.y;
    }

    void Update()
    {
        float h = moveJoystick != null ? moveJoystick.Horizontal : Input.GetAxis("Horizontal");
        float v = moveJoystick != null ? moveJoystick.Vertical : Input.GetAxis("Vertical");

        Vector3 move = (transform.forward * v + transform.right * h);
        float speed = walkSpeed;
        cc.SimpleMove(move.normalized * speed);

        // Sağ swipe ile kamera (mobil): Input.touches ile ikinci parmak
        // Editörde mouse ile test
        if (Input.GetMouseButton(1) || Input.touchCount > 1)
        {
            yaw += Input.GetAxis("Mouse X") * lookSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * lookSensitivity;
            pitch = Mathf.Clamp(pitch, -60f, 60f);
            transform.rotation = Quaternion.Euler(0, yaw, 0);
            if (cameraRig) cameraRig.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
    }

    public void ToggleFlashlight()
    {
        if (flashlight) flashlight.enabled = !flashlight.enabled;
    }
}
