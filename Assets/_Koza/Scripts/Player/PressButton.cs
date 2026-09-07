using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Bas-çalış buton: parmak kalkmadan BASILDIĞI AN ateşlenir.
// (Normal Button parmak kayarsa iptal olur, mobilde butonlar ölü sanılır.)
[RequireComponent(typeof(RectTransform))]
public class PressButton : MonoBehaviour, IPointerDownHandler
{
    public UnityAction onPress;

    public void OnPointerDown(PointerEventData eventData)
    {
        onPress?.Invoke();
    }
}
