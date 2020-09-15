using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    [SerializeField]
    private GameObject component;

    private void OnMouseEnter()
    {
        //Debug.Log("Hi");
        component.SetActive(true);
    }

    private void OnMouseExit()
    {
        //Debug.Log("Bye");
        component.SetActive(false);
    }
}
