using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;

namespace AvalonLogViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            createPanel(1);
            createPanel(2);
            createPanel(3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string host = minerIP.Text;
            int port = Int32.Parse(minerPort.Text);
            string command = minerCommand.Text;

            GetSocket getSocket = new GetSocket();

            string result = GetSocket.SocketSendReceive(host, port, command);

            char[] delimiterChars = { '|', ',', '\t' };

            string text = result;
            System.Console.WriteLine($"Original text: '{text}'");

            string[] words = text.Split(delimiterChars);
            System.Console.WriteLine($"{words.Length} words in text:");

            string[] hash0data = null;
            string[] temps = null;
            string[] volts = null;
            foreach (var word in words)
            {
                System.Console.WriteLine($"<{word}>");
                if (word.IndexOf("PVT_V0") != -1)
                {
                    volts = getPVTV(0, word, 1);
                }
                if (word.IndexOf("PVT_T0") != -1)
                {
                    temps = getPVTT(0, word, 1);
                }
            }
            string bla = "";
            //hashOUT.Lines = temps;
            if (temps != null)
                for (int i = 1; i < temps.Length + 1; i++)
                {
                    foreach (Control panel in tableLayoutPanel1.Controls)
                    {
                        if (panel.Name == "h1p" + i.ToString())
                        {
                                try
                                {
                                    panel.BackColor = tempRGB(Int32.Parse(temps[i - 1]));
                                }
                                catch { System.Windows.Forms.MessageBox.Show("Ошибка, отображения цвета: "+ temps[i - 1]); }
                            foreach (Control tempLabel in panel.Controls)
                                {
                                    try
                                    {
                                        if (tempLabel.Name == "h1t" + i.ToString())
                                            tempLabel.Text = temps[i - 1].ToString();
                                        if (tempLabel.Name == "h1v" + i.ToString())
                                            tempLabel.Text = volts[i - 1].ToString();
                                    }
                                    catch { }
                                }
                        }
                    }
                }

            foreach (var word in words)
            {
                System.Console.WriteLine($"<{word}>");
                if (word.IndexOf("PVT_V1") != -1)
                {
                    volts = getPVTV(0, word, 2);
                }
                if (word.IndexOf("PVT_T1") != -1)
                {
                    temps = getPVTT(0, word, 2);
                }
            }
            if (temps != null)
                for (int i = 1; i < temps.Length + 1; i++)
                {
                    foreach (Control panel in tableLayoutPanel2.Controls)
                    {
                        if (panel.Name == "h2p" + i.ToString())
                        {
                            try
                            {
                                panel.BackColor = tempRGB(Int32.Parse(temps[i - 1]));
                            }
                            catch { System.Windows.Forms.MessageBox.Show("Ошибка, отображения цвета: " + temps[i - 1]); }
                            foreach (Control tempLabel in panel.Controls)
                            {
                                try
                                {
                                    if (tempLabel.Name == "h2t" + i.ToString())
                                        tempLabel.Text = temps[i - 1].ToString();
                                    if (tempLabel.Name == "h2v" + i.ToString())
                                        tempLabel.Text = volts[i - 1].ToString();
                                }
                                catch { }
                            }
                        }
                    }
                }


            if (checkParse.Checked)
                outText.Lines = words;
            else
                outText.Text = result;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void minerIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkParse_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel23_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel37_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
        private static string[] getPVTT(int num_plate, string strToFind, int hashNum)
        {
            try
            {
                hashNum -= 1;
                string rightPart = strToFind.Substring(strToFind.IndexOf("PVT_T"+hashNum.ToString()));
                rightPart = rightPart.Substring(rightPart.IndexOf('[') + 1);
                rightPart = rightPart.Substring(0, rightPart.IndexOf(']'));

                string[] tempsAll = rightPart.Split(new[] { " " }, StringSplitOptions.None);

                string[] temps = new string[120];
                int idx = 0;
                foreach (string temp in tempsAll)
                {
                    if (temp != "")
                    {
                        temps[idx] = temp;
                        idx++;
                    }
                }

                return temps;

            }
            catch { System.Windows.Forms.MessageBox.Show("Ошибка, заполнения массива"); }
            return null;
        }

        private static string[] getPVTV(int num_plate, string strToFind, int hashNum)
        {
            try
            {
                hashNum -= 1;
                string rightPart = strToFind.Substring(strToFind.IndexOf("PVT_V" + hashNum.ToString()));
                rightPart = rightPart.Substring(rightPart.IndexOf('[') + 1);
                rightPart = rightPart.Substring(0, rightPart.IndexOf(']'));

                string[] temps = rightPart.Split(new[] { " " }, StringSplitOptions.None);

                return temps;

            }
            catch { }
            return null;
        }

        public static Color tempRGB(int temp)
        {
            if (temp < 60)
                return HsvToRgb(120, 1, 1);
            else if (temp > 90)
                return HsvToRgb(0, 1, 1);

            int revTemp = (90 - temp) * 4;

            return HsvToRgb(revTemp, 1, 1);

        }

        public static Color HsvToRgb(double h, double s, double v)
        {
            int hi = (int)Math.Floor(h / 60.0) % 6;
            double f = (h / 60.0) - Math.Floor(h / 60.0);

            double p = v * (1.0 - s);
            double q = v * (1.0 - (f * s));
            double t = v * (1.0 - ((1.0 - f) * s));

            Color ret;

            switch (hi)
            {
                case 0:
                    ret = GetRgb(v, t, p);
                    break;
                case 1:
                    ret = GetRgb(q, v, p);
                    break;
                case 2:
                    ret = GetRgb(p, v, t);
                    break;
                case 3:
                    ret = GetRgb(p, q, v);
                    break;
                case 4:
                    ret = GetRgb(t, p, v);
                    break;
                case 5:
                    ret = GetRgb(v, p, q);
                    break;
                default:
                    ret = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
                    break;
            }
            return ret;
        }

        public static Color GetRgb(double r, double g, double b)
        {
            return Color.FromArgb(255, (byte)(r * 255.0), (byte)(g * 255.0), (byte)(b * 255.0));
        }

        public void createPanel(int hashNum)
        {
            bool chet = true;
            int row = 0;
            int col = 3;
            for (int i = 1; i <= 60; i += 3)
            {
                int end = i + 2;
                col = 3;
                if (chet)
                {
                    for (int i1 = i; i1 <= end; i1++)
                    {
                        createPanelForm(col, row, i1, hashNum);
                        col++;
                    }
                    chet = !chet;
                }
                else
                {
                    for (int i1 = end; i1 >= i; i1--)
                    {
                        createPanelForm(col, row, i1, hashNum);
                        col++;
                    }
                    chet = !chet;
                }
                row++;
            }
            row = 0;
            chet = true;
            for (int i = 120; i >= 61; i -= 3)
            {
                int end = i - 2;
                col = 2;
                if (chet)
                {
                    for (int i1 = i; i1 >= end; i1--)
                    {
                        createPanelForm(col, row, i1, hashNum);
                        col--;
                    }
                    chet = !chet;
                }
                else
                {
                    for (int i1 = end; i1 <= i; i1++)
                    {
                        createPanelForm(col, row, i1, hashNum);
                        col--;
                    }
                    chet = !chet;
                }
                row++;
            }
        }
        public void createPanelForm(int col, int row, int ch, int hashNum)
        {
            Panel nPanel = new Panel();
            nPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            nPanel.Location = new System.Drawing.Point(291, 1);
            nPanel.Margin = new System.Windows.Forms.Padding(1);
            nPanel.Name = "h"+ hashNum.ToString() + "p"+ch.ToString();
            nPanel.Size = new System.Drawing.Size(56, 41);
            switch (hashNum)
            { 
                case 1: 
                    tableLayoutPanel1.Controls.Add(nPanel, col, row);
                    break;
                case 2:
                    tableLayoutPanel2.Controls.Add(nPanel, col, row);
                    break;
                case 3:
                    tableLayoutPanel3.Controls.Add(nPanel, col, row);
                    break;
            }
                

            Label nText = new Label();
            nPanel.Controls.Add(nText);
            nText.AutoSize = true;
            nText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            nText.ForeColor = System.Drawing.Color.Black;
            nText.Location = new System.Drawing.Point(12, 12);
            nText.MaximumSize = new System.Drawing.Size(33, 15);
            nText.MinimumSize = new System.Drawing.Size(33, 15);
            nText.Name = "h" + hashNum.ToString() + "t" + ch.ToString();
            nText.Size = new System.Drawing.Size(33, 15);
            //nText.TabIndex = 128;
            nText.Text = "-";
            nText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            Label vText = new Label();
            nPanel.Controls.Add(vText);
            vText.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            vText.ForeColor = System.Drawing.Color.Blue;
            vText.Location = new System.Drawing.Point(30, 0);
            vText.MaximumSize = new System.Drawing.Size(28, 15);
            vText.MinimumSize = new System.Drawing.Size(28, 15);
            vText.Name = "h" + hashNum.ToString() + "v" + ch.ToString();
            vText.Size = new System.Drawing.Size(28, 15);
            //vText.TabIndex = 129;
            vText.Text = "-";
            vText.TextAlign = System.Drawing.ContentAlignment.TopRight;

            Label lText = new Label();
            nPanel.Controls.Add(lText);
            lText.AutoSize = true;
            lText.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lText.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            lText.Location = new System.Drawing.Point(-2, -2);
            lText.MaximumSize = new System.Drawing.Size(22, 15);
            lText.MinimumSize = new System.Drawing.Size(22, 15);
            lText.Name = "h" + hashNum.ToString() + "l" + ch.ToString();
            lText.Size = new System.Drawing.Size(22, 15);
            //lText.TabIndex = 127;
            lText.Text = ch.ToString();
            lText.TextAlign = System.Drawing.ContentAlignment.TopLeft;
        }

        private void timerBtn_Click(object sender, EventArgs e)
        {
            if (!workTimer.Enabled)
            {
                button1_Click_1(null, null);
                workTimer.Enabled = true;
                timerBtn.Text = "Остановить";
            }
            else 
            {
                workTimer.Enabled = false;
                timerBtn.Text = "Запустить";
            }
        }

        private void workTimer_Tick(object sender, EventArgs e)
        {
            workTimer.Enabled = false;
            button1_Click_1(null, null);
            workTimer.Enabled = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            trackLabel.Text = "Задержка: " + trackBar1.Value.ToString() + "сек";
            workTimer.Interval = trackBar1.Value * 1000;
        }
    }
}
