using Newtonsoft.Json;
using ServerInWpf.Helpers;
using ServerInWpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerInWpf.Network
{
    public class NetworkServer
    {
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private const int BUFFER_SIZE = 1000000;
        private static byte[] buffer = new byte[BUFFER_SIZE];
        private static List<Socket> clientSockets = new List<Socket>();
        public ObservableCollection<User> Users { get; set; }
        public bool IsNetworkStarted { get; set; } = false;
        public void Start()
        {
            if (!IsNetworkStarted)
            {
                IsNetworkStarted = true;
                Users = new ObservableCollection<User>();
                serverSocket.Bind(new IPEndPoint(IPAddress.Parse("10.1.18.4"), 27001));
                serverSocket.Listen(0);
                serverSocket.BeginAccept(AcceptCallBack, null);
            }

        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket socket;
            try
            {
                socket = serverSocket.EndAccept(ar);
            }
            catch (Exception)
            {
                return;
            }

            clientSockets.Add(socket);
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            Console.WriteLine(socket.RemoteEndPoint.ToString(), " : Client connected  waiting for request . . .");
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket current = ar.AsyncState as Socket;
            int received;
            try
            {
                received = current.EndReceive(ar);
            }
            catch (Exception)
            {
                Console.WriteLine("Client disconnected . . .");
                current.Close();
                clientSockets.Remove(current);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string jsonString = Encoding.ASCII.GetString(recBuf);

            var userModel = JsonConvert.DeserializeObject<UserModel>(jsonString);

            var imagePath = ImageConvert.GetImagePath(userModel.ImageBytes);
            imagePath = imagePath.Substring(5);
            var user = new User
            {
                Fullname = userModel.Fullname,
                Age = userModel.Age,
                ImagePath = imagePath
            };

            App.Current.Dispatcher.Invoke(() =>
            {

                Users.Add(user);
            });


            //File.WriteAllText("user.txt", user.Name + "  " + user.Surname + "  " + user.Age + "   " + user.Message);
            //Console.WriteLine("Name : ",user.Name);
            //Console.WriteLine("Surname : ",user.Surname);
            //Console.WriteLine("Age : ",user.Age);

            //if (text.ToLower() == "get-time")
            //{
            //    //byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
            //    //current.Send(data);
            //    Console.WriteLine("Time sent to the client");
            //}


            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);

        }

    }
}
