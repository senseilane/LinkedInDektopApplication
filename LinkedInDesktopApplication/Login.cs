using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace LinkedInDesktopApplication
{
    public partial class Login : Form
    {
        public static string name;
        public Login()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Read the data from the Users database and compare the passwords.
            string storedPassword = "";
            string userName = txtUsername.Text;
            string thisPassword = txtPassword.Text;
            string constring = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\sense\OneDrive\Documents\Volstate\Capstone\Group Project\LinkedInDesktopApplication\LinkedInDesktopApplication\ProjectDatabase.mdf"";Integrated Security=True";
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string selectString = "SELECT Password, Name FROM Users WHERE Username= '" + userName + "'";
            SqlCommand cmd = new SqlCommand(selectString, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                storedPassword = reader[0].ToString();
                name = reader[1].ToString();
            }
            if (userName == "username...")
            {
                lblError.Text = "Please enter your username.";
                txtUsername.Focus();
            }
            else if(thisPassword == "password...")
            {
                lblError.Text = "Please enter your password.";
                txtPassword.Focus();
            }
            else if(thisPassword != storedPassword)
            {
                lblError.Text = "Invalid username or password.";
            }
            else
            {
                PostsPage postsPage = new PostsPage();
                postsPage.Show();
                this.Close();
                con.Close();
            }
        }
        //This code draws a custom border around the form.
        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void userEnter(object sender, EventArgs e)
        {
            if(txtUsername.Text == "username...")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }

        private void userLeave(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                txtUsername.Text = "username...";
                txtUsername.ForeColor = Color.Silver;
            }
        }

        private void passEnter(object sender, EventArgs e)
        {
            if(txtPassword.Text == "password...")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                txtPassword.UseSystemPasswordChar = true; //this code blocks the password with bullets.
            }
        }

        private void passLeave(object sender, EventArgs e)
        {
            if(txtPassword.Text == "")
            {
                txtPassword.UseSystemPasswordChar = false;
                txtPassword.Text = "password...";
                txtPassword.ForeColor = Color.Silver;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            startForm start = new startForm();
            this.Hide();
            start.Show();
        }
    }
}
