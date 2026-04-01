using CsvHelper;
using CsvHelper.Configuration;
using FftSharp;
using IronXL;
using MathNet.Numerics; 
using ScottPlot;
using ScottPlot.Colormaps;
using ScottPlot.MultiplotLayouts;
using ScottPlot.WinForms;
using System;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Text;

using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;

using System.Globalization;


namespace WinFormsApp5
{
    public partial class Clarity : Form
    {

        double[] healthy_Magnitude;
        double[] Healthy_Power;
        double[] Healthy_freq;

        /// Declaration of variables needed for diagnosis functions throughout code 
        /// HI and Low gear mesh freq
        Double[] InSG_GFH;
        Double[] InSG_GFL;

        Double[] WmSG_GFH;
        Double[] WmSG_GFL;

        Double[] DsLSG_GFH;
        Double[] DsLSG_GFL;

        Double[] DsSSG_GFH;
        Double[] DsSSG_GFL;

        Double[] MdLSG_GFH;
        Double[] MdLSG_GFL;

        Double[] LtBG_GFH;
        Double[] LtBG_GFL;

        Double[] DsBG_GFH;
        Double[] DsBG_GFL;

        Double[] MdSSG_GFH;
        Double[] MdSSG_GFL;

        //Acceptable Variance range from gear mesh HI and Low freq for harmonic search 
        Double[] InSG_GFH_VarH;
        Double[] InSG_GFH_VarL;
        Double[] InSG_GFL_VarH;
        Double[] InSG_GFL_VarL;

        Double[] WmSG_GFH_VarH;
        Double[] WmSG_GFH_VarL;
        Double[] WmSG_GFL_VarH;
        Double[] WmSG_GFL_VarL;

        Double[] DsLSG_GFH_VarH;
        Double[] DsLSG_GFH_VarL;
        Double[] DsLSG_GFL_VarH;
        Double[] DsLSG_GFL_VarL;

        Double[] DsSSG_GFH_VarH;
        Double[] DsSSG_GFH_VarL;
        Double[] DsSSG_GFL_VarH;
        Double[] DsSSG_GFL_VarL;

        Double[] MdLSG_GFH_VarH;
        Double[] MdLSG_GFH_VarL;
        Double[] MdLSG_GFL_VarH;
        Double[] MdLSG_GFL_VarL;

        Double[] LtBG_GFH_VarH;
        Double[] LtBG_GFH_VarL;
        Double[] LtBG_GFL_VarH;
        Double[] LtBG_GFL_VarL;

        Double[] DsBG_GFH_VarH;
        Double[] DsBG_GFH_VarL;
        Double[] DsBG_GFL_VarH;
        Double[] DsBG_GFL_VarL;

        Double[] MdSSG_GFH_VarH;
        Double[] MdSSG_GFH_VarL;
        Double[] MdSSG_GFL_VarH;
        Double[] MdSSG_GFL_VarL;

        //Gear mesh freq 

        Double InSG_GMF = 0;
        Double WmSG_GMF = 0;
        Double DsLSG_GMF = 0;
        Double DsSSG_GMF = 0;
        Double MdLSG_GMF = 0;
        Double LtBG_GMF = 0;
        Double DsBG_GMF = 0;
        Double MdSSG_GMF = 0;

        int graph_line_index = 0;
        public Clarity()
        {
            InitializeComponent();
            this.Text = "Clarity"; // Top Right Text
                                   //this.fft_data =  new double[][] { FftSharp.SampleData.SampleAudio1(), FftSharp.SampleData.OddSines() };


            // First Cluster of Graphs
            panel1.Controls.Add(formsPlot1);
            panel1.Controls.Add(formsPlot2);
            panel1.Controls.Add(formsPlot3);
            panel1.Controls.Add(formsPlot4);
            panel1.Show();

            // Second Cluster of Graphs
            panel2.Controls.Add(formsPlot5);
            panel2.Controls.Add(formsPlot6);
            panel2.Hide();


            //
            // plot1
            //
            int gear_groups_num1 = 3; // i.e. number of legend entries
            int load_num = 4; // i.e. number of seperate load clusters

            // This will be replaced with serial data
            double[,] plot1_y = { { 1, 0, 6 }, { 2, 6, 10 }, { 1, 3, 7 }, { 8, 9, 1 } };
            int[] plot_x = { 12, 20, 30, 38 };

            //This will be replaced with our actual testing gear types
            string[] plot1_labels = { "Gear#1", "Gear#2", "Gear#3" };

            // Unpack X and Y, plot according to plot labels
            for (int i = 0; i < gear_groups_num1; i++)
            {
                double[] tmpy = new double[load_num];
                for (int j = 0; j < load_num; j++)
                {
                    tmpy[j] = plot1_y[j, i]; ;
                }

                double[] tmpx = new double[load_num];
                for (int j = 0; j < load_num; j++)
                {
                    tmpx[j] = plot_x[j] + i;
                }
                var bars = formsPlot1.Plot.Add.Bars(tmpx, tmpy);
                bars.LegendText = plot1_labels[i];

            }
            formsPlot1.Plot.XLabel("Frequency");
            formsPlot1.Plot.YLabel("Current");
            formsPlot1.Plot.Title("Current Peaks at differing frequencys");
            formsPlot1.Refresh();

            //
            // plot2
            //
            int gear_groups_num2 = 3;
            double[,] plot2_y = { { 0, 3, 9 }, { 2, 4, 8 }, { 12, 6, 10 }, { 1, 3, 7 } };
            int[] plot2_x = { 12, 20, 30, 38 };

            string[] plot2_labels = { "Gear#1", "Gear#2", "Gear#3" };

            // Unpack X and Y, plot according to plot labels
            for (int i = 0; i < gear_groups_num2; i++)
            {
                double[] tmpy = new double[load_num];
                for (int j = 0; j < load_num; j++)
                {
                    tmpy[j] = plot2_y[j, i]; ;
                }

                double[] tmpx = new double[load_num];
                for (int j = 0; j < load_num; j++)
                {
                    tmpx[j] = plot2_x[j] + i;
                }
                var bars = formsPlot2.Plot.Add.Bars(tmpx, tmpy);
                bars.LegendText = plot2_labels[i];

            }
            formsPlot2.Plot.XLabel("Frequency");
            formsPlot2.Plot.YLabel("Current");
            formsPlot2.Plot.Title("Current Peaks at differing frequencys");
            formsPlot2.Refresh();

            //
            // plot3
            //
            int diff_groups_num = 2; // i.e. number of legend entries
            double[,] plot3_y = { { 1, 2 }, { 5, 7 }, { 1, 1 }, { 1, 1 }, { -2, -1 } };


            // Unpack X and Y, plot according to plot labels - plot 3
            for (int i = 0; i < diff_groups_num; i++)
            {
                double[] tmpy = new double[load_num];
                for (int j = 0; j < load_num; j++)
                {
                    tmpy[j] = plot3_y[j, i]; ;
                }

                double[] tmpx = new double[load_num];
                for (int j = 0; j < load_num; j++)
                {
                    tmpx[j] = plot_x[j] + i;
                }
                var bars = formsPlot3.Plot.Add.Bars(tmpx, tmpy);

            }

            formsPlot3.Plot.XLabel("Frequency");
            formsPlot3.Plot.YLabel("Difference (%)");
            formsPlot3.Plot.Title("Difference of Average Performance");
            formsPlot3.Refresh();

            //
            // plot4
            //
            int diff_groups_num4 = 2; // i.e. number of legend entries
            double[,] plot4_y = { { -3, 1 }, { 6, 3 }, { 4, -2 }, { 1, 7 }, { -3, -5 } };


            // Unpack X and Y, plot according to plot labels - plot 3
            for (int i = 0; i < diff_groups_num4; i++)
            {
                double[] tmpy = new double[load_num];
                for (int j = 0; j < load_num; j++)
                {
                    tmpy[j] = plot4_y[j, i]; ;
                }

                double[] tmpx = new double[load_num];
                for (int j = 0; j < load_num; j++)
                {
                    tmpx[j] = plot_x[j] + i;
                }
                var bars = formsPlot4.Plot.Add.Bars(tmpx, tmpy);

            }
            formsPlot4.Plot.XLabel("Frequency");
            formsPlot4.Plot.YLabel("Difference (%)");
            formsPlot4.Plot.Title("Difference from Average Performance");
            formsPlot4.Refresh();

            //
            //plot5&6
            //
            // Sampling parameters
            double[][] signals = new double[][] { FftSharp.SampleData.SampleAudio1(), FftSharp.SampleData.OddSines() };
            int signal_count = 2;

            set_fft_graphs(signals, signal_count);
        }

