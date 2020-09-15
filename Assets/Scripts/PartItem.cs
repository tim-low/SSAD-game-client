using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartItem : MonoBehaviour {
    private int partIndex = 0;
    private int setIndex = 0;
    private Customizer customizer;
    public Image image;
    private PlaySFX playSFX;
    public void SetPartItem(int partIndex , int setIndex , Customizer customizer , Sprite sprite)
    {
        this.partIndex = partIndex;
        this.setIndex = setIndex;
        this.customizer = customizer;
        ChangeImage(sprite);
    }
    public void ChangeImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
    public void setPlaySFX(PlaySFX playSFX)
    {
        this.playSFX = playSFX;
    }
    public void ChangeSkin()
    {
        customizer.ChangeSkin(partIndex, setIndex);
        playSFX.Play2DSoundEffect("ButtonClick");
    }
}
