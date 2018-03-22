using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace Milestone2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Friend
        {
            public Friend(string name, string avgStar, string yelpSince)
            {
                this.name = name;
                this.avgStar = avgStar;
                this.yelpSince = yelpSince;
            }
            public string name { get; set; }
            public string avgStar { get; set; }
            public string yelpSince { get; set; }


        }

        string userName;
        public MainWindow()
        {
            InitializeComponent();
            AddColumnsToGrid();
        }

        private string BuildConnString()
        {
            return "Server=localhost; Database=yelpdb; Port=5432; Username=postgres; Password=Compaq27";
        }

        private void searchNameButton_Click(object sender, RoutedEventArgs e)
        {
            userName = searchName.Text;
            getUserIds();
        }

        public void getUserIds()
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT user_id FROM userTable WHERE name = '" + userName + "';";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userIds.Items.Add(reader.GetString(0));
                        }
                    }
                }

                conn.Close();
            }
        }

        public void getUserFriends(string id)
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT temp.name, temp.average_stars, temp.yelping_since FROM userTable as U, friendsTable as F, (SELECT * FROM userTable) as temp WHERE U.user_id = '" + id + "' AND U.user_id = F.user_id AND F.friend_id = temp.user_id;";
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //userFriendsList.Items.Add(new Friend(reader.GetString(0), Convert.ToDouble(reader.GetString(1)), reader.GetString(2)));
                            //double temp = reader.GetDouble(1);
                            userFriendsList.Items.Add(new Friend(reader.GetString(0), reader.GetDouble(1).ToString(), reader.GetString(2)));

                        }
                    }
                }

                conn.Close();
            }
        }

        public void AddColumnsToGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Name";
            col1.Binding = new Binding("name");
            col1.Width = 100;
            userFriendsList.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Avg Stars";
            col2.Binding = new Binding("avgStar");
            col2.Width = 60;
            userFriendsList.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Yelping Since";
            col3.Binding = new Binding("yelpSince");
            col3.Width = 80;
            userFriendsList.Columns.Add(col3);
        }

        private void userIds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string id = userIds.SelectedValue.ToString();
            userFriendsList.Items.Clear();
            getUserFriends(id);
        }
    }
}