        //
        //Calculations triggerd by button press to update gear mesh frequencies and variance range for diagnostics
        //
        public void Update_Harmonics_Click(object sender, EventArgs e)
        {
            //Conversion of harmonic num to useable int 
            int Harmonic_Num = Convert.ToInt32(HarmNum_Entry.Value);


            /// Declaration of variables needed for diagnosis functions throughout code 
            /// HI and Low gear mesh freq
            Double[] InSG_GFH = new Double[Harmonic_Num];
            Double[] InSG_GFL = new Double[Harmonic_Num];

            Double[] WmSG_GFH = new Double[Harmonic_Num];
            Double[] WmSG_GFL = new Double[Harmonic_Num];

            Double[] DsLSG_GFH = new Double[Harmonic_Num];
            Double[] DsLSG_GFL = new Double[Harmonic_Num];

            Double[] DsSSG_GFH = new Double[Harmonic_Num];
            Double[] DsSSG_GFL = new Double[Harmonic_Num];

            Double[] MdLSG_GFH = new Double[Harmonic_Num];
            Double[] MdLSG_GFL = new Double[Harmonic_Num];

            Double[] LtBG_GFH = new Double[Harmonic_Num];
            Double[] LtBG_GFL = new Double[Harmonic_Num];

            Double[] DsBG_GFH = new Double[Harmonic_Num];
            Double[] DsBG_GFL = new Double[Harmonic_Num];

            Double[] MdSSG_GFH = new Double[Harmonic_Num];
            Double[] MdSSG_GFL = new Double[Harmonic_Num];

            //Acceptable Variance range from gear mesh HI and Low freq for harmonic search 
            Double[] InSG_GFH_VarH = new Double[Harmonic_Num];
            Double[] InSG_GFH_VarL = new Double[Harmonic_Num];
            Double[] InSG_GFL_VarH = new Double[Harmonic_Num];
            Double[] InSG_GFL_VarL = new Double[Harmonic_Num];

            Double[] WmSG_GFH_VarH = new Double[Harmonic_Num];
            Double[] WmSG_GFH_VarL = new Double[Harmonic_Num];
            Double[] WmSG_GFL_VarH = new Double[Harmonic_Num];
            Double[] WmSG_GFL_VarL = new Double[Harmonic_Num];

            Double[] DsLSG_GFH_VarH = new Double[Harmonic_Num];
            Double[] DsLSG_GFH_VarL = new Double[Harmonic_Num];
            Double[] DsLSG_GFL_VarH = new Double[Harmonic_Num];
            Double[] DsLSG_GFL_VarL = new Double[Harmonic_Num];

            Double[] DsSSG_GFH_VarH = new Double[Harmonic_Num];
            Double[] DsSSG_GFH_VarL = new Double[Harmonic_Num];
            Double[] DsSSG_GFL_VarH = new Double[Harmonic_Num];
            Double[] DsSSG_GFL_VarL = new Double[Harmonic_Num];

            Double[] MdLSG_GFH_VarH = new Double[Harmonic_Num];
            Double[] MdLSG_GFH_VarL = new Double[Harmonic_Num];
            Double[] MdLSG_GFL_VarH = new Double[Harmonic_Num];
            Double[] MdLSG_GFL_VarL = new Double[Harmonic_Num];

            Double[] LtBG_GFH_VarH = new Double[Harmonic_Num];
            Double[] LtBG_GFH_VarL = new Double[Harmonic_Num];
            Double[] LtBG_GFL_VarH = new Double[Harmonic_Num];
            Double[] LtBG_GFL_VarL = new Double[Harmonic_Num];

            Double[] DsBG_GFH_VarH = new Double[Harmonic_Num];
            Double[] DsBG_GFH_VarL = new Double[Harmonic_Num];
            Double[] DsBG_GFL_VarH = new Double[Harmonic_Num];
            Double[] DsBG_GFL_VarL = new Double[Harmonic_Num];

            Double[] MdSSG_GFH_VarH = new Double[Harmonic_Num];
            Double[] MdSSG_GFH_VarL = new Double[Harmonic_Num];
            Double[] MdSSG_GFL_VarH = new Double[Harmonic_Num];
            Double[] MdSSG_GFL_VarL = new Double[Harmonic_Num];


            //Math to convert RPM entry to HZ 

            Double InSS = Convert.ToSingle(InSS_Entry.Value);
            Double WmSS = Convert.ToSingle(WmSS_Entry.Value);
            Double DrSS = Convert.ToSingle(DrSS_Entry.Value);
            Double LtSS = Convert.ToSingle(LtSS_Entry.Value);
            Double MdSS = Convert.ToSingle(MdSS_Entry.Value);

            Double InSSHZ = InSS / 60;
            Double WmSSHZ = WmSS / 60;
            Double DrSSHZ = DrSS / 60;
            Double LtSSHZ = LtSS / 60;
            Double MdSSHZ = MdSS / 60;

            //Conversion of gear teeth nums to usable float 
            Double InSG = Convert.ToDouble(InSG_Entry.Value);
            Double WmSG = Convert.ToDouble(WmSG_Entry.Value);
            Double DsLSG = Convert.ToDouble(DsLSG_Entry.Value);
            Double DsSSG = Convert.ToDouble(DsSSG_Entry.Value);
            Double MdLSG = Convert.ToDouble(MdLSG_Entry.Value);
            Double LtBG = Convert.ToDouble(LtBG_Entry.Value);
            Double DsBG = Convert.ToDouble(DsBG_Entry.Value);
            Double MdSSG = Convert.ToDouble(MdSSG_Entry.Value);

            //Conversions of motor power freq and percent variance to usable float 
            Double MotFreq = Convert.ToDouble(MotFrq_Entry.Value);
            Double PVar = (Convert.ToDouble(PVar_Entry.Value)) / 100;

            //Calculations for Gear mesh freq 

            Double InSG_GMF = InSSHZ * InSG;
            Double WmSG_GMF = WmSSHZ * WmSG;
            Double DsLSG_GMF = DrSSHZ * DsLSG;
            Double DsSSG_GMF = DrSSHZ * DsSSG;
            Double MdLSG_GMF = MdSSHZ * MdLSG;
            Double LtBG_GMF = LtSSHZ * LtBG;
            Double DsBG_GMF = DrSSHZ * DsBG;
            Double MdSSG_GMF = MdSSHZ * MdSSG;
            Double[] InSG_GMF_G = new double[Harmonic_Num];
            Double[] WmSG_GMF_G = new double[Harmonic_Num];
            Double[] DsLSG_GMF_G = new double[Harmonic_Num];
            Double[] DsSSG_GMF_G = new double[Harmonic_Num];
            Double[] MdLSG_GMF_G = new double[Harmonic_Num];
            Double[] LtBG_GMF_G = new double[Harmonic_Num];
            Double[] DsBG_GMF_G = new double[Harmonic_Num];
            Double[] MdSSG_GMF_G = new double[Harmonic_Num];

            formsPlot7.Plot.Clear();
            formsPlot8.Plot.Clear();
            formsPlot9.Plot.Clear();
            formsPlot10.Plot.Clear();
            formsPlot11.Plot.Clear();
            formsPlot12.Plot.Clear();
            formsPlot13.Plot.Clear();
            formsPlot14.Plot.Clear();

            panel3.Controls.Add(formsPlot7);
            panel3.Controls.Add(formsPlot8);
            panel3.Controls.Add(formsPlot9);
            panel3.Controls.Add(formsPlot10);
            panel3.Controls.Add(formsPlot11);
            panel3.Controls.Add(formsPlot12);
            panel3.Controls.Add(formsPlot13);
            panel3.Controls.Add(formsPlot14);


            for (int i = 0; i < Harmonic_Num; i++)
            {

                //Gear fault freq Hi and Low 
                InSG_GFH[i] = MotFreq + ((i + 1) * InSG_GMF);
                InSG_GFL[i] = Math.Abs(MotFreq - ((i + 1) * InSG_GMF));


                WmSG_GFH[i] = MotFreq + ((i + 1) * WmSG_GMF);
                WmSG_GFL[i] = Math.Abs(MotFreq - ((i + 1) * WmSG_GMF));


                DsLSG_GFH[i] = MotFreq + ((i + 1) * DsLSG_GMF);
                DsLSG_GFL[i] = Math.Abs(MotFreq - ((i + 1) * DsLSG_GMF));

                DsSSG_GFH[i] = MotFreq + ((i + 1) * DsSSG_GMF);
                DsSSG_GFL[i] = Math.Abs(MotFreq - ((i + 1) * DsSSG_GMF));

                MdLSG_GFH[i] = MotFreq + ((i + 1) * MdLSG_GMF);
                MdLSG_GFL[i] = Math.Abs(MotFreq - ((i + 1) * MdLSG_GMF));

                LtBG_GFH[i] = MotFreq + ((i + 1) * LtBG_GMF);
                LtBG_GFL[i] = Math.Abs(MotFreq - ((i + 1) * LtBG_GMF));

                DsBG_GFH[i] = MotFreq + ((i + 1) * DsBG_GMF);
                DsBG_GFL[i] = Math.Abs(MotFreq - ((i + 1) * DsBG_GMF));

                MdSSG_GFH[i] = MotFreq + ((i + 1) * MdSSG_GMF);
                MdSSG_GFL[i] = Math.Abs(MotFreq - ((i + 1) * MdSSG_GMF));

                //Acceptable Variance range for harmonic search (LOWER PERCENT VARIANCE = LESS GEAR DAMAGE = FARTHER SIDEBAND SPACING)

                InSG_GFH_VarH[i] = InSG_GFH[i] + (InSG_GFH[i] / PVar);
                InSG_GFH_VarL[i] = Math.Abs(InSG_GFH[i] - (InSG_GFH[i] / PVar));
                InSG_GFL_VarH[i] = InSG_GFL[i] + (InSG_GFL[i] / PVar);
                InSG_GFL_VarL[i] = Math.Abs(InSG_GFL[i] - (InSG_GFL[i] / PVar));
                InSG_GMF_G[i] = InSG_GMF * (i + 1);

                WmSG_GFH_VarH[i] = WmSG_GFH[i] + (WmSG_GFH[i] / PVar);
                WmSG_GFH_VarL[i] = Math.Abs(WmSG_GFH[i] - (WmSG_GFH[i] / PVar));
                WmSG_GFL_VarH[i] = WmSG_GFL[i] + (WmSG_GFL[i] / PVar);
                WmSG_GFL_VarL[i] = Math.Abs(WmSG_GFL[i] - (WmSG_GFL[i] / PVar));
                WmSG_GMF_G[i] = WmSG_GMF * (i + 1);

                DsLSG_GFH_VarH[i] = DsLSG_GFH[i] + (DsLSG_GFH[i] / PVar);
                DsLSG_GFH_VarL[i] = Math.Abs(DsLSG_GFH[i] - (DsLSG_GFH[i] / PVar));
                DsLSG_GFL_VarH[i] = DsLSG_GFL[i] + (DsLSG_GFL[i] / PVar);
                DsLSG_GFL_VarL[i] = Math.Abs(DsLSG_GFL[i] - (DsLSG_GFL[i] / PVar));
                DsLSG_GMF_G[i] = DsLSG_GMF * (i + 1);

                DsSSG_GFH_VarH[i] = DsSSG_GFH[i] + (DsSSG_GFH[i] / PVar);
                DsSSG_GFH_VarL[i] = Math.Abs(DsSSG_GFH[i] - (DsSSG_GFH[i] / PVar));
                DsSSG_GFL_VarH[i] = DsSSG_GFL[i] + (DsSSG_GFL[i] / PVar);
                DsSSG_GFL_VarL[i] = Math.Abs(DsSSG_GFL[i] - (DsSSG_GFL[i] / PVar));
                DsSSG_GMF_G[i] = DsSSG_GMF * (i + 1);

                MdLSG_GFH_VarH[i] = MdLSG_GFH[i] + (MdLSG_GFH[i] / PVar);
                MdLSG_GFH_VarL[i] = Math.Abs(MdLSG_GFH[i] - (MdLSG_GFH[i] / PVar));
                MdLSG_GFL_VarH[i] = MdLSG_GFL[i] + (MdLSG_GFL[i] / PVar);
                MdLSG_GFL_VarL[i] = Math.Abs(MdLSG_GFL[i] - (MdLSG_GFL[i] / PVar));
                MdLSG_GMF_G[i] = MdLSG_GMF * (i + 1);

                LtBG_GFH_VarH[i] = LtBG_GFH[i] + (LtBG_GFH[i] / PVar);
                LtBG_GFH_VarL[i] = Math.Abs(LtBG_GFH[i] - (LtBG_GFH[i] / PVar));
                LtBG_GFL_VarH[i] = LtBG_GFL[i] + (LtBG_GFL[i] / PVar);
                LtBG_GFL_VarL[i] = Math.Abs(LtBG_GFL[i] - (LtBG_GFL[i] / PVar));
                LtBG_GMF_G[i] = LtBG_GMF * (i + 1);

                DsBG_GFH_VarH[i] = DsBG_GFH[i] + (DsBG_GFH[i] / PVar);
                DsBG_GFH_VarL[i] = Math.Abs(DsBG_GFH[i] - (DsBG_GFH[i] / PVar));
                DsBG_GFL_VarH[i] = DsBG_GFL[i] + (DsBG_GFL[i] / PVar);
                DsBG_GFL_VarL[i] = Math.Abs(DsBG_GFL[i] - (DsBG_GFL[i] / PVar));
                DsBG_GMF_G[i] = DsBG_GMF * (i + 1);

                MdSSG_GFH_VarH[i] = MdSSG_GFH[i] + (MdSSG_GFH[i] / PVar);
                MdSSG_GFH_VarL[i] = Math.Abs(MdSSG_GFH[i] - (MdSSG_GFH[i] / PVar));
                MdSSG_GFL_VarH[i] = MdSSG_GFL[i] + (MdSSG_GFL[i] / PVar);
                MdSSG_GFL_VarL[i] = Math.Abs(MdSSG_GFL[i] - (MdSSG_GFL[i] / PVar));
                MdSSG_GMF_G[i] = MdSSG_GMF * (i + 1);


                formsPlot7.Plot.Title("Gear Mesh Frequencies Worm Shaft Spur Gear");
                formsPlot7.Plot.YLabel("Estimated Magnitude(dB)");
                formsPlot7.Plot.XLabel("Estimated Frequency (Hz)");
                var sig1 = formsPlot7.Plot.Add.Bar(InSG_GFH_VarH[i], 2500 / (i + 1));
                sig1.Color = Colors.Red;
                sig1.LegendText = "High Span";


                var sig2 = formsPlot7.Plot.Add.Bar(InSG_GFH_VarL[i], 2500 / (i + 1));
                sig2.Color = Colors.Blue;
                sig2.LegendText = "Low Span";


                var sig3 = formsPlot7.Plot.Add.Bar(InSG_GFL_VarH[i], 2500 / (i + 1));
                sig3.Color = Colors.Red;
                sig3.LegendText = "High Span";


                var sig4 = formsPlot7.Plot.Add.Bar(InSG_GFL_VarL[i], 2500 / (i + 1));
                sig4.Color = Colors.Blue;
                sig4.LegendText = "Low Span";


                var sig6 = formsPlot7.Plot.Add.Bar(InSG_GFL[i], 5000);
                sig6.Color = Colors.Cyan;
                sig6.LegendText = "Low Gear Mesh Freq";


                var sig7 = formsPlot7.Plot.Add.Bar(InSG_GFH[i], 5000);
                sig7.Color = Colors.Coral;
                sig7.LegendText = "High Gear Mesh Freq";

                var sig77 = formsPlot7.Plot.Add.Bar(InSG_GMF_G[i], 10000);
                sig77.Color = Colors.YellowGreen;
                sig77.LegendText = "Iterative Gear mesh Freq";

                var sig8 = formsPlot7.Plot.Add.Bar(InSG_GMF, 10000);
                sig8.Color = Colors.Green;
                sig8.LegendText = "Healthy Gear Mesh Freq";


                //Worm Spur Gear

                formsPlot8.Plot.Title("Gear Mesh Frequencies Worm Shaft Spur Gear");
                formsPlot8.Plot.YLabel("Estimated Magnitude(dB)");
                formsPlot8.Plot.XLabel("Estimated Frequency (Hz)");
                var sig9 = formsPlot8.Plot.Add.Bar(WmSG_GFH_VarH[i], 2500 / (i + 1));
                sig9.Color = Colors.Red;

                sig9.LegendText = "High Span";
                var sig10 = formsPlot8.Plot.Add.Bar(WmSG_GFH_VarL[i], 2500 / (i + 1));
                sig10.Color = Colors.Blue;

                sig10.LegendText = "Low Span";
                var sig11 = formsPlot8.Plot.Add.Bar(WmSG_GFL_VarH[i], 2500 / (i + 1));
                sig11.Color = Colors.Red;

                sig11.LegendText = "High Span";
                var sig12 = formsPlot8.Plot.Add.Bar(WmSG_GFL_VarL[i], 2500 / (i + 1));
                sig12.Color = Colors.Blue;

                sig12.LegendText = "Low Span";
                var sig13 = formsPlot8.Plot.Add.Bar(WmSG_GFL[i], 5000);
                sig13.Color = Colors.Cyan;

                sig13.LegendText = "Low Gear Mesh Freq";
                var sig14 = formsPlot8.Plot.Add.Bar(WmSG_GFH[i], 5000);
                sig14.Color = Colors.Coral;


                var sig115 = formsPlot8.Plot.Add.Bar(WmSG_GMF_G[i], 10000);
                sig115.Color = Colors.YellowGreen;
                sig115.LegendText = "Iterative Gear mesh Freq";

                var sig15 = formsPlot8.Plot.Add.Bar(WmSG_GMF, 10000);
                sig15.Color = Colors.Green;

                sig15.LegendText = "Healthy Gear Mesh Freq";

                //Drive Large Spur gear


                formsPlot9.Plot.Title("Gear Mesh Frequencies Drive Shaft Large Spur Gear");
                formsPlot9.Plot.YLabel("Estimated Magnitude(dB)");
                formsPlot9.Plot.XLabel("Estimated Frequency (Hz)");
                var sig16 = formsPlot9.Plot.Add.Bar(DsLSG_GFH_VarH[i], 2500 / (i + 1));
                sig16.Color = Colors.Red;

                sig16.LegendText = "High Span";
                var sig17 = formsPlot9.Plot.Add.Bar(DsLSG_GFH_VarL[i], 2500 / (i + 1));
                sig17.Color = Colors.Blue;

                sig17.LegendText = "Low Span";
                var sig18 = formsPlot9.Plot.Add.Bar(DsLSG_GFL_VarH[i], 2500 / (i + 1));
                sig18.Color = Colors.Red;

                sig18.LegendText = "High Span";
                var sig19 = formsPlot9.Plot.Add.Bar(DsLSG_GFL_VarL[i], 2500 / (i + 1));
                sig19.Color = Colors.Blue;

                sig19.LegendText = "Low Span";
                var sig20 = formsPlot9.Plot.Add.Bar(DsLSG_GFL[i], 5000);
                sig20.Color = Colors.Cyan;

                sig20.LegendText = "Low Gear Mesh Freq";
                var sig21 = formsPlot9.Plot.Add.Bar(DsLSG_GFH[i], 5000);
                sig21.Color = Colors.Coral;

                sig21.LegendText = "High Gear Mesh Freq";
                var sig221 = formsPlot9.Plot.Add.Bar(DsLSG_GMF_G[i], 10000);
                sig221.Color = Colors.YellowGreen;
                sig221.LegendText = "Iterative Gear mesh Freq";
                var sig22 = formsPlot9.Plot.Add.Bar(DsLSG_GMF, 10000);
                sig22.Color = Colors.Green;

                sig22.LegendText = "Healthy Gear Mesh Freq";

                //Drive Small Spur gear

                formsPlot10.Plot.Title("Gear Mesh Frequencies Drive Shaft Small Spur Gear");
                formsPlot10.Plot.YLabel("Estimated Magnitude(dB)");
                formsPlot10.Plot.XLabel("Estimated Frequency (Hz)");
                var sig23 = formsPlot10.Plot.Add.Bar(DsSSG_GFH_VarH[i], 2500 / (i + 1));
                sig23.Color = Colors.Red;

                sig23.LegendText = "High Span";
                var sig24 = formsPlot10.Plot.Add.Bar(DsSSG_GFH_VarL[i], 2500 / (i + 1));
                sig24.Color = Colors.Blue;

                sig24.LegendText = "Low Span";
                var sig25 = formsPlot10.Plot.Add.Bar(DsSSG_GFL_VarH[i], 2500 / (i + 1));
                sig25.Color = Colors.Red;

                sig25.LegendText = "High Span";
                var sig26 = formsPlot10.Plot.Add.Bar(DsSSG_GFL_VarL[i], 2500 / (i + 1));
                sig26.Color = Colors.Blue;

                sig26.LegendText = "Low Span";
                var sig27 = formsPlot10.Plot.Add.Bar(DsSSG_GFL[i], 5000);
                sig27.Color = Colors.Cyan;

                sig27.LegendText = "Low Gear Mesh Freq";
                var sig28 = formsPlot10.Plot.Add.Bar(DsSSG_GFH[i], 5000);
                sig28.Color = Colors.Coral;

                sig28.LegendText = "High Gear Mesh Freq";
                var sig228 = formsPlot10.Plot.Add.Bar(DsSSG_GMF_G[i], 10000);
                sig228.Color = Colors.YellowGreen;
                sig228.LegendText = "Iterative Gear mesh Freq";
                var sig29 = formsPlot10.Plot.Add.Bar(DsSSG_GMF, 10000);
                sig29.Color = Colors.Green;

                sig29.LegendText = "Healthy Gear Mesh Freq";

                //Main Drive Large Spur gear


                formsPlot11.Plot.Title("Gear Mesh Frequencies Main Drive Shaft Large Spur Gear");
                formsPlot11.Plot.YLabel("Estimated Magnitude(dB)");
                formsPlot11.Plot.XLabel("Estimated Frequency (Hz)");
                var sig30 = formsPlot11.Plot.Add.Bar(MdLSG_GFH_VarH[i], 2500 / (i + 1));
                sig30.Color = Colors.Red;

                sig30.LegendText = "High Span";
                var sig31 = formsPlot11.Plot.Add.Bar(MdLSG_GFH_VarL[i], 2500 / (i + 1));
                sig31.Color = Colors.Blue;

                sig31.LegendText = "Low Span";
                var sig32 = formsPlot11.Plot.Add.Bar(MdLSG_GFL_VarH[i], 2500 / (i + 1));
                sig32.Color = Colors.Red;

                sig32.LegendText = "High Span";
                var sig33 = formsPlot11.Plot.Add.Bar(MdLSG_GFL_VarL[i], 2500 / (i + 1));
                sig33.Color = Colors.Blue;

                sig33.LegendText = "Low Span";
                var sig34 = formsPlot11.Plot.Add.Bar(MdLSG_GFL[i], 5000);
                sig34.Color = Colors.Cyan;

                sig34.LegendText = "Low Gear Mesh Freq";
                var sig35 = formsPlot11.Plot.Add.Bar(MdLSG_GFH[i], 5000);
                sig35.Color = Colors.Coral;

                sig35.LegendText = "High Gear Mesh Freq";
                var sig335 = formsPlot11.Plot.Add.Bar(MdLSG_GMF_G[i], 10000);
                sig335.Color = Colors.YellowGreen;
                sig335.LegendText = "Iterative Gear mesh Freq";
                var sig36 = formsPlot11.Plot.Add.Bar(MdLSG_GMF, 10000);
                sig36.Color = Colors.Green;

                sig36.LegendText = "Healthy Gear Mesh Freq";

                //Lance tube bevel gear

                formsPlot12.Plot.Title("Gear Mesh Frequencies Lance Tube Bevel Gear");
                formsPlot12.Plot.YLabel("Estimated Magnitude(dB)");
                formsPlot12.Plot.XLabel("Estimated Frequency (Hz)");
                var sig37 = formsPlot12.Plot.Add.Bar(LtBG_GFH_VarH[i], 2500 / (i + 1));
                sig37.Color = Colors.Red;

                sig37.LegendText = "High Span";
                var sig38 = formsPlot12.Plot.Add.Bar(LtBG_GFH_VarL[i], 2500 / (i + 1));
                sig38.Color = Colors.Blue;

                sig38.LegendText = "Low Span";
                var sig39 = formsPlot12.Plot.Add.Bar(LtBG_GFL_VarH[i], 2500 / (i + 1));
                sig39.Color = Colors.Red;

                sig39.LegendText = "High Span";
                var sig40 = formsPlot12.Plot.Add.Bar(LtBG_GFL_VarL[i], 2500 / (i + 1));
                sig40.Color = Colors.Blue;

                sig40.LegendText = "Low Span";
                var sig41 = formsPlot12.Plot.Add.Bar(LtBG_GFL[i], 5000);
                sig41.Color = Colors.Cyan;

                sig41.LegendText = "Low Gear Mesh Freq";
                var sig42 = formsPlot12.Plot.Add.Bar(LtBG_GFH[i], 5000);
                sig42.Color = Colors.Coral;

                sig42.LegendText = "High Gear Mesh Freq";
                var sig442 = formsPlot12.Plot.Add.Bar(LtBG_GMF_G[i], 10000);
                sig442.Color = Colors.YellowGreen;
                sig442.LegendText = "Iterative Gear mesh Freq";
                var sig43 = formsPlot12.Plot.Add.Bar(LtBG_GMF, 10000);
                sig43.Color = Colors.Green;

                sig43.LegendText = "Healthy Gear Mesh Freq";

                //Drive Bevel Gear
                formsPlot13.Plot.Title("Gear Mesh Frequencies Drive Shaft Bevel Gear");
                formsPlot13.Plot.YLabel("Estimated Magnitude(dB)");
                formsPlot13.Plot.XLabel("Estimated Frequency (Hz)");
                var sig44 = formsPlot13.Plot.Add.Bar(DsBG_GFH_VarH[i], 2500 / (i + 1));
                sig44.Color = Colors.Red;

                sig44.LegendText = "High Span";
                var sig45 = formsPlot13.Plot.Add.Bar(DsBG_GFH_VarL[i], 2500 / (i + 1));
                sig45.Color = Colors.Blue;

                sig45.LegendText = "Low Span";
                var sig46 = formsPlot13.Plot.Add.Bar(DsBG_GFL_VarH[i], 2500 / (i + 1));
                sig46.Color = Colors.Red;

                sig46.LegendText = "High Span";
                var sig47 = formsPlot13.Plot.Add.Bar(DsBG_GFL_VarL[i], 2500 / (i + 1));
                sig47.Color = Colors.Blue;

                sig47.LegendText = "Low Span";
                var sig48 = formsPlot13.Plot.Add.Bar(DsBG_GFL[i], 5000);
                sig48.Color = Colors.Cyan;

                sig48.LegendText = "Low Gear Mesh Freq";
                var sig49 = formsPlot13.Plot.Add.Bar(DsBG_GFH[i], 5000);
                sig49.Color = Colors.Coral;

                sig49.LegendText = "High Gear Mesh Freq";
                var sig449 = formsPlot13.Plot.Add.Bar(DsBG_GMF_G[i], 10000);
                sig449.Color = Colors.YellowGreen;
                sig449.LegendText = "Iterative Gear mesh Freq";
                var sig50 = formsPlot13.Plot.Add.Bar(DsBG_GMF, 10000);
                sig50.Color = Colors.Green;

                sig50.LegendText = "Healthy Gear Mesh Freq";

                //Main Drive Small Spur Gear


                formsPlot14.Plot.Title("Gear Mesh Frequencies Main Drive Shaft Small Spur Gear");
                formsPlot14.Plot.YLabel("Estimated Magnitude(dB)");
                formsPlot14.Plot.XLabel("Estimated Frequency (Hz)");
                var sig51 = formsPlot14.Plot.Add.Bar(MdSSG_GFH_VarH[i], 2500 / (i + 1));
                sig51.Color = Colors.Red;

                sig51.LegendText = "High Span";
                var sig52 = formsPlot14.Plot.Add.Bar(MdSSG_GFH_VarL[i], 2500 / (i + 1));
                sig52.Color = Colors.Blue;

                sig52.LegendText = "Low Span";
                var sig53 = formsPlot14.Plot.Add.Bar(MdSSG_GFL_VarH[i], 2500 / (i + 1));
                sig53.Color = Colors.Red;

                sig53.LegendText = "High Span";
                var sig54 = formsPlot14.Plot.Add.Bar(MdSSG_GFL_VarL[i], 2500 / (i + 1));
                sig54.Color = Colors.Blue;

                sig54.LegendText = "Low Span";
                var sig55 = formsPlot14.Plot.Add.Bar(MdSSG_GFL[i], 5000);
                sig55.Color = Colors.Cyan;

                sig55.LegendText = "Low Gear Mesh Freq";
                var sig56 = formsPlot14.Plot.Add.Bar(MdSSG_GFH[i], 5000);
                sig56.Color = Colors.Coral;

                sig56.LegendText = "High Gear Mesh Freq";
                var sig556 = formsPlot14.Plot.Add.Bar(MdSSG_GMF_G[i], 10000);
                sig556.Color = Colors.YellowGreen;
                sig556.LegendText = "Iterative Gear mesh Freq";
                var sig57 = formsPlot14.Plot.Add.Bar(MdSSG_GMF, 10000);
                sig57.Color = Colors.Green;

                sig57.LegendText = "Healthy Gear Mesh Freq";

                if (i > 0)
                {
                    sig1.LegendText = "";
                    sig221.LegendText = "";
                    sig2.LegendText = "";
                    sig228.LegendText = "";
                    sig335.LegendText = "";
                    sig3.LegendText = "";
                    sig449.LegendText = "";
                    sig442.LegendText = "";
                    sig4.LegendText = "";
                    sig556.LegendText = "";
                    sig6.LegendText = "";
                    sig7.LegendText = "";
                    sig8.LegendText = "";
                    sig77.LegendText = "";
                    sig115.LegendText = "";
                    sig9.LegendText = "";
                    sig10.LegendText = "";
                    sig11.LegendText = "";
                    sig12.LegendText = "";
                    sig13.LegendText = "";
                    sig14.LegendText = "";
                    sig15.LegendText = "";
                    sig16.LegendText = "";
                    sig17.LegendText = "";
                    sig18.LegendText = "";
                    sig19.LegendText = "";
                    sig20.LegendText = "";
                    sig21.LegendText = "";
                    sig22.LegendText = "";
                    sig23.LegendText = "";
                    sig24.LegendText = "";
                    sig25.LegendText = "";
                    sig26.LegendText = "";
                    sig27.LegendText = "";
                    sig28.LegendText = "";
                    sig29.LegendText = "";
                    sig30.LegendText = "";
                    sig31.LegendText = "";
                    sig32.LegendText = "";
                    sig33.LegendText = "";
                    sig34.LegendText = "";
                    sig35.LegendText = "";
                    sig36.LegendText = "";
                    sig37.LegendText = "";
                    sig38.LegendText = "";
                    sig39.LegendText = "";
                    sig40.LegendText = "";
                    sig41.LegendText = "";
                    sig42.LegendText = "";
                    sig43.LegendText = "";
                    sig44.LegendText = "";
                    sig45.LegendText = "";
                    sig46.LegendText = "";
                    sig47.LegendText = "";
                    sig48.LegendText = "";
                    sig49.LegendText = "";
                    sig50.LegendText = "";
                    sig51.LegendText = "";
                    sig52.LegendText = "";
                    sig53.LegendText = "";
                    sig54.LegendText = "";
                    sig55.LegendText = "";
                    sig56.LegendText = "";
                    sig57.LegendText = "";
                }
                formsPlot7.Plot.Axes.AutoScale();
                formsPlot8.Plot.Axes.AutoScale();
                formsPlot9.Plot.Axes.AutoScale();
                formsPlot10.Plot.Axes.AutoScale();
                formsPlot11.Plot.Axes.AutoScale();
                formsPlot12.Plot.Axes.AutoScale();
                formsPlot13.Plot.Axes.AutoScale();
                formsPlot14.Plot.Axes.AutoScale();
                formsPlot7.Refresh();
                formsPlot8.Refresh();
                formsPlot9.Refresh();
                formsPlot10.Refresh();
                formsPlot11.Refresh();
                formsPlot12.Refresh();
                formsPlot13.Refresh();
                formsPlot14.Refresh();



            }



        }

