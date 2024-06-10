using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using ZXing;
using System.IO;
using System.Diagnostics;
using Emgu.CV.CvEnum;
using System.Collections.Generic;
using Emgu.CV.Util;
using Microsoft.VisualBasic.Devices;
using System.Linq;
using System.Threading;
using System.Reflection;

namespace camera_show_denali {
    public partial class Form1 : Form {
        private string head = "1";
        private string steptest;
        private int process_value = 170;
        private bool flag_process = false;
        private VideoCapture capture = null;
        private Image<Bgr, Byte> img;
        private static Rectangle rect;
        private Stopwatch timeout = new Stopwatch();
        private Stopwatch timeout_show = new Stopwatch();
        private int time_out = 10000;
        private bool debug = true;
        private bool flag_set_camera = false;
        private bool steptest_fail = false;
        private int crop = 30;
        private bool steptest_camera_check_led_red_green = false;
        private Bgr bgr_low;
        private Bgr bgr_high;
        private Hsv hsv_low;
        private Hsv hsv_high;
        private bool flag_hsv = false;
        private Image<Hsv, Byte> img_hsv;
        private bool flag_hsv_test = false;
        private int hsv_mask = 10;
        private int hsv_timeout = 100;
        private Stopwatch stopwatch_hsv_timeout = new Stopwatch();
        private bool flag_result = false;
        private string result_blackup = "";
        private string flag_set_port = "";
        private bool flag_add_step = false;
        private VideoCapture.API captureApi = VideoCapture.API.Any;

