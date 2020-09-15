using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateViewCharacter : MonoBehaviour {

    private float rotSpeed = 500f;
    public Transform rotateThis;
    private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        rotateThis.Rotate(Vector3.up, -rotX);
    }
}
