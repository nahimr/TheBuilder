using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [HideInInspector] public bool pressed;
   
    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;

    }
    
}
