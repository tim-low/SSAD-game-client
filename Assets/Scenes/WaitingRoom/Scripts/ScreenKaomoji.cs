using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenKaomoji : Kaomoji {

    [SerializeField]
    private Image avatarIcon;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetAvatarIcon(Sprite avatar)
    {
        avatarIcon.sprite = avatar;
    }

    protected override void Translate()
    {
        transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
    }
}
