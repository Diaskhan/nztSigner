namespace nztSigner
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            imageList1 = new ImageList(components);
            treeView1 = new TreeView();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel2 = new ToolStripStatusLabel();
            toolStrip1 = new ToolStrip();
            toolStripButton1 = new ToolStripButton();
            SignersButton = new ToolStripButton();
            ExtractButton = new ToolStripButton();
            toolStripButton4 = new ToolStripButton();
            statusStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "box.png");
            imageList1.Images.SetKeyName(1, "carton-box.png");
            imageList1.Images.SetKeyName(2, "eco-friendly-package.png");
            imageList1.Images.SetKeyName(3, "favicon.ico");
            imageList1.Images.SetKeyName(4, "handwriting.png");
            imageList1.Images.SetKeyName(5, "labeling.png");
            imageList1.Images.SetKeyName(6, "open-folder.png");
            imageList1.Images.SetKeyName(7, "package.png");
            imageList1.Images.SetKeyName(8, "write.png");
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(0, 55);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(685, 374);
            treeView1.TabIndex = 4;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel2 });
            statusStrip1.Location = new Point(0, 429);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(685, 22);
            statusStrip1.TabIndex = 8;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new Size(177, 17);
            toolStripStatusLabel2.Text = "coded by Diaskhan@yandex.kz";
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(48, 48);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1, SignersButton, ExtractButton, toolStripButton4 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(685, 55);
            toolStrip1.TabIndex = 9;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.Image = Properties.Resources.open_folder;
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(163, 52);
            toolStripButton1.Text = "Выбрать файл cms";
            toolStripButton1.Click += toolStripButton1_Click;
            // 
            // SignersButton
            // 
            SignersButton.Image = Properties.Resources.handwriting;
            SignersButton.ImageTransparentColor = Color.Magenta;
            SignersButton.Name = "SignersButton";
            SignersButton.Size = new Size(128, 52);
            SignersButton.Text = "Подписанты";
            SignersButton.Click += toolStripButton2_Click;
            // 
            // ExtractButton
            // 
            ExtractButton.Image = Properties.Resources.carton_box;
            ExtractButton.ImageTransparentColor = Color.Magenta;
            ExtractButton.Name = "ExtractButton";
            ExtractButton.Size = new Size(213, 52);
            ExtractButton.Text = "Извлечь документ и окрыть";
            ExtractButton.Click += toolStripButton3_Click;
            // 
            // toolStripButton4
            // 
            toolStripButton4.Image = Properties.Resources.information;
            toolStripButton4.ImageTransparentColor = Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new Size(134, 52);
            toolStripButton4.Text = "О программе";
            toolStripButton4.Click += toolStripButton4_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(685, 451);
            Controls.Add(treeView1);
            Controls.Add(toolStrip1);
            Controls.Add(statusStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Form1";
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ImageList imageList1;
        private TreeView treeView1;
        private StatusStrip statusStrip1;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton SignersButton;
        private ToolStripButton ExtractButton;
        private ToolStripButton toolStripButton4;
        private ToolStripStatusLabel toolStripStatusLabel2;
    }
}
