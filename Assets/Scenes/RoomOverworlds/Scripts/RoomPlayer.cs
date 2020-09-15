using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using SuperSad.Model;
using SuperSad.Gameplay;

public class RoomPlayer : BasePlayer3D {

    [SerializeField]
    private GameObject ownerIndicator;  // this player is host of the room

    [SerializeField]
    [Tooltip("Coordinate Units per second")]
    private float moveSpeed = 10f;

    //private float yPos;
    private IEnumerator moveRoutine = null;

    public Character Character { get; private set; }

    public void SetCharacterDetails(Character character, Color color)
    {
        // Set username
        SetUsername(character.Username, color);

        // Set costume on Avatar
        SetAvatar(character.Attributes);
    }

    public void RotateCharacter(Vector3 dir)
    {
        // Create the rotation we need to be in to look at the target
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        // Rotate Player
        transform.rotation = lookRotation;
    }

    public void SetRoomOwner(bool isOwner)
    {
        ownerIndicator.SetActive(isOwner);
    }

    public void MoveToTargetPos(Vector3 startPos, Vector3 targetPos)
    {
        //targetPos.y = yPos;

        // find direction vector towards target position
        Vector3 dir = targetPos - transform.position;
        // Rotate Player
        RotateCharacter(dir);

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = MoveToPos(targetPos);
        StartCoroutine(moveRoutine);
    }

    private IEnumerator MoveToPos(Vector3 targetPos)
    {
        // Start Move Animation
        PlayMoveAnimation();

        // Calculate time needed to reach target
        float distance = (targetPos - transform.position).magnitude;
        float moveTime = distance / moveSpeed;
        
        // Lerp throughout duration
        float timer = 0f;
        Vector3 startPos = transform.position;
        while (timer < moveTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, timer / moveTime);
            yield return null;
        }
        // Set to target position
        transform.position = targetPos;

        // End Move Animation
        StopMoveAnimation();
    }
}
