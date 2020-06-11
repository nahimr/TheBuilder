using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [HideInInspector] public bool pressed;
    [HideInInspector] public bool take;
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!take)
        {
            pressed = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (take)
        {
          GameEvents.Current.Take();
        }
        else
        {
            pressed = true;
        }
        
    }
    
}