        int Graph_Select = 1;
        private void UpdateGraph()
        {
            switch (Graph_Select)
            {
                case 1:
                    formsPlot7.BringToFront();
                    break;

                case 2:
                    formsPlot8.BringToFront();
                    break;

                case 3:
                    formsPlot9.BringToFront();
                    break;

                case 4:
                    formsPlot10.BringToFront();
                    break;

                case 5:
                    formsPlot11.BringToFront();
                    break;

                case 6:
                    formsPlot12.BringToFront();
                    break;

                case 7:
                    formsPlot13.BringToFront();
                    break;

                case 8:
                    formsPlot14.BringToFront();
                    break;

            }

        }
        ///Target harmonic plot scroll functionality 


        private void Scroll_Button_P_Click(object sender, EventArgs e)
        {

            Graph_Select += 1;

            if (Graph_Select > 8)
            {
                Graph_Select = 1;
            }
            UpdateGraph();
        }
        private void Scroll_Button_N_Click(object sender, EventArgs e)
        {

            Graph_Select -= 1;

            if (Graph_Select < 1)
            {
                Graph_Select = 8;
            }
            UpdateGraph();
        }


        double[] magnitude;
        double[] power;
        double[] freq;

        private void clear_fft_graphs()
        {
            graph_line_index = 0;
            formsPlot5.Plot.Clear();
            formsPlot6.Plot.Clear();

            formsPlot5.Refresh();
            formsPlot6.Refresh();
        }
        private void set_fft_graphs(double[][] signals, int signal_count)
        {

            // formsPlot5.Plot.Clear();
            // formsPlot6.Plot.Clear();

            // Evil Sample Rate Hack
            double sample_period = signals[0][1] - signals[0][0]; // Time Delta between signal reads.
            double sample_rate = 1 / sample_period;

            // double sample_rate = 1000; // Samples per second
            // double sample_period = sample_rate / 1000.0; // 
            //string[] legend_text = { "Baseline", "Tooth Breakage" };
            LinePattern[] line_patterns = { LinePattern.Solid, LinePattern.DenselyDashed, LinePattern.Dotted };

            // Only Plot Current
            for (int i = 1; i < signal_count; i++)
            {

                var window = new FftSharp.Windows.Hanning();
                window.ApplyInPlace(signals[i]);

                System.Numerics.Complex[] spectrum = FftSharp.FFT.Forward(signals[i]);

                // get the magnitude (units^2) or power (dB) as real numbers
                magnitude = FftSharp.FFT.Magnitude(spectrum);
                power = FftSharp.FFT.Power(spectrum);
                freq = FftSharp.FFT.FrequencyScale(power.Length, sample_rate);

                var plt = formsPlot5.Plot.Add.Signal(signals[i], sample_period);
                //plt.LegendText = legend_text[i];
                plt.LinePattern = line_patterns[i];

                var plt6 = formsPlot6.Plot.Add.ScatterLine(freq, power);
                //var plt6_1 = formsPlot6.Plot.Add.ScatterLine(freq, magnitude);
                plt6.LinePattern = line_patterns[graph_line_index % line_patterns.Length];
                //plt6_1.LinePattern = LinePattern.DenselyDashed;



            }

            formsPlot5.Plot.YLabel("Amplitude");
            formsPlot5.Plot.XLabel("Time (s)");
            formsPlot5.Plot.Title("Original Waveform");
            formsPlot5.Refresh();

            formsPlot6.Plot.YLabel("Magnitude ");
            formsPlot6.Plot.XLabel("Frequency (Hz)");
            formsPlot6.Plot.Title("FFT");
        }

