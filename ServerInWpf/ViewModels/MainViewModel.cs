using ServerInWpf.Commands;
using ServerInWpf.Models;
using ServerInWpf.Network;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ServerInWpf.ViewModels
{
    public class MainViewModel :BaseViewModel
    {
        public RelayCommand StartServerCommand { get; set; }
        private ObservableCollection<User> allUsers;
        public ObservableCollection<User> AllUsers
        {
            get { return allUsers; }
            set { allUsers = value; OnPropertyChanged(); }
        }
        public DispatcherTimer Timer { get; set; } = new DispatcherTimer();
        NetworkServer networkServer = new NetworkServer();
        public MainViewModel()
        {
            AllUsers = new ObservableCollection<User>();
            AllUsers.Add(new User
            {
                Fullname = "Kamran Eliyev",
                Age = 24,
                //C:\Users\e.camalzade\source\repos\ServerInWpf\ServerInWpf\bin\Debug\Images
                ImagePath = @"/Images/image1.jpg"
            }); ;
            StartServerCommand = new RelayCommand((sender) =>
              {
                  //networkServer.Start();
                  Timer.Interval = TimeSpan.FromMilliseconds(500);
                  Timer.Tick += Timer_Tick;
                  AllUsers = networkServer.Users;
                  Timer.Start();
              });
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => {
                AllUsers = networkServer.Users;
            });
        }
    }
}
