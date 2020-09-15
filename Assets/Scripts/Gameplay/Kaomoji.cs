using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaomoji : MonoBehaviour {

    [SerializeField]
    protected float speed = 1f;

    private Vector3 initialLocalPosition;

	// Use this for initialization
	void Awake() {

        initialLocalPosition = transform.localPosition;
    }

    void OnDisable()
    {
        ResetPosition();
    }

    public void ResetPosition()
    {
        transform.localPosition = initialLocalPosition;
    }

	// Update is called once per frame
	void Update () {

        Translate();
    }

    protected virtual void Translate()
    {
        transform.localPosition += speed * Time.deltaTime * transform.up;
    }
}
