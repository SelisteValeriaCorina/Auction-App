using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class BarcharUserControl1 : UserControl
    {



        private BarChartValue[] Data;

        public BarChartValue[] getData ()
        {
            return Data;
        }

        public void setData(BarChartValue[] vector)
        {
            Data = vector;
        }

        public BarcharUserControl1()
        {
            InitializeComponent();
            ResizeRedraw = true;

            Data = new BarChartValue[]
            {
                new BarChartValue("hmm",0,"red")
            };
        }

        public BarcharUserControl1(BarChartValue[] vector)
        {
            InitializeComponent();
            ResizeRedraw = true;
            Data = vector;
           
        }

        

        private void BarcharUserControl1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;//cu ce o sa desenam user controlul
            Rectangle rectangle = e.ClipRectangle;//toata suprafata dreptunghiulara a user controlului

            var maxBarHeigth = rectangle.Height * 0.9;
            var barWidth = rectangle.Width / Data.Length;

            var Scale = maxBarHeigth / Data.Max(x => x.value);//lambda expression
            for (int index = 0; index < Data.Length; index++)
            {
                var barheight = Data.ElementAt(index).value * Scale;
                Brush brush = new SolidBrush(Color.FromName(Data[index].Color));
                
                graphics.FillRectangle(brush, index * barWidth, (float)(rectangle.Height - barheight), barWidth * 0.95f, (float)barheight);//desenarea incepe din stanga sus, si continua dreapta si stanga jos
            }
        }
        private void BarcharUserControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