        public Form1() {
            InitializeComponent();
            head = File.ReadAllText("../../config_cameraDenali/head.txt");
            File.WriteAllText("call_exe_tric.txt", "");
            try
            { flag_set_port = File.ReadAllText("set_port.txt"); } catch { }
            try
            { flag_add_step = Convert.ToBoolean(File.ReadAllText("add_step.txt")); } catch { }
            File.Delete("add_step.txt");
            try
            { steptest = File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_steptest.txt"); } catch { }
            ComputerInfo computerInfo = new ComputerInfo();
            if (!computerInfo.OSFullName.Contains("Windows 7"))
                captureApi = VideoCapture.API.DShow;
            if (flag_set_port == "set port")
            {
                File.Delete("set_port.txt");
                head = File.ReadAllText("camera_head_set_port.txt");
                Form f2 = new Form();
                f2.Size = new Size(100, 100);
                ComboBox c = new ComboBox();
                c.Size = new Size(60, 7);
                for (int i = 0; i < 9; i++)
                {
                    capture = new VideoCapture(i, captureApi);
                    if (capture.Width != 0)
                        c.Items.Add(i);
                    capture.Dispose();
                }
                f2.Controls.Add(c);
                f2.ShowDialog();
                bool flag_onebyone_check = false;
                try
                { flag_onebyone_check = Convert.ToBoolean(File.ReadAllText("../../config_cameraDenali/camera_show_onebyone" + ".txt")); } catch { }
                if (flag_onebyone_check)
                {
                    File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_port.txt", c.Text);
                }
                else
                {
                    File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_port.txt", c.Text);
                }
                capture = new VideoCapture(Convert.ToInt32(c.Text), captureApi);
            }
            else
            {
                bool flag_while = true;

                try
                {
                    string ppp = File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_port.txt");
                } catch (Exception)
                {
                    flag_while = false;
                }

                if (flag_while == false)
                { MessageBox.Show("_กรุณาเลือก port camera"); capture = new VideoCapture(); }
                show_form_cancel_camera();
                Stopwatch timeout_opencam = new Stopwatch();
                timeout_opencam.Restart();
                while (flag_while)
                {
                    //if (timeout_opencam.ElapsedMilliseconds > 5000) { capture = new VideoCapture(); break; }
                    if (f_cancel_camera.IsDisposed)
                    { Application.Exit(); return; }
                    try
                    {
                        string ff = File.ReadAllText("camera_show_list.txt");
                        if (ff == "")
                            break;
                        if (ff.Substring(0, 1) != head)
                        { Thread.Sleep(50); DelaymS(50); continue; }
                    } catch { Thread.Sleep(50); DelaymS(50); continue; }
                    break;
                }
                while (flag_while)
                {
                    //if (timeout_opencam.ElapsedMilliseconds > 5000) { capture = new VideoCapture(); break; }
                    if (f_cancel_camera.IsDisposed)
                    { Application.Exit(); return; }
                    try
                    {
                        string pathDAFSD = "../../config_cameraDenali/test_head_" + head + "_port.txt";
                        int asddas = Convert.ToInt32(File.ReadAllText(pathDAFSD));
                        capture = new VideoCapture(asddas, captureApi);

                        if (capture == null || capture.Ptr == IntPtr.Zero || capture.Width == 0)
                        { DelaymS(50); Thread.Sleep(200); continue; }
                        break;
                    } catch { Thread.Sleep(50); }
                    DelaymS(50);
                    Thread.Sleep(200);
                }
                timeout_opencam.Stop();
                f_cancel_camera.Close();
            }
            if (steptest.Contains("check_led"))
            {
                steptest_camera_check_led_red_green = true;
            }
            if (steptest_camera_check_led_red_green == true)
            {
                Application.Idle += check_led;
            }
            else
            {
                MessageBox.Show("steptest.txt error : สเต็ปเทสที่ส่งเข้ามาในไฟล์ ไม่ตรงกับ ในตัวโปรแกรม camera.exe");
                File.WriteAllText("test_head_" + head + "_result.txt", "function\r\nFail");
                steptest_fail = true;
            }
            setup();
            File.WriteAllText("camera_show_" + head + "_open_completed.txt", "");
        }
        private Form f_cancel_camera = new Form();
        private void show_form_cancel_camera() {
            f_cancel_camera.Icon = Properties.Resources.icon;
            f_cancel_camera.Size = new Size(200, 70);
            f_cancel_camera.ControlBox = false;
            f_cancel_camera.Text = steptest;
            Button b = new Button();
            b.Click += B_Click;
            b.Size = new Size(75, 30);
            b.Location = new Point(0, 0);
            b.Text = "cancel";
            Button p = new Button();
            p.Click += P_Click;
            p.Size = new Size(75, 30);
            p.Location = new Point(80, 0);
            p.Text = "set port";
            f_cancel_camera.Controls.Add(b);
            f_cancel_camera.Controls.Add(p);
            f_cancel_camera.Show();
            //f_cancel_camera.Location = new Point(0, 0);
        }
        private void P_Click(object sender, EventArgs e) {
            File.WriteAllText("set_port.txt", "set port");
            File.WriteAllText("camera_head_set_port.txt", head);
            Application.Restart();
        }
        private void B_Click(object sender, EventArgs e) {
            steptest_fail = true;
            f_cancel_camera.Close();
        }

        private void setup() {
            if (flag_led_test == 1)
            {
                if (flag_add_step)
                {
                    try
                    { Frame_height.Text = File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_frame_height_" + steptest.Substring(0, steptest.Length - 1) + ".txt"); } catch { }
                    try
                    { Frame_width.Text = File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_frame_width_" + steptest.Substring(0, steptest.Length - 1) + ".txt"); } catch { }
                }
                else
                {
                    try
                    { Frame_height.Text = File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_frame_height_" + steptest + ".txt"); } catch { }
                    try
                    { Frame_width.Text = File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_frame_width_" + steptest + ".txt"); } catch { }
                }
                try
                {
                    SetCapture(CapProp.FrameHeight, Convert.ToInt32(Frame_height.Text));//800
                    SetCapture(CapProp.FrameWidth, Convert.ToInt32(Frame_width.Text));//600
                    SetCapture(CapProp.Zoom, Convert.ToDouble(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_zoom_" + steptest + ".txt")));
                    SetCapture(CapProp.Pan, Convert.ToDouble(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_pan_" + steptest + ".txt")));
                    SetCapture(CapProp.Tilt, Convert.ToDouble(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_tilt_" + steptest + ".txt")));
                    SetCapture(CapProp.Contrast, Convert.ToDouble(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_contrast_" + steptest + ".txt")));
                    SetCapture(CapProp.Brightness, Convert.ToDouble(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_brightness_" + steptest + ".txt")));
                    SetCapture(CapProp.Focus, Convert.ToDouble(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_focus_" + steptest + ".txt")));
                    SetCapture(CapProp.Exposure, Convert.ToDouble(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_exposure_" + steptest + ".txt")));
                    SetCapture(CapProp.Saturation, Convert.ToDouble(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_saturation_" + steptest + ".txt")));
                    SetCapture(CapProp.Sharpness, Convert.ToDouble(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_sharpness_" + steptest + ".txt")));
                    SetCapture(CapProp.Gain, Convert.ToDouble(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_gain_" + steptest + ".txt")));
                    SetCapture(CapProp.Gamma, Convert.ToDouble(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_gamma_" + steptest + ".txt")));
                } catch { }
                try
                { process_value = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_process_" + steptest + ".txt")); } catch { }
                try
                { flag_process = Convert.ToBoolean(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_flag_process_" + steptest + ".txt")); } catch { }
                try
                { debug = Convert.ToBoolean(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_debug.txt")); } catch { }
                try
                { time_out = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_timeout.txt")); } catch { }
                try
                { onebyone.Checked = Convert.ToBoolean(File.ReadAllText("../../config_cameraDenali/camera_show_onebyone" + ".txt")); } catch { }
            }

            string mjh = "";
            try
            { if (ctms_led_no.Text == "2") mjh = "_2"; } catch { }
            try
            { rect.X = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_rect_x_" + steptest + mjh + ".txt")); } catch { }
            try
            { rect.Y = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_rect_y_" + steptest + mjh + ".txt")); } catch { }
            try
            { rect.Width = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_rect_width_" + steptest + mjh + ".txt")); } catch { }
            try
            { rect.Height = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_rect_height_" + steptest + mjh + ".txt")); } catch { }
            try
            {
                if (flag_led_test == 1 || flag_led_test == 4)
                {//แดง 0 60 0 255 150 255
                    hsv_low = new Hsv(0, 0, 150);
                    hsv_high = new Hsv(60, 255, 255);
                }
                if (flag_led_test == 2 || flag_led_test == 5)
                {//เขียว 70 100 100 255 0 50
                    bgr_low = new Bgr(0, 100, 0);
                    bgr_high = new Bgr(100, 150, 50);
                }
                if (flag_led_test == 3 || flag_led_test == 6)
                {//น้ำเงิน 200 210 0 50 0 50
                 //น้ำเงิน 120 220 0 50 0 50 เครื่อง1
                    bgr_low = new Bgr(120, 0, 0);
                    bgr_high = new Bgr(220, 50, 50);
                }
                stopwatch_hsv_timeout.Restart();
            } catch { }
            this.Text = head + "." + steptest;
            this.Size = new Size(capture.Width, capture.Height);
            pictureBox1.Size = new Size(capture.Width, capture.Height);
            timeout.Restart();
        }

        private void Form1_Load(object sender, EventArgs e) {
            if (steptest_fail == true)
                this.Close();
        }

        private void fail_check_led() {
            if (debug == true)
            { timeout.Restart(); return; }
            File.WriteAllText("test_head_" + head + "_result.txt", "time over\r\nFAIL");
            this.Close();
        }

        private int flag_led_test = 1;
        private bool flag_return = false;
        private bool flag_first_test = false;
        private void check_led(object sender, EventArgs e) {
            if (!flag_first_test)
            {
                flag_first_test = true;
                this.Activate();
                if (Form.ActiveForm != this)
                {
                    this.WindowState = FormWindowState.Minimized;
                    this.WindowState = FormWindowState.Normal;
                }
            }

            if (flag_return)
            { flag_return = false; timeout.Restart(); }
            if (timeout.ElapsedMilliseconds >= time_out)
            { fail_check_led(); if (debug == false) return; }
            if (IsMouseDown == true)
                return;
            if (capture == null || capture.Ptr == IntPtr.Zero || capture.Width == 0)
            {
                try
                { capture.Dispose(); } catch { }
                try
                { capture = new VideoCapture(Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_port.txt")), captureApi); } catch { }
                Thread.Sleep(250);
                this.Size = new Size(capture.Width, capture.Height);
                pictureBox1.Size = new Size(capture.Width, capture.Height);
                setup();
                flag_return = true;
                return;
            }
            Mat frame;
            try
            {
                if (flag_hsv_test == false)
                    frame = capture.QueryFrame();
                else
                    frame = new Mat("../../config_cameraDenali/hsv_test.png");
                img = frame.ToImage<Bgr, Byte>();
                img_hsv = frame.ToImage<Hsv, Byte>();
            } catch
            {
                MessageBox.Show("ไม่สามารถเปิดกล้องได้");
                Application.Exit();
                return;
            }
            int redpixels = 0;
            if (flag_led_test == 1 || flag_led_test == 4)
            {//แดง
                Graphics g = Graphics.FromImage(img.Bitmap);
                g.DrawRectangle(Pens.Red, rect);
                Image<Hsv, byte> img_cut2 = null;
                Image<Hsv, byte> img2 = null;
                img_cut2 = img_hsv.Copy();
                try
                { img_cut2.ROI = rect; } catch { }
                img2 = img_cut2.Copy();
                try
                { redpixels = img2.InRange(hsv_low, hsv_high).CountNonzero()[0]; } catch { }
            }
            else
            {//เขียว //น้ำเงิน
                Graphics g = Graphics.FromImage(img.Bitmap);
                if (flag_led_test == 2 || flag_led_test == 5)
                    g.DrawRectangle(Pens.Lime, rect);
                else
                    g.DrawRectangle(Pens.Blue, rect);
                Image<Bgr, byte> img_cut = null;
                Image<Bgr, byte> img1 = null;
                img_cut = img.Copy();
                try
                { img_cut.ROI = rect; } catch { }
                img1 = img_cut.Copy();
                try
                { redpixels = img1.InRange(bgr_low, bgr_high).CountNonzero()[0]; } catch { }
            }
            bool mask = false;
            if (redpixels >= hsv_mask)
            {
                timeout.Restart();
                if (stopwatch_hsv_timeout.ElapsedMilliseconds >= hsv_timeout)
                    mask = true;
            }
            else
            {
                stopwatch_hsv_timeout.Restart();
                mask = false;
            }
            CvInvoke.PutText(img, redpixels.ToString(), new Point(20, 30), FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
            CvInvoke.PutText(img, mask.ToString(), new Point(20, 60), FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
            CvInvoke.PutText(img, stopwatch_hsv_timeout.ElapsedMilliseconds.ToString(), new Point(pictureBox1.Size.Width - 100, 30), FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
            pictureBox1.Image = img.Bitmap;
            if (mask == true)
            {
                if (flag_set_camera == false && debug == false)
                {
                    if (flag_led_test != 7)
                    {
                        flag_led_test++;
                        ctms_flag_test.Text = flag_led_test.ToString();
                        if (flag_led_test == 4)
                            ctms_led_no.Text = "2";
                        setup();
                        timeout.Restart();
                        return;
                    }
                    Thread.Sleep(2000);
                    File.WriteAllText("test_head_" + head + "_result.txt", "Color detected\r\nPASS");
                    this.Close();
                }
                flag_result = true;
                result_blackup = "Color detected";
            }
            else
            {
                flag_result = false;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
            if (flag_result == true)
            {
                if (steptest.Contains("image_compar"))
                    File.WriteAllText("test_head_" + head + "_result.txt", result_blackup + "\r\nNEXT");
                else
                    File.WriteAllText("test_head_" + head + "_result.txt", result_blackup + "\r\nPASS");
            }
            else
            {
                File.WriteAllText("test_head_" + head + "_result.txt", "Unreadable\r\nFAIL");
            }
            this.Close();
        }
        /// <summary>
        /// Checlk password of set gamma address
        /// </summary>
        /// <returns>Ture is password success</returns>
        private bool CheckPasswordSetAddress() {
            while (true)
            {
                KeyPassword form = new KeyPassword();
                DialogResult result = form.ShowDialog();
                if (result != DialogResult.OK)
                {
                    return false;
                }
                if (form.inputValue != "camera")
                {
                    MessageBox.Show("Password Error!!");
                    continue;
                }
                break;
            }
            return true;
        }
        /// <summary>
        /// Set new port camera follow address in csv
        /// </summary>
        /// <param name="portNew">is New port</param>
        private void SetPortCameraFollowAddress(int portNew) {
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_port.txt", portNew.ToString());
        }
        /// <summary>
        /// For gen log error set camera capProp
        /// </summary>
        /// <param name="value">is Value at want set capProp</param>
        /// <param name="capProp">is Type of value at want set</param>
        private void LogSendError(string value, string capProp) {
            string path = "D:\\LogCameraError";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            DateTime now = DateTime.Now;
            StreamWriter swOut = new StreamWriter(path + "\\" + now.Year + "_" + now.Month + ".csv", true);
            string time = now.Day.ToString("00") + ":" + now.Hour.ToString("00") + ":" + now.Minute.ToString("00") + ":" + now.Second.ToString("00");
            swOut.WriteLine(time + ",Value=" + value + ",Head=" + head + ",Step=" + steptest + "," + capProp);
            swOut.Close();
        }
        /// <summary>
        /// Function set camera for set and check error to gen log error csv
        /// </summary>
        /// <param name="capProp">is Type of value at want set</param>
        /// <param name="value">is Value at set in camera</param>
        private void SetCapture(CapProp capProp, double value) {
            if (value == -1)
            {
                return;
            }
            if (!capture.SetCaptureProperty(capProp, value))
            {
                LogSendError(value.ToString(), capProp.ToString());
            }
        }

        Form f1;
        HScrollBar h_zoom;
        Label l_zoom;
        Label s_zoom;
        HScrollBar h_pan;
        Label l_pan;
        Label s_pan;
        HScrollBar h_tilt;
        Label l_tilt;
        Label s_tilt;
        HScrollBar h_contrast;
        Label l_contrast;
        Label s_contrast;
        HScrollBar h_brightness;
        Label l_brightness;
        Label s_brightness;
        HScrollBar h_focus;
        Label l_focus;
        Label s_focus;
        HScrollBar h_process;
        Label l_process;
        Label s_process;
        Button b_process;
        Label l_bgr;
        TextBox t_bgr;
        Label l_hsv;
        TextBox t_hsv;
        Button b_hsv;
        TextBox t_hsv_mask;
        Label l_mask;
        Label l_hsv_test;
        Label l_timeout;
        TextBox t_timeout;
        Button b_example;
        CheckBox show_all;
        HScrollBar h_exposure;
        Label l_exposure;
        Label s_exposure;
        HScrollBar h_saturation;
        Label l_saturation;
        Label s_saturation;
        HScrollBar h_sharpness;
        Label l_sharpness;
        Label s_sharpness;
        HScrollBar h_gain;
        Label l_gain;
        Label s_gain;
        HScrollBar h_gamma;
        Label l_gamma;
        Label s_gamma;
        private void setCameraToolStripMenuItem_Click(object sender, EventArgs e) {
            //if (flag_add_step) return;
            flag_set_camera = true;
            f1 = new Form();
            f1.FormClosed += F1_FormClosed;
            f1.Size = new Size(400, 400);
            h_zoom = new HScrollBar();
            l_zoom = new Label();
            s_zoom = new Label();
            h_pan = new HScrollBar();
            l_pan = new Label();
            s_pan = new Label();
            h_tilt = new HScrollBar();
            l_tilt = new Label();
            s_tilt = new Label();
            h_contrast = new HScrollBar();
            l_contrast = new Label();
            s_contrast = new Label();
            h_brightness = new HScrollBar();
            l_brightness = new Label();
            s_brightness = new Label();
            h_focus = new HScrollBar();
            l_focus = new Label();
            s_focus = new Label();
            h_process = new HScrollBar();
            l_process = new Label();
            s_process = new Label();
            b_process = new Button();
            l_bgr = new Label();
            t_bgr = new TextBox();
            l_hsv = new Label();
            t_hsv = new TextBox();
            b_hsv = new Button();
            t_hsv_mask = new TextBox();
            l_mask = new Label();
            l_hsv_test = new Label();
            l_timeout = new Label();
            t_timeout = new TextBox();
            b_example = new Button();
            show_all = new CheckBox();
            h_exposure = new HScrollBar();
            l_exposure = new Label();
            s_exposure = new Label();
            h_saturation = new HScrollBar();
            l_saturation = new Label();
            s_saturation = new Label();
            h_sharpness = new HScrollBar();
            l_sharpness = new Label();
            s_sharpness = new Label();
            h_gain = new HScrollBar();
            l_gain = new Label();
            s_gain = new Label();
            h_gamma = new HScrollBar();
            l_gamma = new Label();
            s_gamma = new Label();
            string str_read2d = "";
            bool flag_try_min_max_value = false;

            l_zoom.Text = "zoom";
            l_zoom.Size = new Size(50, 15);
            l_zoom.Location = new Point(1, 1);
            f1.Controls.Add(l_zoom);
            h_zoom.Scroll += H_zoom_Scroll;
            h_zoom.LargeChange = 1;
            try
            { h_zoom.Minimum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_zoom_min" + str_read2d + ".txt")); } catch (Exception) { h_zoom.Minimum = -999; }
            try
            { h_zoom.Maximum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_zoom_max" + str_read2d + ".txt")); } catch (Exception) { h_zoom.Maximum = 999; }
            try
            { h_zoom.Value = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_zoom_" + steptest + ".txt")); } catch { flag_try_min_max_value = true; }
            if (flag_try_min_max_value)
            {
                try
                { h_zoom.Value = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Zoom); } catch { }
                flag_try_min_max_value = false;
            }
            h_zoom.Size = new Size(300, h_zoom.Height);
            h_zoom.Location = new Point(1, 15);
            f1.Controls.Add(h_zoom);
            s_zoom.Text = h_zoom.Value.ToString();
            s_zoom.Size = new Size(50, 15);
            s_zoom.Location = new Point(h_zoom.Size.Width + 5, h_zoom.Location.Y + 2);
            f1.Controls.Add(s_zoom);

            l_pan.Text = "pan";
            l_pan.Size = new Size(300, 15);
            l_pan.Location = new Point(1, 40);
            f1.Controls.Add(l_pan);
            h_pan.Scroll += H_pan_Scroll;
            h_pan.LargeChange = 1;
            try
            { h_pan.Minimum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_pan_min" + str_read2d + ".txt")); } catch (Exception) { h_pan.Minimum = -999; }
            try
            { h_pan.Maximum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_pan_max" + str_read2d + ".txt")); } catch (Exception) { h_pan.Maximum = 999; }
            try
            { h_pan.Value = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_pan_" + steptest + ".txt")); } catch { flag_try_min_max_value = true; }
            if (flag_try_min_max_value)
            {
                try
                { h_pan.Value = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Pan); } catch { }
                flag_try_min_max_value = false;
            }
            h_pan.Size = new Size(300, h_pan.Height);
            h_pan.Location = new Point(1, 55);
            f1.Controls.Add(h_pan);
            s_pan.Text = h_pan.Value.ToString();
            s_pan.Size = new Size(300, 15);
            s_pan.Location = new Point(h_pan.Size.Width + 5, h_pan.Location.Y + 2);
            f1.Controls.Add(s_pan);

            l_tilt.Text = "tilt";
            l_tilt.Size = new Size(300, 15);
            l_tilt.Location = new Point(1, 80);
            f1.Controls.Add(l_tilt);
            h_tilt.Scroll += H_tilt_Scroll;
            h_tilt.LargeChange = 1;
            try
            { h_tilt.Minimum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_tilt_min" + str_read2d + ".txt")); } catch (Exception) { h_tilt.Minimum = -999; }
            try
            { h_tilt.Maximum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_tilt_max" + str_read2d + ".txt")); } catch (Exception) { h_tilt.Maximum = 999; }
            try
            { h_tilt.Value = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_tilt_" + steptest + ".txt")); } catch { flag_try_min_max_value = true; }
            if (flag_try_min_max_value)
            {
                try
                { h_tilt.Value = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Tilt); } catch { }
                flag_try_min_max_value = false;
            }
            h_tilt.Size = new Size(300, h_tilt.Height);
            h_tilt.Location = new Point(1, 95);
            f1.Controls.Add(h_tilt);
            s_tilt.Text = h_tilt.Value.ToString();
            s_tilt.Size = new Size(300, 15);
            s_tilt.Location = new Point(h_tilt.Size.Width + 5, h_tilt.Location.Y + 2);
            f1.Controls.Add(s_tilt);

            l_contrast.Text = "contrast";
            l_contrast.Size = new Size(300, 15);
            l_contrast.Location = new Point(1, 120);
            f1.Controls.Add(l_contrast);
            h_contrast.Scroll += H_contrast_Scroll;
            h_contrast.LargeChange = 1;
            try
            { h_contrast.Minimum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_contrast_min" + str_read2d + ".txt")); } catch (Exception) { h_contrast.Minimum = -999; }
            try
            { h_contrast.Maximum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_contrast_max" + str_read2d + ".txt")); } catch (Exception) { h_contrast.Maximum = 999; }
            try
            { h_contrast.Value = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_contrast_" + steptest + ".txt")); } catch { flag_try_min_max_value = true; }
            if (flag_try_min_max_value)
            {
                try
                { h_contrast.Value = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Contrast); } catch { }
                flag_try_min_max_value = false;
            }
            h_contrast.Size = new Size(300, h_contrast.Height);
            h_contrast.Location = new Point(1, 135);
            f1.Controls.Add(h_contrast);
            s_contrast.Text = h_contrast.Value.ToString();
            s_contrast.Size = new Size(300, 15);
            s_contrast.Location = new Point(h_contrast.Size.Width + 5, h_contrast.Location.Y + 2);
            f1.Controls.Add(s_contrast);

            l_brightness.Text = "brightness";
            l_brightness.Size = new Size(300, 15);
            l_brightness.Location = new Point(1, 160);
            f1.Controls.Add(l_brightness);
            h_brightness.Scroll += H_brightness_Scroll;
            h_brightness.LargeChange = 1;
            try
            { h_brightness.Minimum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_brightness_min" + str_read2d + ".txt")); } catch (Exception) { h_brightness.Minimum = -999; }
            try
            { h_brightness.Maximum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_brightness_max" + str_read2d + ".txt")); } catch (Exception) { h_brightness.Maximum = 999; }
            try
            { h_brightness.Value = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_brightness_" + steptest + ".txt")); } catch { flag_try_min_max_value = true; }
            if (flag_try_min_max_value)
            {
                try
                { h_brightness.Value = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Brightness); } catch { }
                flag_try_min_max_value = false;
            }
            h_brightness.Size = new Size(300, h_brightness.Height);
            h_brightness.Location = new Point(1, 175);
            f1.Controls.Add(h_brightness);
            s_brightness.Text = h_brightness.Value.ToString();
            s_brightness.Size = new Size(300, 15);
            s_brightness.Location = new Point(h_brightness.Size.Width + 5, h_brightness.Location.Y + 2);
            f1.Controls.Add(s_brightness);

            l_focus.Text = "focus";
            l_focus.Size = new Size(300, 15);
            l_focus.Location = new Point(1, 200);
            f1.Controls.Add(l_focus);
            h_focus.Scroll += H_focus_Scroll;
            h_focus.LargeChange = 1;
            try
            { h_focus.Minimum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_focus_min" + str_read2d + ".txt")); } catch (Exception) { h_focus.Minimum = -999; }
            try
            { h_focus.Maximum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_focus_max" + str_read2d + ".txt")); } catch (Exception) { h_focus.Maximum = 999; }
            try
            { h_focus.Value = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_focus_" + steptest + ".txt")); } catch { flag_try_min_max_value = true; }
            if (flag_try_min_max_value)
            {
                try
                { h_focus.Value = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Focus); } catch { }
                flag_try_min_max_value = false;
            }
            h_focus.Size = new Size(300, h_focus.Height);
            h_focus.Location = new Point(1, 215);
            f1.Controls.Add(h_focus);
            s_focus.Text = h_focus.Value.ToString();
            s_focus.Size = new Size(30, 15);
            s_focus.Location = new Point(h_focus.Size.Width + 5, h_focus.Location.Y + 2);
            f1.Controls.Add(s_focus);
            Button b_focus = new Button();
            b_focus.Click += B_focus_Click;
            b_focus.Text = "auto";
            b_focus.Size = new Size(40, 20);
            b_focus.Location = new Point(h_focus.Size.Width + 40, h_focus.Location.Y);
            f1.Controls.Add(b_focus);

            l_process.Text = "process";
            l_process.Size = new Size(300, 15);
            l_process.Location = new Point(1, 240);
            f1.Controls.Add(l_process);
            h_process.Scroll += H_process_Scroll;
            h_process.LargeChange = 1;
            h_process.Minimum = 0;
            h_process.Maximum = 255;
            h_process.Value = process_value;
            h_process.Size = new Size(300, h_process.Height);
            h_process.Location = new Point(1, 255);
            f1.Controls.Add(h_process);
            s_process.Text = h_process.Value.ToString();
            s_process.Size = new Size(30, 15);
            s_process.Location = new Point(h_process.Size.Width + 5, h_process.Location.Y + 2);
            f1.Controls.Add(s_process);
            b_process.Click += B_process_Click;
            b_process.Text = flag_process.ToString();
            b_process.Size = new Size(40, 20);
            b_process.Location = new Point(h_process.Size.Width + 40, h_process.Location.Y);
            f1.Controls.Add(b_process);

            l_bgr.Text = "bgr: \"BlurLow BlueHigh GreenLow GreenHigh RedLow RedHigh\"";
            l_bgr.Size = new Size(400, 15);
            l_bgr.Location = new Point(1, 280);
            f1.Controls.Add(l_bgr);
            t_bgr.Text = bgr_low.Blue.ToString() + " " + bgr_high.Blue.ToString() + " " +
                         bgr_low.Green.ToString() + " " + bgr_high.Green.ToString() + " " +
                         bgr_low.Red.ToString() + " " + bgr_high.Red.ToString();
            t_bgr.Size = new Size(180, 20);
            t_bgr.Location = new Point(1, l_bgr.Location.Y + 15);
            t_bgr.KeyDown += T_bgr_KeyDown;
            f1.Controls.Add(t_bgr);
            l_mask.Text = "mask :";
            l_mask.Size = new Size(40, 15);
            l_mask.Location = new Point(t_bgr.Size.Width + 85, t_bgr.Location.Y + 2);
            f1.Controls.Add(l_mask);
            t_hsv_mask.Text = hsv_mask.ToString();
            t_hsv_mask.Size = new Size(75, 20);
            t_hsv_mask.Location = new Point(t_bgr.Size.Width + 125, t_bgr.Location.Y);
            t_hsv_mask.KeyDown += T_hsv_mask_KeyDown;
            f1.Controls.Add(t_hsv_mask);
            b_example.Click += B_example_Click;
            b_example.Text = "example";
            b_example.Size = new Size(60, 20);
            b_example.Location = new Point(t_bgr.Size.Width + 10, t_bgr.Location.Y);
            f1.Controls.Add(b_example);

            l_hsv.Text = "hsv: \"HueLow HueHigh SatuationLow SatuationHigh ValueLow ValueHigh\"";
            l_hsv.Size = new Size(400, 15);
            l_hsv.Location = new Point(1, 320);
            f1.Controls.Add(l_hsv);
            t_hsv.Text = hsv_low.Hue.ToString() + " " + hsv_high.Hue.ToString() + " " +
                         hsv_low.Satuation.ToString() + " " + hsv_high.Satuation.ToString() + " " +
                         hsv_low.Value.ToString() + " " + hsv_high.Value.ToString();
            t_hsv.Size = new Size(180, 20);
            t_hsv.Location = new Point(1, l_hsv.Location.Y + 15);
            t_hsv.KeyDown += T_hsv_KeyDown;
            f1.Controls.Add(t_hsv);
            l_timeout.Text = "timeout :";
            l_timeout.Size = new Size(47, 15);
            l_timeout.Location = new Point(t_hsv.Size.Width + 10, t_hsv.Location.Y + 2);
            f1.Controls.Add(l_timeout);
            t_timeout.Text = hsv_timeout.ToString();
            t_timeout.Size = new Size(60, 20);
            t_timeout.Location = new Point(t_hsv.Size.Width + 57, t_hsv.Location.Y);
            t_timeout.KeyDown += T_timeout_KeyDown;
            f1.Controls.Add(t_timeout);
            l_hsv_test.Text = "ms";
            l_hsv_test.Size = new Size(30, 15);
            l_hsv_test.Location = new Point(t_hsv.Size.Width + 120, t_hsv.Location.Y + 2);
            f1.Controls.Add(l_hsv_test);
            b_hsv.Click += B_hsv_Click;
            b_hsv.Text = flag_hsv_test.ToString();
            b_hsv.Size = new Size(40, 20);
            b_hsv.Location = new Point(t_hsv.Size.Width + 160, t_hsv.Location.Y);
            f1.Controls.Add(b_hsv);

            show_all.Location = new Point(365, 0);
            show_all.Click += Show_all_Click;
            f1.Controls.Add(show_all);

            l_exposure.Text = "exposure";
            l_exposure.Size = new Size(300, 15);
            l_exposure.Location = new Point(1, 360);
            f1.Controls.Add(l_exposure);
            h_exposure.Scroll += H_exposure_Scroll;
            h_exposure.LargeChange = 1;
            try
            { h_exposure.Minimum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_exposure_min" + str_read2d + ".txt")); } catch (Exception) { h_exposure.Minimum = -999; }
            try
            { h_exposure.Maximum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_exposure_max" + str_read2d + ".txt")); } catch (Exception) { h_exposure.Maximum = 999; }
            try
            { h_exposure.Value = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_exposure_" + steptest + ".txt")); } catch { flag_try_min_max_value = true; }
            if (flag_try_min_max_value)
            {
                try
                { h_exposure.Value = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure); } catch { }
                flag_try_min_max_value = false;
            }
            h_exposure.Size = new Size(300, h_exposure.Height);
            h_exposure.Location = new Point(1, 375);
            f1.Controls.Add(h_exposure);
            s_exposure.Text = h_exposure.Value.ToString();
            s_exposure.Size = new Size(300, 15);
            s_exposure.Location = new Point(h_exposure.Size.Width + 5, h_exposure.Location.Y + 2);
            f1.Controls.Add(s_exposure);

            l_saturation.Text = "saturation" + " (Address In Camera)";
            l_saturation.Size = new Size(300, 15);
            l_saturation.Location = new Point(1, 400);
            f1.Controls.Add(l_saturation);
            h_saturation.Scroll += H_saturation_Scroll;
            h_saturation.LargeChange = 1;
            //try { h_saturation.Minimum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_saturation_min" + str_read2d + ".txt")); } catch (Exception) { h_saturation.Minimum = -999; }
            //try { h_saturation.Maximum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_saturation_max" + str_read2d + ".txt")); } catch (Exception) { h_saturation.Maximum = 999; }
            //try { h_saturation.Value = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_saturation_" + steptest + ".txt")); } catch { flag_try_min_max_value = true; }
            h_saturation.Minimum = -999;
            h_saturation.Maximum = 999;
            try
            {
                h_saturation.Value = (int)capture.GetCaptureProperty(CapProp.Saturation);
            } catch { }
            h_saturation.Size = new Size(300, h_saturation.Height);
            h_saturation.Location = new Point(1, 415);
            h_saturation.Enabled = false;
            f1.Controls.Add(h_saturation);
            s_saturation.Text = h_saturation.Value.ToString();
            s_saturation.Size = new Size(300, 15);
            s_saturation.Location = new Point(h_saturation.Size.Width + 5, h_saturation.Location.Y + 2);
            f1.Controls.Add(s_saturation);

            l_sharpness.Text = "sharpness";
            l_sharpness.Size = new Size(300, 15);
            l_sharpness.Location = new Point(1, 440);
            f1.Controls.Add(l_sharpness);
            h_sharpness.Scroll += H_sharpness_Scroll;
            h_sharpness.LargeChange = 1;
            try
            { h_sharpness.Minimum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_sharpness_min" + str_read2d + ".txt")); } catch (Exception) { h_sharpness.Minimum = -999; }
            try
            { h_sharpness.Maximum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_sharpness_max" + str_read2d + ".txt")); } catch (Exception) { h_sharpness.Maximum = 999; }
            try
            { h_sharpness.Value = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_sharpness_" + steptest + ".txt")); } catch { flag_try_min_max_value = true; }
            if (flag_try_min_max_value)
            {
                try
                { h_sharpness.Value = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Sharpness); } catch { }
                flag_try_min_max_value = false;
            }
            h_sharpness.Size = new Size(300, h_sharpness.Height);
            h_sharpness.Location = new Point(1, 455);
            f1.Controls.Add(h_sharpness);
            s_sharpness.Text = h_sharpness.Value.ToString();
            s_sharpness.Size = new Size(300, 15);
            s_sharpness.Location = new Point(h_sharpness.Size.Width + 5, h_sharpness.Location.Y + 2);
            f1.Controls.Add(s_sharpness);

            l_gain.Text = "gain" + " (Address In Camera)";
            l_gain.Size = new Size(300, 15);
            l_gain.Location = new Point(1, 480);
            f1.Controls.Add(l_gain);
            h_gain.Scroll += H_gain_Scroll;
            h_gain.LargeChange = 1;
            //try { h_gain.Minimum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_gain_min" + str_read2d + ".txt")); } catch (Exception) { h_gain.Minimum = -999; }
            //try { h_gain.Maximum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_gain_max" + str_read2d + ".txt")); } catch (Exception) { h_gain.Maximum = 999; }
            //try { h_gain.Value = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_gain_" + steptest + ".txt")); } catch { flag_try_min_max_value = true; }
            h_gain.Minimum = -999;
            h_gain.Maximum = 999;
            try
            {
                h_gain.Value = (int)capture.GetCaptureProperty(CapProp.Gain);
            } catch { }
            h_gain.Size = new Size(300, h_gain.Height);
            h_gain.Location = new Point(1, 495);
            h_gain.Enabled = false;
            f1.Controls.Add(h_gain);
            s_gain.Text = h_gain.Value.ToString();
            s_gain.Size = new Size(300, 15);
            s_gain.Location = new Point(h_gain.Size.Width + 5, h_gain.Location.Y + 2);
            f1.Controls.Add(s_gain);

            l_gamma.Text = "gamma" + " (Address In Camera)";
            l_gamma.Size = new Size(300, 15);
            l_gamma.Location = new Point(1, 520);
            f1.Controls.Add(l_gamma);
            h_gamma.Scroll += H_gamma_Scroll;
            h_gamma.LargeChange = 1;
            //try { h_gamma.Minimum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_gamma_min" + str_read2d + ".txt")); } catch (Exception) { h_gamma.Minimum = -999; }
            //try { h_gamma.Maximum = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_cam_gamma_max" + str_read2d + ".txt")); } catch (Exception) { h_gamma.Maximum = 999; }
            //try { h_gamma.Value = Convert.ToInt32(File.ReadAllText("../../config_cameraDenali/test_head_" + head + "_gamma_" + steptest + ".txt")); } catch { flag_try_min_max_value = true; }
            h_gamma.Minimum = -999;
            h_gamma.Maximum = 999;
            try
            { h_gamma.Value = (int)capture.GetCaptureProperty(CapProp.Gamma); } catch { }
            h_gamma.Size = new Size(300, h_gamma.Height);
            h_gamma.Location = new Point(1, 535);
            h_gamma.Enabled = false;
            f1.Controls.Add(h_gamma);
            s_gamma.Text = h_gamma.Value.ToString();
            s_gamma.Size = new Size(300, 15);
            s_gamma.Location = new Point(h_gamma.Size.Width + 5, h_gamma.Location.Y + 2);
            f1.Controls.Add(s_gamma);

            f1.Show();
        }

        private void B_example_Click(object sender, EventArgs e) {
            MessageBox.Show("green : bgr : 0 100 100 255 0 50" +
                            "red : hsv : 0 60 0 255 150 255");
        }

        private void T_timeout_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyValue != 13)
                return;
            string cc = t_timeout.Text;
            int aa;
            try
            {
                aa = Convert.ToInt32(cc);
            } catch (Exception) { MessageBox.Show("not formath"); return; }
            hsv_timeout = aa;
        }

        private void T_hsv_mask_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyValue != 13)
                return;
            string cc = t_hsv_mask.Text;
            int aa;
            try
            {
                aa = Convert.ToInt32(cc);
            } catch (Exception) { MessageBox.Show("not formath"); return; }
            hsv_mask = aa;
        }

        private void B_hsv_Click(object sender, EventArgs e) {
            if (b_hsv.Text == "True")
            {
                b_hsv.Text = "False";
                flag_hsv_test = false;
            }
            else
            {
                b_hsv.Text = "True";
                flag_hsv_test = true;
            }
        }

        private void T_hsv_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyValue != 13)
                return;
            string cc = t_hsv.Text;
            string[] zz;
            int[] xx = { 0, 0, 0, 0, 0, 0 };
            try
            {
                zz = cc.Split(' ');
            } catch (Exception) { MessageBox.Show("not formath"); return; }
            if (zz.Length != 6)
            { MessageBox.Show("not formath"); return; }
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    xx[i] = Convert.ToInt32(zz[i]);
                }
            } catch (Exception) { MessageBox.Show("not formath"); return; }
            hsv_low = new Hsv(xx[0], xx[2], xx[4]);
            hsv_high = new Hsv(xx[1], xx[3], xx[5]);
            flag_hsv = true;
        }

        private void T_bgr_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyValue != 13)
                return;
            string cc = t_bgr.Text;
            string[] zz;
            int[] xx = { 0, 0, 0, 0, 0, 0 };
            try
            {
                zz = cc.Split(' ');
            } catch (Exception) { MessageBox.Show("not formath"); return; }
            if (zz.Length != 6)
            { MessageBox.Show("not formath"); return; }
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    xx[i] = Convert.ToInt32(zz[i]);
                }
            } catch (Exception) { MessageBox.Show("not formath"); return; }
            bgr_low = new Bgr(xx[0], xx[2], xx[4]);
            bgr_high = new Bgr(xx[1], xx[3], xx[5]);
            flag_hsv = false;
        }

        private void B_process_Click(object sender, EventArgs e) {
            if (b_process.Text == "True")
            {
                b_process.Text = "False";
                flag_process = false;
            }
            else
            {
                b_process.Text = "True";
                flag_process = true;
            }
        }

        private void H_process_Scroll(object sender, ScrollEventArgs e) {
            s_process.Text = h_process.Value.ToString();
            process_value = h_process.Value;
        }

        private void B_focus_Click(object sender, EventArgs e) {
            SetCapture(CapProp.Autofocus, 1);
            try
            { h_focus.Value = (int)capture.GetCaptureProperty(CapProp.Focus); } catch (Exception) { }
            s_focus.Text = h_focus.Value.ToString();
        }

        private void H_focus_Scroll(object sender, ScrollEventArgs e) {
            s_focus.Text = h_focus.Value.ToString();
            SetCapture(CapProp.Focus, h_focus.Value);
        }

        private void H_brightness_Scroll(object sender, ScrollEventArgs e) {
            s_brightness.Text = h_brightness.Value.ToString();
            SetCapture(CapProp.Brightness, h_brightness.Value);
        }

        private void H_contrast_Scroll(object sender, ScrollEventArgs e) {
            s_contrast.Text = h_contrast.Value.ToString();
            SetCapture(CapProp.Contrast, h_contrast.Value);
        }

        private void F1_FormClosed(object sender, FormClosedEventArgs e) {
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_zoom_" + steptest + ".txt", h_zoom.Value.ToString());
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_pan_" + steptest + ".txt", h_pan.Value.ToString());
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_tilt_" + steptest + ".txt", h_tilt.Value.ToString());
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_contrast_" + steptest + ".txt", h_contrast.Value.ToString());
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_brightness_" + steptest + ".txt", h_brightness.Value.ToString());
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_focus_" + steptest + ".txt", h_focus.Value.ToString());
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_exposure_" + steptest + ".txt", h_exposure.Value.ToString());
            //File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_saturation_" + steptest + ".txt", h_saturation.Value.ToString());
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_sharpness_" + steptest + ".txt", h_sharpness.Value.ToString());
            //File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_gain_" + steptest + ".txt", h_gain.Value.ToString());
            //File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_gamma_" + steptest + ".txt", h_gamma.Value.ToString());
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_process_" + steptest + ".txt", h_process.Value.ToString());
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_flag_process_" + steptest + ".txt", flag_process.ToString());
            if (steptest_camera_check_led_red_green == true)
            {

            }
            flag_set_camera = false;
        }

        private void H_tilt_Scroll(object sender, ScrollEventArgs e) {
            s_tilt.Text = h_tilt.Value.ToString();
            SetCapture(CapProp.Tilt, h_tilt.Value);
        }

        private void H_pan_Scroll(object sender, ScrollEventArgs e) {
            s_pan.Text = h_pan.Value.ToString();
            SetCapture(CapProp.Pan, h_pan.Value);
        }

        private void H_zoom_Scroll(object sender, ScrollEventArgs e) {
            s_zoom.Text = h_zoom.Value.ToString();
            SetCapture(CapProp.Zoom, h_zoom.Value);
        }

        private void H_exposure_Scroll(object sender, ScrollEventArgs e) {
            s_exposure.Text = h_exposure.Value.ToString();
            SetCapture(CapProp.Exposure, h_exposure.Value);
        }

        private void H_saturation_Scroll(object sender, ScrollEventArgs e) {
            //s_saturation.Text = h_saturation.Value.ToString();
            //SetCapture(CapProp.Saturation, h_saturation.Value);
        }

        private void H_sharpness_Scroll(object sender, ScrollEventArgs e) {
            s_sharpness.Text = h_sharpness.Value.ToString();
            SetCapture(CapProp.Sharpness, h_sharpness.Value);
        }

        private void H_gain_Scroll(object sender, ScrollEventArgs e) {
            //s_gain.Text = h_gain.Value.ToString();
            //SetCapture(CapProp.Gain, h_gain.Value);
        }

        private void H_gamma_Scroll(object sender, ScrollEventArgs e) {
            //s_gamma.Text = h_gamma.Value.ToString();
            //SetCapture(CapProp.Gamma, h_gamma.Value);
        }

        private void Show_all_Click(object sender, EventArgs e) {
            if (show_all.Checked)
            {
                f1.Size = new Size(400, 600);
                h_exposure.Visible = true;
                l_exposure.Visible = true;
                s_exposure.Visible = true;
                h_saturation.Visible = true;
                l_saturation.Visible = true;
                s_saturation.Visible = true;
                h_sharpness.Visible = true;
                l_sharpness.Visible = true;
                s_sharpness.Visible = true;
                h_gain.Visible = true;
                l_gain.Visible = true;
                s_gain.Visible = true;
                h_gamma.Visible = true;
                l_gamma.Visible = true;
                s_gamma.Visible = true;
            }
            else
            {
                f1.Size = new Size(400, 400);
                h_exposure.Visible = false;
                l_exposure.Visible = false;
                s_exposure.Visible = false;
                h_saturation.Visible = false;
                l_saturation.Visible = false;
                s_saturation.Visible = false;
                h_sharpness.Visible = false;
                l_sharpness.Visible = false;
                s_sharpness.Visible = false;
                h_gain.Visible = false;
                l_gain.Visible = false;
                s_gain.Visible = false;
                h_gamma.Visible = false;
                l_gamma.Visible = false;
                s_gamma.Visible = false;
            }
        }

        private static void DelaymS(int mS) {
            Stopwatch stopwatchDelaymS = new Stopwatch();
            stopwatchDelaymS.Restart();
            while (mS > stopwatchDelaymS.ElapsedMilliseconds)
            {
                if (!stopwatchDelaymS.IsRunning)
                    stopwatchDelaymS.Start();
                Application.DoEvents();
            }
            stopwatchDelaymS.Stop();
        }

        bool IsMouseDown = false;
        Point StartLocation;
        Point EndLcation;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
            if (!flag_set_camera)
                return;
            if (e.Button != MouseButtons.Left)
                return;
            IsMouseDown = true;
            StartLocation = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
            if (IsMouseDown == true)
            {
                EndLcation = e.Location;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) {
            if (IsMouseDown != true)
                return;
            if (rect.Size.Width < 30 || rect.Size.Height < 30)
                return;
            Image<Bgr, byte> imgInput;
            EndLcation = e.Location;
            IsMouseDown = false;
            if (steptest_camera_check_led_red_green == true)
            {
                if (rect != null)
                {
                    imgInput = img.Copy();
                    imgInput.ROI = rect;
                    Image<Bgr, byte> temp = imgInput.Copy();
                }
            }
            string mkmj = "";
            if (ctms_led_no.Text == "2")
                mkmj = "_2";
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_rect_x_" + steptest + mkmj + ".txt", rect.X.ToString());
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_rect_y_" + steptest + mkmj + ".txt", rect.Y.ToString());
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_rect_width_" + steptest + mkmj + ".txt", rect.Width.ToString());
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_rect_height_" + steptest + mkmj + ".txt", rect.Height.ToString());
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) {
            if (rect != null && IsMouseDown == true)
            {
                e.Graphics.DrawRectangle(Pens.Red, GetRectangle());
            }
        }

        private Rectangle GetRectangle() {
            rect.X = Math.Min(StartLocation.X, EndLcation.X);
            rect.Y = Math.Min(StartLocation.Y, EndLcation.Y);
            rect.Width = Math.Abs(StartLocation.X - EndLcation.X);
            rect.Height = Math.Abs(StartLocation.Y - EndLcation.Y);
            return rect;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            //capture.Dispose();
        }

        private void setDebugToolStripMenuItem_Click(object sender, EventArgs e) {
            debug = false;
        }

        private void setPortToolStripMenuItem_Click(object sender, EventArgs e) {
            File.WriteAllText("set_port.txt", "set port");
            File.WriteAllText("camera_head_set_port.txt", head);
            capture.Dispose();
            Application.Restart();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            try
            { capture.Dispose(); } catch { }
            while (true)
            {
                try
                {
                    string ff = File.ReadAllText("camera_show_list.txt");
                    File.WriteAllText("camera_show_list.txt", ff.Trim().Replace(head, ""));
                    break;
                } catch { Thread.Sleep(50); continue; }
            }
        }

        private void Frame_height_Click(object sender, EventArgs e) {
            int asd = 1;
            while (true)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("_ใส่ frame height\r\ndefault = 800", "frame height", Frame_height.Text, 500, 300);
                if (input == "")
                    return;
                try
                {
                    asd = Convert.ToInt32(input);
                } catch (Exception)
                {
                    MessageBox.Show("not format");
                    continue;
                }
                break;
            }
            Frame_height.Text = asd.ToString();
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_frame_height_" + steptest + ".txt", asd.ToString());
        }

        private void Frame_width_Click(object sender, EventArgs e) {
            int asd = 1;
            while (true)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("_ใส่ frame width\r\ndefault = 600", "frame width", Frame_width.Text, 500, 300);
                if (input == "")
                    return;
                try
                {
                    asd = Convert.ToInt32(input);
                } catch (Exception)
                {
                    MessageBox.Show("not format");
                    continue;
                }
                break;
            }
            Frame_width.Text = asd.ToString();
            File.WriteAllText("../../config_cameraDenali/test_head_" + head + "_frame_width_" + steptest + ".txt", asd.ToString());
        }

        private void onebyone_Click(object sender, EventArgs e) {
            File.WriteAllText("../../config_cameraDenali/camera_show_onebyone" + ".txt", onebyone.Checked.ToString());
        }

        private void ctms_led_no_Click(object sender, EventArgs e) {
            int asd = 1;
            while (true)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("_ใส่ led no (1 - 2)", "led no", ctms_led_no.Text, 500, 300);
                if (input == "")
                    return;
                try
                {
                    asd = Convert.ToInt32(input);
                    if (asd > 2 || asd < 1)
                        throw new Exception();
                } catch
                {
                    MessageBox.Show("not format");
                    continue;
                }
                break;
            }
            ctms_led_no.Text = asd.ToString();
        }

        private void ctms_flag_test_Click(object sender, EventArgs e) {
            int asd = 1;
            while (true)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("_ใส่ flag_test (1 - 6)", "flag test", ctms_flag_test.Text, 500, 300);
                if (input == "")
                    return;
                try
                {
                    asd = Convert.ToInt32(input);
                    if (asd > 6 || asd < 1)
                        throw new Exception();
                } catch
                {
                    MessageBox.Show("not format");
                    continue;
                }
                break;
            }
            ctms_flag_test.Text = asd.ToString();
            flag_led_test = asd;
            setup();
        }

        public static class Address {
            public static readonly string gain = "Gain";
            public static readonly string gamma = "Gamma";
            public static readonly string saturation = "Saturation";
        }

        private void ctms_propertySetup_Click(object sender, EventArgs e) {
            if (!CheckPasswordSetAddress())
            {
                return;
            }
            SetCapture(CapProp.Settings, 0);
        }

        private void ctms_propertySave_Click(object sender, EventArgs e) {
            if (!CheckPasswordSetAddress())
            {
                return;
            }
            ReadConfigCameraToCsvFile();
        }
        /// <summary>
        /// For read value config camera current to csv
        /// </summary>
        private void ReadConfigCameraToCsvFile() {
            double zoom = capture.GetCaptureProperty(CapProp.Zoom);
            double pan = capture.GetCaptureProperty(CapProp.Pan);
            double tilt = capture.GetCaptureProperty(CapProp.Tilt);
            double contrast = capture.GetCaptureProperty(CapProp.Contrast);
            double brightness = capture.GetCaptureProperty(CapProp.Brightness);
            double focus = capture.GetCaptureProperty(CapProp.Focus);
            double exposure = capture.GetCaptureProperty(CapProp.Exposure);
            double saturation = capture.GetCaptureProperty(CapProp.Saturation);
            double sharpness = capture.GetCaptureProperty(CapProp.Sharpness);
            double gain = capture.GetCaptureProperty(CapProp.Gain);
            double gamma = capture.GetCaptureProperty(CapProp.Gamma);

            string pathConfig = $"../../config_cameraDenali/test_head_{head}_";
            File.WriteAllText($"{pathConfig}zoom_{steptest}.txt", zoom.ToString());
            File.WriteAllText($"{pathConfig}pan_{steptest}.txt", pan.ToString());
            File.WriteAllText($"{pathConfig}tilt_{steptest}.txt", tilt.ToString());
            File.WriteAllText($"{pathConfig}contrast_{steptest}.txt", contrast.ToString());
            File.WriteAllText($"{pathConfig}focus_{steptest}.txt", focus.ToString());
            File.WriteAllText($"{pathConfig}exposure_{steptest}.txt", exposure.ToString());
            File.WriteAllText($"{pathConfig}saturation_{steptest}.txt", saturation.ToString());
            File.WriteAllText($"{pathConfig}gain_{steptest}.txt", gain.ToString());
            File.WriteAllText($"{pathConfig}gamma_{steptest}.txt", gamma.ToString());
            File.WriteAllText($"{pathConfig}sharpness_{steptest}.txt", sharpness.ToString());
            File.WriteAllText($"{pathConfig}brightness_{steptest}.txt", brightness.ToString());
        }
    }
}
