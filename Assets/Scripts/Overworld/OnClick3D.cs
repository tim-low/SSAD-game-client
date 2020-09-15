using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnClick3D : MonoBehaviour {

    [SerializeField]
    private UnityEvent onMouseDownEvent;

    private void OnMouseDown()
    {
        if (onMouseDownEvent != null)
        {
            if (!IsPointerOverUIObject())
                onMouseDownEvent.Invoke();
        }
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}