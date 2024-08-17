using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Windows.Input;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using Microsoft.Win32;
using WinTheme;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Compressor
{
    public partial class Form1 : Form
    {
        private UserPreferenceChangedEventHandler UserPreferenceChanged;

        // Constants for the Windows messages
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        // DLL import to release the mouse capture
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        // DLL import to send the message to the Windows procedure
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public Form1()
        {
            InitializeComponent();
            //this.Text = "Media Compressor";
            radioButton1.Checked = true;
            // Set the form's double-buffering to reduce flickering (optional)
            this.DoubleBuffered = true;

            // Attach the mouse down event handler to the form's mouse down event
            this.MouseDown += MainForm_MouseDown;
            comboBox2.SelectedIndex = 0;
            checkBox28.Checked = true;
        }
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            // If the left mouse button is clicked
            if (e.Button == MouseButtons.Left)
            {
                // Release the mouse capture
                ReleaseCapture();
                // Send the message to the Windows procedure to start moving the form
                SendMessage(this.Handle, WM_NCHITTEST, IntPtr.Zero, new IntPtr(HTCAPTION));
            }
        }

        // Override the WndProc method to handle the WM_NCHITTEST message
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST)
            {
                if ((int)m.Result == HTCLIENT)
                {
                    m.Result = (IntPtr)HTCAPTION;
                }
            }
        }

        private void LoadTheme()
        {
            //var themeColor = WinTheme.GetAccentColor();//Windows Accent Color
            var themeColor = WinTheme.WinTheme.GetAccentColor();
            var lightColor = ControlPaint.Light(themeColor);
            var darkColor = ControlPaint.Dark(themeColor);
            BackColor = themeColor;
            foreach (System.Windows.Forms.Button button in this.Controls.OfType<System.Windows.Forms.Button>())
            {
                button.BackColor = themeColor;
                button.ForeColor = Color.Black;
            }
            foreach (Control control in this.Controls)
            {
                if (control is Panel || control is Label || control is System.Windows.Forms.RadioButton || control is System.Windows.Forms.CheckBox)
                {
                    control.ForeColor = Color.Black;
                }
            }
        }
        private void LoadLightTheme()
        {
            
            //var themeColor = WinTheme.GetAccentColor();//Windows Accent Color
            var themeColor = WinTheme.WinTheme.GetAccentColor();
            var lightColor = ControlPaint.Light(themeColor);
            var darkColor = ControlPaint.Dark(themeColor);
            BackColor = lightColor;
            foreach (System.Windows.Forms.Button button in this.Controls.OfType<System.Windows.Forms.Button>())
            {
                button.BackColor = lightColor;
                button.ForeColor = Color.Black;
            }
            foreach (Control control in this.Controls)
            {
                if (control is Panel || control is Label || control is System.Windows.Forms.RadioButton || control is System.Windows.Forms.CheckBox)
                {
                    control.ForeColor = Color.Black;
                }
            }
        }
        private void LoadDarkTheme()
        {
            //var themeColor = WinTheme.GetAccentColor();//Windows Accent Color
            var themeColor = WinTheme.WinTheme.GetAccentColor();
            var lightColor = ControlPaint.Light(themeColor);
            var darkColor = ControlPaint.Dark(themeColor);
            BackColor = darkColor;
            foreach (System.Windows.Forms.Button button in this.Controls.OfType<System.Windows.Forms.Button>())
            {
                button.BackColor = darkColor;
                button.ForeColor = Color.White;
            }
            foreach (Control control in this.Controls)
            {
                if (control is Panel || control is Label || control is System.Windows.Forms.RadioButton || control is System.Windows.Forms.CheckBox)
                {
                    control.ForeColor = Color.White;
                }
            }
        }
        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General || e.Category == UserPreferenceCategory.VisualStyle)
            {
                LoadTheme();
            }
        }
        private void Form_Disposed(object sender, EventArgs e)
        {
            SystemEvents.UserPreferenceChanged -= UserPreferenceChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = listBox1.Items.Count;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            richTextBox1.Clear();
            richTextBox1.Focus();
            var stopwatch = new Stopwatch();


            if (listBox1.Items.Count > 0)
            {
                for (int NumberOfListBox = 0; NumberOfListBox < listBox1.Items.Count; NumberOfListBox++)
                {
                    stopwatch.Reset();
                    stopwatch.Start();
                    Process p = new Process();
                    Process p2 = new Process();
                    p.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
                    p2.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
                    string FilePath = "";
                    int length = int.Parse(textBox1.Text);
                    int width = int.Parse(textBox2.Text);
                    string bitarte = textBox3.Text;
                    int fps = int.Parse(textBox4.Text);
                    string OutputFile = textBox5.Text;

                    FilePath = listBox1.Items[NumberOfListBox].ToString();
                    string ParentFolder = Directory.GetParent(FilePath).FullName;
                    string addressfiles = Path.Combine(ParentFolder, Path.GetFileName(FilePath));
                    string filename = Path.GetFileName(addressfiles);
                    string filenamewithoutextension = Path.GetFileNameWithoutExtension(addressfiles);
                    string extentionfile = Path.GetExtension(addressfiles);
                    string addressfileswithoutextension = Path.Combine(ParentFolder + "\\" + filenamewithoutextension);

                    string pathnew = Path.Combine(ParentFolder + "\\New Converted File\\");
                    System.IO.Directory.CreateDirectory(pathnew);


                    if (checkBox20.Checked)
                        p.StartInfo.Arguments = ("/C ffmpeg -i " + "\"" + addressfiles + "\"");
                    else
                        p.StartInfo.Arguments = ("/C ffmpeg-bar -i " + "\"" + addressfiles + "\"");

                    if (checkBox4.Checked && checkBox8.Checked)
                    {
                        p.StartInfo.Arguments = p.StartInfo.Arguments + (" -c:v libx264 -map_metadata 0 ");
                    }
                    if (checkBox4.Checked && checkBox9.Checked)
                    {
                        p.StartInfo.Arguments = p.StartInfo.Arguments + (" -c:v libx265 -map_metadata 0 ");
                    }
                    if (checkBox4.Checked && checkBox10.Checked)
                    {
                        p.StartInfo.Arguments = p.StartInfo.Arguments + (" -c:v libvpx-vp9 -map_metadata 0 ");
                    }

                    if (checkBox2.Checked && checkBox8.Checked)
                    {
                        p.StartInfo.Arguments = p.StartInfo.Arguments + (" -c:v h264_nvenc -map_metadata 0 ");
                    }
                    if (checkBox2.Checked && checkBox9.Checked)
                    {
                        p.StartInfo.Arguments = p.StartInfo.Arguments + (" -c:v hevc_nvenc -map_metadata 0 ");
                    }
                    if (checkBox2.Checked && checkBox10.Checked)
                    {
                        MessageBox.Show("Is not avilabel");
                        return;
                    }

                    if (checkBox11.Checked && checkBox8.Checked)
                    {
                        p.StartInfo.Arguments = ("/C ffmpeg -hwaccel cuda -hwaccel_output_format cuda -i " + "\"" + addressfiles + "\"" + " -c:v h264_nvenc -map_metadata 0 ");
                    }
                    if (checkBox11.Checked && checkBox9.Checked)
                    {
                        p.StartInfo.Arguments = ("/C ffmpeg -hwaccel cuda -hwaccel_output_format cuda -i " + "\"" + addressfiles + "\"" + " -c:v hevc_nvenc -map_metadata 0 ");
                    }
                    if (checkBox11.Checked && checkBox10.Checked)
                    {
                        MessageBox.Show("Is not avilabel");
                        return;
                    }

                    if (checkBox6.Checked && checkBox8.Checked)
                    {
                        p.StartInfo.Arguments = p.StartInfo.Arguments + (" -c:v h264_amf -map_metadata 0 ");
                    }
                    if (checkBox6.Checked && checkBox9.Checked)
                    {
                        p.StartInfo.Arguments = p.StartInfo.Arguments + (" -c:v hevc_amf -map_metadata 0 ");
                    }
                    if (checkBox6.Checked && checkBox10.Checked)
                    {
                        MessageBox.Show("Is not avilabel");
                        return;
                    }

                    if (checkBox13.Checked)
                    {
                        p.StartInfo.Arguments = p.StartInfo.Arguments + (" -r " + fps + " ");
                    }
                    if (checkBox14.Checked)
                    {
                        if (checkBox5.Checked)
                        {
                            p.StartInfo.Arguments = p.StartInfo.Arguments + ("-vf scale=" + "\"" + length + ":" + width + ",transpose = 1" + "\" ");
                            if (checkBox13.Checked)
                            {
                                p.StartInfo.Arguments = p.StartInfo.Arguments + ("-r " + fps + " ");
                            }
                        }
                        else
                        {
                            p.StartInfo.Arguments = p.StartInfo.Arguments + ("-vf " + "\"" + "transpose = 1" + "\" ");
                            if (checkBox13.Checked)
                            {
                                p.StartInfo.Arguments = p.StartInfo.Arguments + ("-r " + fps + " ");
                            }
                        }
                    }
                    if (checkBox15.Checked)
                    {
                        if (checkBox5.Checked)
                        {
                            p.StartInfo.Arguments = p.StartInfo.Arguments + ("-vf scale=" + "\"" + length + ":" + width + ",transpose = 2" + "\" ");
                            if (checkBox13.Checked)
                            {
                                p.StartInfo.Arguments = p.StartInfo.Arguments + ("-r " + fps + " ");
                            }
                        }
                        else
                        {
                            p.StartInfo.Arguments = p.StartInfo.Arguments + ("-vf " + "\"" + "transpose = 2" + "\" ");
                            if (checkBox13.Checked)
                            {
                                p.StartInfo.Arguments = p.StartInfo.Arguments + ("-r " + fps + " ");
                            }
                        }
                    }
                    if (checkBox16.Checked)
                    {
                        if (checkBox5.Checked)
                        {
                            p.StartInfo.Arguments = p.StartInfo.Arguments + ("-vf scale=" + "\"" + length + ":" + width + ",transpose = 2,transpose = 2" + "\" ");
                            if (checkBox13.Checked)
                            {
                                p.StartInfo.Arguments = p.StartInfo.Arguments + ("-r " + fps + " ");
                            }
                        }
                        else
                        {
                            p.StartInfo.Arguments = p.StartInfo.Arguments + ("-vf " + "\"" + "transpose = 2,transpose = 2" + "\" ");
                            if (checkBox13.Checked)
                            {
                                p.StartInfo.Arguments = p.StartInfo.Arguments + ("-r " + fps + " ");
                            }
                        }
                    }
                    if (checkBox5.Checked)
                    {
                        if (checkBox16.Checked == false && checkBox15.Checked == false && checkBox14.Checked == false)
                            p.StartInfo.Arguments = p.StartInfo.Arguments + ("-vf scale=" + "\"" + length + ":" + width + "\" ");
                    }
                    if (checkBox31.Checked)
                    {
                        p.StartInfo.Arguments = p.StartInfo.Arguments + ("-preset " + comboBox1.SelectedItem.ToString() + " -crf " + textBox12.Text.ToString() + " ");
                    }

                    if (checkBox19.Checked)
                    {
                        p.StartInfo.Arguments = p.StartInfo.Arguments + ("-filter:v " + "\"" + "setpts =" + 1 / double.Parse(textBox6.Text) + "*PTS" + "\" -an ");
                    }


                    if (checkBox7.Checked)
                    {
                        p.StartInfo.Arguments = p.StartInfo.Arguments + ("-b:v " + bitarte + " ");
                    }
                    if (checkBox12.Checked)
                    {
                        p.StartInfo.Arguments = p.StartInfo.Arguments + ("-an ");
                    }
                    if (checkBox17.Checked)
                    {
                        string ModifedFilename = textBox5.Text;
                        if (checkBox40.Checked == false)
                        {
                            ModifedFilename = "out_ % 1d.png";
                        }

                        if (checkBox9.Checked)
                        {
                            p.StartInfo.Arguments = ("/C ffmpeg -i " + "\"" + ParentFolder + "\\" + ModifedFilename + "\"" + " -vcodec libx265 -vf scale=" + textBox1.Text + ":-2 -r " + fps + " -y -an " + "\"" + addressfileswithoutextension + ".mp4" + "\"");
                        }
                        else
                        {
                            p.StartInfo.Arguments = ("/C ffmpeg -i " + "\"" + ParentFolder + "\\" + ModifedFilename + "\"" + " -vcodec libx264 -vf scale=" + textBox1.Text + ":-2 -r " + fps + " -y -an " + "\"" + addressfileswithoutextension + ".mp4" + "\"");
                        }
                        p.StartInfo.Arguments = ("/C ffmpeg -i " + "\"" + addressfiles + "\"" + " -vf fps=" + fps + " " + "\"" + pathnew + OutputFile + "\"");
                    }
                    if (checkBox18.Checked)
                    {
                        string ModifedFilename = textBox5.Text;
                        if (checkBox40.Checked == false)
                        {
                            ModifedFilename = ModifyFilename(filename);
                        }

                        if (checkBox9.Checked)
                        {
                            p.StartInfo.Arguments = ("/C ffmpeg -i " + "\"" + ParentFolder + "\\" + ModifedFilename + "\"" + " -vcodec libx265 -vf scale=" + textBox1.Text + ":-2 -r " + fps + " -y -an " + "\"" + addressfileswithoutextension + ".mp4" + "\"");
                        }
                        else
                        {
                            p.StartInfo.Arguments = ("/C ffmpeg -i " + "\"" + ParentFolder + "\\" + ModifedFilename + "\"" + " -vcodec libx264 -vf scale=" + textBox1.Text + ":-2 -r " + fps + " -y -an " + "\"" + addressfileswithoutextension + ".mp4" + "\"");
                        }

                    }

                    if (checkBox25.Checked)
                    {
                        p.StartInfo.Arguments = ("/C ffmpeg -i " + "\"" + addressfiles + "\"" + " -ss " + textBox9.Text + " -to " + textBox11.Text + " -c:v copy ");
                    }

                    string NewFileName = Path.Combine(pathnew + filenamewithoutextension + ".mp4");

                    if (checkBox17.Checked == false && checkBox18.Checked == false)
                    {
                        p.StartInfo.Arguments = p.StartInfo.Arguments + ("\"" + NewFileName);
                    }
                    string extImage = extentionfile;
                    if (checkBox21.Checked)
                    {

                        if (checkBox22.Checked)
                        {
                            extImage = ".JPG";
                        }
                        NewFileName = Path.Combine(pathnew + filenamewithoutextension + extImage);
                        p.StartInfo.Arguments = ("/C ffmpeg -hide_banner -i " + "\"" + addressfiles + "\"");
                        if (checkBox36.Checked)
                                    {
                    p.StartInfo.Arguments = p.StartInfo.Arguments + (" -vf rotate=" + textBox7.Text + "*PI/180");
                }
                        if (comboBox2.Text=="90")
                        { p.StartInfo.Arguments = p.StartInfo.Arguments + (" -vf transpose=1");
                        }
                        if(comboBox2.Text == "180")
                        {
                            p.StartInfo.Arguments = p.StartInfo.Arguments + (" -vf transpose=1,transpose=1");
                        }
                        if (comboBox2.Text == "-90")
                        {
                            p.StartInfo.Arguments = p.StartInfo.Arguments + (" -vf transpose=2");
                        }
                        if (checkBox37.Checked)
                        {
                            p.StartInfo.Arguments = p.StartInfo.Arguments + (" -vf hflip");
                        }
                        if (checkBox38.Checked)
                        {
                            p.StartInfo.Arguments = p.StartInfo.Arguments + (" -vf vflip");
                        }
                        if (checkBox23.Checked == false)
                        {
                            if (checkBox35.Checked == false)
                                p.StartInfo.Arguments = p.StartInfo.Arguments + (" \"" + Path.Combine(pathnew + filenamewithoutextension + extImage));
                            else
                                p.StartInfo.Arguments = p.StartInfo.Arguments + (" -qscale:v 1 " + " \"" + Path.Combine(pathnew + filenamewithoutextension + extImage));
                        }
                        else
                        {
                            if (checkBox35.Checked == false)
                                p.StartInfo.Arguments = p.StartInfo.Arguments + (" -vf scale=" + "\"" + textBox8.Text + ":-1" + "\"" + " \"" + Path.Combine(pathnew + filenamewithoutextension + extImage));
                            else
                                p.StartInfo.Arguments = p.StartInfo.Arguments + (" -qscale:v 1 " + " -vf scale=" + "\"" + textBox8.Text + ":-1" + "\"" + " \"" + Path.Combine(pathnew + filenamewithoutextension + extImage));
                        }
                        

                            if (checkBox32.Checked)
                        {
                            p2.StartInfo.Arguments = ("/C exiftool -tagsFromFile " + "\"" + addressfiles + "\"" + " -FileModifyDate " + " \"" + Path.Combine(pathnew + filenamewithoutextension + extImage) + "\"");
                        }
                    }

                    if (checkBox26.Checked)
                    {
                        p.StartInfo.Arguments = ("/C ffmpeg -i " + "\"" + addressfiles + "\"");
                        if (checkBox39.Checked)
                        {
                            p.StartInfo.Arguments = p.StartInfo.Arguments + (" -af highpass = f = 200, lowpass = f = 3000, equalizer = f = 1000:t = q:w = 1:g = 5, compand = attacks = 0:points = -80 / -900 | -20 / -20 | 0 / -10 | 20 | 20:soft - knee = 6, afftdn = nf = -20"+ " \"" +NewFileName);
                        }
                        if (checkBox27.Checked)
                        {
                            NewFileName = Path.Combine(pathnew + filenamewithoutextension + ".mp3");
                            p.StartInfo.Arguments = p.StartInfo.Arguments+(" -map 0:a:0 -b:a " + textBox10.Text + "k " + " \"" + Path.Combine(pathnew + filenamewithoutextension + ".mp3"));
                        }
                    }
                    if (checkBox41.Checked)
                    {
                        p.StartInfo.Arguments = ("/C ffmpeg -i " + "\"" + addressfiles + "\"" + " -map 0:a -acodec copy " + "\"" + Path.Combine(pathnew + filenamewithoutextension + "_audio.mp4") );
                    }
                    if (checkBox42.Checked)
                    {
                        p.StartInfo.Arguments = ("/C ffmpeg -i " + "\"" + addressfiles + "\"" + " -i " + "\"" + Path.Combine(Directory.GetParent(listBox1.Items[NumberOfListBox + 1].ToString()).FullName, Path.GetFileName(listBox1.Items[NumberOfListBox + 1].ToString())) + "\"" + " -c copy "+ "\""+ Path.Combine(pathnew + filenamewithoutextension + ".mp4"));
                    }
                    if (checkBox29.Checked)
                    {
                        p.StartInfo.Arguments = ("/C ffmpeg -i " + "\"" + addressfiles + "\"" + " -f srt -i " + "\"" + addressfileswithoutextension + ".srt" + "\"" + " -map 0:0 -map 0:1 -map 1:0 -c:v copy -c:a copy -c:s mov_text " + "\"" + NewFileName);
                    }
                    p.StartInfo.UseShellExecute = false;
                    if (checkBox24.Checked == false)
                        p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardInput = true;
                    if (checkBox30.Checked == true)
                    {
                        richTextBox1.AppendText(p.StartInfo.Arguments);
                        break;
                    }
                    if (checkBox32.Checked && checkBox28.Checked)
                    {
                        p2.StartInfo.UseShellExecute = false;
                        p2.StartInfo.CreateNoWindow = true;
                        p2.StartInfo.RedirectStandardOutput = true;
                        p2.StartInfo.RedirectStandardInput = true;
                        p2.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                        p2.StartInfo.Arguments = ("/C exiftool -overwrite_original -tagsfromfile " + "\"" + addressfiles + "\"" + " -all:all " + " \"" + Path.Combine(pathnew + filenamewithoutextension + ".mp4") + "\"");
                        p2.Start();
                        p2.WaitForExit();
                    }
                    if (checkBox32.Checked && checkBox21.Checked)
                    {
                        p2.StartInfo.UseShellExecute = false;
                        p2.StartInfo.CreateNoWindow = true;
                        p2.StartInfo.RedirectStandardOutput = true;
                        p2.StartInfo.RedirectStandardInput = true;
                        p2.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                        p2.Start();
                    }
                    if (checkBox34.Checked==false)
                    {
                        Thread t = new Thread(() =>
                        {
                            p.Start();
                            p.WaitForExit();
                        });
                        t.Start();
                        t.Join();
                        if (checkBox32.Checked || checkBox33.Checked)
                        {
                            p2.Start();
                            p2.WaitForExit();
                        }
                    }
                    if (checkBox34.Checked)
                        RunProcessInThread(p.StartInfo.Arguments, p2.StartInfo.Arguments);

                    progressBar1.PerformStep();
                    stopwatch.Stop();
                    label2.Text = (NumberOfListBox + 1).ToString();
                    richTextBox1.AppendText(((NumberOfListBox + 1).ToString() + "- Done " + Convert.ToString(NumberOfListBox + 1) + "/" + listBox1.Items.Count.ToString() + ", in " + stopwatch.ElapsedMilliseconds.ToString() + "ms :" + addressfiles + "\r\n"));
                    if (checkBox3.Checked)
                    {
                        File.Delete(addressfiles);
                    }
                    if (checkBox1.Checked)
                    {
                        p.WaitForExit();
                        Process.Start("shutdown", "/s /t 0");
                    }
                }
            }
        }

        private void RunProcessInThread(string arguments, string arguments2)
        {
            // Create a new thread to run the process
            Thread thread = new Thread(() =>
            {
                try
                {
                    // Create a new process and start it
                    Process p = new Process();
                    string processPath = "C:\\Windows\\system32\\cmd.exe";
                    string processPath2 = "C:\\Windows\\system32\\cmd.exe";
                    p.StartInfo.FileName = processPath;
                    p.StartInfo.Arguments = arguments;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                    p.Start();
                    // Wait for the process to exit
                    p.WaitForExit();
                    Process p2 = new Process();
                    p2.StartInfo.FileName = processPath2;
                    p2.StartInfo.Arguments = arguments2;
                    p2.StartInfo.UseShellExecute = false;
                    p2.StartInfo.CreateNoWindow = true;
                    p2.StartInfo.RedirectStandardOutput = true;
                    p2.StartInfo.RedirectStandardInput = true;
                    p2.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                    p2.Start();
                    p2.WaitForExit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error running the process: {ex.Message}");
                }
            });

            // Start the thread
            thread.Start();
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
                listBox1.Items.Add(file);
            label1.Text = listBox1.Items.Count.ToString();
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                checkBox2.Checked = false;
                checkBox11.Checked = false;
                checkBox6.Checked = false;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox4.Checked = false;
                checkBox11.Checked = false;
                checkBox6.Checked = false;
            }
        }
        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked)
            {
                checkBox4.Checked = false;
                checkBox2.Checked = false;
                checkBox6.Checked = false;
            }
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                checkBox4.Checked = false;
                checkBox11.Checked = false;
                checkBox2.Checked = false;
            }
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                checkBox9.Checked = false;
                checkBox10.Checked = false;
            }
            if (checkBox8.Checked==false && checkBox9.Checked == false && checkBox10.Checked == false)
            {
                checkBox8.Checked = true;
            }
        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked)
            {
                checkBox8.Checked = false;
                checkBox10.Checked = false;
            }
            if (checkBox8.Checked == false && checkBox9.Checked == false && checkBox10.Checked == false)
            {
                checkBox8.Checked = true;
            }
        }
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                checkBox8.Checked = false;
                checkBox9.Checked = false;
            }
            if (checkBox8.Checked == false && checkBox9.Checked == false && checkBox10.Checked == false)
            {
                checkBox8.Checked = true;
            }
        }

        private void listBox1_MouseDoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count != 0)
            {
                while (listBox1.SelectedIndex != -1)
                {
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                }
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                textBox1.Visible = true;
                textBox2.Visible = true;
            }
            else
            {
                textBox1.Visible = false;
                textBox2.Visible = false;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                textBox3.Visible = true;
            }
            else
            {
                textBox3.Visible = false;
            }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox13.Checked)
            {
                textBox4.Visible = true;
            }
            else
            {
                textBox4.Visible = false;
            }
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox19.Checked)
            {
                textBox6.Visible = true;
                label8.Visible = true;
            }
            else
            {
                textBox6.Visible = false;
                label8.Visible = false;
            }
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox25.Checked)
            {
                textBox9.Visible = true;
                label11.Visible = true;
                textBox11.Visible = true;
                checkBox17.Checked = false;
                checkBox18.Checked = false;
            }
            else
            {
                textBox9.Visible = false;
                label11.Visible = false;
                textBox11.Visible = false;
            }
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox21.Checked)
            {
                checkBox22.Visible = true;
                checkBox23.Visible = true;
                textBox8.Visible = false;
                checkBox28.Checked = false;
                checkBox26.Checked = false;
                checkBox32.Visible = true;
                checkBox35.Visible = true;
                checkBox34.Visible = true;
                checkBox36.Visible = true;
                textBox7.Visible = false;
                checkBox37.Visible = true;
                checkBox38.Visible = true;
                comboBox2.Visible= true;
                panel1.Size = new System.Drawing.Size(270, 120);
                panel3.Size = new System.Drawing.Size(270, 30);
                panel2.Size = new System.Drawing.Size(270, 30);

            }
            else
            {
                checkBox22.Visible = false;
                checkBox23.Visible = false;
                textBox8.Visible = false;
                checkBox32.Visible = false;
                checkBox35.Visible = false;
                checkBox34.Visible = false;
                checkBox36.Visible = false;
                textBox7.Visible = false;
                checkBox37.Visible = false;
                checkBox38.Visible = false;
                comboBox2.Visible = false;
                if (checkBox26.Checked == false)
                    checkBox28.Checked = true;
            }
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox26.Checked)
            {
                checkBox27.Visible = true;
                label10.Visible = true;
                textBox10.Visible = true;
                checkBox21.Checked = false;
                checkBox28.Checked = false;
                checkBox39.Visible = true;
                panel1.Size = new System.Drawing.Size(270, 30);
                panel2.Size = new System.Drawing.Size(270, 30);
                panel3.Size = new System.Drawing.Size(270, 44);
            }
            else
            {
                checkBox27.Visible = false;
                label10.Visible = false;
                textBox10.Visible = false;
                checkBox39.Visible = false;
                if (checkBox21.Checked == false)
                    checkBox28.Checked = true;
            }
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox23.Checked)
            {
                textBox8.Visible = true;
            }
            else
            {
                textBox8.Visible = false;
            }
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox24.Checked)
            {
                checkBox20.Visible = true;
            }
            else
            {
                checkBox20.Visible = false;
            }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox14.Checked)
            {
                checkBox15.Checked = false;
                checkBox16.Checked = false;
            }
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox15.Checked)
            {
                checkBox14.Checked = false;
                checkBox16.Checked = false;
            }
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox16.Checked)
            {
                checkBox14.Checked = false;
                checkBox15.Checked = false;
            }
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox28.Checked)
            {
                checkBox21.Checked = false;
                checkBox26.Checked = false;
                checkBox31.Visible = true;
                checkBox4.Visible = true;
                checkBox5.Visible = true;
                checkBox2.Visible = true;
                checkBox7.Visible = true;
                checkBox33.Visible = true;
                checkBox11.Visible = true;
                checkBox12.Visible = true;
                checkBox13.Visible = true;
                textBox4.Visible = false;
                checkBox6.Visible = true;
                checkBox8.Visible = true;
                checkBox9.Visible = true;
                checkBox10.Visible = true;
                label6.Visible = true;
                checkBox14.Visible = true;
                checkBox15.Visible = true;
                checkBox16.Visible = true;
                label7.Visible = true;
                checkBox19.Visible = true;
                checkBox24.Visible = true;
                checkBox17.Visible = true;
                textBox5.Visible = false;
                checkBox18.Visible = true;
                checkBox25.Visible = true;
                textBox9.Visible = false;
                label11.Visible = false;
                textBox11.Visible = false;
                checkBox29.Visible = true;
                panel2.Size = new System.Drawing.Size(270, 250);
                panel1.Size = new System.Drawing.Size(270, 30);
                panel3.Size = new System.Drawing.Size(270, 30);
                checkBox41.Visible = true;
                checkBox42.Visible = true;
                checkBox41.Checked = false;
                checkBox42.Checked = false;
            }
            else
            {
                comboBox1.Visible = false;
                checkBox31.Visible = false;
                textBox12.Visible = false;
                checkBox4.Visible = false;
                checkBox5.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                checkBox2.Visible = false;
                checkBox7.Visible = false;
                textBox3.Visible = false;
                checkBox33.Visible = false;
                checkBox11.Visible = false;
                checkBox12.Visible = false;
                checkBox13.Visible = false;
                textBox4.Visible = false;
                checkBox6.Visible = false;
                checkBox8.Visible = false;
                checkBox9.Visible = false;
                checkBox10.Visible = false;
                label6.Visible = false;
                checkBox14.Visible = false;
                checkBox15.Visible = false;
                checkBox16.Visible = false;
                checkBox20.Visible = false;
                label7.Visible = false;
                checkBox19.Visible = false;
                textBox6.Visible = false;
                label8.Visible = false;
                checkBox24.Visible = false;
                checkBox17.Visible = false;
                textBox5.Visible = false;
                checkBox18.Visible = false;
                checkBox25.Visible = false;
                textBox9.Visible = false;
                label11.Visible = false;
                textBox11.Visible = false;
                checkBox29.Visible = false;
                checkBox41.Visible = false;
                checkBox42.Visible = false;
            }
            
        }
        public static string ModifyFilename(string filename)
        {
            Regex regex = new Regex(@"(.*)_(\d+)(\..*)");
            Match match = regex.Match(filename);
            if (match.Success)
            {
                string namePart = match.Groups[1].Value;
                int digitCount = match.Groups[2].Value.Length;
                string extensionPart = match.Groups[3].Value;
                return $"{namePart}_%{digitCount}d{extensionPart}";
            }
            return filename; // Return original filename if pattern doesn't match
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox17.Checked)
            {
                checkBox13.Checked = true;
                checkBox4.Checked = false;
                checkBox18.Checked = false;
                checkBox40.Visible = true;
                checkBox25.Checked = false;
            }
            if (checkBox17.Checked == false)
            {
                checkBox40.Visible = false;
                checkBox40.Checked =false;
                textBox5.Visible = false;
            }
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox18.Checked)
            {
                checkBox13.Checked = true;
                checkBox5.Checked = true;
                checkBox4.Checked = false;
                checkBox17.Checked = false;
                checkBox40.Visible = true;
                checkBox25.Checked = false;
            }
            if (checkBox18.Checked==false)
            {
                checkBox40.Visible = false;
                textBox5.Visible = false;
                checkBox40.Checked = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText(("1-Drag and drop file to the box" + "\r\n"));
            richTextBox1.AppendText(("2-choose format of files that you want convert" + "\r\n"));
            richTextBox1.AppendText(("3-click convert button" + "\r\n"));
            richTextBox1.AppendText(("this app is based on FFMPEG and is not for commercial goals" + "\r\n"));
            richTextBox1.AppendText(("*************************************************" + "\r\n"));
            richTextBox1.AppendText(("Video:" + "\r\n" + "CPU: for cpu intel converting" + "\r\n" + "Nvidia: for cpu as same as gpu converting" + "\r\n" + "Just CUDA: for only GPU convrting" + "\r\n" + "AMD: for AMD proccessor converting" + "\r\n"));
            richTextBox1.AppendText(("Video to image seq.: convert a video file to fps images in second" + "\r\n"));
            richTextBox1.AppendText(("Image seq. to video: convert collection of images to video file with fps in second" + "\r\n"));
            richTextBox1.AppendText(("*************************************************" + "\r\n"));
            richTextBox1.AppendText(("For Image convert, select Image" + "\r\n"));
            richTextBox1.AppendText(("*************************************************" + "\r\n"));
            richTextBox1.AppendText(("For Sound convert, select Audio" + "\r\n"));
            richTextBox1.AppendText(("*************************************************" + "\r\n"));
            richTextBox1.AppendText(("If farsi in name of file is existed, for using Exiftool, you must change number, click F2E No." + "\r\n"));
        }

        private void richTextBox1_DoubleClick(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int NumberOfListBox = 0; NumberOfListBox < listBox1.Items.Count; NumberOfListBox++)
            {
                string FilePath = listBox1.Items[NumberOfListBox].ToString();
                //string filePath = Directory.GetParent(FilePath).FullName;
                //ChangePersianNumbersInFilenames(ParentFolder);
                string fileName = Path.GetFileName(FilePath);
                string englishNumberFileName = ReplacePersianWithEnglishNumbers(fileName);
                string newFilePath = Path.Combine(Path.GetDirectoryName(FilePath), englishNumberFileName);
                File.Move(FilePath, newFilePath);
                Console.WriteLine($"Renamed: {FilePath} to {newFilePath}");
            }

        }
        static string ReplacePersianWithEnglishNumbers(string input)
        {
            // Define a mapping for Persian and English numbers
            string persianNumbers = "۰۱۲۳۴۵۶۷۸۹";
            string englishNumbers = "0123456789";
            // Create a translation dictionary
            var translation = new Dictionary<char, char>();
            for (int i = 0; i < persianNumbers.Length; i++)
            {
                translation[persianNumbers[i]] = englishNumbers[i];
            }
            // Use a regular expression to replace Persian numbers with English numbers
            string pattern = "[" + string.Join("", persianNumbers) + "]";
            string converted = Regex.Replace(input, pattern, m => translation[m.Value[0]].ToString());
            return converted;
        }

        private void checkBox33_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox33.Checked)
                checkBox32.Checked = true;
            if (checkBox33.Checked == false)
                checkBox32.Checked =false;
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox32.Checked)
                checkBox33.Checked = true;
            if (checkBox32.Checked == false)
                checkBox33.Checked = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/RE-MO-SH?tab=repositories");
        }

        private void checkBox33_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox33.Checked)
            {
                checkBox32.Checked = true;
            }
            else
            {   checkBox32.Checked = false;
            }
        }

        private void checkBox32_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox32.Checked)
            {
                checkBox33.Checked = true;
            }
            else
            {   checkBox33.Checked = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true; // Allow multiple file selection

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected files
                    string[] files = openFileDialog.FileNames;
                    foreach (string file in files)
                    {
                        // Add each file to the ListBox
                        listBox1.Items.Add(file);
                    }

                    // Update the label to show the count of items in the ListBox
                    listBox1.Text = listBox1.Items.Count.ToString();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected folder path
                    string folderPath = folderBrowserDialog.SelectedPath;

                    // Get all files in the selected folder
                    string[] files = Directory.GetFiles(folderPath);
                    foreach (string file in files)
                    {
                        // Add each file to the ListBox
                        listBox1.Items.Add(file);
                    }

                    // Update the label to show the count of items in the ListBox
                    listBox1.Text = listBox1.Items.Count.ToString();
                }
            }
        }

        private void checkBox39_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox39.Checked)
            {
                checkBox27.Checked = false;
            }
            else
            {
                checkBox27.Checked = true;
            }
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox27.Checked)
            {
                checkBox39.Checked = false;
            }
            else
            {
                checkBox39.Checked = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

            LoadTheme();
            UserPreferenceChanged = new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            SystemEvents.UserPreferenceChanged += UserPreferenceChanged;
            this.Disposed += new EventHandler(Form_Disposed);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            
            LoadLightTheme();
            UserPreferenceChanged = new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            SystemEvents.UserPreferenceChanged += UserPreferenceChanged;
            this.Disposed += new EventHandler(Form_Disposed);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            LoadDarkTheme();
            UserPreferenceChanged = new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            SystemEvents.UserPreferenceChanged += UserPreferenceChanged;
            this.Disposed += new EventHandler(Form_Disposed);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox40_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox40.Checked)
            {
                textBox5.Visible = true;
            }
            if (checkBox40.Checked==false)
            {
                textBox5.Visible = false;
            }
        }

        private void checkBox36_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox36.Checked)
            {
                textBox7.Visible = true;
            }
            else
            {
                textBox7.Visible = false;
            }
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox31.Checked) { textBox12.Visible= true; comboBox1.Visible = true; comboBox1.SelectedIndex = 2; } else { textBox12.Visible = false; comboBox1.Visible = false; };
        }

        private void checkBox41_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox41.Checked)
                checkBox42.Checked = false;
        }

        private void checkBox42_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox42.Checked)
                checkBox41.Checked= false;
        }
    }
    public class ProcessRunner
    {
        public void RunProcessInThread(string arguments, string arguments2)
        {
            // Create a new thread to run the process
            Thread thread = new Thread(() =>
            {
                try
                {
                    // Create a new process and start it
                    Process process = new Process();
                   string processPath = "C:\\Windows\\system32\\cmd.exe";
                    string processPath2 = "C:\\Windows\\system32\\cmd.exe";
                    process.StartInfo.FileName = processPath;
                    process.StartInfo.Arguments = arguments;
                    process.Start();

                    // Wait for the process to exit
                    process.WaitForExit();
                    Process process2 = new Process();
                    process2.StartInfo.FileName = processPath2;
                    process2.StartInfo.Arguments = arguments2;
                    process2.Start();
                    process2.WaitForExit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error running the process: {ex.Message}");
                }
            });

            // Start the thread
            thread.Start();
        }
    }
}