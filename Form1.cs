using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace condb
{
    
    public partial class Form1 : Form
    {
        static string conString = "server=localhost; port=3306;database=test;Uid=root;pwd=;";

        MySqlConnection con = new MySqlConnection(conString);
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        DataTable dt = new DataTable();


        public Form1()
        {
            InitializeComponent();

            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "id";
            dataGridView1.Columns[1].Name = "name";
            dataGridView1.Columns[2].Name = "position";
            dataGridView1.Columns[3].Name = "team";


            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //selection 
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
        }
        private void add(string name, string pos, string team)
        {
            string sql = "INSERT INTO student(name,position,team) VALUES(@name,@POSITION,@TEAM)";
            cmd = new MySqlCommand(sql, con);

            //add parameters
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@POSITION", pos);
            cmd.Parameters.AddWithValue("@TEAM", team);

            try
            {
                con.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    cleartxt();
                    MessageBox.Show("successfully insert");

                }
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
                retrive();
            }
            }

        //add to dg view
        private void populate(String id,String name,string pos,string team)
        {
            dataGridView1.Rows.Add(id, name, pos, team);
        }
        

        private void retrive()
        {
            dataGridView1.Rows.Clear();
            string sql = "SELECT * FROM student";
            cmd = new MySqlCommand(sql, con);
            try
            {
                con.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    populate(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString());
                }
                con.Close();

                dt.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //update db
        private void update(int id, string name, string pos, string team)
        {

            string sql = "UPDATE student SET name='" + name + "',position='" + pos + "',team='" + team + "'WHERE id=" + id +"";
            cmd = new MySqlCommand(sql, con);

            //open con,Upadte

            try
            {
                con.Open();

                adapter = new MySqlDataAdapter(cmd);
                adapter.UpdateCommand = con.CreateCommand();
                adapter.UpdateCommand.CommandText = sql;

                if (adapter.UpdateCommand.ExecuteNonQuery() > 0)
                {
                    cleartxt();
                    MessageBox.Show("succefully update");
                }
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
            

        }

        //delete
        private void delete(int id)
        {
            string sql = "DELETE from student WHERE id=" + id + "";

            cmd = new MySqlCommand(sql, con);

            //open con,execute
            try
            {
                con.Open();

                adapter = new MySqlDataAdapter(cmd);
                adapter.DeleteCommand = con.CreateCommand();
                adapter.DeleteCommand.CommandText = sql;


                if (MessageBox.Show("Are you Sure ??", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        cleartxt();
                        MessageBox.Show("sucssesfuly deleted");
                    }
                }
                con.Close();
                retrive();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }
        private void cleartxt()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {

            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();

        }

        private void addbut_Click(object sender, EventArgs e)
        {
            add(textBox1.Text, textBox2.Text, textBox3.Text);
        }

        private void ret_Click(object sender, EventArgs e)
        {
            retrive();
        }

        private void upd_Click(object sender, EventArgs e)
        {
            string selected = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);
            update(id, textBox1.Text, textBox2.Text, textBox3.Text);
        }

        private void delet_Click(object sender, EventArgs e)
        {
            string selected = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);

            delete(id);
        }

        private void cler_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
         
       

        }

        
   }

