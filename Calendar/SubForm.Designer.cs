namespace Calendar
{
	partial class SubForm
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
			this.dgView = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dgView)).BeginInit();
			this.SuspendLayout();
			// 
			// dgView
			// 
			this.dgView.AllowUserToAddRows = false;
			this.dgView.AllowUserToDeleteRows = false;
			this.dgView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgView.Location = new System.Drawing.Point(0, 0);
			this.dgView.MultiSelect = false;
			this.dgView.Name = "dgView";
			this.dgView.ReadOnly = true;
			this.dgView.RowHeadersVisible = false;
			this.dgView.RowTemplate.Height = 21;
			this.dgView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgView.Size = new System.Drawing.Size(633, 261);
			this.dgView.TabIndex = 1;
			this.dgView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgView_MouseDoubleClick);
			// 
			// SubForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(633, 261);
			this.Controls.Add(this.dgView);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SubForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "その他の候補者";
			this.Load += new System.EventHandler(this.SubForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgView;
	}
}