        public void BaseLine_Config_Click(object sender, EventArgs e)
        {
            clear_fft_graphs(); // Make this the clear button for now
            {
                // Ensure FFT has been run and arrays exist
                if (magnitude == null || power == null || freq == null)
                {
                    MessageBox.Show("Populate FFT graph before configuring baseline.");
                    return;
                }

                int signal_count = magnitude.Length;

                // Allocate Healthy arrays to the correct size
                healthy_Magnitude = new double[signal_count];
                Healthy_Power = new double[signal_count];
                Healthy_freq = new double[signal_count];

                for (int i = 0; i < signal_count; i++)
                {
                    healthy_Magnitude[i] = magnitude[i];
                    Healthy_Power[i] = power[i];
                    Healthy_freq[i] = freq[i];
                }
            }
        }

        public void Attempt_Diagnosis_Click(object sender, EventArgs e)
        {
            int signal_count = magnitude.Length;
            //gear mesh frequencies
            int[] gear_indexs = { Convert.ToInt32(InSG_GMF), Convert.ToInt32(WmSG_GMF), Convert.ToInt32(DsLSG_GMF), Convert.ToInt32(DsSSG_GMF), Convert.ToInt32(LtBG_GMF), Convert.ToInt32(DsBG_GMF), Convert.ToInt32(MdSSG_GMF), Convert.ToInt32(MdLSG_GMF) };
            ToolStripLabel[] damage_label = { toolStripStatusLabel1, toolStripStatusLabel2, toolStripStatusLabel3, toolStripStatusLabel4, toolStripStatusLabel5, toolStripStatusLabel6, toolStripStatusLabel7, toolStripStatusLabel8 };
            Debug.Print("{0}\n", signal_count);
            int[] degree_of_damage = new int[8];

            //testing all the gear indexes and finding how damages they are
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < signal_count; i++)
                {
                    //magnitude change
                    if (magnitude[gear_indexs[j]] > healthy_Magnitude[gear_indexs[j]] + healthy_Magnitude[gear_indexs[j]] * .10)
                    {
                        degree_of_damage[j] = Math.Max(degree_of_damage[j], 1);
                    }

                    if (magnitude[gear_indexs[j]] > healthy_Magnitude[gear_indexs[j]] + healthy_Magnitude[gear_indexs[j]] * .50)
                    {
                        degree_of_damage[j] = Math.Max(degree_of_damage[j], 2);
                    }
                }
            }
            //setting the label to diplay the damage
            
