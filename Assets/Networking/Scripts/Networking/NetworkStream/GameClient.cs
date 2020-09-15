using System;
using System.Net.Sockets;
using UnityEngine;
using System.IO;
using System.Linq;

using SADCrypt;
using SADCrypt.Helper;

namespace SuperSad.Networking
{
    public class GameClient : TcpClient
    {
        private NetworkStream _ns { get { return this.GetStream(); } }

        private byte[] _buffer;
        private int _bytesToRead;

        private ushort _packetLength, _packetId;
        private byte[] packetIdBytes;

        private IPacketReceiver packetReceiver;

        private Supercrypt _crypt = null;

        public static bool WasConnectedToServer { get; private set; }

        /*
         * TCP NetworkStream Read values
         */
        private const int HeaderLength = 4;

        public void SetPacketReceiver(IPacketReceiver newReceiver)
        {
            packetReceiver = newReceiver;
            Log.Instance.AppendLine("Packet Receiver Set");
        }

        public GameClient() : base()
        {
            //nullPingCount = 0;
            _crypt = null;
            WasConnectedToServer = false;
        }
        public GameClient(string hostname, int port, IPacketReceiver receiver) : base(hostname, port)
        {
            //nullPingCount = 0;
            _crypt = null;
            WasConnectedToServer = false;
            this.packetReceiver = receiver;
        }

        public void Listen()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Start Listening ..."));

            // Async Callback
            _buffer = new byte[4];
            _bytesToRead = _buffer.Length;
            _ns.BeginRead(_buffer, 0, 4, OnHeader, null);

            // Synchronous
            /*while (true)
            {
                //int bufferLength = this.ReceiveBufferSize;
                //if (bufferLength > 0)
                if (_ns.DataAvailable)
                {
                    Debug.Log("Start read");

                    int expectedReadLength;

                    _buffer = new byte[1024];
                    expectedReadLength = HeaderLength;
                    //Debug.Log("Expected read length: " + expectedReadLength);
                    while (expectedReadLength > 0)
                    {
                        int readLength = _ns.Read(_buffer, HeaderLength - expectedReadLength, expectedReadLength);
                        //Debug.Log("Read: " + readLength);
                        expectedReadLength -= readLength;
                        //Debug.Log("Expected remaining: " + expectedReadLength);
                    }
                    // Get packet length and packet Id
                    _packetLength = BitConverter.ToUInt16(_buffer, 0);
                    Debug.Log("Packet Received. Length: " + _packetLength);
                    packetIdBytes = new byte[2];
                    packetIdBytes[0] = _buffer[2];
                    packetIdBytes[1] = _buffer[3];

                    // Read packet contents
                    int contentLength = _packetLength - 4;
                    byte[] readbuffer = new byte[contentLength];
                    expectedReadLength = contentLength;
                    //Debug.Log("Expected read length: " + expectedReadLength);
                    while (expectedReadLength > 0)
                    {
                        int readLength = _ns.Read(readbuffer, contentLength - expectedReadLength, expectedReadLength);
                        //Debug.Log("Read: " + readLength);
                        expectedReadLength -= readLength;
                        //Debug.Log("Expected remaining: " + expectedReadLength);
                    }

                    //Debug.Log("Packet bits received; going to process");

                    // Retrieve contents
                    _buffer = new byte[contentLength];
                    Array.Copy(readbuffer, 0, _buffer, 0, contentLength);

                    // Process packet
                    if (_crypt == null) // initial SymKeyAck
                    {
                        // Get packet Id
                        _packetId = BitConverter.ToUInt16(_buffer, 0);
                        if (_packetId == Packets.SymKeyAck)
                        {
                            var packet = new Packet(this, _packetId, _buffer.Skip(2).ToArray());

                            SymKeyAck ack = new SymKeyAck(packet);
                            _crypt = new Supercrypt(ack.Key, ack.IV);

                            UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Encryption Key received"));
                        }
                        else
                        {
                            UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Received packet " + _packetId + " (id) when this client has yet to receive encryption key"));
                        }
                    }
                    else    // regular packet, encrypted
                    {
                        /// NOT NEEDED ANYMORE AS READING ONLY PACKET LENGTH; READING PACKET ID AND PACKET CONTENTS TOGETHER
                        // Combine the buffer for packetId and the data because we encrypt those together, we have to combine and decrypt them together
                        _buffer = packetIdBytes.Concat(_buffer).ToArray();

                        // Decrypt the buffer to retrieve the actual packetId and the data
                        _buffer = _crypt.Decrypt(_buffer).ToArray();

                        // Get packet Id after decryption
                        _packetId = BitConverter.ToUInt16(_buffer, 0);
                        UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Received Packet: " + _packetId));

                        // Process packet
                        if (_packetId == 0) // CmdNullPing
                        {
                            Debug.Log("CmdNullPing; Send NullPingAck back");

                            Packet ack = new NullPingAck().CreatePacket();
                            Send(ack, true);
                        }
                        else
                        {
                            var packet = new Packet(this, _packetId, _buffer.Skip(2).ToArray());

                            Debug.Log("Call IPacketReceiver.Receive");

                            UnityMainThreadDispatcher.Instance().Enqueue(packetReceiver.Receive(packet));
                        }
                    }
                }
            }*/

        }

