using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Core.Network;


namespace Core
{
    public class Program
    {
        static Random rand = new Random();
        private static sbyte[] GenerateRandomBytes(int length)
        {
            var result = new sbyte[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = (sbyte)rand.Next(0, 255);
            }
            return result;
        }

        public static byte[] GenerateBytes(int len)
        {
            byte[] res = new byte[len];
            rand.NextBytes(res);
            return res;
        }


        static void Main(string[] args)
        {
            var login = new LoginServer(new TcpListener(IPAddress.Parse("127.0.01"), 2106));
            login.Start();
            while(login.Running)
            {
                login.Tick();
            }
        }
    }
}
