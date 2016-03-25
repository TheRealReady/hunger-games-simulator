﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hunger_games_simulator.level;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using hunger_games_simulator.core.networking;

namespace hunger_games_simulator.core
{
    class GameClient
    {
        Arena ClientArena;

        TcpClient tcpClient;
        public IPEndPoint ServerEp;
        public bool Connected { get { return tcpClient.Connected; } }
        bool busy = false;

        public GameClient(IPEndPoint ip)
        {
            ServerEp = ip;
            tcpClient = new TcpClient();
        }

        public void Connect()
        {
            tcpClient.BeginConnect(ServerEp.Address, ServerEp.Port, new AsyncCallback(OnConnect), this);
        }

        public void Sync()
        {
            while (busy)
            {
                Thread.Sleep(50);
            }
        }

        void OnConnect(IAsyncResult result)
        {
            busy = true;
            Thread.Sleep(1000);
            if (tcpClient.Connected)
            {
                NetworkStream stream = tcpClient.GetStream();

                ClientRequest req = new ClientRequest(-1);
                req.Purpose = ClientRequest.RequestType.Connect;
                req.SendTo(stream);

                ServerResponse resp = ServerResponse.ReceiveFrom(stream);
            }
            busy = false;
        }
    }
}
