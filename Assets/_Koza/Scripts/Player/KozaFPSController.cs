using UnityEngine;
using UnityEngine.EventSystems;

// KOZA 3D - Mobil FPS kontrol: sol dokunmatik joystick + sağ yarıda sürükleyerek kamera
// Kamera yumuşatmalı (sarsıntısız), fener 3 kademeli (beyaz / morötesi / kapalı)
[RequireComponent(typeof(CharacterController))]
public class KozaFPSController : MonoBehaviour
{
    public enum TorchMode { Off, White, UV }

    [Header("Hareket")]
    public float walkSpeed = 3.5f;
    public float runSpeed = 5.5f;
    public Joystick moveJoystick; // eski stub (klavye)
    public TouchJoystick touchJoystick; // ekrandaki joystick (öncelikli)
    public float lookSensitivity = 2f;
    public float lookSmooth = 12f; // sarsıntı önleme

    [Header("Fener (3 kademe)")]
    public Light flashlight;
    public Transform cameraRig;
    public TorchMode torchMode = TorchMode.White;

    CharacterController cc;
    float yaw, pitch, targetYaw, targetPitch;
    int lookFingerId = -1;

    [Header("Zıplama")]
    public float jumpForce = 5f;
    public float gravity = 15f;
    float verticalVel = 0f;
    bool jumpQueued = false;

    public void QueueJump()
    {
        if (PauseMenu.IsPaused) return;
        if (cc != null && cc.isGrounded) jumpQueued = true;
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
        yaw = targetYaw = transform.eulerAngles.y;
        pitch = targetPitch = 0f;
        ApplyTorch();
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

        Vector3 move = (transform.forward * v + transform.right * h).normalized * walkSpeed;

        // Zıplama + yerçekimi (SimpleMove yerine Move: zıplama şart)
        if (cc.isGrounded)
            verticalVel = jumpQueued ? jumpForce : -1f;
        else
            verticalVel -= gravity * Time.deltaTime;
        jumpQueued = false;
        move.y = verticalVel;
        cc.Move(move * Time.deltaTime);

        // Kamera hedefi: ekranın sağ yarısında sürükle (dokunmatik) veya sağ tık (editör)
        // UI butonuna basılan parmak kamerayı oynatmaz (sapıtma çözümü)
        foreach (var t in Input.touches)
        {
            if (t.phase == TouchPhase.Began && lookFingerId == -1 && t.position.x > Screen.width * 0.4f
                && (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject(t.fingerId)))
                lookFingerId = t.fingerId;
            else if (t.fingerId == lookFingerId && t.phase == TouchPhase.Moved)
                AddLook(t.deltaPosition.x * 0.09f, t.deltaPosition.y * 0.09f);
            else if (t.fingerId == lookFingerId && (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled))
                lookFingerId = -1;
        }
        if (Input.GetMouseButton(1))
            AddLook(Input.GetAxis("Mouse X") * 2f, Input.GetAxis("Mouse Y") * 2f);

        // Yumuşatma: hedefe üstel yaklaş (sarsıntı yok)
        float k = 1f - Mathf.Exp(-lookSmooth * Time.deltaTime);
        yaw = Mathf.LerpAngle(yaw, targetYaw, k);
        pitch = Mathf.Lerp(pitch, targetPitch, k);
        transform.rotation = Quaternion.Euler(0, yaw, 0);
        if (cameraRig) cameraRig.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    void AddLook(float dx, float dy)
    {
        if (PauseMenu.IsPaused) return;
        targetYaw += dx * lookSensitivity;
        targetPitch = Mathf.Clamp(targetPitch - dy * lookSensitivity, -60f, 60f);
    }

    // FENER butonu: her basışta kademe değişir (beyaz -> morötesi -> kapalı)
    public void ToggleFlashlight()
    {
        torchMode = (TorchMode)(((int)torchMode + 1) % 3);
        ApplyTorch();
        Debug.Log($"KOZA Fener: {torchMode}");
    }

    void ApplyTorch()
    {
        if (!flashlight) return;
        if (torchMode == TorchMode.White)
        {
            flashlight.enabled = true;
            flashlight.color = Color.white;
            flashlight.intensity = 4.5f;
            flashlight.range = 38f;
            flashlight.spotAngle = 80f;
        }
        else if (torchMode == TorchMode.UV)
        {
            flashlight.enabled = true;
            flashlight.color = new Color(0.55f, 0.25f, 1f); // morötesi mor
            flashlight.intensity = 3.5f;
            flashlight.range = 30f;
            flashlight.spotAngle = 70f;
        }
        else flashlight.enabled = false;
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
