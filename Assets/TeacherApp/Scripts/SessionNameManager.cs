using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SessionNameManager : MonoBehaviour {

    [Header("UI Elements")]
    [SerializeField] Text text;

    // Use this for initialization
    void Start () {
        text.text = SessionOnClick.selectedsession;

    }

}
