using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputShortcuts : MonoBehaviour {

    [SerializeField]
    private Selectable firstSelectable;
    [SerializeField]
    private bool enableSelectableOnStart;

    private EventSystem system;

    // Use this for initialization
    void Start () {

        system = EventSystem.current;

        if (enableSelectableOnStart)
        {
            if (firstSelectable != null)
                EnableSelectable(firstSelectable);
        }
    }

    // Update is called once per frame
    void Update () {

        // Navigate to next Selectable UI element
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                EnableSelectable(next);
            }
            //else Debug.Log("next nagivation element not found");
        }
    }

    private void EnableSelectable(Selectable selectable)
    {
        InputField inputfield = selectable.GetComponent<InputField>();
        if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

        system.SetSelectedGameObject(selectable.gameObject, new BaseEventData(system));
    }
}
