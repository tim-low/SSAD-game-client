using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackButton : MonoBehaviour
{
    private Button referenceToTheButton;
    // Start is called before the first frame update
    void Start()
    {
        referenceToTheButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            referenceToTheButton.onClick.Invoke();
        }
    }
}
