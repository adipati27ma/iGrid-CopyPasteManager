namespace iGrid_CopyPasteManager
{
	partial class Form1
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.fCheckBoxCopyColumnHeaders = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.fButtonPaste = new System.Windows.Forms.Button();
			this.fButtonCopy = new System.Windows.Forms.Button();
			this.fGrid = new TenTec.Windows.iGridLib.iGrid();
			((System.ComponentModel.ISupportInitialize)(this.fGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// fCheckBoxCopyColumnHeaders
			// 
			this.fCheckBoxCopyColumnHeaders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.fCheckBoxCopyColumnHeaders.AutoSize = true;
			this.fCheckBoxCopyColumnHeaders.Location = new System.Drawing.Point(14, 423);
			this.fCheckBoxCopyColumnHeaders.Name = "fCheckBoxCopyColumnHeaders";
			this.fCheckBoxCopyColumnHeaders.Size = new System.Drawing.Size(128, 17);
			this.fCheckBoxCopyColumnHeaders.TabIndex = 11;
			this.fCheckBoxCopyColumnHeaders.Text = "Copy column headers";
			this.fCheckBoxCopyColumnHeaders.UseVisualStyleBackColor = true;
			this.fCheckBoxCopyColumnHeaders.CheckedChanged += new System.EventHandler(this.fCheckBoxCopyColumnHeaders_CheckedChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(174, 398);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(344, 38);
			this.label2.TabIndex = 10;
			this.label2.Text = "Pay attention to the extra small buttons on the vertical scroll bar:\r\nyou can use" +
					" them to select or deselect all cells in the grid.\r\nThe CTRL+A/CTRL+D keyboard c" +
					"ombinations do the same.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(7, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(511, 68);
			this.label1.TabIndex = 9;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// fButtonPaste
			// 
			this.fButtonPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.fButtonPaste.Location = new System.Drawing.Point(91, 394);
			this.fButtonPaste.Name = "fButtonPaste";
			this.fButtonPaste.Size = new System.Drawing.Size(75, 23);
			this.fButtonPaste.TabIndex = 8;
			this.fButtonPaste.Text = "Paste";
			this.fButtonPaste.Click += new System.EventHandler(this.fButtonPaste_Click);
			// 
			// fButtonCopy
			// 
			this.fButtonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.fButtonCopy.Location = new System.Drawing.Point(10, 394);
			this.fButtonCopy.Name = "fButtonCopy";
			this.fButtonCopy.Size = new System.Drawing.Size(75, 23);
			this.fButtonCopy.TabIndex = 7;
			this.fButtonCopy.Text = "Copy";
			this.fButtonCopy.Click += new System.EventHandler(this.fButtonCopy_Click);
			// 
			// fGrid
			// 
			this.fGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.fGrid.ForeColor = System.Drawing.SystemColors.WindowText;
			this.fGrid.Header.Height = 19;
			this.fGrid.Location = new System.Drawing.Point(10, 84);
			this.fGrid.Name = "fGrid";
			this.fGrid.Size = new System.Drawing.Size(508, 304);
			this.fGrid.TabIndex = 6;
			this.fGrid.TreeCol = null;
			this.fGrid.TreeLines.Color = System.Drawing.SystemColors.WindowText;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(524, 447);
			this.Controls.Add(this.fCheckBoxCopyColumnHeaders);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.fButtonPaste);
			this.Controls.Add(this.fButtonCopy);
			this.Controls.Add(this.fGrid);
			this.Name = "Form1";
			this.Text = "iGrid.NET Copy Paste Demo";
			this.Load += new System.EventHandler(this.Form2_Load);
			((System.ComponentModel.ISupportInitialize)(this.fGrid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox fCheckBoxCopyColumnHeaders;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button fButtonPaste;
		private System.Windows.Forms.Button fButtonCopy;
		private TenTec.Windows.iGridLib.iGrid fGrid;
	}
}