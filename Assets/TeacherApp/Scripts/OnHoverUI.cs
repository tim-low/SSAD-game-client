using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnHoverUI : MonoBehaviour {
    public GameObject hoverObj;
    public Text hoverText;
    public void Disable()
    {
        followMouse();
        hoverObj.SetActive(false);
    }
    public void Enable()
    {
        followMouse();
        hoverObj.SetActive(true);
    }
    public void followMouse()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = hoverObj.transform.position.z;
        hoverObj.transform.position = pos;
    }
}
