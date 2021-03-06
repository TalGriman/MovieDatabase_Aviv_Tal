using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieFile
{
    public partial class Form1 : Form
    {
        private string queryString = @"Data Source=DESKTOP-FTT4EST\SQLEXPRESS;Initial Catalog=ATMovies;Integrated Security=True";
        SqlConnection con;
        SqlCommand cmd;
        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(queryString);
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pnlInsertMovie.BringToFront();
            RenderGrid();
        }
        private void RenderGrid()
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SelectMoviesTable", con); // set of data commands and a database connection, used to fill the datatable.
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dataTable = new DataTable(); // represent one table
            sqlDataAdapter.Fill(dataTable); // update the datatable that shown on the delete page
            dgvMovies.DataSource = dataTable;
        }

        private void ExecuteNonQuery()
        {
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void btnInsertMovie_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMovieIdInsert.Text.Equals("") || txtMovieTitleInsert.Text.Equals("") || cbCategoryInsert.Text.Equals(""))
                {
                    MessageBox.Show("All the fields must to be filled.");
                    return;
                }
                cmd.CommandText = "AddMovie";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@id", SqlDbType.NVarChar, 150).Value = txtMovieIdInsert.Text;
                cmd.Parameters.Add("@title", SqlDbType.NVarChar, 150).Value = txtMovieTitleInsert.Text;
                cmd.Parameters.Add("@category", SqlDbType.NVarChar, 150).Value = cbCategoryInsert.Text;
                cmd.Parameters.Add("@release_date", SqlDbType.NVarChar, 150).Value = dtpMovieReleasedInsert.Value.ToShortDateString();
                ExecuteNonQuery();
                txtMovieIdInsert.Clear();
                txtMovieTitleInsert.Clear();
                dtpMovieReleasedInsert.Value = DateTime.Now;
                cbCategoryInsert.Text = "";
                RenderGrid();
                MessageBox.Show("The movie has been added!");
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdateMovie_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMovieIdUpdate.Text.Equals("") || txtMovieTitleUpdate.Text.Equals("") || cbCategoryUpdate.Text.Equals(""))
                {
                    MessageBox.Show("All the fields must to be filled.");
                    return;
                }
                cmd.CommandText = "UpdateMovieDetails";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@id", SqlDbType.NVarChar, 150).Value = txtMovieIdUpdate.Text;
                cmd.Parameters.Add("@title", SqlDbType.NVarChar, 150).Value = txtMovieTitleUpdate.Text;
                cmd.Parameters.Add("@category", SqlDbType.NVarChar, 150).Value = cbCategoryUpdate.Text;
                cmd.Parameters.Add("@release_date", SqlDbType.NVarChar, 150).Value = dtpMovieReleasedUpdate.Value.ToShortDateString();
                ExecuteNonQuery();
                RenderGrid();
                MessageBox.Show("The movie has been updated!");
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteMovie_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMovieIdDelete.Text.Equals("") || txtMovieTitleDelete.Text.Equals("") || cbCategoryDelete.Text.Equals(""))
                {
                    MessageBox.Show("All the fields must to be filled.");
                    return;
                }
                DialogResult result;
                //Message box with yes / no 
                result = MessageBox.Show($"Are you sure that you want to delete Movie ID: {txtMovieIdDelete.Text}", "Delete",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (result.Equals(DialogResult.Yes))
                {
                    cmd.CommandText = "DeleteMovie";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@id", SqlDbType.NVarChar, 150).Value = txtMovieIdUpdate.Text;
                    ExecuteNonQuery();
                    RenderGrid();
                    MessageBox.Show("Delete succeed");
                }
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show(ex.Message);
            }
        }


        private void dgvMovies_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvMovies.SelectedRows)
            {
                txtMovieIdUpdate.Text = row.Cells[0].Value.ToString();
                txtMovieTitleUpdate.Text = row.Cells[1].Value.ToString();
                txtMovieIdDelete.Text = row.Cells[0].Value.ToString();
                txtMovieTitleDelete.Text = row.Cells[1].Value.ToString();
                string category = row.Cells[2].Value.ToString();
                if (cbCategoryUpdate.Items[0].ToString().Equals(category))
                {
                    cbCategoryUpdate.SelectedItem = cbCategoryUpdate.Items[0];
                }
                if (cbCategoryUpdate.Items[1].ToString().Equals(category))
                {
                    cbCategoryUpdate.SelectedItem = cbCategoryUpdate.Items[1];
                }
                if (cbCategoryUpdate.Items[2].ToString().Equals(category))
                {
                    cbCategoryUpdate.SelectedItem = cbCategoryUpdate.Items[2];
                }

                if (cbCategoryDelete.Items[0].ToString().Equals(category))
                {
                    cbCategoryDelete.SelectedItem = cbCategoryUpdate.Items[0];
                }
                if (cbCategoryDelete.Items[1].ToString().Equals(category))
                {
                    cbCategoryDelete.SelectedItem = cbCategoryUpdate.Items[1];
                }
                if (cbCategoryDelete.Items[2].ToString().Equals(category))
                {
                    cbCategoryDelete.SelectedItem = cbCategoryUpdate.Items[2];
                }

                string date = row.Cells[3].Value.ToString();
                string[] dateArray = date.Split('/');
                dtpMovieReleasedUpdate.Value = new DateTime(int.Parse(dateArray[2]), int.Parse(dateArray[1]), int.Parse(dateArray[0]));
                dtpMovieReleasedDelete.Value = new DateTime(int.Parse(dateArray[2]), int.Parse(dateArray[1]), int.Parse(dateArray[0]));

            }
        }

        private void btnInsertMovieView_Click(object sender, EventArgs e)
        {
            pnlInsertMovie.BringToFront();
        }

        private void btnUpdateMovieView_Click(object sender, EventArgs e)
        {
            pnlUpdateMovie.BringToFront();
        }
        private void btnDeleteMovieView_Click(object sender, EventArgs e)
        {
            pnlDeleteMovie.BringToFront();
        }

    }
}
