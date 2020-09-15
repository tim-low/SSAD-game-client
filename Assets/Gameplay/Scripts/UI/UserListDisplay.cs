using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserListDisplay : MonoBehaviour {

    [SerializeField]
    private UserListItem userListItem;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddUserItem(int playerId, string username, Color playerColor)
    {
        // Create list item
        UserListItem item = Instantiate(userListItem);
        item.transform.SetParent(transform);

        item.transform.localScale = new Vector3(1f, 1f, 1f);

        // Adjust playerColor
        /*playerColor = new Color(Mathf.Max(0f, playerColor.r - 0.1f),
                                Mathf.Max(0f, playerColor.g - 0.1f),
                                Mathf.Max(0f, playerColor.b - 0.1f)
                               );
        */

        // Set variables
        //item.SetPlayerText(playerId, playerColor);
        item.SetUsername(username, playerColor);
    }
}
