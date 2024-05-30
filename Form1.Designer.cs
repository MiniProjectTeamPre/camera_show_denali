namespace camera_show_denali {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ledNoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctms_led_no = new System.Windows.Forms.ToolStripMenuItem();
            this.flagtestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctms_flag_test = new System.Windows.Forms.ToolStripMenuItem();
            this.frameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frameHeightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Frame_height = new System.Windows.Forms.ToolStripMenuItem();
            this.frameWidthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Frame_width = new System.Windows.Forms.ToolStripMenuItem();
            this.onebyone = new System.Windows.Forms.ToolStripMenuItem();
            this.setSaturationAddressInCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctms_setSaturationAddressCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.ctms_setSaturationAddress = new System.Windows.Forms.ToolStripMenuItem();
            this.ctms_setGainAddress_ = new System.Windows.Forms.ToolStripMenuItem();
            this.ctms_setGainAddressCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.ctms_setGainAddress = new System.Windows.Forms.ToolStripMenuItem();
            this.setGammaAddressInCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctms_setGammaAddressCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.ctms_setGammaAddress = new System.Windows.Forms.ToolStripMenuItem();
            this.ctms_propertySetting = new System.Windows.Forms.ToolStripMenuItem();
            this.versionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.v202202ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox1.Location = new System.Drawing.Point(1, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(260, 237);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setCameraToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.setDebugToolStripMenuItem,
            this.setPortToolStripMenuItem,
            this.ledNoToolStripMenuItem,
            this.flagtestToolStripMenuItem,
            this.frameToolStripMenuItem,
            this.onebyone,
            this.setSaturationAddressInCSVToolStripMenuItem,
            this.ctms_setGainAddress_,
            this.setGammaAddressInCSVToolStripMenuItem,
            this.versionToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(238, 290);
            // 
            // setCameraToolStripMenuItem
            // 
            this.setCameraToolStripMenuItem.Name = "setCameraToolStripMenuItem";
            this.setCameraToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.setCameraToolStripMenuItem.Text = "set camera";
            this.setCameraToolStripMenuItem.Click += new System.EventHandler(this.setCameraToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.closeToolStripMenuItem.Text = "close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // setDebugToolStripMenuItem
            // 
            this.setDebugToolStripMenuItem.Name = "setDebugToolStripMenuItem";
            this.setDebugToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.setDebugToolStripMenuItem.Text = "set debug";
            this.setDebugToolStripMenuItem.Click += new System.EventHandler(this.setDebugToolStripMenuItem_Click);
            // 
            // setPortToolStripMenuItem
            // 
            this.setPortToolStripMenuItem.Name = "setPortToolStripMenuItem";
            this.setPortToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.setPortToolStripMenuItem.Text = "set port";
            this.setPortToolStripMenuItem.Click += new System.EventHandler(this.setPortToolStripMenuItem_Click);
            // 
            // ledNoToolStripMenuItem
            // 
            this.ledNoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctms_led_no});
            this.ledNoToolStripMenuItem.Name = "ledNoToolStripMenuItem";
            this.ledNoToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.ledNoToolStripMenuItem.Text = "led no.";
            // 
            // ctms_led_no
            // 
            this.ctms_led_no.Name = "ctms_led_no";
            this.ctms_led_no.Size = new System.Drawing.Size(80, 22);
            this.ctms_led_no.Text = "1";
            this.ctms_led_no.Click += new System.EventHandler(this.ctms_led_no_Click);
            // 
            // flagtestToolStripMenuItem
            // 
            this.flagtestToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctms_flag_test});
            this.flagtestToolStripMenuItem.Name = "flagtestToolStripMenuItem";
            this.flagtestToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.flagtestToolStripMenuItem.Text = "flag_test";
            // 
            // ctms_flag_test
            // 
            this.ctms_flag_test.Name = "ctms_flag_test";
            this.ctms_flag_test.Size = new System.Drawing.Size(80, 22);
            this.ctms_flag_test.Text = "1";
            this.ctms_flag_test.Click += new System.EventHandler(this.ctms_flag_test_Click);
            // 
            // frameToolStripMenuItem
            // 
            this.frameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.frameHeightToolStripMenuItem,
            this.frameWidthToolStripMenuItem});
            this.frameToolStripMenuItem.Name = "frameToolStripMenuItem";
            this.frameToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.frameToolStripMenuItem.Text = "frame";
            // 
            // frameHeightToolStripMenuItem
            // 
            this.frameHeightToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Frame_height});
            this.frameHeightToolStripMenuItem.Name = "frameHeightToolStripMenuItem";
            this.frameHeightToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.frameHeightToolStripMenuItem.Text = "frame height";
            // 
            // Frame_height
            // 
            this.Frame_height.Name = "Frame_height";
            this.Frame_height.Size = new System.Drawing.Size(92, 22);
            this.Frame_height.Text = "800";
            this.Frame_height.Click += new System.EventHandler(this.Frame_height_Click);
            // 
            // frameWidthToolStripMenuItem
            // 
            this.frameWidthToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Frame_width});
            this.frameWidthToolStripMenuItem.Name = "frameWidthToolStripMenuItem";
            this.frameWidthToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.frameWidthToolStripMenuItem.Text = "frame width";
            // 
            // Frame_width
            // 
            this.Frame_width.Name = "Frame_width";
            this.Frame_width.Size = new System.Drawing.Size(92, 22);
            this.Frame_width.Text = "600";
            this.Frame_width.Click += new System.EventHandler(this.Frame_width_Click);
            // 
            // onebyone
            // 
            this.onebyone.Checked = true;
            this.onebyone.CheckOnClick = true;
            this.onebyone.CheckState = System.Windows.Forms.CheckState.Checked;
            this.onebyone.Name = "onebyone";
            this.onebyone.Size = new System.Drawing.Size(237, 22);
            this.onebyone.Text = "1 head 1 camera";
            this.onebyone.Click += new System.EventHandler(this.onebyone_Click);
            // 
            // setSaturationAddressInCSVToolStripMenuItem
            // 
            this.setSaturationAddressInCSVToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctms_setSaturationAddressCsv,
            this.ctms_setSaturationAddress});
            this.setSaturationAddressInCSVToolStripMenuItem.Name = "setSaturationAddressInCSVToolStripMenuItem";
            this.setSaturationAddressInCSVToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.setSaturationAddressInCSVToolStripMenuItem.Text = "Set Saturation (Address in CSV)";
            // 
            // ctms_setSaturationAddressCsv
            // 
            this.ctms_setSaturationAddressCsv.Name = "ctms_setSaturationAddressCsv";
            this.ctms_setSaturationAddressCsv.Size = new System.Drawing.Size(180, 22);
            this.ctms_setSaturationAddressCsv.Text = "100";
            this.ctms_setSaturationAddressCsv.Click += new System.EventHandler(this.ctms_setSaturationAddressCsv_Click);
            // 
            // ctms_setSaturationAddress
            // 
            this.ctms_setSaturationAddress.Name = "ctms_setSaturationAddress";
            this.ctms_setSaturationAddress.Size = new System.Drawing.Size(180, 22);
            this.ctms_setSaturationAddress.Text = "Set";
            this.ctms_setSaturationAddress.Click += new System.EventHandler(this.ctms_setSaturationAddress_Click);
            // 
            // ctms_setGainAddress_
            // 
            this.ctms_setGainAddress_.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctms_setGainAddressCsv,
            this.ctms_setGainAddress});
            this.ctms_setGainAddress_.Name = "ctms_setGainAddress_";
            this.ctms_setGainAddress_.Size = new System.Drawing.Size(237, 22);
            this.ctms_setGainAddress_.Text = "Set Gain (Address in CSV)";
            // 
            // ctms_setGainAddressCsv
            // 
            this.ctms_setGainAddressCsv.Name = "ctms_setGainAddressCsv";
            this.ctms_setGainAddressCsv.Size = new System.Drawing.Size(180, 22);
            this.ctms_setGainAddressCsv.Text = "100";
            this.ctms_setGainAddressCsv.Click += new System.EventHandler(this.ctms_setGainAddressCsv_Click);
            // 
            // ctms_setGainAddress
            // 
            this.ctms_setGainAddress.Name = "ctms_setGainAddress";
            this.ctms_setGainAddress.Size = new System.Drawing.Size(180, 22);
            this.ctms_setGainAddress.Text = "Set";
            this.ctms_setGainAddress.Click += new System.EventHandler(this.ctms_setGainAddress_Click);
            // 
            // setGammaAddressInCSVToolStripMenuItem
            // 
            this.setGammaAddressInCSVToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctms_setGammaAddressCsv,
            this.ctms_setGammaAddress,
            this.ctms_propertySetting});
            this.setGammaAddressInCSVToolStripMenuItem.Name = "setGammaAddressInCSVToolStripMenuItem";
            this.setGammaAddressInCSVToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.setGammaAddressInCSVToolStripMenuItem.Text = "Set Gamma (Address in CSV)";
            // 
            // ctms_setGammaAddressCsv
            // 
            this.ctms_setGammaAddressCsv.Name = "ctms_setGammaAddressCsv";
            this.ctms_setGammaAddressCsv.Size = new System.Drawing.Size(180, 22);
            this.ctms_setGammaAddressCsv.Text = "100";
            this.ctms_setGammaAddressCsv.Click += new System.EventHandler(this.ctms_setGammaAddressCsv_Click);
            // 
            // ctms_setGammaAddress
            // 
            this.ctms_setGammaAddress.Name = "ctms_setGammaAddress";
            this.ctms_setGammaAddress.Size = new System.Drawing.Size(180, 22);
            this.ctms_setGammaAddress.Text = "Set";
            this.ctms_setGammaAddress.Click += new System.EventHandler(this.ctms_setGammaAddress_Click);
            // 
            // ctms_propertySetting
            // 
            this.ctms_propertySetting.Name = "ctms_propertySetting";
            this.ctms_propertySetting.Size = new System.Drawing.Size(180, 22);
            this.ctms_propertySetting.Text = "Property Setting";
            this.ctms_propertySetting.Click += new System.EventHandler(this.ctms_propertySetting_Click);
            // 
            // versionToolStripMenuItem
            // 
            this.versionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.v202202ToolStripMenuItem});
            this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
            this.versionToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.versionToolStripMenuItem.Text = "Version";
            // 
            // v202202ToolStripMenuItem
            // 
            this.v202202ToolStripMenuItem.Name = "v202202ToolStripMenuItem";
            this.v202202ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.v202202ToolStripMenuItem.Text = "V2022.03";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(274, 247);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.TransparencyKey = System.Drawing.Color.DarkRed;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setCameraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setDebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setPortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frameHeightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Frame_height;
        private System.Windows.Forms.ToolStripMenuItem frameWidthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Frame_width;
        private System.Windows.Forms.ToolStripMenuItem onebyone;
        private System.Windows.Forms.ToolStripMenuItem ledNoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ctms_led_no;
        private System.Windows.Forms.ToolStripMenuItem flagtestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ctms_flag_test;
        private System.Windows.Forms.ToolStripMenuItem versionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem v202202ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setSaturationAddressInCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ctms_setSaturationAddressCsv;
        private System.Windows.Forms.ToolStripMenuItem ctms_setSaturationAddress;
        private System.Windows.Forms.ToolStripMenuItem ctms_setGainAddress_;
        private System.Windows.Forms.ToolStripMenuItem ctms_setGainAddressCsv;
        private System.Windows.Forms.ToolStripMenuItem ctms_setGainAddress;
        private System.Windows.Forms.ToolStripMenuItem setGammaAddressInCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ctms_setGammaAddressCsv;
        private System.Windows.Forms.ToolStripMenuItem ctms_setGammaAddress;
        private System.Windows.Forms.ToolStripMenuItem ctms_propertySetting;
    }
}