            for (int i = 0; i < 8; i++)
            {
                Debug.Print("Label {0} {1}\n", damage_label[i].Text, i);
                if (degree_of_damage[i] == 0)
                {
                    damage_label[i].Text = "Normal";
                    Debug.Print("Normal\n");
                }
                else if (degree_of_damage[i] == 1)
                {
                    damage_label[i].Text = "Minor Damage";
                    Debug.Print("Minor Damage\n");
                }
                else
                {
                    damage_label[i].Text = "Major Damage";
                    Debug.Print("Major Damage\n");
                }
            }

        }

        //
        //button1
        //
        private void button1_Click(object sender, EventArgs e)
        {

            if (panel3.Visible)
            {
                panel1.Show();
                panel3.Hide();
            }
            if (panel2.Visible)
            {
                panel2.Hide();
                panel1.Show();
            }
            else
            {
                panel1.Show();
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {

            if (panel3.Visible)
            {
                panel1.Show();
                panel3.Hide();
            }
            if (panel2.Visible)
            {
                panel2.Hide();
                panel1.Show();
            }
            else
            {
                panel1.Show();
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {

            if (panel3.Visible)
            {
                panel1.Show();
                panel3.Hide();
            }
            if (panel2.Visible)
            {
                panel2.Hide();
                panel1.Show();
            }
            else
            {
                panel1.Show();
            }

        }
        private void button4_Click(object sender, EventArgs e)
        {

            if (panel3.Visible)
            {
                panel1.Show();
                panel3.Hide();
            }
            if (panel2.Visible)
            {
                panel2.Hide();
                panel1.Show();
            }
            else
            {
                panel1.Show();
            }

        }

        //
        //button2
        //
        private void button2_Click(object sender, EventArgs e)
        {

            if (panel3.Visible)
            {
                panel2.Show();
                panel3.Hide();
            }
            if (panel1.Visible)
            {
                panel1.Hide();
                panel2.Show();
            }
            else
            {
                panel2.Show();
            }

        }
        private void button11_Click(object sender, EventArgs e)
        {

            if (panel3.Visible)
            {
                panel2.Show();
                panel3.Hide();
            }
            if (panel1.Visible)
            {
                panel1.Hide();
                panel2.Show();
            }
            else
            {
                panel2.Show();
            }

        }
        private void button7_Click(object sender, EventArgs e)
        {

            if (panel3.Visible)
            {
                panel2.Show();
                panel3.Hide();
            }
            if (panel1.Visible)
            {
                panel1.Hide();
                panel2.Show();
            }
            else
            {
                panel2.Show();
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {

            if (panel3.Visible)
            {
                panel2.Show();
                panel3.Hide();
            }
            if (panel1.Visible)
            {
                panel1.Hide();
                panel2.Show();
            }
            else
            {
                panel2.Show();
            }

        }


        //
        //Settings
        //
        private void settings_Click(object sender, EventArgs e)
        {
            if (panel1.Visible)
            {
                panel1.Hide();
                panel3.Show();
            }
            if (panel2.Visible)
            {
                panel2.Hide();
                panel3.Show();
            }
            else
            {
                panel3.Show();
            }

        }
        private void button14_Click(object sender, EventArgs e)
        {
            if (panel1.Visible)
            {
                panel1.Hide();
                panel3.Show();
            }
            if (panel2.Visible)
            {
                panel2.Hide();
                panel3.Show();
            }
            else
            {
                panel3.Show();
            }

        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (panel1.Visible)
            {
                panel1.Hide();
                panel3.Show();
            }
            if (panel2.Visible)
            {
                panel2.Hide();
                panel3.Show();
            }
            else
            {
                panel3.Show();
            }

        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (panel1.Visible)
            {
                panel1.Hide();
                panel3.Show();
            }
            if (panel2.Visible)
            {
                panel2.Hide();
                panel3.Show();
            }
            else
            {
                panel3.Show();
            }

        }

        //
        //fft button
        //

        private void fft_button_Click_1(object sender, EventArgs e)
        {
            Debug.WriteLine("Button Clicked!");

            // Open File Dialogue
            var fileContent = string.Empty;
            var filePath = string.Empty;
            double[][] records_list = new double[2][];
            double[][] signals = new double[2][];
            int row_count = 0;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    //var fileStream = openFileDialog.OpenFile();

                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        PrepareHeaderForMatch = args => Char.ToUpper(args.Header[0]) + args.Header.Substring(1).ToLower(),
                    };
                    using (var reader = new StreamReader(filePath))
                    using (var csv = new CsvReader(reader, config))
                    {
                        var records = csv.GetRecords<signal_entry>().ToList();
                        int max_rows = 1;
                        Debug.Print("{0}\n", records.Count());

                        Debug.Print("{0}\n", records.Count());
                        while (max_rows < records.Count())
                        {
                            max_rows *= 2;
                        }
                        // max_rows /= 2;

                        Debug.Print("{0}\n", max_rows);


                        for (int i = 0; i < 2; i++)
                        {
                            signals[i] = new double[max_rows];
                        }



                        foreach (signal_entry record in records)
                        {
                            if (row_count >= max_rows) break;
                            signals[0][row_count] = record.time;
                            signals[1][row_count] = record.current;
                            row_count++;


                        }
                    }
                }
                else
                {
                    return;
                }
            }











            /*for (int row = 1; row < rows.Length; row++)
            {
                for (int col = 0; col < 2; col++)
                { 
                    Debug.WriteLine($" {row} {col}");
                    var cell = worksheet.GetCellAt(row, col);
                    signals[col][row - 1] = cell.DoubleValue;
                }
            }
            Debug.WriteLine($" {signals[0].Length}");*/

            set_fft_graphs(signals, 2);
        }

        private void MdSS_Entry_ValueChanged(object sender, EventArgs e)
        {

        }

        private void MdLSG_Entry_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void formsPlot5_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void formsPlot12_Load(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void status7_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void status8_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void status6_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        

    }

    public class signal_entry
    {
        public double time { get; set; }
        public double current { get; set; }
    }
}