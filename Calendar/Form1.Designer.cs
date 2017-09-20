namespace Calendar
{
	partial class Form1
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.btnCreate = new System.Windows.Forms.Button();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.menuNew = new System.Windows.Forms.ToolStripMenuItem();
			this.menuGlobalSetting = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCreate = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPrint = new System.Windows.Forms.ToolStripMenuItem();
			this.予定表印刷ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.calendar1 = new CalendarControl.Calendar();
			this.btnPrevMonth = new System.Windows.Forms.Button();
			this.btnNextMonth = new System.Windows.Forms.Button();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCreate
			// 
			this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCreate.Location = new System.Drawing.Point(493, 697);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.Size = new System.Drawing.Size(134, 23);
			this.btnCreate.TabIndex = 1;
			this.btnCreate.Text = "待機表の自動作成";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(12, 31);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(200, 19);
			this.dateTimePicker1.TabIndex = 3;
			this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNew,
            this.menuGlobalSetting,
            this.mnuCreate,
            this.mnuPrint,
            this.予定表印刷ToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(639, 24);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// menuNew
			// 
			this.menuNew.Name = "menuNew";
			this.menuNew.Size = new System.Drawing.Size(86, 20);
			this.menuNew.Text = "新規作成(&N)";
			this.menuNew.Click += new System.EventHandler(this.menuNew_Click);
			// 
			// menuGlobalSetting
			// 
			this.menuGlobalSetting.Name = "menuGlobalSetting";
			this.menuGlobalSetting.Size = new System.Drawing.Size(61, 20);
			this.menuGlobalSetting.Text = "設定(&S)";
			this.menuGlobalSetting.Click += new System.EventHandler(this.menuGlobalSetting_Click);
			// 
			// mnuCreate
			// 
			this.mnuCreate.Name = "mnuCreate";
			this.mnuCreate.Size = new System.Drawing.Size(131, 20);
			this.mnuCreate.Text = "待機表の自動作成(&C)";
			this.mnuCreate.Click += new System.EventHandler(this.mnuCreate_Click);
			// 
			// mnuPrint
			// 
			this.mnuPrint.Name = "mnuPrint";
			this.mnuPrint.Size = new System.Drawing.Size(60, 20);
			this.mnuPrint.Text = "印刷(&P)";
			this.mnuPrint.Click += new System.EventHandler(this.mnuPrint_Click);
			// 
			// 予定表印刷ToolStripMenuItem
			// 
			this.予定表印刷ToolStripMenuItem.Name = "予定表印刷ToolStripMenuItem";
			this.予定表印刷ToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
			this.予定表印刷ToolStripMenuItem.Text = "予定表印刷";
			this.予定表印刷ToolStripMenuItem.Click += new System.EventHandler(this.予定表印刷ToolStripMenuItem_Click);
			// 
			// calendar1
			// 
			this.calendar1.AllowChangeMonth = true;
			this.calendar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.calendar1.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.calendar1.FooterMessage = null;
			this.calendar1.Header = 18;
			this.calendar1.HolidayColor = System.Drawing.Color.Red;
			this.calendar1.Location = new System.Drawing.Point(0, 58);
			this.calendar1.Margin = new System.Windows.Forms.Padding(5);
			this.calendar1.Name = "calendar1";
			this.calendar1.PrintFooterHeight = 50;
			this.calendar1.PrintHeaderHeight = 50;
			this.calendar1.SaturdayColor = System.Drawing.Color.Blue;
			this.calendar1.Size = new System.Drawing.Size(639, 626);
			this.calendar1.StartWeekday = System.DayOfWeek.Monday;
			this.calendar1.TabIndex = 0;
			this.calendar1.WeekDayColor = System.Drawing.Color.Black;
			this.calendar1.ChangeDate += new System.EventHandler(this.calendar1_ChangeDate);
			this.calendar1.ChangeDrawDate += new System.EventHandler(this.calendar1_ChangeDrawDate);
			this.calendar1.ScheduleLabelMouseUp += new System.EventHandler<CalendarControl.Calendar.ScheduleLabelMouseEventArgs>(this.calendar1_ScheduleLabelMouseUp);
			this.calendar1.ScheduleLabelMouseDoubleClick += new System.EventHandler<CalendarControl.Calendar.ScheduleLabelMouseEventArgs>(this.calendar1_ScheduleLabelMouseDoubleClick);
			this.calendar1.ScheduleLabelKeyDown += new System.EventHandler<CalendarControl.Calendar.ScheduleLabelKeyEventArgs>(this.calendar1_ScheduleLabelKeyDown);
			this.calendar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.calendar1_MouseUp);
			// 
			// btnPrevMonth
			// 
			this.btnPrevMonth.Location = new System.Drawing.Point(218, 29);
			this.btnPrevMonth.Name = "btnPrevMonth";
			this.btnPrevMonth.Size = new System.Drawing.Size(35, 23);
			this.btnPrevMonth.TabIndex = 6;
			this.btnPrevMonth.Text = "<";
			this.btnPrevMonth.UseVisualStyleBackColor = true;
			this.btnPrevMonth.Click += new System.EventHandler(this.btnPrevMonth_Click);
			// 
			// btnNextMonth
			// 
			this.btnNextMonth.Location = new System.Drawing.Point(253, 29);
			this.btnNextMonth.Name = "btnNextMonth";
			this.btnNextMonth.Size = new System.Drawing.Size(35, 23);
			this.btnNextMonth.TabIndex = 7;
			this.btnNextMonth.Text = ">";
			this.btnNextMonth.UseVisualStyleBackColor = true;
			this.btnNextMonth.Click += new System.EventHandler(this.btnNextMonth_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(639, 732);
			this.Controls.Add(this.btnNextMonth);
			this.Controls.Add(this.btnPrevMonth);
			this.Controls.Add(this.dateTimePicker1);
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.calendar1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Click += new System.EventHandler(this.Form1_Click);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private CalendarControl.Calendar calendar1;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem mnuPrint;
		private System.Windows.Forms.ToolStripMenuItem menuGlobalSetting;
		private System.Windows.Forms.ToolStripMenuItem mnuCreate;
		private System.Windows.Forms.Button btnPrevMonth;
		private System.Windows.Forms.Button btnNextMonth;
		private System.Windows.Forms.ToolStripMenuItem menuNew;
		private System.Windows.Forms.ToolStripMenuItem 予定表印刷ToolStripMenuItem;
	}
}