        private void OnHeader(IAsyncResult result)
        {
            //Debug.Log("OnHeader");
            try
            {
                _bytesToRead -= _ns.EndRead(result);
                //_bytesToRead -= _ns.Read(result._b, int offset, int size);
                //UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("_bytesToRead: " + _bytesToRead));
                if (_bytesToRead > 0)
                {
                    //UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("OnHeader _bytesToRead > 0"));
                    _ns.BeginRead(_buffer, _buffer.Length - _bytesToRead, _bytesToRead, OnHeader, null);
                    return;
                }

                // Able to receive bits means socket was connected to server before
                WasConnectedToServer = true;

                _packetLength = BitConverter.ToUInt16(_buffer, 0);
                //_packetId = BitConverter.ToUInt16(_buffer, 2);
                packetIdBytes = _buffer.Skip(2).ToArray();

                _bytesToRead = _packetLength - 4;
                _buffer = new byte[_bytesToRead];
                _ns.BeginRead(_buffer, 0, _bytesToRead, OnData, null);
            }
            catch (Exception ex)
            {
                throw ex;
                //UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread(String.Format("[INFO] {0}", ex.Message)));
                //UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread(String.Format("[INFO] {0}", ex.StackTrace)));
            }
        }

        private void OnData(IAsyncResult result)
        {
            //UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("OnData"));
            try
            {
                _bytesToRead -= _ns.EndRead(result);
                if (_bytesToRead > 0)
                {
                    _ns.BeginRead(_buffer, _buffer.Length - _bytesToRead, _bytesToRead, OnData, null);
                    //UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("OnData _bytesToRead > 0"));
                    return;
                }

                if (_crypt == null) // initial SymKeyAck    should be if (_crypt == null) when encryption is added
                {
                    _packetId = BitConverter.ToUInt16(packetIdBytes, 0);

                    var packet = new Packet(this, _packetId, _buffer);

                    if (_packetId == Packets.SymKeyAck)
                    {
                        SymKeyAck ack = new SymKeyAck(packet);
                        _crypt = new Supercrypt(ack.Key, ack.IV);

                        UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Encryption Key received"));
                    }
                    else
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Received packet " + _packetId + " (id) when this client has yet to receive encryption key"));
                    }
                }
                else    // regular packet, encrypted
                {
                    // Combine the buffer for packetId and the data because we encrypt those together, we have to combine and decrypt them together
                    _buffer = packetIdBytes.Concat(_buffer).ToArray();
                    // Decrypt the buffer to retrieve the actual packetId and the data
                    _buffer = _crypt.Decrypt(_buffer).ToArray();

                    var packet = new Packet(this, BitConverter.ToUInt16(_buffer, 0), _buffer.Skip(2).ToArray());

                    if (packet.Id == 0) // CmdNullPing
                    {
                        Debug.Log("CmdNullPing; Send NullPingAck back");

                        Packet ack = new NullPingAck().CreatePacket();
                        Send(ack, true);
                    }
                    else
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Received Packet: " + packet.Id));

                        UnityMainThreadDispatcher.Instance().Enqueue(packetReceiver.Receive(packet));
                    }
                }

                // Prepare to read next incoming Packet
                UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread("Begin Next Read"));
                _buffer = new byte[4];
                _bytesToRead = _buffer.Length;
                _ns.BeginRead(_buffer, 0, 4, OnHeader, null);

            }
            catch (Exception ex)
            {
                throw ex;
                //UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread(String.Format("[INFO] {0}", ex.Message)));
                //UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread(String.Format("[INFO] {0}", ex.StackTrace)));
            }
        }

        public void Send(byte[] bytes)  // for now, this will not require encryption
        {
            Debug.Log("Send initialization bytes");

            ushort bufferLength = (ushort)bytes.Length;

            try
            {
                //_ns.Write(BitConverter.GetBytes(length), 0, 2);
                _ns.Write(bytes, 0, bufferLength);
            }
            catch (Exception ex)
            {
                throw ex;
                //UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread(String.Format("[INFO] {0}", ex.Message)));
            }
        }

        public void Send(Packet packet, bool encrypt)
        {
            Debug.Log("Send Packet: " + packet.Id);

            var buffer = packet.Writer.GetBuffer();

            // Encrypt packet - all packets should be encrypted, including CmdNullPing and NullPingAck
            if (encrypt)
                buffer = _crypt.Encrypt(packet.Writer.GetBuffer());

            var bufferLength = buffer.Length;
            var length = (ushort)(bufferLength + 2); // Length includes itself

            try
            {
                _ns.Write(BitConverter.GetBytes(length), 0, 2);
                _ns.Write(buffer, 0, bufferLength);
            }
            catch (Exception ex)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(Log.Instance.AppendLineFromOtherThread(String.Format("[INFO] {0}", ex.Message)));
            }
        }

        public void CloseConnection()
        {
            _ns.Dispose();
            _ns.Close();
            this.Client.Shutdown(SocketShutdown.Both);  // Disables both sending and receiving on this Socket
            this.Close();
            Debug.Log("Socket closed");
        }
    }
}
