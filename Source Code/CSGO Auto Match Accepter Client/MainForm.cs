using System;
using System.Collections.Generic;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

namespace CSGO_Auto_Match_Accepter_Client
{
    public partial class MainForm : Form
    {
        static Bitmap currentScreen;
        static int screenW = Screen.PrimaryScreen.Bounds.Width;
        static int screenH = Screen.PrimaryScreen.Bounds.Height;
        static Color acceptColor_1 = Color.FromArgb(76, 176, 80);
        static Color acceptColor_2 = Color.FromArgb(76, 175, 80);
        static bool toContinue = true;
        static string resolution = "16:9";
        static ToolStripStatusLabel _statusLabel;
        static TextBox _numberTextBox;
        static bool sendSMS = false;

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }

        public MainForm()
        {
            InitializeComponent();
            StatusLabel.Text = "";
            _statusLabel = StatusLabel;
            _numberTextBox = numberTextBox;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _statusLabel.Text = "Starting to analize the screen";
            toContinue = true;
            var t = new System.Threading.Timer(OnTimer, null, 0, 1000);
        }

        private static void OnTimer(Object obj)
        {
            if (toContinue)
            {
                Console.WriteLine("Timer on: " + DateTime.Now);

                takeScreenShot();
                analizeScreenShot();

                GC.Collect();
            }
            else
                return;
        }

        private static void takeScreenShot()
        {
            Bitmap bmp = new Bitmap(screenW, screenH);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                currentScreen = bmp;
            }
        }

        private static void analizeScreenShot()
        {
            // All resolutions
            // Center = 930 x 630

            // 16:9
            // 1 = 930 x 600
            // 2 = 930 x 650

            // 4:3
            // 1 = 930 x 610
            // 2 = 930 x 665

            Color currentColor = currentScreen.GetPixel(930, 630);
            Color currentColor_1 = new Color();
            Color currentColor_2 = new Color();

            switch (resolution)
            {
                case "16:9":
                    currentColor_1 = currentScreen.GetPixel(930, 600);
                    currentColor_2 = currentScreen.GetPixel(930, 650);
                    break;
                case "4:3":
                    currentColor_1 = currentScreen.GetPixel(930, 610);
                    currentColor_2 = currentScreen.GetPixel(930, 665);
                    break;
                default: break;
            }


            if (currentColor.Equals(acceptColor_1) || currentColor_1.Equals(acceptColor_1) || currentColor_2.Equals(acceptColor_1) || currentColor.Equals(acceptColor_2) || currentColor_1.Equals(acceptColor_2) || currentColor_2.Equals(acceptColor_2))
            {
                Cursor.Position = new Point(930, 630);
                mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
                toContinue = false;
                _statusLabel.Text = "Match Accepted";

                if (sendSMS && string.IsNullOrEmpty(_numberTextBox.Text) && !_numberTextBox.Text.StartsWith("+"))
                {
                    if (string.IsNullOrEmpty(_numberTextBox.Text))
                    {
                        _statusLabel.Text = "Please type your phone number (with country code and no spaces)";
                    }

                    string accountSid = "ACf3417293e6adf8e089e0202d0d6505bc";
                    string authToken = "37cbf1bc85ad883915bc5d667471b00f";

                    TwilioClient.Init(accountSid, authToken);

                    try
                    {
                        var message = MessageResource.Create(
                            body: "\n\nCSGO Auto Match Accepter \n\n A match has been found and auto accepted",
                            from: new Twilio.Types.PhoneNumber("+19312723940"),
                            to: new Twilio.Types.PhoneNumber(_numberTextBox.Text)
                        );

                        _statusLabel.Text = "Message Sent!";
                    }
                    catch (ApiException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine($"Twilio Error {e.Code} - {e.MoreInfo}");
                    }

                    Console.Write("Press any key to continue.");
                    Console.ReadKey();
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton2.Checked = false;
                resolution = "16:9";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton1.Checked = false;
                resolution = "4:3";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            toContinue = false;
            _statusLabel.Text = "Analysis Stopped";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                sendSMS = false;
                return;
            }  
            if (string.IsNullOrEmpty(numberTextBox.Text))
            {
                checkBox1.Checked = false;
                MessageBox.Show("You have not written down your phone number. Write it including your country's code (i.e.: +1, +51)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            sendSMS = true;
        }
    }
}
