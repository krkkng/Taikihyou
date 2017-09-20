namespace Calendar
{
	partial class FormSetting
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
			this.button1 = new System.Windows.Forms.Button();
			this.lblCalendar = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.num3rd = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.num1st = new System.Windows.Forms.NumericUpDown();
			this.num2nd = new System.Windows.Forms.NumericUpDown();
			this.numHoliday = new System.Windows.Forms.NumericUpDown();
			this.numInterval = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.cmbAttr = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.numPCI = new System.Windows.Forms.NumericUpDown();
			this.num2ndInterval = new System.Windows.Forms.NumericUpDown();
			this.num3rdInterval = new System.Windows.Forms.NumericUpDown();
			this.numPCIInterval = new System.Windows.Forms.NumericUpDown();
			this.num2ndHoliday = new System.Windows.Forms.NumericUpDown();
			this.num3rdHoliday = new System.Windows.Forms.NumericUpDown();
			this.numPCIHoliday = new System.Windows.Forms.NumericUpDown();
			this.txtName = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.calendar1 = new CalendarControl.Calendar();
			((System.ComponentModel.ISupportInitialize)(this.num3rd)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.num1st)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.num2nd)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numHoliday)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPCI)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.num2ndInterval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.num3rdInterval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPCIInterval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.num2ndHoliday)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.num3rdHoliday)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPCIHoliday)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(289, 497);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// lblCalendar
			// 
			this.lblCalendar.AutoSize = true;
			this.lblCalendar.Location = new System.Drawing.Point(10, 180);
			this.lblCalendar.Name = "lblCalendar";
			this.lblCalendar.Size = new System.Drawing.Size(47, 12);
			this.lblCalendar.TabIndex = 2;
			this.lblCalendar.Text = "不可日：";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(31, 69);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(27, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "1st：";
			this.label2.Click += new System.EventHandler(this.label2_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(31, 94);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(29, 12);
			this.label3.TabIndex = 5;
			this.label3.Text = "2nd：";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(265, 52);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(77, 12);
			this.label4.TabIndex = 7;
			this.label4.Text = "休日可能日数";
			// 
			// num3rd
			// 
			this.num3rd.Location = new System.Drawing.Point(82, 118);
			this.num3rd.Name = "num3rd";
			this.num3rd.Size = new System.Drawing.Size(62, 19);
			this.num3rd.TabIndex = 9;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(33, 120);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(27, 12);
			this.label1.TabIndex = 10;
			this.label1.Text = "3rd：";
			// 
			// num1st
			// 
			this.num1st.Location = new System.Drawing.Point(82, 67);
			this.num1st.Name = "num1st";
			this.num1st.Size = new System.Drawing.Size(62, 19);
			this.num1st.TabIndex = 11;
			// 
			// num2nd
			// 
			this.num2nd.Location = new System.Drawing.Point(82, 92);
			this.num2nd.Name = "num2nd";
			this.num2nd.Size = new System.Drawing.Size(62, 19);
			this.num2nd.TabIndex = 12;
			// 
			// numHoliday
			// 
			this.numHoliday.Location = new System.Drawing.Point(267, 67);
			this.numHoliday.Name = "numHoliday";
			this.numHoliday.Size = new System.Drawing.Size(62, 19);
			this.numHoliday.TabIndex = 13;
			// 
			// numInterval
			// 
			this.numInterval.Location = new System.Drawing.Point(173, 67);
			this.numInterval.Name = "numInterval";
			this.numInterval.Size = new System.Drawing.Size(62, 19);
			this.numInterval.TabIndex = 14;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(172, 52);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(63, 12);
			this.label5.TabIndex = 15;
			this.label5.Text = "最小の間隔";
			// 
			// cmbAttr
			// 
			this.cmbAttr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAttr.FormattingEnabled = true;
			this.cmbAttr.Location = new System.Drawing.Point(238, 12);
			this.cmbAttr.Name = "cmbAttr";
			this.cmbAttr.Size = new System.Drawing.Size(104, 20);
			this.cmbAttr.TabIndex = 16;
			this.cmbAttr.SelectedIndexChanged += new System.EventHandler(this.cmbAttr_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(200, 15);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(35, 12);
			this.label6.TabIndex = 17;
			this.label6.Text = "属性：";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(80, 52);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 12);
			this.label7.TabIndex = 18;
			this.label7.Text = "可能日数";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(31, 145);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(29, 12);
			this.label8.TabIndex = 19;
			this.label8.Text = "PCI：";
			// 
			// numPCI
			// 
			this.numPCI.Location = new System.Drawing.Point(82, 143);
			this.numPCI.Name = "numPCI";
			this.numPCI.Size = new System.Drawing.Size(62, 19);
			this.numPCI.TabIndex = 20;
			// 
			// num2ndInterval
			// 
			this.num2ndInterval.Location = new System.Drawing.Point(173, 92);
			this.num2ndInterval.Name = "num2ndInterval";
			this.num2ndInterval.Size = new System.Drawing.Size(62, 19);
			this.num2ndInterval.TabIndex = 21;
			// 
			// num3rdInterval
			// 
			this.num3rdInterval.Location = new System.Drawing.Point(173, 118);
			this.num3rdInterval.Name = "num3rdInterval";
			this.num3rdInterval.Size = new System.Drawing.Size(62, 19);
			this.num3rdInterval.TabIndex = 22;
			// 
			// numPCIInterval
			// 
			this.numPCIInterval.Location = new System.Drawing.Point(173, 143);
			this.numPCIInterval.Name = "numPCIInterval";
			this.numPCIInterval.Size = new System.Drawing.Size(62, 19);
			this.numPCIInterval.TabIndex = 23;
			// 
			// num2ndHoliday
			// 
			this.num2ndHoliday.Location = new System.Drawing.Point(267, 92);
			this.num2ndHoliday.Name = "num2ndHoliday";
			this.num2ndHoliday.Size = new System.Drawing.Size(62, 19);
			this.num2ndHoliday.TabIndex = 24;
			// 
			// num3rdHoliday
			// 
			this.num3rdHoliday.Location = new System.Drawing.Point(267, 118);
			this.num3rdHoliday.Name = "num3rdHoliday";
			this.num3rdHoliday.Size = new System.Drawing.Size(62, 19);
			this.num3rdHoliday.TabIndex = 25;
			// 
			// numPCIHoliday
			// 
			this.numPCIHoliday.Location = new System.Drawing.Point(267, 143);
			this.numPCIHoliday.Name = "numPCIHoliday";
			this.numPCIHoliday.Size = new System.Drawing.Size(62, 19);
			this.numPCIHoliday.TabIndex = 26;
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(53, 12);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(110, 19);
			this.txtName.TabIndex = 27;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(12, 15);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(35, 12);
			this.label9.TabIndex = 28;
			this.label9.Text = "名前：";
			// 
			// calendar1
			// 
			this.calendar1.AllowChangeMonth = false;
			this.calendar1.FooterMessage = null;
			this.calendar1.Header = 18;
			this.calendar1.HolidayColor = System.Drawing.Color.Red;
			this.calendar1.Location = new System.Drawing.Point(10, 195);
			this.calendar1.Name = "calendar1";
			this.calendar1.PrintFooterHeight = 50;
			this.calendar1.PrintHeaderHeight = 50;
			this.calendar1.SaturdayColor = System.Drawing.Color.Blue;
			this.calendar1.Size = new System.Drawing.Size(355, 296);
			this.calendar1.StartWeekday = System.DayOfWeek.Monday;
			this.calendar1.TabIndex = 0;
			this.calendar1.WeekDayColor = System.Drawing.Color.Black;
			this.calendar1.ChangeDate += new System.EventHandler(this.calendar1_ChangeDate);
			this.calendar1.ChangeDrawDate += new System.EventHandler(this.calendar1_ChangeDrawDate);
			this.calendar1.DoubleClick += new System.EventHandler(this.calendar1_DoubleClick);
			// 
			// FormSetting
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(376, 532);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.numPCIHoliday);
			this.Controls.Add(this.num3rdHoliday);
			this.Controls.Add(this.num2ndHoliday);
			this.Controls.Add(this.numPCIInterval);
			this.Controls.Add(this.num3rdInterval);
			this.Controls.Add(this.num2ndInterval);
			this.Controls.Add(this.numPCI);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.cmbAttr);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.numInterval);
			this.Controls.Add(this.numHoliday);
			this.Controls.Add(this.num2nd);
			this.Controls.Add(this.num1st);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.num3rd);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblCalendar);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.calendar1);
			this.Name = "FormSetting";
			this.Text = "FormSetting";
			this.Load += new System.EventHandler(this.FormSetting_Load);
			((System.ComponentModel.ISupportInitialize)(this.num3rd)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.num1st)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.num2nd)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numHoliday)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPCI)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.num2ndInterval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.num3rdInterval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPCIInterval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.num2ndHoliday)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.num3rdHoliday)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPCIHoliday)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private CalendarControl.Calendar calendar1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label lblCalendar;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown num3rd;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown num1st;
		private System.Windows.Forms.NumericUpDown num2nd;
		private System.Windows.Forms.NumericUpDown numHoliday;
		private System.Windows.Forms.NumericUpDown numInterval;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cmbAttr;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown numPCI;
		private System.Windows.Forms.NumericUpDown num2ndInterval;
		private System.Windows.Forms.NumericUpDown num3rdInterval;
		private System.Windows.Forms.NumericUpDown numPCIInterval;
		private System.Windows.Forms.NumericUpDown num2ndHoliday;
		private System.Windows.Forms.NumericUpDown num3rdHoliday;
		private System.Windows.Forms.NumericUpDown numPCIHoliday;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label9;
	}
}