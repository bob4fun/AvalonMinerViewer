using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Security;

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
            workTimer.Enabled = false;
            timerBtn.Text = "Запустить";
            readDataFromMiner(null);
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
            catch 
            { 
                //System.Windows.Forms.MessageBox.Show("Ошибка, заполнения массива");
            }
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
            catch 
            { 
            }
            return null;
        }

        private static string[] getPVTN(int num_plate, string strToFind, int hashNum)
        {
            try
            {
                hashNum -= 1;
                string rightPart = strToFind.Substring(strToFind.IndexOf("MW" + hashNum.ToString()));
                rightPart = rightPart.Substring(rightPart.IndexOf('[') + 1);
                rightPart = rightPart.Substring(0, rightPart.IndexOf(']'));

                string[] temps = rightPart.Split(new[] { " " }, StringSplitOptions.None);

                return temps;

            }
            catch
            {
            }
            return null;
        }

        public static Color tempRGB(int temp)
        {
            if (temp < 30)
                return HsvToRgb(240, 1, 1);
            else if (temp > 95)
                return HsvToRgb(0, 1, 1);

            int revTemp = (95 - temp) * 4;

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

        public void showData(int hashNum, string[] words)
        {
            string[] hash0data = null;
            string[] temps = null;
            string[] volts = null;
            string[] nonce = null;
            int hash = hashNum-1;

            foreach (var word in words)
            {
                System.Console.WriteLine($"<{word}>");
                if (word.IndexOf("PVT_V"+ hash.ToString()) != -1)
                {
                    volts = getPVTV(0, word, hashNum);
                }
                if (word.IndexOf("PVT_T" + hash.ToString()) != -1)
                {
                    temps = getPVTT(0, word, hashNum);
                }
                if (word.IndexOf("MW" + hash.ToString()) != -1)
                {
                    nonce = getPVTN(0, word, hashNum);
                }
            }
            if (temps != null)
                for (int i = 1; i < temps.Length + 1; i++)
                {
                    foreach (Control pageCntrl in tabPage2.Controls)
                    {
                        if (pageCntrl.Name == "hashPanel"+ hashNum.ToString())
                            foreach (Control panel in pageCntrl.Controls)
                            {

                                if (panel.Name == "h" + hashNum.ToString() + "p" + i.ToString())
                                {
                                        try
                                        {
                                            panel.BackColor = tempRGB(Int32.Parse(temps[i - 1]));
                                        }
                                        catch
                                        {
                                        panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                                    }
                                    foreach (Control tempLabel in panel.Controls)
                                    {
                                        if (tempLabel.Name == "h" + hashNum.ToString() + "t" + i.ToString())
                                            try
                                            {
                                                if (temps!=null)
                                                    if (temps[i - 1] != null)
                                                        tempLabel.Text = temps[i - 1].ToString();
                                                    else
                                                        tempLabel.Text = "-";
                                            }
                                            catch
                                            {
                                                tempLabel.Text = "-";
                                            }
                                        if (tempLabel.Name == "h" + hashNum.ToString() + "v" + i.ToString())
                                            try
                                            {
                                                if (volts!=null)
                                                    if (volts[i - 1] != null)
                                                        tempLabel.Text = volts[i - 1].ToString();
                                                    else
                                                        tempLabel.Text = "-";
                                            }
                                            catch
                                            {
                                                tempLabel.Text = "-";
                                            }
                                        if (tempLabel.Name == "h" + hashNum.ToString() + "n" + i.ToString())
                                            try
                                            {
                                                if (nonce != null)
                                                    if (nonce[i - 1] != null)
                                                        tempLabel.Text = nonce[i - 1].ToString();
                                                    else
                                                        tempLabel.Text = "-";
                                            }
                                            catch
                                            {
                                                tempLabel.Text = "-";
                                            }
                                    }
                                }
                            }
                    }
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
                    hashPanel1.Controls.Add(nPanel, col, row);
                    break;
                case 2:
                    hashPanel2.Controls.Add(nPanel, col, row);
                    break;
                case 3:
                    hashPanel3.Controls.Add(nPanel, col, row);
                    break;
            }

            Label tText = new Label();
            nPanel.Controls.Add(tText);
            tText.AutoSize = true;
            tText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            tText.ForeColor = System.Drawing.Color.Black;
            tText.Location = new System.Drawing.Point(1, 9);
            tText.MaximumSize = new System.Drawing.Size(40, 16);
            tText.MinimumSize = new System.Drawing.Size(40, 16);
            tText.Name = "h" + hashNum.ToString() + "t" + ch.ToString();
            //nText.BorderStyle = BorderStyle.FixedSingle;
            //nText.TabIndex = 128;
            tText.Text = "-";
            tText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            Label nText = new Label();
            nPanel.Controls.Add(nText);
            nText.AutoSize = true;
            nText.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            nText.ForeColor = System.Drawing.Color.Black;
            nText.Location = new System.Drawing.Point(1, 20);
            nText.MaximumSize = new System.Drawing.Size(40, 16);
            nText.MinimumSize = new System.Drawing.Size(40, 16);
            nText.Name = "h" + hashNum.ToString() + "n" + ch.ToString();
            //nText.BorderStyle = BorderStyle.FixedSingle;
            //nText.TabIndex = 128;
            nText.Text = "-";
            nText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            Label vText = new Label();
            nPanel.Controls.Add(vText);
            vText.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            vText.ForeColor = System.Drawing.Color.Black;
            vText.Location = new System.Drawing.Point(17, -1);
            vText.MaximumSize = new System.Drawing.Size(28, 15);
            vText.MinimumSize = new System.Drawing.Size(28, 15);
            vText.Name = "h" + hashNum.ToString() + "v" + ch.ToString();
            vText.Size = new System.Drawing.Size(28, 15);
            //vText.TabIndex = 129;
            vText.Text = "-";
            vText.TextAlign = System.Drawing.ContentAlignment.TopRight;

            Label lText = new Label();
            lText.AutoSize = true;
            nPanel.Controls.Add(lText);
            lText.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
                timerBtn.Text = "Остановить";
                workTimer.Enabled = true;
                readDataFromMiner(null);
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
            readDataFromMiner(null);
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

        private void hashPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private string getParameter(string words, string parameter, string value)
        {
            string rightPart = null;
            System.Console.WriteLine($"<{words}>");
            if (words.IndexOf(parameter) != -1)
            {
                try
                {
                    rightPart = words.Substring(words.IndexOf(parameter));
                    rightPart = rightPart.Substring(rightPart.IndexOf('[') + 1);
                    rightPart = rightPart.Substring(0, rightPart.IndexOf(']'));
                    return getParameterValue(rightPart, value);
                }
                catch
                {
                }
            }
            return null;
        }

        private string getParameter(string words, string parameter, int index)
        {
            string rightPart = null;
            System.Console.WriteLine($"<{words}>");
            if (words.IndexOf(parameter) != -1)
            {
                try
                {
                    rightPart = words.Substring(words.IndexOf(parameter));
                    rightPart = rightPart.Substring(rightPart.IndexOf('[') + 1);
                    rightPart = rightPart.Substring(0, rightPart.IndexOf(']'));
                    return getParameterValue(rightPart, index);
                }
                catch
                {
                }
            }
            return null;
        }

        private string getParameter(string words, string parameter)
        {
            string rightPart = null;
            System.Console.WriteLine($"<{words}>");
            if (words.IndexOf(parameter) != -1)
            {
                try
                {
                    rightPart = words.Substring(words.IndexOf(parameter));
                    rightPart = rightPart.Substring(rightPart.IndexOf('[') + 1);
                    rightPart = rightPart.Substring(0, rightPart.IndexOf(']'));
                    return rightPart;
                }
                catch
                {
                }
            }
            return null;
        }

        private string getParameterValue(string text, string value)
        {

            char[] delimiterChars = { '|', ',', '\t', '[', ']', ':' };
            bool nextString = false;

            System.Console.WriteLine($"Original text: '{text}'");

            string[] words = text.Split(delimiterChars);

            foreach (string word in words)
            { 
                if (nextString)
                    return word.Trim();
                if (word.Trim() == value)
                    nextString = true;
            }
            return null;
        }

        private string getParameterValue(string text, int index)
        {

            char[] delimiterChars = { '|', ',', '\t', '[', ']', ':', ' '};
            bool nextString = false;

            System.Console.WriteLine($"Original text: '{text}'");

            string[] words = text.Split(delimiterChars);

            try
            {
                return words[index];
            }
            catch
            { }
            return null;
        }

        private bool[] getErrorArray(int error)
        {
            bool[] errorArray = new bool[18];
            //try
            {
                string binary = Convert.ToString(error, 2);
                binary = binary.PadLeft(18, '0');
                for (int i = binary.Length-1; i > 0; i--)
                {
                    string currentChar = binary.Substring(i, 1);
                    if (binary.Substring(i,1) == "1")
                        errorArray[i] = true;
                }
            }
            //catch (Exception ex)
            { }

            return errorArray;
        }

        private string[] getErrorStringArray(bool[] errors)
        {

            if (errors != null && errors.Length == 18)
            { 
                int idx = 18;
                int newCount = 0; 
                string[] arrayErrors = new string[18];
                foreach (bool error in errors)
                {
                    switch (idx)
                    {
                        case 18:
                            if (error)
                            {
                                arrayErrors[idx - 1] = "HU configration check failed";
                                newCount++;
                            }
                            break;
                        case 17:
                            if (error)
                            {
                                arrayErrors[idx - 1] = "PLL configration check failed";
                                newCount++;
                            }
                            break;
                        case 16:
                            if (error)
                            {
                                arrayErrors[idx - 1] = "It's just a notice";
                                newCount++;
                            }
                            break;
                        case 15:
                            if (error)
                            {
                                arrayErrors[idx - 1] = "Voltage output sensor check failed";
                                newCount++;
                            }
                            break;
                        case 14:
                            if (error)
                            {
                                arrayErrors[idx - 1] = "Voltage input sensor check failed";
                                newCount++;
                            }
                            break;
                        case 13:
                            if (error)
                            {
                                arrayErrors[idx - 1] = "Temperature sensor check failed";
                                newCount++;
                            }
                            break;
                        case 12:
                            if (error)
                            {
                                arrayErrors[idx - 1] = "Power not good";
                                newCount++;
                            }
                            break;
                        case 11:
                            if (error)
                            { 
                                arrayErrors[idx - 1] = "Invalid pmu firmware version or pmu is broken";
                                newCount++;
                            }
                            break;
                        case 10:
                            if (error)
                            {
                                arrayErrors[idx - 1] = "Core test failed";
                                newCount++;
                            }
                            break;
                        case 9:
                            if (error)
                            { 
                                arrayErrors[idx - 1] = "loopback test failed";
                                newCount++;
                            }
                            break;
                        case 8:
                            if (error)
                            { 
                                arrayErrors[idx - 1] = "Toohot was found on MM";
                                    newCount++;
                            }
                            break;
                        case 7:
                            if (error)
                            { 
                                arrayErrors[idx - 1] = "The modular or mm board is too hot";
                                newCount++;
                            }
                            break;
                        case 6:
                            if (error)
                            { 
                                arrayErrors[idx - 1] = "Nonce ringbuffer overflow";
                                newCount++;
                            }
                            break;
                        case 5:
                            if (error)
                            { 
                                arrayErrors[idx - 1] = "API fifo overflow flag";
                                newCount++;
                            }
                            break;
                        case 4:
                            if (error)
                            { 
                                arrayErrors[idx - 1] = "Reserved";
                                newCount++;
                            }
                            break;
                        case 3:
                            if (error)
                            { 
                                arrayErrors[idx - 1] = "Fan cann't be found";
                                newCount++;
                            }
                            break;
                        case 2:
                            if (error)
                            { 
                                arrayErrors[idx - 1] = "IIC rx irc mismatch was found";
                                newCount++;
                            }
                            break;
                        case 1:
                            if (error)
                            { 
                                arrayErrors[idx - 1] = "Idle";
                                newCount++;
                            }
                            break;
                    }
                    idx--;
                }
                if (newCount > 0)
                {
                    string[] newArray = new string[newCount];
                    idx = 0;
                    foreach (string item in arrayErrors)
                    {
                        if (item != null)
                        {
                            newArray[idx] = item;
                            idx++;
                        }
                    }
                    return newArray;
                }
            }
            return null;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click_1(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void ECHU1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openLog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sr = new StreamReader(openLog.FileName);
                    outText.Text = sr.ReadToEnd();
                    logFile.Text = openLog.FileName;
                    readDataFromMiner(outText.Text);
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }

        }

        private void readDataFromMiner(string txtData)
        {
            outText.Text = "";
            string host = minerIP.Text;
            int port = Int32.Parse(minerPort.Text);
            string command = minerCommand.Text;

            GetSocket getSocket = new GetSocket();

            string result = null;

            if (txtData == null)
                result = GetSocket.SocketSendReceive(host, port, command);
            else
                result = txtData;

            if (result == "Connection failed" || result == null)
            {
                workTimer.Enabled = false;
                workTimer.Stop();
                timerBtn.Text = "Запустить";
                return;
            }

            char[] delimiterChars = { '|', ',', '\t' };

            string text = result;
            System.Console.WriteLine($"Original text: '{text}'");

            string[] words = text.Split(delimiterChars);
            System.Console.WriteLine($"{words.Length} words in text:");
            //try
            {
                if (getParameter(result, "Ver") != null)
                    ver.Text = getParameter(result, "Ver");
                if (getParameter(result, "PING") != null)
                    ping.Text = getParameter(result, "PING") + "мс";
                if (getParameter(result, "SYSTEMSTATU", "Work") != null)
                    systemStatus.Text = getParameter(result, "SYSTEMSTATU", "Work");
                if (getParameter(result, "SYSTEMSTATU", "Hash Board") != null)
                    hashCount.Text = getParameter(result, "SYSTEMSTATU", "Hash Board");
                if (getParameter(result, "Core") != null)
                    coreType.Text = getParameter(result, "Core");
                if (getParameter(result, "Temp") != null)
                    tempIn.Text = getParameter(result, "Temp");
                if (tempRGB(Int32.Parse(tempIn.Text)) != null)
                    tempIn.BackColor = tempRGB(Int32.Parse(tempIn.Text));
                if (getParameter(result, "TAvg") != null)
                {
                    tempAverage.Text = getParameter(result, "TAvg");
                    tempAverage.BackColor = tempRGB(Int32.Parse(tempAverage.Text));
                }
                if (getParameter(result, "TMax") != null)
                {
                    tempMax.Text = getParameter(result, "TMax");
                    tempMax.BackColor = tempRGB(Int32.Parse(tempMax.Text));
                }
                if (getParameter(result, "Fan1") != null && getParameter(result, "Fan2") != null && getParameter(result, "Fan3") != null && getParameter(result, "Fan4") != null && getParameter(result, "FanR") != null)
                    fan1.Text = getParameter(result, "Fan1") + " / " + getParameter(result, "Fan2") + " / " + getParameter(result, "Fan3") + " / " + getParameter(result, "Fan4") + " - " + getParameter(result, "FanR");

                double volt = 0;
                if (getParameter(result, "PS", 1) != null)
                {
                    volt = Convert.ToDouble(getParameter(result, "PS", 1)) / 100;
                    psuControl.Text = volt.ToString() + "V";
                }
                if (getParameter(result, "PS", 0) != null)
                {
                    if (getParameter(result, "PS", 0) == "0")
                        psuError.Text = "Нет";
                    else
                        psuError.Text = getParameter(result, "PS", 0);
                }
                if (getParameter(result, "PS", 2) != null)
                { 
                    volt = Convert.ToDouble(getParameter(result, "PS", 2)) / 100;
                    psu3.Text = volt.ToString() + "V";
                }
                if (getParameter(result, "PS", 3)!= null)
                    psu4.Text = getParameter(result, "PS", 3) + "A"; ;
                if (getParameter(result, "PS", 4) != null)
                    psu5.Text = getParameter(result, "PS", 4) + "W";
                if (getParameter(result, "PS", 5) != null)
                {
                    volt = Convert.ToDouble(getParameter(result, "PS", 5)) / 100;
                    psu6.Text = volt.ToString() + "V";
                }
                if (getParameter(result, "PS", 6) != null)
                    psu7.Text = getParameter(result, "PS", 6) + "W";
                if (getParameter(result, "MTmax", 0) != null)
                {
                    tempMaxHash1.Text = getParameter(result, "MTmax", 0);
                    tempMaxHash1.BackColor = tempRGB(Int32.Parse(tempMaxHash1.Text));
                }
                if (getParameter(result, "MTmax", 1) != null)
                {
                    tempMaxHash2.Text = getParameter(result, "MTmax", 1);
                    tempMaxHash2.BackColor = tempRGB(Int32.Parse(tempMaxHash2.Text));
                }

                currentFreq.Text = "0Mhz";
                if (getParameter(result, "Freq") != null)
                    currentFreq.Text = getParameter(result, "Freq") + "Mhz";
                    

                GHSavg.Text = "-";
                if (getParameter(result, "GHSavg") != null)
                {
                    double ghsAvg = Convert.ToDouble(getParameter(result, "GHSavg").Replace('.', ',')) / 1000;
                    GHSavg.Text = String.Format("{0:0.##}", ghsAvg) + " TH/s";
                }


                GHSspd.Text = "-";
                if (getParameter(result, "GHSspd") != null)
                {
                    double ghsSpd = Convert.ToDouble(getParameter(result, "GHSspd").Replace('.', ',')) / 1000;
                    GHSspd.Text = String.Format("{0:0.##}", ghsSpd) + " TH/s";
                }

                string MGHS1 = "0";
                string MGHS2 = "0";
                string MGHS3 = "0";


                if (getParameter(result, "MGHS", 0) != null)
                {
                    double mghs1 = Convert.ToDouble(getParameter(result, "MGHS", 0).Replace('.', ',')) / 1000;
                    MGHS1 = String.Format("{0:0.##}", mghs1);
                }

                if (getParameter(result, "MGHS", 1) != null)
                {
                    double mghs2 = Convert.ToDouble(getParameter(result, "MGHS", 1).Replace('.', ',')) / 1000;
                    MGHS2 = String.Format("{0:0.##}", mghs2);
                }
                if (getParameter(result, "MGHS", 2) != null)
                {
                    double mghs3 = Convert.ToDouble(getParameter(result, "MGHS", 2).Replace('.', ',')) / 1000;
                    MGHS3 = String.Format("{0:0.##}", mghs3);
                }

                MGHS.Text = MGHS2 + " / "+ MGHS3 + " / " + MGHS1 + " TH/s ";

                SF0.Text = "-";
                if (getParameter(result, "SF0", 0) != null && getParameter(result, "SF0", 2) != null && getParameter(result, "SF0", 2) != null && getParameter(result, "SF0", 3) != null)
                {
                    SF0.Text = getParameter(result, "SF0", 0) + "/" + getParameter(result, "SF0", 1) + "/" + getParameter(result, "SF0", 2) + "/" + getParameter(result, "SF0", 3);
                }
                SF1.Text = "-";
                if (getParameter(result, "SF1", 0) != null && getParameter(result, "SF1", 2) != null && getParameter(result, "SF1", 2) != null && getParameter(result, "SF1", 3) != null)
                {
                    SF1.Text = getParameter(result, "SF1", 0) + "/" + getParameter(result, "SF1", 1) + "/" + getParameter(result, "SF1", 2) + "/" + getParameter(result, "SF1", 3);
                }
                SF2.Text = "-";
                if (getParameter(result, "SF2", 0) != null && getParameter(result, "SF2", 2) != null && getParameter(result, "SF2", 2) != null && getParameter(result, "SF2", 3) != null)
                {
                    SF2.Text = getParameter(result, "SF2", 0) + "/" + getParameter(result, "SF2", 1) + "/" + getParameter(result, "SF2", 2) + "/" + getParameter(result, "SF2", 3);
                }

                PLL0.Text = "-";
                if (getParameter(result, "PLL0", 0) != null && getParameter(result, "PLL0", 2) != null && getParameter(result, "PLL0", 2) != null && getParameter(result, "PLL0", 3) != null)
                {
                    PLL0.Text = getParameter(result, "PLL0", 0) + "/" + getParameter(result, "PLL0", 1) + "/" + getParameter(result, "PLL0", 2) + "/" + getParameter(result, "PLL0", 3);
                }
                PLL1.Text = "-";
                if (getParameter(result, "PLL1", 0) != null && getParameter(result, "PLL1", 2) != null && getParameter(result, "PLL1", 2) != null && getParameter(result, "PLL1", 3) != null)
                {
                    PLL1.Text = getParameter(result, "PLL1", 0) + "/" + getParameter(result, "PLL1", 1) + "/" + getParameter(result, "PLL1", 2) + "/" + getParameter(result, "PLL1", 3);
                }
                PLL2.Text = "-";
                if (getParameter(result, "PLL2", 0) != null && getParameter(result, "PLL2", 2) != null && getParameter(result, "PLL2", 2) != null && getParameter(result, "PLL2", 3) != null)
                {
                    PLL2.Text = getParameter(result, "PLL2", 0) + "/" + getParameter(result, "PLL2", 1) + "/" + getParameter(result, "PLL2", 2) + "/" + getParameter(result, "PLL2", 3);
                }

                tempMaxHash3.Visible = false;
                tempMaxHash3.Text = "-";
                if (getParameter(result, "MTmax", 2) != null)
                {
                    tempMaxHash3.Text = getParameter(result, "MTmax", 2);
                    tempMaxHash3.BackColor = tempRGB(Int32.Parse(tempMaxHash3.Text));
                    tempMaxHash3.Visible = true;
                }

                if (getParameter(result, "BOOTBY") != null)
                    switch (getParameter(result, "BOOTBY"))
                    {
                        case "0x00.00000000":
                            BOOTBY.Text = "AM_BOOTBY_CLEAR";
                            break;
                        case "0x01.00000000":
                            BOOTBY.Text = "Включение/выключение";
                            break;
                        case "0x02.00000000":
                            BOOTBY.Text = "Перегрев";
                            break;
                        case "0x03.00000000":
                            BOOTBY.Text = "Проблемы с сетью";
                            break;
                        case "0x04.00000000":
                            BOOTBY.Text = "Перезагрузка по сети";
                            break;
                        case "0x05.00000000":
                            BOOTBY.Text = "API перезагрузка";
                            break;
                        case "0x06.00000000":
                            BOOTBY.Text = "AM_BOOTBY_OPTIONS";
                            break;
                        case "0x07.00000000":
                            BOOTBY.Text = "AM_BOOTBY_POLLING";
                            break;
                        case "0x08.00000000":
                            BOOTBY.Text = "Неактивен пул";
                            break;
                        case "0x09.00000000":
                            BOOTBY.Text = "Мало памяти";
                            break;
                        case "0x10.00000000":
                            BOOTBY.Text = "Неизвестный";
                            break;
                        case "0x11.00000000":
                            BOOTBY.Text = "AM_BOOTBY_CLEAR";
                            break;
                        case "0x12.00000000":
                            BOOTBY.Text = "Нет хешрейта 5мин.";
                            break;
                        case "0x21.00000000":
                            BOOTBY.Text = "Программня перезагрузка";
                            break;
                        default:
                            BOOTBY.Text = getParameter(result, "BOOTBY");
                            break;
                    }

                int curECHU = 0;
                bool[] echuBool = null;
                string[] errors = null;
                bool first = true;

                ECMM.Items.Clear();
                if (getParameter(result, "ECMM") != null)
                {
                    curECHU = Int32.Parse(getParameter(result, "ECMM"));
                    echuBool = getErrorArray(curECHU);
                    errors = getErrorStringArray(echuBool);
                    first = true;
                    if (errors != null)
                    {
                        foreach (string error in errors)
                        {
                            ECMM.Items.Add(error);
                        }
                    }
                    else
                        ECMM.Items.Add("Нет");
                }
                ECMM.SelectedIndex = 0;

                ECHU1.Items.Clear();
                if (getParameter(result, "ECHU", 0) != null)
                {
                    curECHU = Int32.Parse(getParameter(result, "ECHU", 0));
                    echuBool = getErrorArray(curECHU);
                    errors = getErrorStringArray(echuBool);
                    first = true;
                    if (errors != null)
                    {
                        foreach (string error in errors)
                        {
                            ECHU1.Items.Add(error);
                        }
                        ECHU1.SelectedIndex = 0;
                    }
                    else
                    {
                        ECHU1.Items.Add("Нет");
                        ECHU1.SelectedIndex = 0;
                    }
                }

                ECHU2.Items.Clear();
                if (getParameter(result, "ECHU", 1) != null)
                {
                    curECHU = Int32.Parse(getParameter(result, "ECHU", 1));
                    echuBool = getErrorArray(curECHU);
                    errors = getErrorStringArray(echuBool);
                    first = true;
                    if (errors != null)
                    {
                        foreach (string error in errors)
                        {
                            ECHU2.Items.Add(error);
                        }
                        ECHU2.SelectedIndex = 0;
                    }
                    else
                    {
                        ECHU2.Items.Add("Нет");
                        ECHU2.SelectedIndex = 0;
                    }
                }

                ECHU3.Items.Clear();
                if (getParameter(result, "ECHU", 2) != null)
                {
                    curECHU = Int32.Parse(getParameter(result, "ECHU", 2));
                    echuBool = getErrorArray(curECHU);
                    errors = getErrorStringArray(echuBool);
                    first = true;
                    if (errors != null)
                    {
                        foreach (string error in errors)
                        {
                            ECHU3.Items.Add(error);
                        }
                        ECHU3.SelectedIndex = 0;
                    }
                    else
                    {
                        ECHU3.Items.Add("Нет");
                        ECHU3.SelectedIndex = 0;
                    }
                }


                if (getParameter(result, "ADJ") != null)
                {
                    int adjStatus = Int32.Parse(getParameter(result, "ADJ"));
                    switch (adjStatus)
                    {
                        case 2:
                            adjing.Text = "Закончен";
                            adjBar.Visible = true;
                            adjBar.Value = 100;
                            break;
                        case 1:
                            adjing.Text = "Отключен";
                            adjBar.Visible = false;
                            break;
                        case 0:
                            adjing.Text = getParameter(result, "Bar");
                            if (getParameter(result, "Bar") != null)
                            {
                                adjBar.Visible = true;
                                adjBar.Value = Int32.Parse(getParameter(result, "Bar").Trim('%'));
                            }
                            else
                                adjBar.Visible = false;
                            break;
                    }
                }
            }
            //catch { }

            Elapsed.Text = "-";
            if (getParameter(result, "Elapsed[") != null)
            {
                int elapsed = Int32.Parse(getParameter(result, "Elapsed["));
                TimeSpan time = TimeSpan.FromSeconds(elapsed);

                Elapsed.Text = time.ToString(@"hh\:mm\:ss");
            }

            WORKMODE.Text = "-";
            if (getParameter(result, "WORKMODE[") != null)
            {
                int WM = Int32.Parse(getParameter(result, "WORKMODE["));
                switch (WM)
                {
                    case 0:
                        WORKMODE.Text = "Normal mode";
                        break;
                    case 1:
                        WORKMODE.Text = "High perfomance";
                        break;
                    default:
                        WORKMODE.Text = "-";
                        break;
                }
            }

            showData(1, words);
            showData(2, words);
            showData(3, words);

            if (checkParse.Checked)
                outText.Lines = words;
            else
                outText.Text = result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            newForm.StartPosition = FormStartPosition.CenterParent;
            newForm.ShowDialog();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {


        }

        private void logFile_DragOver(object sender, DragEventArgs e)
        {

        }

        private void Form1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[]; // get all files droppeds  
            if (files != null && files.Any())
            {
                logFile.Text = files.First(); //select the first one  
                var sr = new StreamReader(files.First());
                outText.Text = sr.ReadToEnd();
                workTimer.Enabled = false;
                workTimer.Stop();
                timerBtn.Text = "Запустить";
                readDataFromMiner(outText.Text);
            }
        }

        private void label27_Click(object sender, EventArgs e)
        {

        }
    }
}

