using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocketErrorHandler : MonoBehaviour {

    [SerializeField]
    private GameObject socketErrorPanel;
    [SerializeField]
    private Text socketErrorText;

    public IEnumerator SocketDestroyedError(bool wasConnected)
    {
        Debug.Log("Why");
        yield return null;
        socketErrorPanel.SetActive(true);
        if (wasConnected)
            socketErrorText.text = "Connection to the server was lost.";
        else
            socketErrorText.text = "Failed to connect to server.";
    }

    public void HandleSocketDestroyed()
    {
        // Close the application
        Application.Quit();
    }

}
