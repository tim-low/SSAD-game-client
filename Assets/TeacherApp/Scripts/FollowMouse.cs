using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    private void Update()
    {
        followMouse();
    }
    public void followMouse()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = transform.position.z;
        transform.position = pos;
    }
}
