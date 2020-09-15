using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {
    public GameObject[] selections;
    private int lastSelected = 0;
	
    public void selectObject(int index)
    {
        selections[lastSelected].SetActive(false);
        lastSelected = index;
        selections[index].SetActive(true);
    }
}
