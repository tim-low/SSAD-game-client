using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIcon : MonoBehaviour {
    private RectTransform rect;
    [SerializeField]
    private float rotspd;
	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        rect.Rotate(new Vector3(0, Time.deltaTime * rotspd, 0));

    }
}
