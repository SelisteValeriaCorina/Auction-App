using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class EditForm : Form
    {
        Bidder bidder;
        private const string ConnectionString = "Data Source=proiectfinal.db";

        public EditForm(Bidder bidder)
        {
            this.bidder = bidder;
            InitializeComponent();
        }

        private void EditForm_Load(object sender, EventArgs e)
        {
            tbLastName.Text = bidder.LastName;
            tbFirstName.Text = bidder.FirstName;
            tbAmount.Text = bidder.Amount.ToString();
        }

        private void ChangeBidder (string LastName, string FirstName, float Amount)
        { 
            /*Ca sa modificam un record trebuie dati toti parametrii, chiar daca nu toti se modifica*/
            var queryString = "UPDATE Bidder " +
                "SET LastName=@LastName, FirstName=@FirstName, Amount=@Amount, Produs=@Produs " +
                "WHERE LastName=@OriginalLastName;";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand(queryString, connection);

                var lastNameParameter = new SQLiteParameter("@LastName");
                var fisrtNameParameter = new SQLiteParameter("@FirstName");
                var amountParameter = new SQLiteParameter("@Amount");
                var produsParameter = new SQLiteParameter("@Produs");
                var originalNameParameter = new SQLiteParameter("@OriginalLastName");

                lastNameParameter.Value = LastName;
                fisrtNameParameter.Value = FirstName;
                amountParameter.Value = Amount;
                produsParameter.Value = bidder.Produs;
                originalNameParameter.Value = bidder.LastName;

                command.Parameters.Add(lastNameParameter);
                command.Parameters.Add(fisrtNameParameter);
                command.Parameters.Add(amountParameter);
                command.Parameters.Add(produsParameter);
                command.Parameters.Add(originalNameParameter);

                command.ExecuteScalar();

                connection.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ChangeBidder(tbLastName.Text, tbFirstName.Text, float.Parse(tbAmount.Text));

            bidder.LastName = tbLastName.Text;
            bidder.FirstName = tbFirstName.Text;
            bidder.Amount = float.Parse(tbAmount.Text);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tbLastName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
