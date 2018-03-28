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

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Business
        {
            public Business(string name, string address, string state, string city, string stars, string reviewCount, string avgReview, string numCheckins)
            {
                this.name = name;
                this.address = address;
                this.state = state;
                this.city = city;

                this.stars = stars;
                this.reviewCount = reviewCount;
                this.avgReview = avgReview;
                this.numCheckins = numCheckins;
            }

            public string name { get; set; }
            public string address { get; set; }
            public string state { get; set; }
            public string city { get; set; }

            public string stars { get; set; }
            public string reviewCount { get; set; }
            public string avgReview { get; set; }
            public string numCheckins { get; set; }

        }


        public class User
        {
            public User(string name, string user_id, string yelping_since, int fans, double average_stars, int funny, int useful, int cool)
            {
                this.name = name;
                this.user_id = user_id;
                this.yelping_since = yelping_since;
                this.fans = fans;
                this.average_stars = average_stars;
                this.funny = funny;
                this.useful = useful;
                this.cool = cool;

            }

            public string name { get; set; }
            public string user_id { get; set; }
            public string yelping_since { get; set; }

            public int fans { get; set; }
            public double average_stars { get; set; }
            public int funny { get; set; }
            public int useful { get; set; }
            public int cool { get; set; }

        }

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

        public class FriendReview
        {
            public FriendReview(string name, string businessName, string city, string text)
            {
                this.name = name;
                this.businessName = businessName;
                this.city = city;
                this.text = text;
            }

            public string name { get; set; }
            public string businessName { get; set; }
            public string city { get; set; }
            public string text { get; set; }
        }


        public MainWindow()
        {
            InitializeComponent();
            AddStates();
            AddColumnsToGrid();
            AddColumnsToFriendsGrid();
            AddColumnsToFriendsTipsGrid();
        }

        private string BuildConnString()
        {
            return "Server=localhost; Database=yelpdb; Port=5432; Username=postgres; Password=Compaq27";
        }

        public void AddStates()
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT state FROM businessTable ORDER BY state;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StateList.Items.Add(reader.GetString(0));
                        }
                    }
                }

                conn.Close();
            }
        }

        public void AddColumnsToGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Business Name";
            col1.Binding = new Binding("name");
            col1.Width = 150;
            BusinessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Address";
            col2.Binding = new Binding("address");
            col2.Width = 200;
            BusinessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            col3.Width = 170;
            BusinessGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "State";
            col4.Binding = new Binding("state");
            col4.Width = 50;
            BusinessGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Distance";
            col5.Binding = new Binding("distance");
            col5.Width = 100;
            BusinessGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Stars";
            col6.Binding = new Binding("stars");
            col6.Width = 50;
            BusinessGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = "# of Reviews";
            col7.Binding = new Binding("reviewCount");
            col7.Width = 100;
            BusinessGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Header = "Avg Review Rating";
            col8.Binding = new Binding("avgReview");
            col8.Width = 100;
            BusinessGrid.Columns.Add(col8);

            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Header = "Total Checkins";
            col9.Binding = new Binding("numCheckins");
            col9.Width = 100;
            BusinessGrid.Columns.Add(col9);
        }

        private void StateListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CityList.Items.Clear();

            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT city FROM businessTable WHERE state = '" + StateList.SelectedItem.ToString() + "' ORDER BY city;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CityList.Items.Add(reader.GetString(0));
                        }
                    }
                }

                conn.Close();
            }

        }


        private void CityListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CityList.SelectedItem != null)
            {
                ZipList.Items.Clear();
                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT DISTINCT postal_code FROM businessTable WHERE city = '" + CityList.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "' ORDER BY postal_code;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ZipList.Items.Add(reader.GetString(0));
                            }
                        }
                    }

                    conn.Close();
                }
            }
        }

        private void ZipListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ZipList.SelectedItem != null)
            {
                CategoryList.Items.Clear();
                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT DISTINCT category_type " +
                                          "FROM categoriesTable, " +
                                          "(SELECT business_id FROM businessTable WHERE state = '" + StateList.SelectedItem.ToString() + "' AND city = '" + CityList.SelectedItem.ToString() + "' AND postal_code = " + ZipList.SelectedItem.ToString() + ") " +
                                          "AS temp " +
                                          "WHERE temp.business_id = categoriesTable.business_id;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoryList.Items.Add(reader.GetString(0));
                            }
                        }
                    }


                    BusinessGrid.Items.Clear();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * FROM businessTable WHERE city = '" + CityList.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "' AND postal_code = '" + ZipList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BusinessGrid.Items.Add(new Business(reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetDouble(8).ToString(), reader.GetString(9), reader.GetDouble(12).ToString(), reader.GetString(11)));
                            }
                        }
                    }


                    conn.Close();
                }
            }

        }

        private void CategoryListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryList.SelectedItem != null)
            {
                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    BusinessGrid.Items.Clear();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * " +
                                          "FROM categoriesTable, " +
                                          "(SELECT * FROM businessTable WHERE state = '" + StateList.SelectedItem.ToString() + "' AND city = '" + CityList.SelectedItem.ToString() + "' AND postal_code = " + ZipList.SelectedItem.ToString() + ") " +
                                          "AS temp " +
                                          "WHERE temp.business_id = categoriesTable.business_id AND categoriesTable.category_type = '" + CategoryList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BusinessGrid.Items.Add(new Business(reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetDouble(10).ToString(), reader.GetString(11), reader.GetDouble(14).ToString(), reader.GetString(13)));
                            }
                        }
                    }

                    conn.Close();
                }
            }
        }


        public void getUserIds(string userName)
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT user_id,name,average_stars,fans,yelping_since,funny,useful,cool FROM userTable WHERE name = '" + userName + "';";

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

        private void searchUserIdButton_Click(object sender, RoutedEventArgs e)
        {
            getUserIds(searchName.Text);

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
                            userFriendsList.Items.Add(new Friend(reader.GetString(0), reader.GetDouble(1).ToString(), reader.GetString(2)));
                        }
                    }
                }

                conn.Close();
            }
        }

        public void AddColumnsToFriendsGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Name";
            col1.Binding = new Binding("name");
            col1.Width = 140;
            userFriendsList.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Avg Stars";
            col2.Binding = new Binding("avgStar");
            col2.Width = 80;
            userFriendsList.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Yelping Since";
            col3.Binding = new Binding("yelpSince");
            col3.Width = 150;
            userFriendsList.Columns.Add(col3);
        }

        private void userIds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string id = userIds.SelectedValue.ToString();
           
            getUserInformation(id);

            userFriendsList.Items.Clear();
            getUserFriends(id);

            userFriendTipsGrid.Items.Clear();
            getUserFriendsReviews(id);
        }



        public void AddColumnsToFriendsTipsGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "User Name";
            col1.Binding = new Binding("name");
            col1.Width = 80;
            userFriendTipsGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Business";
            col2.Binding = new Binding("businessName");
            col2.Width = 140;
            userFriendTipsGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            col3.Width = 80;
            userFriendTipsGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Text";
            col4.Binding = new Binding("text");
            col4.Width = 270;
            userFriendTipsGrid.Columns.Add(col4);

        }

        public void getUserInformation(string userId)
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT user_id,name,average_stars,fans,yelping_since,funny,useful,cool FROM userTable WHERE user_id = '" + userId + "';";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userNameTextBox.Text = reader.GetString(1);
                            userStarsTextBox.Text = reader.GetDouble(2).ToString();
                            userFansTextBox.Text = reader.GetDouble(3).ToString();
                            userYelpSinceTextBox.Text = reader.GetString(4);
                            funnyTextBox.Text = reader.GetDouble(5).ToString();
                            coolTextBox.Text = reader.GetDouble(6).ToString();
                            usefulTextBox.Text = reader.GetDouble(7).ToString();

                        }
                    }
                }

                conn.Close();
            }
        }

        public void getUserFriendsReviews(string id)
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT temp.name, B.name as businessName, B.city, temp.text " +
                                       "FROM businessTable as B, (SELECT temp.name, R.user_id, R.business_id, R.text " +
                                                                 "FROM reviewTable as R, (SELECT temp.user_id, temp.name, temp.average_stars, temp.yelping_since " +
                                                                                         "FROM userTable as U, friendsTable as F, (SELECT * FROM userTable) as temp " +
                                                                                         "WHERE U.user_id = '" + id + "' AND U.user_id = F.user_id AND F.friend_id = temp.user_id) as temp " +
                                                                 "WHERE R.user_id = temp.user_id) as temp " +
                                       "WHERE B.business_id = temp.business_id " +
                                       "ORDER BY temp.name;";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userFriendTipsGrid.Items.Add(new FriendReview(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                        }
                    }
                }

                conn.Close();
            }
        }

    }


}
