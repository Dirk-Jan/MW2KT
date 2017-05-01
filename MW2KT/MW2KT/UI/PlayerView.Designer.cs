namespace MW2KT.UI
{
    partial class PlayerView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblIP = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblBlock = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pboxPartyIcon = new System.Windows.Forms.PictureBox();
            this.pboxIcon = new System.Windows.Forms.PictureBox();
            this.lblName = new MW2KT.UI.Mw2Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxPartyIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIP.Location = new System.Drawing.Point(146, 30);
            this.lblIP.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(158, 18);
            this.lblIP.TabIndex = 4;
            this.lblIP.Text = "192.168.123.123";
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevel.ForeColor = System.Drawing.Color.White;
            this.lblLevel.Location = new System.Drawing.Point(100, 29);
            this.lblLevel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(36, 23);
            this.lblLevel.TabIndex = 7;
            this.lblLevel.Text = "70";
            // 
            // lblBlock
            // 
            this.lblBlock.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlock.ForeColor = System.Drawing.Color.White;
            this.lblBlock.Location = new System.Drawing.Point(196, 24);
            this.lblBlock.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lblBlock.Name = "lblBlock";
            this.lblBlock.Size = new System.Drawing.Size(247, 20);
            this.lblBlock.TabIndex = 8;
            this.lblBlock.Text = "[Block]";
            this.lblBlock.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblBlock.Click += new System.EventHandler(this.lblBlock_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox2.Image = global::MW2KT.Properties.Resources.tag_cheater;
            this.pictureBox2.Location = new System.Drawing.Point(469, 4);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(43, 39);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // pboxPartyIcon
            // 
            this.pboxPartyIcon.Location = new System.Drawing.Point(4, 4);
            this.pboxPartyIcon.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.pboxPartyIcon.Name = "pboxPartyIcon";
            this.pboxPartyIcon.Size = new System.Drawing.Size(44, 44);
            this.pboxPartyIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pboxPartyIcon.TabIndex = 9;
            this.pboxPartyIcon.TabStop = false;
            // 
            // pboxIcon
            // 
            this.pboxIcon.Image = global::MW2KT.Properties.Resources._10;
            this.pboxIcon.Location = new System.Drawing.Point(56, 4);
            this.pboxIcon.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.pboxIcon.Name = "pboxIcon";
            this.pboxIcon.Size = new System.Drawing.Size(44, 44);
            this.pboxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pboxIcon.TabIndex = 6;
            this.pboxIcon.TabStop = false;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(109, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(352, 27);
            this.lblName.TabIndex = 11;
            this.lblName.Text = "Player name";
            this.lblName.Text2 = null;
            // 
            // PlayerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pboxPartyIcon);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.lblBlock);
            this.Controls.Add(this.pboxIcon);
            this.Controls.Add(this.lblLevel);
            this.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "PlayerView";
            this.Size = new System.Drawing.Size(520, 52);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxPartyIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.PictureBox pboxIcon;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblBlock;
        private System.Windows.Forms.PictureBox pboxPartyIcon;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Mw2Label lblName;
    }
}
