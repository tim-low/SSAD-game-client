using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
public class ToggleCounter : MonoBehaviour
{
    public static int checkBoxesOn = 0;
    Toggle toggle;
    //public GameObject questionObj;
    GameObject toggleObj;
    GameObject labelObj;

    // each toggle calls this:
    public void checkboxvaluechanged(bool isOn)
    {
        checkBoxesOn += isOn ? 1 : -1;
        print("checkboxes on = " + checkBoxesOn);


        //Fetch the Toggle GameObject
        toggleObj = GetComponent<Toggle>().gameObject;
        //Add listener for when the state of the Toggle changes, to take action
        toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(toggle);
        });

        //if (ison)
        //{
        //    //toggleobj = instantiate(toggleprefab);
        //    toggleobj = toggle.gameobject;
        //    //toggle.transform.setparent(questionobj.transform);
        //    labelobj = toggleobj.transform.getchild(1).gameobject;
        //    print("label name: " + labelobj.getcomponent<text>().text);
        //}
    }

    public int TogglesOn { get; private set; }
    public int TogglesOff { get; private set; }

    public int TotalToggles
    {
        get { return checkboxes.Count; }
    }

    private List<Toggle> checkboxes;

    private void Start()
    {
        checkboxes = GetComponentsInChildren<Toggle>().ToList();

        foreach (Toggle toggle in checkboxes)
        {
            // get an initial count of on/off
            if (toggle.isOn)
            {
                TogglesOn++;
            }
            else
            {
                TogglesOff++;
            }

            // listen for changes
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    private void OnToggleValueChanged(bool isOn)
    {
        TogglesOn += isOn ? 1 : -1;
        TogglesOff += !isOn ? 1 : -1;

        //print("Toggles On: " + TogglesOn);
        //print("Toggles Off: " + TogglesOff);
        //print("Checks: " + TotalToggles);

        checkboxvaluechanged(isOn);
    }

    public void ToggleValueChanged(Toggle change)
    {
        toggleObj = change.gameObject;
        labelObj = toggleObj.transform.GetChild(1).gameObject;
        print("label name: " + labelObj.GetComponent<Text>().text);
    }

}