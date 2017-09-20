namespace Calendar
{
	partial class AggregationForm
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
			this.dgView1 = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dgView1)).BeginInit();
			this.SuspendLayout();
			// 
			// dgView1
			// 
			this.dgView1.AllowUserToAddRows = false;
			this.dgView1.AllowUserToDeleteRows = false;
			this.dgView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgView1.Location = new System.Drawing.Point(0, 0);
			this.dgView1.Name = "dgView1";
			this.dgView1.ReadOnly = true;
			this.dgView1.RowTemplate.Height = 21;
			this.dgView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgView1.Size = new System.Drawing.Size(456, 388);
			this.dgView1.TabIndex = 0;
			this.dgView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgView1_CellContentDoubleClick);
			// 
			// AggregationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(456, 388);
			this.Controls.Add(this.dgView1);
			this.Name = "AggregationForm";
			this.Text = "集計";
			((System.ComponentModel.ISupportInitialize)(this.dgView1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgView1;
	}
}