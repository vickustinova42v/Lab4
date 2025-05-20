using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab4
{
    public partial class Form1 : Form
    {
        public string filledChart;
        public Form1()
        {
            InitializeComponent();
            chart1.Series.Clear();
            chart2.Series.Clear();
        }

        private void calculateClick(object sender, EventArgs e)
        {
            if (filledChart == "" || filledChart == "chart2") 
            {
                chart1.Series.Clear();
                chart1.Series.Add("Y");
                chart1.Series.Add("Z");
                chart1.Series["Y"].ChartType = SeriesChartType.Line;
                chart1.Series["Z"].ChartType = SeriesChartType.Line;
                chart1.ChartAreas[0].AxisY.Minimum = Double.NaN; // Авто-масштаб
                chart1.ChartAreas[0].AxisY.Maximum = Double.NaN; // Авто-масштаб
                filledChart = "chart1";
            } else
            {
                chart2.Series.Clear();
                chart2.Series.Add("Y");
                chart2.Series.Add("Z");
                chart2.Series["Y"].ChartType = SeriesChartType.Line;
                chart2.Series["Z"].ChartType = SeriesChartType.Line;
                chart2.ChartAreas[0].AxisY.Minimum = Double.NaN; // Авто-масштаб
                chart2.ChartAreas[0].AxisY.Maximum = Double.NaN; // Авто-масштаб
                filledChart = "chart2";
            }

            string x0String = this.x0.Text;
            string xEndString = this.xEnd.Text;
            string y0String = this.y0.Text;
            string z0String = this.z0.Text;
            string stepsString = this.steps.Text;

            double x0 = double.Parse(x0String);
            double xEnd = double.Parse(xEndString);
            double y0 = double.Parse(y0String);
            double z0 = double.Parse(z0String);
            int steps = int.Parse(stepsString);

            chart1.ChartAreas[0].AxisX.Minimum = x0;
            chart1.ChartAreas[0].AxisX.Maximum = xEnd;
            chart2.ChartAreas[0].AxisX.Minimum = x0;
            chart2.ChartAreas[0].AxisX.Maximum = xEnd;

            double h = (xEnd - x0) / steps;//шаг интергирования
            double x = x0;
            double y = y0;
            double z = z0;

            for (int i = 0; i <= steps; i++) {
                if (filledChart == "chart1")
                {
                    chart1.Series["Y"].Points.AddXY(x, y);
                    chart1.Series["Z"].Points.AddXY(x, z);
                } else
                {
                    chart2.Series["Y"].Points.AddXY(x, y);
                    chart2.Series["Z"].Points.AddXY(x, z);
                }

                double k1_y = h * f1(x, y, z);
                double k1_z = h * f2(x, y, z);

                double k2_y = h * f1(x + h / 2, y + k1_y / 2, z + k1_z / 2);
                double k2_z = h * f2(x + h / 2, y + k1_y / 2, z + k1_z / 2);

                double k3_y = h * f1(x + h / 2, y + k2_y / 2, z + k2_z / 2);
                double k3_z = h * f2(x + h / 2, y + k2_y / 2, z + k2_z / 2);

                double k4_y = h * f1(x + h, y + k3_y, z + k3_z);
                double k4_z = h * f2(x + h, y + k3_y, z + k3_z);

                y += (k1_y + 2 * k2_y + 2 * k3_y + k4_y) / 6;
                z += (k1_z + 2 * k2_z + 2 * k3_z + k4_z) / 6;
                x += h;
            }
        }

        // Пример функции: dy/dx = z
        private double f1(double x, double y, double z)
        {
            return (x * x - z) * y;
        }

        // Пример функции: dz/dx = -y
        private double f2(double x, double y, double z)
        {
            return (x + y * y) * Math.Cos(z);
        }
    }
}
