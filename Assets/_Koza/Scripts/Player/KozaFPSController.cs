using UnityEngine;

// KOZA 3D - Mobil FPS kontrol: sol dokunmatik joystick + sağ yarıda sürükleyerek kamera
[RequireComponent(typeof(CharacterController))]
public class KozaFPSController : MonoBehaviour
{
    [Header("Hareket")]
    public float walkSpeed = 3.5f;
    public float runSpeed = 5.5f;
    public Joystick moveJoystick; // eski stub (klavye)
    public TouchJoystick touchJoystick; // ekrandaki joystick (öncelikli)
    public float lookSensitivity = 2f;

    [Header("Fener (Poppy atmosferi)")]
    public Light flashlight;
    public Transform cameraRig;

    CharacterController cc;
    float yaw, pitch;
    int lookFingerId = -1;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        yaw = transform.eulerAngles.y;
    }

    void Update()
    {
        if (PauseMenu.IsPaused) return;

        // Hareket: önce dokunmatik joystick, yoksa klavye
        float h, v;
        if (touchJoystick != null && (Mathf.Abs(touchJoystick.Horizontal) > 0.05f || Mathf.Abs(touchJoystick.Vertical) > 0.05f))
        {
            h = touchJoystick.Horizontal;
            v = touchJoystick.Vertical;
        }
        else if (moveJoystick != null)
        {
            h = moveJoystick.Horizontal;
            v = moveJoystick.Vertical;
        }
        else
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }

        Vector3 move = (transform.forward * v + transform.right * h);
        cc.SimpleMove(move.normalized * walkSpeed);

        // Kamera: ekranın sağ yarısında sürükle (dokunmatik) veya sağ tık (editör)
        foreach (var t in Input.touches)
        {
            if (t.phase == TouchPhase.Began && lookFingerId == -1 && t.position.x > Screen.width * 0.4f)
                lookFingerId = t.fingerId;
            else if (t.fingerId == lookFingerId && t.phase == TouchPhase.Moved)
                ApplyLook(t.deltaPosition.x * 0.12f, t.deltaPosition.y * 0.12f);
            else if (t.fingerId == lookFingerId && (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled))
                lookFingerId = -1;
        }
        if (Input.GetMouseButton(1))
            ApplyLook(Input.GetAxis("Mouse X") * 3f, Input.GetAxis("Mouse Y") * 3f);
    }

    void ApplyLook(float dx, float dy)
    {
        if (PauseMenu.IsPaused) return;
        yaw += dx * lookSensitivity;
        pitch -= dy * lookSensitivity;
        pitch = Mathf.Clamp(pitch, -60f, 60f);
        transform.rotation = Quaternion.Euler(0, yaw, 0);
        if (cameraRig) cameraRig.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    public void ToggleFlashlight()
    {
        if (flashlight) flashlight.enabled = !flashlight.enabled;
    }

    // Sağdaki EL butonu: yakındaki sigorta/vana/şalteri kullanır
    public void Interact()
    {
        if (PauseMenu.IsPaused) return;
        foreach (var c in Physics.OverlapSphere(transform.position, 4f))
        {
            var fuse = c.GetComponent<FusePickup>();
            if (fuse != null) { fuse.CollectNow(); return; }
            var valve = c.GetComponent<ValvePickup>();
            if (valve != null) { valve.CloseNow(); return; }
            if (c.gameObject.name == "Shalter")
            {
                var burhan = FindObjectOfType<RoketMotoruBurhan>();
                if (burhan != null) { burhan.PullSwitch(); return; }
            }
        }
        Debug.Log("KOZA: Yakında kullanılabilir bir şey yok.");
    }
}
