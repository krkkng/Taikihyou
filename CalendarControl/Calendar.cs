using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace CalendarControl
{
	public partial class Calendar : UserControl
	{
		private int year;
		private int month;
		private int day;

		private int drawyear;
		private int drawmonth;
		private float header; // 曜日を表示するヘッダーの高さ

		private int x_column;
		private int y_column;
		private float column_width;
		private float column_height;
		private List<Schedule> schedules;
		private List<ScheduleLabel>[] schedulelabels;
		private Button[] buttons;
		public bool AllowChangeMonth { get; set; }
		public int PrintHeaderHeight { get; set; }
		public int PrintFooterHeight { get; set; }
		public class ScheduleLabelMouseEventArgs : MouseEventArgs
		{
			public ScheduleLabel Label { get; private set; }
			public ScheduleLabelMouseEventArgs(ScheduleLabel label, MouseButtons buttons, int clicks, int x, int y, int delta)
				: base(buttons, clicks, x, y, delta)
			{
				Label = label;
			}
		}

		public class ScheduleLabelKeyEventArgs : KeyEventArgs
		{
			public ScheduleLabel Label { get; private set; }
			public ScheduleLabelKeyEventArgs(ScheduleLabel label, Keys keydata) : base(keydata)
			{
				Label = label;
			}
		}
		#region    Property ***********************************
		/// <summary>
		/// 選択されている年（西暦）
		/// </summary>
		public int Year	{　get { return year; }　}
		/// <summary>
		/// 選択されている月
		/// </summary>
		public int Month { get { return month; } }
		/// <summary>
		/// 選択されている日
		/// </summary>
		public int Day { get { return day; } }
		/// <summary>
		/// カレンダーの始まりの曜日
		/// </summary>
		public DayOfWeek StartWeekday { get; set; }
		/// <summary>
		/// 描画対象の年（西暦）
		/// </summary>
		public int DrawYear { get { return drawyear; } }
		/// <summary>
		/// 描画対象の月
		/// </summary>
		public int DrawMonth { get { return drawmonth; } }
		public Color WeekDayColor { get; set; }
		public Color HolidayColor { get; set; }
		public Color SaturdayColor { get; set; }
		/// <summary>
		/// ヘッダー（曜日領域）の高さを取得・設定します
		/// </summary>
		public float Header { get { return header; } set { header = value; } }
		public string FooterMessage { get; set; }
		public DateTime SelectedDate { get { return new DateTime(Year, Month, Day); } }
		public ScheduleLabel SelectedScheduleLabel { 
			get {
				var result =  schedulelabels.ToList().FirstOrDefault(t => t.Any(p => p.IsSelected));
				if (result != null)
					return result.FirstOrDefault(p => p.IsSelected);
				else
					return null;
			} }
		#endregion ********************************************
		#region    Event **************************************
		/// <summary>
		/// 選択された日付が変更されたことを知らせるイベント
		/// </summary>
		public event EventHandler ChangeDate;
		/// <summary>
		/// 描画対象の日付が変更されたことを知らせるイベント
		/// </summary>
		public event EventHandler ChangeDrawDate;
		/// <summary>
		/// スケジュールラベル上でのMouseUpイベント
		/// </summary>
		public event EventHandler<ScheduleLabelMouseEventArgs> ScheduleLabelMouseUp;

		public event EventHandler<ScheduleLabelMouseEventArgs> ScheduleLabelMouseDoubleClick;

		public event EventHandler<ScheduleLabelKeyEventArgs> ScheduleLabelKeyDown;
		/// <summary>
		/// 日付変更に対応するイベントパラメータ
		/// </summary>
		public class ChangeDateEventArgs : EventArgs
		{
			/// <summary>
			/// 日付が変更される前の日付
			/// </summary>
			public DateTime OldDate { get; private set; }
			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <param name="dt">日付が変更される前の日付</param>
			public ChangeDateEventArgs(DateTime dt)
			{
				OldDate = dt;
			}
		}
		#endregion ********************************************
		#region    Constractor ********************************
		public Calendar()
		{
			InitializeComponent();
			init();
		}
		private void init()
		{
			schedules = new List<Schedule>();
			//schedulelabels = new List<ScheduleLabel>[]();
			this.year = DateTime.MinValue.Year;
			this.month = DateTime.MinValue.Month;
			this.day = DateTime.MinValue.Day;
			SetDate(new DateTime(DateTime.Today.AddMonths(1).Year, DateTime.Today.AddMonths(1).Month, 1));
			SetDrawDate(DateTime.Today.AddMonths(1));
			ExtractMonthSchedules(DrawYear, DrawMonth);
			StartWeekday = DayOfWeek.Monday;
			WeekDayColor = Color.Black;
			HolidayColor = Color.Red;
			SaturdayColor = Color.Blue;
			header = this.Font.Height + 6;
			AllowChangeMonth = true;
			PrintHeaderHeight = 50;
			PrintFooterHeight = 50;
			//for (int i = 0; i < 1000; i++)
			//{
			//	AddSchedule(new Schedule { Start = DateTime.Now, Item = "test" });
			//}
		}

		private void setEvents(ScheduleLabel sl)
		{
			sl.MouseUp += sl_MouseUp;
			sl.MouseDoubleClick += sl_MouseDoubleClick;
			sl.SelectionChanged += sl_SelectionChanged;
			sl.Leave += sl_Leave;
			sl.KeyDown += sl_KeyDown;
			this.EnabledChanged += Calendar_EnabledChanged;
		}

		void Calendar_EnabledChanged(object sender, EventArgs e)
		{
			this.Invalidate();
		}

		private void deleteEvents(ScheduleLabel sl)
		{
			sl.MouseUp -= sl_MouseUp;
			sl.MouseDoubleClick -= sl_MouseDoubleClick;
			sl.SelectionChanged -= sl_SelectionChanged;
			sl.Leave -= sl_Leave;
			sl.KeyDown -= sl_KeyDown;
		}
		#endregion ********************************************

		#region    Methods*************************************
		public void AddSchedule(Schedule sd)
		{
			
			schedules.Add(sd);
			if (schedulelabels != null && DrawYear == sd.Start.Year && DrawMonth == sd.Start.Month)
			{
				ScheduleLabel sl = new ScheduleLabel(this, sd);
				setEvents(sl);
				schedulelabels[sd.Start.Day - 1].Add(sl);
				//sl.ClientRectangle = GetDateRect(sd.Start);
				Invalidate(Rectangle.Round(GetDateRect(sd.Start, this.ClientRectangle)));
			}
		}

		void sl_Leave(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}


		void sl_SelectionChanged(object sender, EventArgs e)
		{
			ScheduleLabel target = sender as ScheduleLabel;
			if(!target.IsSelected) return;
			schedulelabels.ToList().ForEach(sl =>
			{
				sl.ForEach(sl2 =>
				{
					if (sl2 != target && sl2.IsSelected)
						sl2.SetSelected(false);
				});
			});
		}
		public Schedule[] GetSchedule(DateTime dt)
		{
			return schedules.Where(s => s.Start.Date == dt.Date).ToArray();
		}
		/// <summary>
		/// 全てのスケジュールを取得します。
		/// </summary>
		/// <returns></returns>
		public List<Schedule> GetSchedules()
		{
			return schedules;
		}
		/// <summary>
		/// 選択する日付を設定します
		/// </summary>
		/// <param name="dt">表示する日付</param>
		public void SetDate(DateTime dt)
		{
			if (dt == DateTime.MinValue)
				return;
			try
			{
				DateTime prevdate = new DateTime(this.year, this.month, this.day);
				if (prevdate == dt.Date) return;
				this.year = dt.Year;
				this.month = dt.Month;
				this.day = dt.Day;
				OnChangeDate(new ChangeDateEventArgs(prevdate));
			}
			catch (ArgumentOutOfRangeException e)
			{
				MessageBox.Show(e.Message);
			}
		}
		/// <summary>
		/// 選択する日付を設定します
		/// </summary>
		/// <param name="year">年</param>
		/// <param name="month">月</param>
		/// <param name="day">日</param>
		public void SetDate(int year, int month, int day)
		{
			try
			{
				SetDate(new DateTime(year, month, day));
			}
			catch(ArgumentOutOfRangeException e)
			{
				MessageBox.Show(e.Message);
			}
		}

		/// <summary>
		/// 描画するカレンダーの日付を設定します
		/// </summary>
		/// <param name="dt"></param>
		public void SetDrawDate(DateTime dt)
		{
			if (DateTime.MinValue == dt) return;
			DateTime prevdate = new DateTime(this.year, this.month, this.day);
			this.drawyear = dt.Year;
			this.drawmonth = dt.Month;

			OnChangeDrawDate(new ChangeDateEventArgs(prevdate));
		}
		/// <summary>
		/// 描画するカレンダーの日付を設定します
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		public void SetDrawDate(int year, int month)
		{
			SetDrawDate(new DateTime(year, month, 1));
		}
		#endregion *********************************************

		#region    Override Methods ****************************
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			e.Graphics.FillRectangle(SystemBrushes.Window, this.ClientRectangle);
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			draw(e.Graphics, this.ClientRectangle);
		}
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.Invalidate();
		}
		protected override void OnMouseClick(MouseEventArgs e)
		{
			//base.OnMouseClick(e);
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			DateTime dt = PointToDate(e.Location);
			if (!AllowChangeMonth)
			{
				if (dt.Year != DrawYear || dt.Month != DrawMonth)
					return;
				else
				{
					SetDrawDate(dt);
					SetDate(dt);
				}
			}
			else
			{
				SetDrawDate(dt);
				SetDate(dt);
			}
		}
		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			DateTime dt = PointToDate(e.Location);
			//Debug.Print(dt.ToString());
		}
		protected virtual void OnScheduleLabelMouseUp(ScheduleLabelMouseEventArgs e)
		{
			if (ScheduleLabelMouseUp != null)
				ScheduleLabelMouseUp(this, e);
		}

		protected virtual void OnScheduleLabelMouseDoubleClick(ScheduleLabelMouseEventArgs e)
		{
			if (ScheduleLabelMouseDoubleClick != null)
				ScheduleLabelMouseDoubleClick(this, e);
		}

		protected virtual void OnScheduleLabelKeyDown(ScheduleLabelKeyEventArgs e)
		{
			if (ScheduleLabelKeyDown != null)
				ScheduleLabelKeyDown(this, e);
		}
		#endregion *********************************************
		#region Virtual Function********************************
		/// <summary>
		/// 選択される日付が変更された時に呼び出されます
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnChangeDate(ChangeDateEventArgs e)
		{
			// クライアント領域を無効化し、再描画処理を行う
			if (e.OldDate.Year != year || e.OldDate.Month != month)
				this.Invalidate();
			else
			{
				// カレンダーの年・月が変わらなければ再描画領域を最小限にする
				this.Invalidate(Rectangle.Ceiling(GetDateRect(e.OldDate, this.ClientRectangle)));
				this.Invalidate(Rectangle.Ceiling(GetDateRect(new DateTime(Year, Month, Day), this.ClientRectangle)));
			}
			if (ChangeDate != null)
				ChangeDate(this, e);
		}
		/// <summary>
		/// 描画対象の日付が変更された時に呼び出されます
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnChangeDrawDate(ChangeDateEventArgs e)
		{
			// クライアント領域を無効化し、再描画処理を行う
			if (e.OldDate.Year != DrawYear || e.OldDate.Month != DrawMonth)
			{
				ExtractMonthSchedules(DrawYear, DrawMonth);
				this.Invalidate();
			}
			if (ChangeDrawDate != null)
				ChangeDrawDate(this, e);
		}

		#endregion **********************************************
		#region Public Methods***********************************
		/// <summary>
		/// 座標値に対応するカレンダーの日付を取得します
		/// </summary>
		/// <param name="pt">座標値</param>
		/// <returns>座標値に対応するカレンダーの日付</returns>
		public DateTime PointToDate(Point pt)
		{
			if (pt.Y < header || column_height == 0 || column_width == 0) return DateTime.MinValue;

			int row = (int)((pt.Y - header) / column_height);
			int mod = (int)((pt.Y - header) % column_height);
			if (mod != 0) row++;

			int col = (int)(pt.X / column_width);
			mod = (int)(pt.X % column_width);
			if (mod != 0) col++;

			int day = (row - 1) * 7 + col - getOffset(DrawYear, DrawMonth);
			if (day <= DateTime.DaysInMonth(DrawYear, DrawMonth) && day > 0)
				return new DateTime(DrawYear, DrawMonth, day);
			else if (day > DateTime.DaysInMonth(DrawYear, DrawMonth))
				return new DateTime(DrawYear, DrawMonth, DateTime.DaysInMonth(DrawYear, DrawMonth)).AddDays(day - DateTime.DaysInMonth(DrawYear, DrawMonth));
			else if (day < 1)
				return new DateTime(DrawYear, DrawMonth, 1).AddDays(day - 1);
			else
				return DateTime.MinValue;
		}
		#endregion **********************************************
		#region Private Methods**********************************
		/// <summary>
		/// 月の予定を抽出します。
		/// </summary>
		/// <param name="DrawYear"></param>
		/// <param name="DrawMonth"></param>
		private void ExtractMonthSchedules(int DrawYear, int DrawMonth)
		{
			if(schedulelabels != null)
				schedulelabels.ToList().ForEach(t => t.ForEach(l => deleteEvents(l)));
			if (buttons != null)
				buttons.ToList().ForEach(t => t.Click -= Button_Click);
			schedulelabels = new List<ScheduleLabel>[DateTime.DaysInMonth(DrawYear, DrawMonth)];
			buttons = new Button[DateTime.DaysInMonth(DrawYear, DrawMonth)];
			for (int i = 0; i < schedulelabels.Count(); i++)
			{
				schedulelabels[i] = new List<ScheduleLabel>();
				buttons[i] = new Button(this);
				buttons[i].Click += Button_Click;
			}
			schedules.ForEach(t =>
			{
				if (t.Start.Year == DrawYear &&
					t.Start.Month == DrawMonth)
				{
					ScheduleLabel sl = new ScheduleLabel(this, t);
					setEvents(sl);
					schedulelabels[t.Start.Day - 1].Add(sl);
				}
			});
		}

		private void sl_MouseMove(object sender, MouseEventArgs e)
		{
			OnMouseMove(e);
		}

		void sl_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			OnScheduleLabelMouseDoubleClick(new ScheduleLabelMouseEventArgs((ScheduleLabel)sender, e.Button, e.Clicks, e.X, e.Y, e.Delta));
		}

		private void Button_Click(object sender, EventArgs e)
		{
			//MessageBox.Show("kurikku");
		}

		void sl_MouseUp(object sender, MouseEventArgs e)
		{
			//MessageBox.Show(((ScheduleLabel)sender).Schedule.Item);
			OnScheduleLabelMouseUp(new ScheduleLabelMouseEventArgs((ScheduleLabel)sender, e.Button, e.Clicks, e.X, e.Y, e.Delta));
			
		}

		void sl_KeyDown(object sender, KeyEventArgs e)
		{
			ScheduleLabelKeyEventArgs keyevent = new ScheduleLabelKeyEventArgs((ScheduleLabel)sender, e.KeyData);
			OnScheduleLabelKeyDown(new ScheduleLabelKeyEventArgs((ScheduleLabel)sender, e.KeyData));
		}

		/// <summary>
		/// カレンダーテーブルを初期化します
		/// </summary>
		private void setTable(RectangleF bounds)
		{
			x_column = 7;
			y_column = WeekCount(DrawYear, DrawMonth, (int)StartWeekday);
			column_height = (bounds.Height - header) / y_column;
			column_width = bounds.Width / x_column;
		}
		// 月の週数が何週有るかを計算します。
		private int WeekCount(int Year, int Month, int ws)
		{
			DateTime date = new DateTime(Year, Month, 1);
			DayOfWeek y = date.DayOfWeek;
			int days = DateTime.DaysInMonth(Year, Month);
			int offset = getOffset(Year, Month);
			int mod = (int)(offset + days) % 7;
			int row = (int)(offset + days) / 7;
			if (mod != 0) row++;
			return row;
		}
		// 日付に対応するセルの四角形を取得します
		private RectangleF GetDateRect(DateTime dt, RectangleF rc)
		{
			return GetDateRect(dt.Year, dt.Month, dt.Day, rc);
		}
		// 日付に対応するセルの四角形を取得します
		private RectangleF GetDateRect(int Year, int Month, int Day, RectangleF rc)
		{
			int offset = getOffset(Year, Month);
			int row = (offset + Day - 1) / 7;
			int col = (offset + Day - 1) % 7;
			int count = WeekCount(Year, Month, (int)StartWeekday);
			if (row != count - 1)
				if (col != x_column - 1)
					return new RectangleF(col * column_width + rc.X, row * column_height + header + rc.Y, column_width, column_height);
				else
					return new RectangleF(col * column_width + rc.X, row * column_height + header + rc.Y, this.Width - column_width * 6, column_height);
			else
				if (col != x_column - 1)
					return new RectangleF(col * column_width + rc.X, row * column_height + header + rc.Y, column_width, this.Height - (row * column_height + header) - 1);
				else
					return new RectangleF(col * column_width + rc.X, row * column_height + header + rc.Y, this.Width - column_width * 6, this.Height - (row * column_height + header) - 1);
		}
		/// <summary>
		/// 前方オフセットの領域を取得します
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <returns></returns>
		private RectangleF GetOffsetRect(int year, int month, Rectangle rc)
		{
			int offset = getOffset(year, month);
			return new RectangleF(0 + rc.X, header + rc.Y, column_width * offset, column_height);
		}
		/// <summary>
		/// 後方オフセットの領域を取得します
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <returns></returns>
		private RectangleF GetOffsetRectB(int year, int month, Rectangle rc, bool isprint = false)
		{
			int offset = getOffset(year, month);
			int maxrow = WeekCount(year, month, (int)StartWeekday);
			int offsetb = (maxrow * x_column - offset) - DateTime.DaysInMonth(year, month) ;
			float x = rc.X + (x_column - offsetb) * column_width;

			float y;
			if (isprint)
				y = header + (maxrow - 1) * column_height + rc.X + PrintHeaderHeight;
			else
				y = header + (maxrow - 1) * column_height + rc.X;
			float width = rc.Width - (x_column - offsetb) * column_width;// + (rc.Width - ((x_column) * column_width));// + offsetb * column_width + rc.Y - PrintHeaderHeight;
			float height = rc.Height - header - (maxrow - 1) * column_height;
			return new RectangleF(x, y, width, height);
		}
		private void print(Graphics g, Rectangle bounds, string FooterMessage, string Department = "循環器")
		{
			// Headerに表題を付ける
			RectangleF HeaderRect = new RectangleF(new Point(bounds.X, bounds.Y), new Size(bounds.Width, PrintHeaderHeight));
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			Font titlefont = new System.Drawing.Font(Font.FontFamily, 22);
			g.DrawString(Year.ToString() + "年" + Month.ToString() + "月" + " " + Department + "待機表", titlefont, new SolidBrush(Color.Black), HeaderRect, format);
			bounds.Y += PrintHeaderHeight;
			bounds.Height -= (PrintHeaderHeight + PrintFooterHeight);
			RectangleF FooterRect = new RectangleF(new Point(bounds.X, bounds.Y + bounds.Height), new Size(bounds.Width, PrintFooterHeight));
			format.Alignment = StringAlignment.Near;
			format.LineAlignment = StringAlignment.Center;
			g.DrawString(FooterMessage, Font, new SolidBrush(Color.Black), FooterRect, format);

			setTable(bounds);

			g.FillRectangle(SystemBrushes.ControlLight, GetOffsetRect(Year, Month, bounds));
			g.FillRectangle(SystemBrushes.ControlLight, GetOffsetRectB(Year, Month, bounds, true));
			drawTable(g, bounds);
			printWeekday(g, bounds);
			printDate(g, bounds);
		}
		private void printini(Graphics g, Rectangle bounds, string headertext, string footertext, string[] value)
		{
			// Headerに表題を付ける
			RectangleF HeaderRect = new RectangleF(new Point(bounds.X, bounds.Y), new Size(bounds.Width, PrintHeaderHeight));
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			Font titlefont = new System.Drawing.Font(Font.FontFamily, 22);
			g.DrawString(headertext, titlefont, new SolidBrush(Color.Black), HeaderRect, format);
			bounds.Y += PrintHeaderHeight;
			bounds.Height -= (PrintHeaderHeight + PrintFooterHeight);
			RectangleF FooterRect = new RectangleF(new Point(bounds.X, bounds.Y + bounds.Height), new Size(bounds.Width, PrintFooterHeight));
			format.Alignment = StringAlignment.Near;
			format.LineAlignment = StringAlignment.Center;
			g.DrawString(footertext, Font, new SolidBrush(Color.Black), FooterRect, format);

			setTable(bounds);

			g.FillRectangle(SystemBrushes.ControlLight, GetOffsetRect(Year, Month, bounds));
			g.FillRectangle(SystemBrushes.ControlLight, GetOffsetRectB(Year, Month, bounds, true));
			drawTable(g, bounds);
			printWeekday(g, bounds);
			printDateIni(g, bounds, value);
		}

		// カレンダーを描画します
		private void draw(Graphics g, Rectangle bounds, bool isprint = false)
		{
			if(isprint)
			{ 
				// Headerに表題を付ける
				RectangleF HeaderRect = new RectangleF(new Point(bounds.X, bounds.Y), new Size(bounds.Width, PrintHeaderHeight));
				StringFormat format = new StringFormat();
				format.Alignment = StringAlignment.Center;
				format.LineAlignment = StringAlignment.Center;
				Font titlefont = new System.Drawing.Font(Font.FontFamily, 22);
				g.DrawString(Year.ToString() + "年" + Month.ToString() + "月" + " 循環器待機表", titlefont, new SolidBrush(Color.Black), HeaderRect, format);
				bounds.Y += PrintHeaderHeight;
				bounds.Height -= (PrintHeaderHeight + PrintFooterHeight);
				RectangleF FooterRect = new RectangleF(new Point(bounds.X, bounds.Y + bounds.Height), new Size(bounds.Width, PrintFooterHeight));
				format.Alignment = StringAlignment.Near;
				format.LineAlignment = StringAlignment.Center;
				g.DrawString("緊急カテは最下段のドクターもコール。手薄な日はサードコールまで決めています。交代は3年目以下と指導医クラスが必ずペアになるように交代を。", Font, new SolidBrush(Color.Black), FooterRect, format);
			}
			setTable(bounds);
			if(!Enabled)
			{
				g.FillRectangle(SystemBrushes.Control, bounds);
			}
			else
			{
				g.FillRectangle(SystemBrushes.Window, bounds);
			}
			if (!isprint)
			{
				if (DrawYear == DateTime.Today.Year && DrawMonth == DateTime.Today.Month)
				{
					RectangleF todayrc = GetDateRect(DateTime.Today, bounds);
					todayrc.Location = new PointF(todayrc.Left + 1, todayrc.Top + 1);
					todayrc.Size = new System.Drawing.SizeF(todayrc.Size.Width - 2, todayrc.Size.Height - 2);
					g.DrawRectangle(Pens.Red, Rectangle.Ceiling(todayrc));
				}
			}
			g.FillRectangle(SystemBrushes.ControlLight, GetOffsetRect(Year, Month, bounds));
			g.FillRectangle(SystemBrushes.ControlLight, GetOffsetRectB(Year, Month, bounds, isprint));
			drawTable(g, bounds);
			drawWeekday(g, bounds);
			if(!isprint && Enabled)
				g.FillRectangle(new SolidBrush(Color.FromArgb(50, 0, 0, 100)), GetDateRect(Year, Month, Day, bounds));
			drawDate(g, bounds, isprint);
		}
		// カレンダーの罫線を描画します
		private void drawTable(Graphics g, Rectangle rc)
		{
			g.DrawRectangle(Pens.Black, new Rectangle(new Point(rc.X, rc.Y), new Size(rc.Width-1, rc.Height-1)));
			g.DrawLine(Pens.Gray, new PointF(rc.X, header + rc.Y), new PointF(rc.Width - 1 + rc.X, header + rc.Y));
			for (int i = 1; i < y_column; i++)
				g.DrawLine(Pens.Gray, new PointF(rc.X, header + column_height * i + rc.Y), new PointF(rc.Width - 1 + rc.X, header + column_height * i + rc.Y));
			for (int j = 1; j < x_column; j++)
				g.DrawLine(Pens.Gray, new PointF(column_width * j + rc.X, rc.Y), new PointF(column_width * j + rc.X, rc.Height - 1 + rc.Y));
		}
		private void printWeekday(Graphics g, Rectangle rc)
		{
			drawWeekday(g, rc);
		}

		private void drawWeekday(Graphics g, Rectangle rc)
		{
			string[] weekday = {"日", "月", "火", "水", "木", "金", "土"};
			for (int i = 0; i < 7; i++)
			{
				int target = ((int)StartWeekday + i) % 7;
				SizeF strsize = g.MeasureString(weekday[target], this.Font);
				Brush brush;
				switch (target)
				{
					case (int)DayOfWeek.Saturday:
						brush = Brushes.Blue;
						break;
					case (int)DayOfWeek.Sunday:
						brush = Brushes.Red;
						break;
					default:
						brush = Brushes.Black;
						break;
				}
				g.DrawString(weekday[target], this.Font, brush, new PointF(column_width * i + (column_width - (float)strsize.Width) / 2 + rc.X, (header - (float)strsize.Height) / 2 + rc.Y));
			}
		}
		private int getOffset(int Year, int Month)
		{
			DateTime date = new DateTime(DrawYear, DrawMonth, 1);
			int d = (int)date.DayOfWeek - (int)StartWeekday;
			if (d < 0)
				d = 7 + d;
			return d % 7;
		}
		private void printDate(Graphics g, Rectangle rc)
		{
			int xspace = 2; // カレンダーの日付や予定の左側の遊び
			int yspace = 3; // カレンダーの日付の上側の遊び
			int offset = getOffset(DrawYear, DrawMonth);
			for (int i = -offset; i < WeekCount(DrawYear, DrawMonth, (int)StartWeekday) * x_column - offset; i++)
			{
				int row = (offset + i) / 7;
				int col = (offset + i) % 7;
				Font font = this.Font;
				Brush brush;
				StringFormat format = new StringFormat();
				format.LineAlignment = StringAlignment.Center;
				format.Trimming = StringTrimming.EllipsisCharacter;
				if (i >= 0 && i < DateTime.DaysInMonth(DrawYear, DrawMonth))
				{
					DateTime dt = new DateTime(DrawYear, DrawMonth, i + 1);
					var holiday = GenCalendar.HolidayChecker.Holiday(dt);
					if (holiday.holiday == GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.SYUKUJITSU ||
						holiday.holiday == GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.C_HOLIDAY ||
						holiday.holiday == GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.HOLIDAY ||
						dt.DayOfWeek == DayOfWeek.Sunday)
					{
						brush = new SolidBrush(HolidayColor);
					}
					else if (dt.DayOfWeek == DayOfWeek.Saturday)
					{
						brush = new SolidBrush(SaturdayColor);
					}
					else
						brush = new SolidBrush(WeekDayColor);
					g.DrawString((i + 1).ToString() + " " + holiday.name, font, brush, new RectangleF(col * column_width + xspace + rc.X, header + (row * column_height) + yspace + rc.Y, column_width, font.Height), format);

					// スケジュール描画
					int stack = (int)(header + (row * column_height) + yspace + font.Height);
					buttons[i].ClientRectangle = Rectangle.Empty;
					////------
					float schedulelabel_height = (column_height - font.Height - yspace - 4) / 4;
					////------
					for (int j = 0; j < schedulelabels[i].Count(); j++)
					{
						ScheduleLabel scj = schedulelabels[i][j];
						float Xp = col * column_width + xspace + rc.X;	// schedule labelのclientのX座標
						float Yp;										// schedule labelのclientのY座標
						RectangleF screct;								// schedule labelのclientの四角形
						Yp = ((schedulelabel_height) * j) + header + (row * column_height) + yspace + font.Height + rc.Y;
						screct = new RectangleF(Xp, Yp, column_width - 4, schedulelabel_height);
						scj.ClientRectangle = screct;
						scj.Print(g);
					}
				}
				else if (i < 0) // 前の月の日付を描画
				{
					DateTime prevm = new DateTime(DrawYear, DrawMonth, 1).AddDays(i);
					var holiday = GenCalendar.HolidayChecker.Holiday(prevm);
					g.DrawString((prevm.Month.ToString() + "/" + prevm.Day.ToString() + " " + holiday.name), font, Brushes.Gray, new RectangleF(col * column_width + 2 + rc.X, header + (row * column_height) + 3 + rc.Y, column_width, font.Height), format);
				}
				else // 次の月の日付を描画
				{
					DateTime nextm = new DateTime(DrawYear, DrawMonth, DateTime.DaysInMonth(DrawYear, DrawMonth)).AddDays(i - DateTime.DaysInMonth(DrawYear, DrawMonth) + 1);
					var holiday = GenCalendar.HolidayChecker.Holiday(nextm);
					g.DrawString((nextm.Month.ToString() + "/" + nextm.Day.ToString() + " " + holiday.name), font, Brushes.Gray, new RectangleF(col * column_width + 2 + rc.X, header + (row * column_height) + 3 + rc.Y, column_width, font.Height), format);
				}

			}

		}
		private void printDateIni(Graphics g, Rectangle rc, string[] str)
		{
			int xspace = 2; // カレンダーの日付や予定の左側の遊び
			int yspace = 3; // カレンダーの日付の上側の遊び
			int offset = getOffset(DrawYear, DrawMonth);
			for (int i = -offset; i < WeekCount(DrawYear, DrawMonth, (int)StartWeekday) * x_column - offset; i++)
			{
				int row = (offset + i) / 7;
				int col = (offset + i) % 7;
				Font font = this.Font;
				Brush brush;
				StringFormat format = new StringFormat();
				format.LineAlignment = StringAlignment.Center;
				format.Trimming = StringTrimming.EllipsisCharacter;
				if (i >= 0 && i < DateTime.DaysInMonth(DrawYear, DrawMonth))
				{
					DateTime dt = new DateTime(DrawYear, DrawMonth, i + 1);
					var holiday = GenCalendar.HolidayChecker.Holiday(dt);
					if (holiday.holiday == GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.SYUKUJITSU ||
						holiday.holiday == GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.C_HOLIDAY ||
						holiday.holiday == GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.HOLIDAY ||
						dt.DayOfWeek == DayOfWeek.Sunday)
					{
						brush = new SolidBrush(HolidayColor);
					}
					else if (dt.DayOfWeek == DayOfWeek.Saturday)
					{
						brush = new SolidBrush(SaturdayColor);
					}
					else
						brush = new SolidBrush(WeekDayColor);
					g.DrawString((i + 1).ToString() + " " + holiday.name, font, brush, new RectangleF(col * column_width + xspace + rc.X, header + (row * column_height) + yspace + rc.Y, column_width, font.Height), format);

					// スケジュール描画
					////------
					//float schedulelabel_height = (column_height - header + yspace);
					////------
						float Xp = col * column_width + xspace + rc.X;	// schedule labelのclientのX座標
						float Yp;										// schedule labelのclientのY座標
						RectangleF screct;								// schedule labelのclientの四角形
						Yp = this.Font.Height + header + (row * column_height) + yspace + font.Height + rc.Y;
						screct = new RectangleF(Xp, Yp, column_width - 4, this.Font.Height);
						g.DrawString(str[i], this.Font, Brushes.Black, screct, format);
				}
				else if (i < 0) // 前の月の日付を描画
				{
					DateTime prevm = new DateTime(DrawYear, DrawMonth, 1).AddDays(i);
					var holiday = GenCalendar.HolidayChecker.Holiday(prevm);
					g.DrawString((prevm.Month.ToString() + "/" + prevm.Day.ToString() + " " + holiday.name), font, Brushes.Gray, new RectangleF(col * column_width + 2 + rc.X, header + (row * column_height) + 3 + rc.Y, column_width, font.Height), format);
				}
				else // 次の月の日付を描画
				{
					DateTime nextm = new DateTime(DrawYear, DrawMonth, DateTime.DaysInMonth(DrawYear, DrawMonth)).AddDays(i - DateTime.DaysInMonth(DrawYear, DrawMonth) + 1);
					var holiday = GenCalendar.HolidayChecker.Holiday(nextm);
					g.DrawString((nextm.Month.ToString() + "/" + nextm.Day.ToString() + " " + holiday.name), font, Brushes.Gray, new RectangleF(col * column_width + 2 + rc.X, header + (row * column_height) + 3 + rc.Y, column_width, font.Height), format);
				}

			}

		}

		/// <summary>
		/// 日付の描画を行います
		/// </summary>
		/// <param name="g"></param>
		private void drawDate(Graphics g, Rectangle rc, bool isprint = false)
		{
			int xspace = 2; // カレンダーの日付や予定の左側の遊び
			int yspace = 3; // カレンダーの日付の上側の遊び
			int offset = getOffset(DrawYear, DrawMonth);
			for(int i = -offset; i < WeekCount(DrawYear, DrawMonth, (int)StartWeekday) * x_column - offset; i++)
			{
				int row = (offset + i) / 7;
				int col = (offset + i) % 7;
				Font font = this.Font;
				Brush brush;
				StringFormat format = new StringFormat();
				format.LineAlignment = StringAlignment.Center;
				format.Trimming = StringTrimming.EllipsisCharacter;
				if (i >= 0 && i < DateTime.DaysInMonth(DrawYear, DrawMonth))
				{
					DateTime dt = new DateTime(DrawYear, DrawMonth, i + 1);
					var holiday = GenCalendar.HolidayChecker.Holiday(dt);
					if (holiday.holiday == GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.SYUKUJITSU ||
						holiday.holiday == GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.C_HOLIDAY ||
						holiday.holiday == GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.HOLIDAY ||
						dt.DayOfWeek == DayOfWeek.Sunday)
					{
						brush = new SolidBrush(HolidayColor);
					}
					else if (dt.DayOfWeek == DayOfWeek.Saturday)
					{
						brush = new SolidBrush(SaturdayColor);
					}
					else
						brush = new SolidBrush(WeekDayColor);
					g.DrawString((i + 1).ToString() + " " + holiday.name, font, brush, new RectangleF(col * column_width + xspace + rc.X, header + (row * column_height) + yspace + rc.Y, column_width, font.Height), format);

					// スケジュール描画
					int space;
					if (isprint) space = 20;
					else space = 6;
					int buttongap = 0;	// ボタンの上下の遊び
					int stack = (int)(header + (row * column_height) + yspace + font.Height);
					buttons[i].ClientRectangle = Rectangle.Empty;
					////------
					float schedulelabel_height = (column_height - header + yspace) / 4;
					////------
					for (int j = 0; j < schedulelabels[i].Count(); j++)
					{
						ScheduleLabel scj = schedulelabels[i][j];
						float Xp = col * column_width + xspace + rc.X;	// schedule labelのclientのX座標
						float Yp;										// schedule labelのclientのY座標
						RectangleF screct;								// schedule labelのclientの四角形
						if (isprint)
						{
							Yp = ((schedulelabel_height) * j) + header + (row * column_height) + yspace + font.Height + rc.Y;
							screct = new RectangleF(Xp, Yp, column_width - 4, schedulelabel_height);
						}
						else
						{
							Yp = ((font.Height + space) * j) + header + (row * column_height) + yspace + font.Height + rc.Y;
							screct = new RectangleF(Xp, Yp, column_width - 4, font.Height + space);
						}
						stack += (int)screct.Height;
						if (stack > header + (row * column_height) + column_height - font.Height - buttongap && !isprint)
						{
							buttons[i].ClientRectangle = new Rectangle((int)screct.X + rc.X, (int)(header + (row * column_height) + column_height - font.Height - buttongap + rc.Y), (int)screct.Width, font.Height + buttongap);
							buttons[i].Text = "他" + (schedulelabels[i].Count - j).ToString() + "件";
							buttons[i].Draw(g);
							break;
						}
						scj.ClientRectangle = screct;
						scj.Paint(g, isprint);
					}
				}
				else if (i < 0) // 前の月の日付を描画
				{
					DateTime prevm = new DateTime(DrawYear, DrawMonth, 1).AddDays(i);
					var holiday = GenCalendar.HolidayChecker.Holiday(prevm);
					g.DrawString((prevm.Month.ToString() + "/" + prevm.Day.ToString() + " " + holiday.name), font, Brushes.Gray, new RectangleF(col * column_width + 2 + rc.X, header + (row * column_height) + 3 + rc.Y, column_width, font.Height), format);
				}
				else // 次の月の日付を描画
				{
					DateTime nextm = new DateTime(DrawYear, DrawMonth, DateTime.DaysInMonth(DrawYear, DrawMonth)).AddDays(i - DateTime.DaysInMonth(DrawYear, DrawMonth) + 1);
					var holiday = GenCalendar.HolidayChecker.Holiday(nextm);
					g.DrawString((nextm.Month.ToString() + "/" + nextm.Day.ToString() + " " + holiday.name), font, Brushes.Gray, new RectangleF(col * column_width + 2 + rc.X, header + (row * column_height) + 3 + rc.Y, column_width, font.Height), format);
				}

			}
		}

		#endregion **********************************************

		public void RemoveAll()
		{
			schedules.Clear();
			for (int i = 0; i < DateTime.DaysInMonth(DrawYear, DrawMonth); i++)
			{
				schedulelabels[i].ForEach(t =>
				{
					t.MouseDoubleClick -= sl_MouseDoubleClick;
					t.MouseMove -= sl_MouseMove;
					t.MouseUp -= sl_MouseUp;
				});
				schedulelabels[i].Clear();
			}

			Parent.Invalidate();
		}

		public void RemoveIndex(int p)
		{
			throw new NotImplementedException();
		}

		public void RemoveSchedule(DateTime dt)
		{
			schedules.RemoveAll(s => s.Start.Date == dt.Date);
			schedulelabels[dt.Day - 1].Clear();
			Invalidate(Rectangle.Ceiling(GetDateRect(dt, this.ClientRectangle)));
		}

		public void PrintCalendar(Graphics g, Rectangle rc, string FooterMessage, string Department = "循環器")
		{
			print(g, rc, FooterMessage, Department);
		}

		public void RemoveMonth(int Year, int Month)
		{
			if (Year < DateTime.MinValue.Year || Year > DateTime.MaxValue.Year || Month > 12 || Month < 1)
				throw new ArgumentOutOfRangeException();
			schedules.RemoveAll(s => s.Start.Year == Year && s.Start.Month == Month);
			for (int i = 0; i < DateTime.DaysInMonth(Year, Month); i++)
			{
				schedulelabels[i].ForEach(t =>
					{
						t.MouseDoubleClick -= sl_MouseDoubleClick;
						t.MouseMove -= sl_MouseMove;
						t.MouseUp -= sl_MouseUp;
					});
				schedulelabels[i].Clear();
			}
			Invalidate();

		}

		public void PrintInitialCalendar(Graphics g, Rectangle rc, string HeaderText, string FooterText, string[] value)
		{
			printini(g, rc, HeaderText, FooterText, value);
		}

		private void printini(Graphics g, Rectangle rc)
		{
			throw new NotImplementedException();
		}
	}
}
