using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Net;

using SuperSad.Networking.Events;

namespace SuperSad.Networking
{
    public class NetworkStreamManager : MonoBehaviour {

        // singleton
        private static NetworkStreamManager instance = null;
        public static NetworkStreamManager Instance
        {
            get { return instance; }
        }
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);    // same NetworkStreamManager object for different scenes

                InitializeSocket();
            }
            else
            {
                Debug.Log("Duplicate NetworkStreamManager object! Destroying NetworkStreamManager component in " + gameObject.name);
                Destroy(this);
            }
        }

        #region private members 	
        private GameClient socketConnection;
        private Thread clientReceiveThread;

        [SerializeField]
        private bool isGameApp = true;
        private static byte[] protocolVersion_game = { 0x00, 0x00, 0x01, 0x0A }; // for initialization
        private static byte[] protocolVersion_teacher = { 0x80, 0x00, 0x01, 0x0A }; // for initialization

        [SerializeField]
        private MessageServer receiver;
        [SerializeField]
        private SocketErrorHandler socketErrorHandler;

        #endregion

        private void InitializeSocket()
        {
            //ListenForData();
            ConnectToTcpServer();
        }

        /// <summary> 	
        /// Setup socket connection. 	
        /// </summary> 	
        private void ConnectToTcpServer()
        {
            try
            {
                Application.backgroundLoadingPriority = UnityEngine.ThreadPriority.Low;

                clientReceiveThread = new Thread(new ThreadStart(ListenForData));
                clientReceiveThread.IsBackground = true;
                clientReceiveThread.Start();
            }
            catch (Exception e)
            {
                Debug.Log("On client connect exception " + e);
            }
        }
        /// <summary> 	
        /// Runs in background clientReceiveThread; Listens for incomming data. 	
        /// </summary>     
        private void ListenForData()
        {
            try
            {
                UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Create Socket"));
                //IPEndPoint remoteAddr = new IPEndPoint(IPAddress.Parse("134.209.98.43"), 12041);
                //socketConnection = new GameClient();
                //socketConnection.Connect(remoteAddr);
                socketConnection = new GameClient("134.209.98.43"/*"157.230.37.22"*/, 12041, receiver);
                // TEMPORARY: assign PacketReceiver
                //socketConnection.SetPacketReceiver(receiver);

                UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Socket Connection established."));

                // Send initialization for SADCrypt
                if (isGameApp)
                    socketConnection.Send(protocolVersion_game);
                else
                    socketConnection.Send(protocolVersion_teacher);
                // Start Listening for packets
                socketConnection.Listen();
            }
            catch (ThreadAbortException e)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("ThreadAbortException: " + e));
            }
            catch (SocketException socketException)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Socket exception: " + socketException));
                UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Socket exception: " + socketException.StackTrace));
                UnityMainThreadDispatcher.Instance().Enqueue(socketErrorHandler.SocketDestroyedError(GameClient.WasConnectedToServer));
            }
            catch (Exception exception)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Exception: " + exception));
                UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Socket exception: " + exception.StackTrace));
                UnityMainThreadDispatcher.Instance().Enqueue(socketErrorHandler.SocketDestroyedError(GameClient.WasConnectedToServer));
            }
        }

        public void SendPacket(Packet packet)
        {
            if (socketConnection == null)
            {
                Log.Instance.AppendLine("Socket not available.");
                return;
            }

            Log.Instance.AppendLine("Send Packet: " + packet.Id);

            try
            {
                socketConnection.Send(packet, true);
            }
            catch (Exception ex)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread(String.Format("[INFO] {0}", ex.Message)));
                UnityMainThreadDispatcher.Instance().Enqueue(socketErrorHandler.SocketDestroyedError(GameClient.WasConnectedToServer));
            }
        }

        void OnDestroy()
        {
            if (socketConnection != null)
            {
                Debug.Log("Destroy NetworkStreamManager");
                try
                {
                    clientReceiveThread.Abort();
                    Debug.Log("Called Abort");
                    //clientReceiveThread.Join();
                    //Debug.Log("Thread Join finished");
                    socketConnection.CloseConnection();
                }
                catch (ThreadAbortException e)
                {
                    Debug.Log("ThreadAbortException: " + e);
                }
                catch (Exception e)
                {
                    Debug.Log("Exception: " + e);
                }
                finally
                {
                    socketConnection.Close();
                    Debug.Log("Socket closed");
                }
            }
             Debug.Log("NetworkStreamManager Destroyed");
        }
    }

}