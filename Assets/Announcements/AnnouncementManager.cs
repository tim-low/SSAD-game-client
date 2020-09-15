using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SuperSad.Networking;
using SuperSad.Networking.Events;

namespace SuperSad.GameEvents
{
    public class AnnouncementManager : EventListener
    {
        [SerializeField]
        private RawImage bannerImage;

        [Header("Announcement Popup")]
        [SerializeField]
        private GameObject popup;
        [SerializeField]
        private Text announcementName;
        [SerializeField]
        private Text announcementDescription;
        [SerializeField]
        private RawImage popupBannerImage;

        private static int[] announcementsToDisplay;
        private static Dictionary<int, Announcement> announcementData = null;

        private int popupDisplayIdx = 0;

        // Use this for initialization
        void Start()
        {
            if (announcementData == null)
                announcementData = new Dictionary<int, Announcement>();

            SendCmdGetAnnouncementsPacket();
        }

        public override void Subscribe(INotifier<Packet> notifier)
        {
            notifier.Register(HandleGetAnnouncements, Packets.GetAnnouncementsAck);
            notifier.Register(HandleAnnouncementData, Packets.AnnouncementDataAck);
        }

        private void SendCmdGetAnnouncementsPacket()
        {
            Packet cmd = new CmdGetAnnouncements().CreatePacket();

            NetworkStreamManager.Instance.SendPacket(cmd);
        }

        private void HandleGetAnnouncements(Packet packet)
        {
            GetAnnouncementsAck ack = new GetAnnouncementsAck(packet);

            if (ack.AnnouncementIds == null)
                announcementsToDisplay = null;
            else
            {
                // Store the announcements to be displayed
                announcementsToDisplay = (int[])ack.AnnouncementIds.Clone();

                // Check if there are any Announcements whose data is not retrieved before
                List<int> announcementsToFetch = new List<int>();
                foreach (int id in ack.AnnouncementIds)
                {
                    if (!announcementData.ContainsKey(id))
                    {
                        announcementsToFetch.Add(id);
                    }
                }

                // Need to fetch
                if (announcementsToFetch.Count > 0)
                {
                    Packet cmd = new CmdAnnouncementData()
                    {
                        AnnouncementIds = announcementsToFetch
                    }.CreatePacket();

                    NetworkStreamManager.Instance.SendPacket(cmd);
                }
                else
                {
                    LoadAnnouncements();
                }
            }
        }

        private void HandleAnnouncementData(Packet packet)
        {
            AnnouncementDataAck ack = new AnnouncementDataAck(packet);

            // Store Announcement data first
            foreach (Announcement announcement in ack.Announcements)
            {
                announcementData.Add(announcement.Id, announcement);
            }

            LoadAnnouncements();
        }

        private void LoadAnnouncements()
        {
            // TEMPORARY testing
            if (announcementsToDisplay != null)
            {
                bannerImage.gameObject.SetActive(true);

                // display first announcement
                int id = announcementsToDisplay[0];
                bannerImage.texture = announcementData[id].BannerTexture;
            }
            else
            {
                bannerImage.gameObject.SetActive(false);
            }

            //foreach (int id in announcementsToDisplay)
            //{
            //    bannerImage.texture
            //}
        }

        public void DisplayAnnouncementPopup()
        {
            popup.SetActive(true);
            UpdateAnnouncementPopup();
        }

        private void UpdateAnnouncementPopup()
        {
            // Get Announcement
            int idToDisplay = announcementsToDisplay[popupDisplayIdx];
            Announcement announcement = announcementData[idToDisplay];

            // Set display
            announcementName.text = announcement.Name;
            announcementDescription.text = announcement.Description;
            popupBannerImage.texture = announcement.BannerTexture;
        }
    }

}