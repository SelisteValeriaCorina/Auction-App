using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class Form2 : Form
    {
        List<Bidder> bidders;
        Selection selected;

       /// <summary>
       /// //toate produsele pentru care s-a concurat la ziua respectiva, vor fii valabile incepand din ziua curenta(si se va selecta data) pana-n urmatoarele 3 zile ; 
       /// </summary>
       /// <param name="bidder"></param>


        public Form2(List<Bidder> bidder)
        {
            bidders = new List<Bidder>();
            InitializeComponent();

            /*Cand se initializeaza forma cream o selectie ce alege cei mai buni participanti.*/
            Selection selection = new Selection(bidder);
            /*bidders(atribut al formei 2) primeste castigatorii decisi de Selection selection*/
            bidders = selection.GetWinners();
        }

       void DisplayProducts()
        {
            listviewProducts.Items.Clear();

            foreach(var bidder in bidders)
            {
                var listViewitem = new ListViewItem(bidder.LastName);
                //listViewitem.SubItems.Add(bidder.FirstName);
                listViewitem.SubItems.Add(bidder.Amount.ToString());
                listViewitem.SubItems.Add(bidder.Produs);


                listViewitem.Tag = bidder;

                listviewProducts.Items.Add(listViewitem);

            }

        }


        //private void btnPrint_Click(object sender, EventArgs e)
        //{
        //    if (printPrevDialog.ShowDialog() == DialogResult.OK)
        //        printDocument1.Print();
        //}

        private void Form2_Load(object sender, EventArgs e)
        {
            /*Load se apeleaza cand este afisata forma. Tot ce avem de facut la form2 este sa afisam castigatorii*/
            DisplayProducts();
        }

        //private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        //{
        //    e.Graphics.DrawString(listviewProducts.Items[0].SubItems[0].Text+"      "
        //       +listviewProducts.Items[0].SubItems[1].Text+"      "+
        //       listviewProducts.Items[])
        //}
    }
}
