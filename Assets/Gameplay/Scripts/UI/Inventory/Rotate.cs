using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float accelerate = 0f;

    // Update is called once per frame
    void Update () {

        transform.RotateAround(transform.position, transform.up, speed * Time.deltaTime);
        if (accelerate > 0f)
            speed += accelerate * Time.deltaTime;
	}
}
