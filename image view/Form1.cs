using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace image_view
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        MemoryStream memory = new MemoryStream();
        SqlConnection connection = new SqlConnection(@"data source = AANDROID-123122\SQLEXPRESS; initial catalog = img; integrated security = true");
        SqlConnection connectionpub = new SqlConnection(@"server =192.168.100.92;database = simpleDatabase ; user id = log1; password=P@ssword1965** ;MultipleActiveResultSets =True;");


        private void Form1_Load(object sender, EventArgs e)
        {
        //    SqlCommand command = new SqlCommand("select id from img", connection);
        //    connection.Open();
        //    SqlDataReader reader = command.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        comboBox1.Items.Add(reader[0].ToString());
        //    }
            

            
        //    reader.Close();
        //    connection.Close();



        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("select * from img", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            label2.Text = reader[0].ToString();
            byte[] b = (byte[])reader[1];
            //clear memry stream for another pic
            memory.SetLength(0);
            memory.Write(b, 0, b.Length);

            pictureBox1.Image = Bitmap.FromStream(memory);
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            pictureBox2.Image = Bitmap.FromFile(dialog.FileName);
            pictureBox2.Image.Save(memory, ImageFormat.Png);
            byte[] b = memory.ToArray();
            //Image b = pictureBox2.Image;
            connection.Open();
            SqlCommand command = new SqlCommand("insert into img (img)values ( @img )", connection);
          //  command.Parameters.AddWithValue("@id", 3);
            command.Parameters.Add("@img", SqlDbType.Image);
            command.Parameters["@img"].SqlValue = b;

            command.ExecuteNonQuery();

            connection.Close();
            MessageBox.Show("image add");

            comboBox1.Items.Clear();
            SqlCommand command2 = new SqlCommand("select id from img", connection);
            connection.Open();
            SqlDataReader reader = command2.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0].ToString());
            }
            reader.Close();
            connection.Close();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            SqlCommand command = new SqlCommand("select img from img where id = "+ comboBox1.SelectedItem.ToString()+"", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            byte[] b = (byte[])reader[0];
            //clear memry stream for another pic
            memory.SetLength(0);
            memory.Write(b, 0, b.Length);
            label2.Text = comboBox1.SelectedItem.ToString();
            
            pictureBox1.InitialImage = null;
            pictureBox1.Image = Bitmap.FromStream(memory);
            reader.Close();
            connection.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            pictureBox2.Image = Bitmap.FromFile(dialog.FileName);
            pictureBox2.Image.Save(memory, ImageFormat.Png);
            byte[] b = memory.ToArray();
            //Image b = pictureBox2.Image;
            connectionpub.Open();
            SqlCommand command = new SqlCommand("insert into Sigle (Sigle,Dénomination,Parti)values(@Sigle,@Dénomination, @Parti)", connectionpub);

            command.Parameters.AddWithValue("@Sigle",textBox1.Text);
            command.Parameters.AddWithValue("@Dénomination",b);
            //command.Parameters.Add("@Dénomination", SqlDbType.Image);
            //command.Parameters["@Dénomination"].SqlValue = b;
            command.Parameters.AddWithValue("@Parti", textBox2.Text);


            command.ExecuteNonQuery();

            connectionpub.Close();
            MessageBox.Show("image add");

            comboBox1.Items.Clear();
            //SqlCommand command2 = new SqlCommand("select id from img", connection);
            //connection.Open();
            //SqlDataReader reader = command2.ExecuteReader();
            //while (reader.Read())
            //{
            //    comboBox1.Items.Add(reader[0].ToString());
            //}
            //reader.Close();
            //connection.Close();
        }
    }
}
