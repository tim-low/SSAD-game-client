using UnityEngine;
using UnityEngine.EventSystems;
using SuperSad.Networking;

public class OverworldControl : MonoBehaviour {

    [SerializeField]
    private RoomPlayerManager roomPlayerManager;

    [SerializeField]
    private string overworldTag;
	
	// Update is called once per frame
	void Update () {

        Vector3 clickPos;
        if (GetInput(out clickPos))
        {
            // Old Networking Demo codes -- ignore
            //networkManager.SendMovePacket(thisPlayer.Character.Token, clickPos);
            //thisPlayer.MoveToTargetPos(clickPos);

            MovePlayer(clickPos);
        }
    }

    private void MovePlayer(Vector3 targetPos)
    {
        string userToken = UserState.Instance().Token;
        Vector3 startPos = roomPlayerManager.GetRoomPlayer(userToken).transform.position;

        // Send Cmd Packet
        SendMovePacket(userToken, startPos, targetPos);

        // Move local player w/o ack packet
        roomPlayerManager.MovePlayer(userToken, startPos, targetPos);
    }

    private void SendMovePacket(string token, Vector3 startPos, Vector3 targetPos)
    {
        // Construct Cmd Packet
        Packet cmd = new CmdRoomPlayerMove()
        {
            Token = token,
            StartPos = startPos,
            TargetPos = targetPos
        }.CreatePacket();

        // Send Cmd Packet
        NetworkStreamManager.Instance.SendPacket(cmd);
    }

    private bool GetInput(out Vector3 clickPos)
    {
        clickPos = Vector3.zero;
        if (OnClick3D.IsPointerOverUIObject())
            return false;

        Vector3 screenPos = Vector3.zero;
        bool input = false;

        // TEMPORARY FOR PC TESTING
        /*if (Input.GetMouseButtonDown(0))
        {
            screenPos = Input.mousePosition;
            input = true;
        }*/

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            screenPos = Input.mousePosition;
            input = true;
        }

#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Log.Instance.AppendLine("Touch Input");

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)// || touch.phase == TouchPhase.Moved)
            {
                screenPos = touch.position;
                input = true;
            }
        }
#endif
        if (input)
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(r, out hit) && hit.transform.tag == overworldTag)
            {
                Log.Instance.AppendLine("Raycast Hit");

                Transform objectHit = hit.transform;
                Debug.DrawRay(r.origin, r.direction * 100, Color.red, 100, true);

                clickPos = hit.point;
                return true;
            }
        }
        return false;
    }
}
