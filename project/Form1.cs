using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace project
{
    public partial class Form1 : Form
    {

        #region Attributes

        private const string  ConnectionString = "Data Source=proiectfinal.db";
        List<Bidder> bidder;
        private object listViewItem;
        BarChartValue[] vector;
        BarcharUserControl1 stat = new BarcharUserControl1();
        //set atribute pt grafic
        Dictionary<String, int> produse = new Dictionary<string, int>();
       
       
        #endregion

        public Form1()
        {
            InitializeComponent();
            bidder = new List<Bidder>();//punem () deoarece se implementeaza constructorul implicit pt obiectul bidders;
        }

        private void tbName_Validating(object sender, CancelEventArgs e)
        {
            string lastname = tbName.Text.Trim();
            if (lastname.Length < 2)
            {
                e.Cancel = true;
                erpLastName.SetError(tbName, ">=2 letters");
            }
        }
        private void tbProduct_Validating(object sender, CancelEventArgs e)
        {
            string Product = tbProduct.Text.Trim();
            if(Product.Length<4)
            {
                e.Cancel = true;
                erpProducts.SetError(tbProduct, ">= 4 letters");
            }

        }

        private void tbProduct_Validated(object sender, EventArgs e)
        {
            erpProducts.Clear();
        }

        private void tbName_Validated(object sender, EventArgs e)
        {
            erpLastName.Clear();
        }

        private void tbPrename_Validating(object sender, CancelEventArgs e)
        {
            string prename = tbPrename.Text.Trim();
            if (prename.Length <= 2)
            {
                e.Cancel = true;
                erpFirstName.SetError(tbPrename, "=> letters");
            }
        }

        private void tbPrename_Validated(object sender, EventArgs e)
        {
            erpFirstName.Clear();
        }

        private void tbAmount_Validating(object sender, CancelEventArgs e)
        {
            var amount = tbAmount.Text;
            //Parsing to float
            float amountFloat = float.Parse(amount);

            //Comparison
            if (amountFloat < 50.0f)
            {
                e.Cancel = true;
                erpAmount.SetError(tbAmount, "The amount must always be over 50!");
            }

        }
        private void tbAmount_Validated(object sender, EventArgs e)
        {
            erpAmount.Clear();
        }

        void Displaybidders()
        {
            lvBidders.Items.Clear();
            foreach (var bidder in bidder)
            {
                var listViewItem = new ListViewItem(bidder.LastName);

                //listViewItem.SubItems.Add(bidder.LastName);
                listViewItem.SubItems.Add(bidder.FirstName);
                listViewItem.SubItems.Add(bidder.Amount.ToString());
                listViewItem.SubItems.Add(bidder.Produs);

                listViewItem.Tag = bidder;

                lvBidders.Items.Add(listViewItem);

            }
        }
        public void AddParticipant(Bidder bidder)
        {
            var queryString = "insert into Bidder(LastName, FirstName, Amount, Produs)" +
              " values(@LastName,@FirstName,@Amount,@Produs);";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand(queryString, connection);
                var lastNameParameter = new SQLiteParameter("@LastName");
                lastNameParameter.Value = bidder.LastName;
                var firstNameParameter = new SQLiteParameter("@FirstName");
                firstNameParameter.Value = bidder.FirstName;
                var value = new SQLiteParameter("@Amount");
                value.Value = bidder.Amount;
                var produs = new SQLiteParameter("@Produs");
                produs.Value = bidder.Produs;

                command.Parameters.Add(lastNameParameter);
                command.Parameters.Add(firstNameParameter);
                command.Parameters.Add(value);
                command.Parameters.Add(produs);

               command.ExecuteScalar();//aici este o eroare grava
            }
        }
        private void btnJoin_Click(object sender, EventArgs e)
        {
          
            var lastname = tbName.Text.Trim();
            var firstname = tbPrename.Text.Trim();
            var value = tbAmount.Text.Trim();
            float valueFloat = float.Parse(value);
            var prod = tbProduct.Text.Trim();

            bool valid = true;
            if (lastname.Length < 2)
            {
                valid = false;
                erpLastName.SetError(tbName, ">=2 letters");
            }
            if (firstname.Length < 2)
            {
                valid = false;
                erpFirstName.SetError(tbPrename, ">=2 letters");
            }
            if (valueFloat <= 50.0f)
            {
                valid = false;
                erpAmount.SetError(tbAmount, ">50  lei");
            }
            if(prod.Length<4)
            {
                valid = false;
                erpProducts.SetError(tbProduct, ">=4 letters");
            }

            if (!valid)
            {
                MessageBox.Show("The form contains errors!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Bidder bidder;
                try
                {
                    bidder = new Bidder(lastname, firstname, valueFloat, prod);
                    this.bidder.Add(bidder);
                    Displaybidders();
                    tbName.Text = tbPrename.Text = tbAmount.Text = tbProduct.Text= string.Empty;

                    AddParticipant(bidder);


                    //dupa ce am creat bidderul si l-am adaugat in baza de date ,vom memora produsul
                    if(produse.ContainsKey(prod))
                    {
                        produse[prod]++;
                    }
                    else
                    {
                        produse.Add(prod,1);
                    }

                }
                catch (ArgumentException ec)
                {
                    MessageBox.Show(ec.Message);
                }
            }
        }

        public void DeleteParticipant(Bidder participant)
        {
            const string stringSql = "DELETE FROM Bidder WHERE LastName=@LastName";
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(stringSql, connection);

                var nameParameter = new SQLiteParameter("@LastName");
                nameParameter.Value = participant.LastName;
                command.Parameters.Add(nameParameter);

                command.ExecuteNonQuery();

                bidder.Remove(participant);
                Displaybidders();
            }

        }

        private void btnAbandon_Click(object sender, EventArgs e)
        {
            if (lvBidders.SelectedItems.Count == 0)
            {
                MessageBox.Show("Choose an offer!");

            }
            else
            {
                if (MessageBox.Show("Are you sure?", "Abandon the offer?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    var listViewitem = lvBidders.SelectedItems[0];
                    Bidder bidder = (Bidder)listViewitem.Tag;

                    this.bidder.Remove(bidder);
                    Displaybidders();
                    DeleteParticipant(bidder);
                }
            }
        } 

        private void btnChange_Click(object sender, EventArgs e) 
        {
            if (lvBidders.SelectedItems.Count == 0)
            {
                MessageBox.Show("Choose a participant");
            }
            else
            {
                var listViewItem = lvBidders.SelectedItems[0];
                Bidder bidder = (Bidder)listViewItem.Tag;

                EditForm editForm = new EditForm(bidder);
                if (editForm.ShowDialog() == DialogResult.OK)
                    Displaybidders();
            }

        }

        private void btnserializationbinary_Click(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.Create("serialize.bin"))
            {
                formatter.Serialize(stream, bidder);
            }
        }

        private void deserializationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream=File.OpenRead("serialize.bin"))
            {
                bidder = (List<Bidder>)formatter.Deserialize(stream);
                Displaybidders();
            }
        }

        private void serializationXml_Click(object sender, EventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Bidder>));
            using (FileStream stream=File.Create("serializer.xml"))
            {
                serializer.Serialize(stream, bidder);
            }
        }

        private void deserializationXml_Click(object sender, EventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Bidder>));
            using (FileStream stream = File.OpenRead("serializer.xml"))
            {
                bidder = (List<Bidder>)serializer.Deserialize(stream);
                Displaybidders();
            }
        }

        private void exportText_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Export CSV";
            saveFileDialog.Filter = "csv|*.csv";

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var sw = new StreamWriter(saveFileDialog.FileName))
                {
                    sw.WriteLine("Lastname,Firstname,Value");
                    foreach(var bidd in bidder)
                    {
                        sw.WriteLine("{0},{1},{2}",
                        bidd.LastName,
                        bidd.FirstName,
                        bidd.Amount.ToString(),
                        bidd.Produs);//aici am adaugat
                    }
                }
            }
        }

        public void LoadParticipants()
        {
            const string stringSql = "SELECT * FROM Bidder";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command=new SQLiteCommand(stringSql, connection);
                SQLiteDataReader sqlReader = command.ExecuteReader();
                try
                {
                    while (sqlReader.Read())//luam rezultatul comenzii din obiectul sqlReader si convertim campurile sale intr-un obiect de tip bidder;
                    {
                        bidder.Add(new Bidder((string)sqlReader["LastName"], (string)sqlReader["FirstName"], (float)(double)sqlReader["Amount"], (string)sqlReader["Produs"]));
                        if (produse.ContainsKey((string)sqlReader["Produs"]))
                        {
                            produse[(string)sqlReader["Produs"]]++;
                        }
                        else
                        {
                            produse.Add((string)sqlReader["Produs"], 1);
                        }
                    }
                    //AICI AM ADAUGAT
                }
                finally
                {
                    sqlReader.Close();
                }

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            try
            {
                LoadParticipants();
                Displaybidders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //if (lvBidders.SelectedItems.Count == 0)
            //{
            //    MessageBox.Show("Choose a participant");
            //}
            //else
            //{
            //    var listViewItem = lvBidders.SelectedItems[0];
            //    Bidder bidder = (Bidder)listViewItem.Tag;

            //    Form2 editForm = new Form2 (bidder);
            //    if (editForm.ShowDialog() == DialogResult.OK)
            //        Displaybidders();
            //}

            /*Ca sa nu mai fie nevoie sa selectam un participant
             Doar declaram o forma de tip form2 si su Show() o afisam*/
            Form2 winnerForm = new Form2(bidder);
            winnerForm.Show();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                printDocument1.Print();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(tbName.Text.Trim(), new Font("Times New Roman", 14, FontStyle.Bold), Brushes.Black, new Point(100, 100));
            e.Graphics.DrawString(tbPrename.Text.Trim(), new Font("Times New Roman", 14, FontStyle.Bold), Brushes.Black, new Point(100, 200));
            e.Graphics.DrawString(tbAmount.Text.Trim(), new Font("Times New Roman", 14, FontStyle.Bold), Brushes.Black, new Point(100, 300));
            e.Graphics.DrawString(tbProduct.Text.Trim(), new Font("Times New Roman", 14, FontStyle.Bold), Brushes.Black, new Point(100, 400));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control==true && e.KeyCode==Keys.K)
            {
                btnChange.PerformClick();
            }

            if (e.Control == true && e.KeyCode == Keys.J)
            {
                btnJoin.PerformClick();
            }
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                btnAbandon.PerformClick();
            }

        }

        private void BarcharUserControl1_Load(object sender, EventArgs e)
        {

        }

        private void btngrafic_Click(object sender, EventArgs e)
        {
            vector = new BarChartValue[produse.Count];
            int i = 0;
            foreach(var produs in produse)
            {
                BarChartValue c = new BarChartValue(produs.Key,produs.Value,"Blue");
                vector[i] = c;
                i++;
            }

            stats.setData(vector);
            stats.Refresh();
            this.Refresh();
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
