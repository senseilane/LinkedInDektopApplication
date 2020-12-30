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
    public partial class PostsPage : Form
    {
        static string post, idString;
        static int id;
        string currentTime = DateTime.Now.ToString();
        static string constring = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\sense\OneDrive\Documents\Volstate\Capstone\Group Project\LinkedInDesktopApplication\LinkedInDesktopApplication\ProjectDatabase.mdf"";Integrated Security=True";
        static SqlConnection con = new SqlConnection(constring);
        static SqlCommand cmd = new SqlCommand("SELECT * FROM Posts", con);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();


        public PostsPage()
        {
            InitializeComponent();
            BindGrid();
            //Disable sorting for the datagridview
            foreach (DataGridViewColumn dataGrid in dataGridView1.Columns)
            {
                dataGrid.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //Formats the first to columns to be center aligned.
            this.dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //Sets the datagridview columns to read only.
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
        }
        void BindGrid()
        {
            con.Open();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            //This wraps the text to fit the cell width making new lines.
            this.dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            con.Close();
        }
        void SaveData()
        {
            //This updates the last post in the database.
            con.Open();
            SqlCommand savePost = new SqlCommand("UPDATE Posts SET Post = @post WHERE Id = '" + id + "'", con);
            savePost.Parameters.Add("@post", SqlDbType.NVarChar).Value = txtPost.Text;
            savePost.ExecuteNonQuery();
            con.Close();
            //sda.Update(dt);
            dt.Clear();
            sda.Fill(dt);

            txtPost.Text = "";
            //Scrolls to the bottom of the grid.
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;

            MessageBox.Show("Edit has been saved.");
        }
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            SaveData();
            btnSaveChanges.Enabled = false;
            btnCancel.Enabled = false;
            btnEdit.Enabled = true;
            btnPost.Enabled = true;
            dataGridView1.Columns[3].ReadOnly = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //dataGridView1.Columns[3].ReadOnly = false;
            btnEdit.Enabled = false;
            btnCancel.Enabled = true;
            btnSaveChanges.Enabled = true;
            btnPost.Enabled = false;

            //Retrieves the id of the last record in the database.
            con.Open();
            SqlCommand edit = new SqlCommand("SELECT TOP 1 * FROM Posts ORDER BY ID DESC", con);
            SqlDataReader editReader = edit.ExecuteReader();
            while (editReader.Read())
            {
                idString = editReader[0].ToString();
                id = Convert.ToInt32(idString);
            }
            con.Close();
            con.Open();

            //Retrieving the post with the id that was just retrieved.
            SqlCommand getPost = new SqlCommand("SELECT Post from Posts WHERE Id = '" + id + "'", con);
            SqlDataReader retrieveReader = getPost.ExecuteReader();
            while (retrieveReader.Read())
            {
                post = retrieveReader[0].ToString();
            }
            con.Close();
            txtPost.Text = post;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            //This clears the unused data and refills the datagridview.
            dt.Clear();
            sda.Fill(dt);

            //Scrolls to the bottom of the grid.
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;

            //This sets the colum back to read only mode and disables cancel and save button while enabling the edit button.
            dataGridView1.Columns[3].ReadOnly = true;
            btnEdit.Enabled = true;
            btnCancel.Enabled = false;
            btnSaveChanges.Enabled = false;
            btnPost.Enabled = true;
            txtPost.Text = "";
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            if(txtPost.Text == "")
            {
                lblError.Visible = true;
            }
            else
            {
                con.Open();
                SqlCommand newCMD = new SqlCommand("INSERT INTO Posts (Date, Name, Post) VALUES (@Date, @Name, @Post)", con);
                newCMD.Parameters.Add("@Date", SqlDbType.SmallDateTime).Value = currentTime;
                newCMD.Parameters.Add("@Name", SqlDbType.NChar).Value = Login.name;
                newCMD.Parameters.Add("@Post", SqlDbType.NVarChar).Value = txtPost.Text;
                newCMD.ExecuteNonQuery();
                con.Close();

                //Clear the data and refill it
                dt.Clear();
                sda.Fill(dt);
                
                //Scrolls to the bottom of the grid.
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
                txtPost.Text = "";
                txtPost.Focus();
                lblError.Visible = false;
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            startForm start = new startForm();
            start.Show();
            this.Close();
        }

        //Had to double click on the form title bar in design view to auto generate the page load.
        private void PostsPage_Load(object sender, EventArgs e)
        {
            //This code auto scrolls to the bottom of datagridview on page load.
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;

            //This clears the selected cell on page load.
            dataGridView1.FirstDisplayedCell = null;
            dataGridView1.ClearSelection();
        }
        //This code draws a custom border around the form.
        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }
    }
}
