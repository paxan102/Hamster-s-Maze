using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventOnChangeDirection : UnityEvent<bool[]> { }

public class InputSyst : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [HideInInspector] public EventOnChangeDirection OnChangeDirection = new EventOnChangeDirection();

    [SerializeField] private Joy joy;

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        var currentPointInScreen = Input.mousePosition;
        var direction = joy.Move(currentPointInScreen);
        if(direction != currentDirection)
        {
            currentDirection = direction;
            OnChangeDirection.Invoke(currentDirection);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var startPointInScreen = Input.mousePosition;
        joy.Enable(startPointInScreen);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrag = false;
        joy.Disable();
        currentDirection = new bool[] { false, false, false, false };
        OnChangeDirection.Invoke(currentDirection);
    }
        
    private bool isDrag = false;
    private bool[] currentDirection;
}
