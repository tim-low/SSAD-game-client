using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitAvatar : MonoBehaviour {
    [SerializeField]
    private Customizer customizer;
    // Use this for initialization
    void Start () {
        InitaliseAvatar();
    }
    /*IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call
        InitaliseAvatar();
    }*/
    private void InitaliseAvatar()
    {
        for(int i =0; i <4;i++)
        {
            customizer.ChangeSkin(i, UserState.Instance().characterWears[i]);
        }
       
    }
}
