using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Ekrandaki dokunmatik joystick: sol altta daire, parmağı takip eder
// Kendi görselini kurar (Canvas altında bir GameObject'e ekle yeter)
public class TouchJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Ayar")]
    public float radius = 130f;

    RectTransform baseRt;
    RectTransform knobRt;
    Vector2 input = Vector2.zero;
    int activeTouchId = -1; // -1: mouse, >=0: fingerId

    public float Horizontal => input.x;
    public float Vertical => input.y;

    void Awake()
    {
        // Zemin daire
        baseRt = gameObject.GetComponent<RectTransform>();
        if (baseRt == null) baseRt = gameObject.AddComponent<RectTransform>();
        baseRt.sizeDelta = new Vector2(radius * 2, radius * 2);
        var bg = gameObject.AddComponent<Image>();
        bg.color = new Color(1f, 1f, 1f, 0.15f);
        bg.raycastTarget = true;

        // Topuz
        var knob = new GameObject("Knob");
        knob.transform.SetParent(transform, false);
        knobRt = knob.AddComponent<RectTransform>();
        knobRt.sizeDelta = new Vector2(radius, radius);
        knobRt.anchoredPosition = Vector2.zero;
        var knobImg = knob.AddComponent<Image>();
        knobImg.color = new Color(1f, 0.3f, 0.3f, 0.5f);
        knobImg.raycastTarget = false;
    }

    public void OnPointerDown(PointerEventData e)
    {
        activeTouchId = e.pointerId;
        OnDrag(e);
    }

    public void OnDrag(PointerEventData e)
    {
        if (e.pointerId != activeTouchId) return;
        Vector2 local;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRt, e.position, e.pressEventCamera, out local))
            return;
        local = Vector2.ClampMagnitude(local, radius);
        knobRt.anchoredPosition = local;
        input = local / radius;
    }

    public void OnPointerUp(PointerEventData e)
    {
        if (e.pointerId != activeTouchId) return;
        activeTouchId = -1;
        input = Vector2.zero;
        knobRt.anchoredPosition = Vector2.zero;
    }
}
