using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperSad.Networking;
using SuperSad.Networking.Events;

public class RandomGacha : EventListener
{
    [SerializeField]
    private Button backButtonScript;
    [SerializeField]
    private BackButton backScript;
    [SerializeField]
    private GameObject chestButton;
    [SerializeField]
    private GameObject backButtonObj;
    [SerializeField]
    private Text chestCountText;
    [SerializeField]
    private SpriteRenderer itemSpriteRen;
    [SerializeField]
    private SpriteRenderer backspriteRen;
    [SerializeField]
    private SpriteRenderer glowRen;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private int[] rarityIndex;
    [SerializeField]
    private Color[] rarityColor;
    [SerializeField]
    private Color[] rarityGlow;
    [SerializeField]
    private AudioSource audClick;
    [SerializeField]
    private AudioSource audParty;
    [SerializeField]
    private AudioSource audDrop;
    [SerializeField]
    private int itemToget;
    [SerializeField]
    private GameObject partyParticles;
    [SerializeField]
    private Animator anim;
    private bool opened = false;
    public float fieldOfView = 60;
    private void Start()
    {
        UpdateChestText();
        float sfxfloat = AudioManager.instance.SfxVolume;
        audClick.volume = sfxfloat;
        audParty.volume = sfxfloat;
        audDrop.volume = sfxfloat;
    }
    private void UpdateChestText()
    {
        chestCountText.text = (UserState.Instance().chestCount > 9) ? "9+" : UserState.Instance().chestCount.ToString();
    }
    public void changeSprite(int index)
    {
        itemSpriteRen.sprite = sprites[index];
    }
    public void changeToObtainItem()
    {
        partyParticles.SetActive(true);
        itemSpriteRen.sprite = sprites[itemToget];
        backspriteRen.color = rarityColor[rarityIndex[itemToget]];
        glowRen.color = rarityGlow[rarityIndex[itemToget]];
        audClick.Play();
        audParty.Play();
        chestButton.SetActive(true);
        setBackButton(true);
    }
    public void changeRandomSprite()
    {
        int random = Random.Range(0, sprites.Length);
        itemSpriteRen.sprite = sprites[random];
        backspriteRen.color = rarityColor[rarityIndex[random]];
        glowRen.color = rarityGlow[rarityIndex[random]];
        audClick.Play();
    }
    public void playDropAud()
    {
        audDrop.Play();
    }
    private void OnMouseDown()
    {
        if(opened==false)
        {
            opened = true;
            setBackButton(false);
            ConstructOpenChestPacket();
        }
        
    }
    private void setBackButton(bool enable)
    {
        backButtonObj.SetActive(enable);
        //backScript.enabled = enable;
        //backButtonScript.enabled = enable;
    }
    public void clickChestButton()
    {
        if(UserState.Instance().chestCount ==0)
        {
            return;
        }
        chestButton.SetActive(false);
        if (opened == true)
        {
            Camera.main.fieldOfView = 60;
            opened = false;
            anim.SetTrigger("ReOpen");
        }
        anim.SetTrigger("Drop");
    }
    public override void Subscribe(INotifier<Packet> notifier)
    {
        // for better separation of concerns (Single Responsibility Principle), these handling of packets should be in separate scripts instead of all-in-one manager like this script
        notifier.Register(HandleErrorPacket, Packets.ErrorAck);
        notifier.Register(HandleGetChest, Packets.GetChestRewardAck);
    }
    public void HandleGetChest(Packet packet)
    {
        Debug.Log("Error Packet Received");
        UserState.Instance().chestCount--;
        UpdateChestText();
        GetChestRewardAck ack = new GetChestRewardAck(packet);
        itemToget = ack.AttributeNumber * 8 + ack.ItemNumber;
        Camera.main.fieldOfView = 40;
        anim.SetTrigger("Open");
        Debug.Log(ack.AttributeNumber);
        Debug.Log(ack.ItemNumber);
    }
    public void HandleErrorPacket(Packet packet)
    {
        Debug.Log("Error Packet Received");
        ErrorAck ack = new ErrorAck(packet);
        Debug.Log(ack.Message);
    }
    public void ConstructOpenChestPacket()
    {
        //expectError = true;
        Packet ack = new CmdOpenChest()
        {
            Token = UserState.Instance().Token
        }.CreatePacket();

        Debug.Log("Get Open chest Packet Created");

        NetworkStreamManager.Instance.SendPacket(ack);
    }
    private void Update()
    {
        if(Camera.main.fieldOfView != fieldOfView)
        {
            Camera.main.fieldOfView = fieldOfView;
        }
        
    }
}
