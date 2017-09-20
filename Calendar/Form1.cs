using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using CalendarControl;
using StandbyList;

namespace Calendar
{
	public partial class Form1 : Form
	{
		private ContextMenuStrip menu;
		private AggregationForm AgForm;
		public Form1()
		{
			InitializeComponent();
			Core.StandbyLists = new List<StandbyList.StandbyList>();
			Text = "待機表作成くん";

			LoadData();
		}

		private void button1_Click(object sender, EventArgs e)
		{
		}

		private void calendar1_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void Form1_Click(object sender, EventArgs e)
		{

		}

		private void calendar1_DoubleClick(object sender, EventArgs e)
		{
			CalendarControl.Schedule item = new CalendarControl.Schedule();
			item.Start = calendar1.SelectedDate;
			item.Item = calendar1.SelectedDate.ToString() + "追加";
			calendar1.AddSchedule(item);
		}

		private void CreateStandby()
		{
			calendar1.RemoveMonth(calendar1.DrawYear, calendar1.DrawMonth);
			StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.Year && t.Month == calendar1.Month);
			if (st == null) return;
				
			StandbyList.StandbyPersons[] sp;
			List<StandbyList.StandbyPersons[]> tmpsp = new List<StandbyPersons[]>();
			int count = 0;
			while(true)
			{
				count++;
				st.Create();
				sp = st.Standby;
				tmpsp.Add(sp);
				if (sp.All(p => p.Judge()) || count >= Setting.TryCount)
				{
					if(count >= Setting.TryCount)
					{
						MessageBox.Show(count.ToString() + "通りの組合せを検討した結果、条件を満たす組合せが見つかりませんでした。最適と考えられる組合せを表示します。" +
						"必要であれば「設定」から個人の必要条件を変更の上、再度お試し下さい。");
						StandbyPersons[] most = null;
						int nullcount = DateTime.DaysInMonth(calendar1.DrawYear, calendar1.DrawMonth) * 2;
						tmpsp.ForEach(t => 
							{
								if(most == null)
								{
									most = t;
								}
								else
								{
									int c = t.ToList().Sum(u => u.NullCount());
									if(nullcount >= c)
									{
										most = t;
										nullcount = c;
									}
								}

							});
						st.Standby = most;
					}
					//var stold = Core.StandbyLists.FirstOrDefault(t => t.Year == st.Year && t.Month == st.Month);
					//if (stold != null)
					//{
					//	Core.StandbyLists.Remove(stold);
					//}
					//Core.StandbyLists.Add(st);
					//Core.Sort();
					break;
				}
			}
			sp = st.Standby;
			for (int i = 0; i < sp.Count(); i++)
			{
				CalendarControl.Schedule item = new CalendarControl.Schedule();
				item.Start = new DateTime(calendar1.DrawYear, calendar1.DrawMonth, i + 1);
				item.Item = sp[i].First;
				if (sp[i].First != null)
				{
					var status = sp[i].First.Possible.First(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth).PossibleDay[i];
					if (status == StandbyList.PossibleDays.Status.Duty)
						item.ForeColor = Setting.DutyColor;
				}
				item.Description = "1st";
				item.BackColor = Setting.FirstColor;
				item.Alignment = StringAlignment.Center;
				calendar1.AddSchedule(item);

				CalendarControl.Schedule item2 = new CalendarControl.Schedule();
				item2.Start = new DateTime(calendar1.DrawYear, calendar1.DrawMonth, i + 1);

				item2.Item = sp[i].Second;
				item2.BackColor = Setting.SecondColor;
				item2.Alignment = StringAlignment.Center;
				item2.Description = "2nd";
				calendar1.AddSchedule(item2);

				CalendarControl.Schedule item3 = new CalendarControl.Schedule();
				item3.Start = new DateTime(calendar1.DrawYear, calendar1.DrawMonth, i + 1);
				item3.Item = sp[i].Third;
				item3.BackColor = Setting.ThirdColor;
				item3.Alignment = StringAlignment.Center;
				item3.Description = "3rd";
				calendar1.AddSchedule(item3);

				CalendarControl.Schedule item4 = new CalendarControl.Schedule();
				item4.Start = new DateTime(calendar1.DrawYear, calendar1.DrawMonth, i + 1);
				item4.Item = sp[i].PCI;
				item4.BackColor = Setting.PCIColor;
				item4.Alignment = StringAlignment.Center;
				item4.Description = "PCI";
				calendar1.AddSchedule(item4);
			}
			PersonsAggregation();

		}
		private void CreateStandbyEmpty()
		{
			calendar1.RemoveMonth(calendar1.DrawYear, calendar1.DrawMonth);
			StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth);
			if (st == null) return;
				
			StandbyList.StandbyPersons[] sp;
			st.CreateEmpty();
			sp = st.Standby;
			var stold = Core.StandbyLists.FirstOrDefault(t => t.Year == st.Year && t.Month == st.Month);
			if(stold != null)
			{
				Core.StandbyLists.Remove(stold);
			}
			Core.StandbyLists.Add(st);
			Core.Sort();
			for (int i = 0; i < sp.Count(); i++)
			{
				CalendarControl.Schedule item = new Schedule();
				item.Start = new DateTime(st.Year, st.Month, i + 1);
				item.Item = null;
				item.Description = "1st";
				item.BackColor = Setting.FirstColor;
				item.Alignment = StringAlignment.Center;
				calendar1.AddSchedule(item);
				Schedule item2 = new Schedule();
				item2.Start = new DateTime(st.Year, st.Month, i + 1);
				item2.Item = null;
				item2.Description = "2nd";
				item2.BackColor = Setting.SecondColor;
				item2.Alignment = StringAlignment.Center;
				calendar1.AddSchedule(item2);
				Schedule item3 = new Schedule();
				item3.Start = new DateTime(st.Year, st.Month, i + 1);
				item3.Item = null;
				item3.Description = "3rd";
				item3.BackColor = Setting.ThirdColor;
				item3.Alignment = StringAlignment.Center;
				calendar1.AddSchedule(item3);
				Schedule item4 = new Schedule();
				item4.Start = new DateTime(st.Year, st.Month, i + 1);
				item4.Item = null;
				item4.Description = "PCI";
				item4.BackColor = Setting.PCIColor;
				item4.Alignment = StringAlignment.Center;
				calendar1.AddSchedule(item4);
			}
			PersonsAggregation();
			
		}
		private void btnCreate_Click(object sender, EventArgs e)
		{
			CreateStandby();
		}

		private void PersonsAggregation()
		{
			if(AgForm == null || AgForm.Visible ==false)
				AgForm = new AggregationForm();
			AgForm.Year = calendar1.DrawYear;
			AgForm.Month = calendar1.DrawMonth;
			AgForm.dgView.Columns.Clear();
			AgForm.dgView.Columns.Add("name", "名前");
			AgForm.dgView.Columns.Add("総数", "待機数");
			AgForm.dgView.Columns.Add("1stの回数", "1st数");
			AgForm.dgView.Columns.Add("2ndの回数", "2nd数");
			AgForm.dgView.Columns.Add("3rdの回数", "3rd数");
			AgForm.dgView.Columns.Add("PCI待機回数", "PCI数");
			AgForm.dgView.Columns.Add("1stの休日", "1st休日数");
			AgForm.dgView.Columns.Add("2ndの休日", "2ndの休日数");
			AgForm.dgView.Columns[0].Width = 70;
			AgForm.dgView.Columns[1].Width = 65;
			AgForm.dgView.Columns[2].Width = 65;
			AgForm.dgView.Columns[3].Width = 65;
			AgForm.dgView.Columns[4].Width = 65;
			AgForm.dgView.Rows.Clear();
			StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth);
			if (st == null) return;
			// 再カウント
			st.Persons.ForEach(p => 
			{
				p.FirstCounter = 0;
				p.SecondCounter = 0;
				p.ThirdCounter = 0;
				p.PCICounter = 0;
				p.HolidayCounter = 0;
				p.SecondHolidayCounter = 0;
				p.ThirdHolidayCounter = 0;
				p.PCIHolidayCounter = 0;
			});
			for(int i = 0; i < st.Standby.Count(); i++)
			{
				var date = new DateTime(calendar1.DrawYear, calendar1.DrawMonth, i + 1);
				if(st.Standby[i].First != null)
				{
					Person person = st.Standby[i].First;
					if(person[date] != PossibleDays.Status.Duty)
					{
						person.FirstCounter++;
						if(CalendarControl.GenCalendar.HolidayChecker.IsHoliday(date))
							person.HolidayCounter++;
					}
				}
				if(st.Standby[i].Second != null)
				{
					Person person = st.Standby[i].Second;
						person.SecondCounter++;
						if(CalendarControl.GenCalendar.HolidayChecker.IsHoliday(date))
							person.SecondHolidayCounter++;
				}
				if(st.Standby[i].Third != null)
				{
					Person person = st.Standby[i].Third;
						person.ThirdCounter++;
						if(CalendarControl.GenCalendar.HolidayChecker.IsHoliday(date))
							person.ThirdHolidayCounter++;
				}
				if(st.Standby[i].PCI != null)
				{
					Person person = st.Standby[i].PCI;
					person.PCICounter++;
					if(CalendarControl.GenCalendar.HolidayChecker.IsHoliday(date))
						person.PCIHolidayCounter++;
				}
			}

			st.Persons.ForEach(p => 
				{
					DataGridViewRow row = new DataGridViewRow();
					row.CreateCells(AgForm.dgView);
					int firstnotholiday = p.FirstCounter - p.HolidayCounter;
					row.Cells[0].Value = p;
					row.Cells[1].Value = p.FirstCounter + p.SecondCounter + p.ThirdCounter;
					row.Cells[2].Value = p.FirstCounter+"("+firstnotholiday + "+" + p.HolidayCounter +")" + "/" + p.Requirement.FirstCallPossibleTimes;
					row.Cells[3].Value = p.SecondCounter + "/" + p.Requirement.SecondCallPossibleTimes;
					row.Cells[4].Value = p.ThirdCounter + "/" + p.Requirement.ThirdCallPossibleTimes;
					row.Cells[5].Value = p.PCICounter + "/" + p.Requirement.PCIPossibleTimes;
					row.Cells[6].Value = p.HolidayCounter + "/" + p.Requirement.HolidayPossibleTimes;
					row.Cells[7].Value = p.SecondHolidayCounter + "/" + p.Requirement.SecondHolidayPossibleTimes;
					AgForm.dgView.Rows.Add(row);
				});
			if(!AgForm.Visible)
				AgForm.Show(this);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Application.ApplicationExit += Application_ApplicationExit;
			menu = new System.Windows.Forms.ContextMenuStrip();
			dateTimePicker1.Value = new DateTime(DateTime.Today.AddMonths(1).Year, DateTime.Today.AddMonths(1).Month, 1);
		}

		void Application_ApplicationExit(object sender, EventArgs e)
		{
			Serializer.SaveDatas(Application.StartupPath + @"\datas.dbx");
		}

		private void calendar1_ScheduleLabelMouseUp(object sender, CalendarControl.Calendar.ScheduleLabelMouseEventArgs e)
		{
			//MessageBox.Show(e.Label.Schedule.Item);
			//menu.Items.Clear();
			//List<StandbyList.Person> list = st.PossibleList(e.Label.Schedule.Start, st.Standby);
			//if (list.Count != 0 && list[0][e.Label.Schedule.Start] == StandbyList.PossibleDays.Status.Duty) return;
			//list.ForEach(t => menu.Items.Add(t.Name, null, MenuClick));
			//this.ContextMenuStrip = menu;
			//this.ContextMenuStrip.Show(PointToScreen(new Point(e.X, e.Y)));
			if(e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				menu.Items.Clear();
				menu.Items.Add("削除", null, MenuDeleteClick);
				menu.Items.Add("-", null, null);
				menu.Items.Add("その他の候補者", null, MenuOtherClick);
				menu.Items.Add("フリー入力", null, MenuFreeCommentClick);
				this.ContextMenuStrip = menu;
				this.ContextMenuStrip.Show(PointToScreen(new Point(e.X, e.Y)));
			}
		}

		private void DeleteSchedule()
		{
			ScheduleLabel label = calendar1.SelectedScheduleLabel;
			StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.Year && t.Month == calendar1.Month);
			if (st == null) return;

			int day = label.Schedule.Start.Day - 1;
			if (label != null)
			{
				Person p = label.Schedule.Item as Person;
				if (p != null)
				{
					Person newp = new Person() { Name = "-" };
					if (label.Schedule.Description == "1st")
					{
						p.FirstCounter--;
						if (CalendarControl.GenCalendar.HolidayChecker.IsHoliday(label.Schedule.Start))
						{
							p.HolidayCounter--;
						}
						st.Standby[day].First = newp;
						label.Schedule.Item = newp;
					}
					else if (label.Schedule.Description == "2nd")
					{
						p.SecondCounter--;
						if (CalendarControl.GenCalendar.HolidayChecker.IsHoliday(label.Schedule.Start))
						{
							p.SecondHolidayCounter--;
						}
						st.Standby[day].Second = newp;
						label.Schedule.Item = newp;
					}
					else if (label.Schedule.Description == "3rd")
					{
						p.ThirdCounter--;
						if (CalendarControl.GenCalendar.HolidayChecker.IsHoliday(label.Schedule.Start))
						{
							p.ThirdHolidayCounter--;
						}
						st.Standby[day].Third = newp;
						label.Schedule.Item = newp;
					}
					else if (label.Schedule.Description == "PCI")
					{
						p.PCICounter--;
						if (CalendarControl.GenCalendar.HolidayChecker.IsHoliday(label.Schedule.Start))
						{
							p.PCIHolidayCounter--;
						}
						st.Standby[day].PCI = new Person(){ Name = "×"};
						label.Schedule.Item = st.Standby[day].PCI;
					}
				}
				PersonsAggregation();
			}
		}
		private void MenuDeleteClick(object sender, EventArgs e)
		{
			DeleteSchedule();
		}
		private void MenuOtherClick(object sender, EventArgs e)
		{
			ShowSubForm(calendar1.SelectedScheduleLabel);
		}
		private void MenuFreeCommentClick(object sender, EventArgs e)
		{
			ShowfreeCommentForm(calendar1.SelectedScheduleLabel);
		}

		private void ShowfreeCommentForm(ScheduleLabel schedulelabel)
		{
			FormComment form = new FormComment();
			StandbyList.StandbyList.StandbyPosition pos;
			Person person = null;
			if(schedulelabel.Schedule.Item != null)
				person = schedulelabel.Schedule.Item as Person;
			if (schedulelabel.Schedule.Description == "1st")
				pos = StandbyList.StandbyList.StandbyPosition.First;
			else if (schedulelabel.Schedule.Description == "2nd")
				pos = StandbyList.StandbyList.StandbyPosition.Second;
			else if (schedulelabel.Schedule.Description == "3rd")
				pos = StandbyList.StandbyList.StandbyPosition.Third;
			else
				pos = StandbyList.StandbyList.StandbyPosition.PCI;
			StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth);
			if (st == null) return;
			int day = schedulelabel.Schedule.Start.Day - 1;
			if(form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				if(form.Person != null)
				{
					if (pos == StandbyList.StandbyList.StandbyPosition.First)
					{
						if (person != null)
						{
							person.FirstCounter--;
							if (CalendarControl.GenCalendar.HolidayChecker.IsHoliday(schedulelabel.Schedule.Start))
								person.HolidayCounter--;
						}
						st.Standby[day].First = form.Person;
						schedulelabel.Schedule.Item = form.Person;
					}
					else if (pos == StandbyList.StandbyList.StandbyPosition.Second)
					{
						if(person != null)
						{
							person.SecondCounter--;
							if (CalendarControl.GenCalendar.HolidayChecker.IsHoliday(schedulelabel.Schedule.Start))
								person.SecondHolidayCounter--;
						}
						st.Standby[day].Second = form.Person;
						schedulelabel.Schedule.Item = form.Person;
					}
					else if(pos == StandbyList.StandbyList.StandbyPosition.Third)
					{
						if(person != null)
						{
							person.ThirdCounter--;
							if (CalendarControl.GenCalendar.HolidayChecker.IsHoliday(schedulelabel.Schedule.Start))
								person.ThirdHolidayCounter--;
						}
						st.Standby[day].Third = form.Person;
						schedulelabel.Schedule.Item = form.Person;
					}
					else if(pos == StandbyList.StandbyList.StandbyPosition.PCI)
					{
						if (person != null)
						{
							person.PCICounter--;
							if (CalendarControl.GenCalendar.HolidayChecker.IsHoliday(schedulelabel.Schedule.Start))
								person.PCIHolidayCounter--;
						}
						st.Standby[day].PCI = form.Person;
						schedulelabel.Schedule.Item = form.Person;
					}
				}
				PersonsAggregation();
			}
		}
		private void ShowSubForm(ScheduleLabel schedulelabel)
		{
			SubForm form = new SubForm();
			List<LimitedPerson> list;
			StandbyList.StandbyList.StandbyPosition pos;
			if (schedulelabel.Schedule.Description == "1st")
				pos = StandbyList.StandbyList.StandbyPosition.First;
			else if (schedulelabel.Schedule.Description == "2nd")
				pos = StandbyList.StandbyList.StandbyPosition.Second;
			else if (schedulelabel.Schedule.Description == "3rd")
				pos = StandbyList.StandbyList.StandbyPosition.Third;
			else
				pos = StandbyList.StandbyList.StandbyPosition.PCI;
			StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth);
			if (st == null) return;
			list = st.PossibleList(schedulelabel.Schedule.Start, st.Standby, pos);//.Where(p => p.LimitStatus == LimitedPerson.Limit.None).Select(t => t.Person).ToList();
			//if (list.Count != 0 && list[0][schedulelabel.Schedule.Start] == StandbyList.PossibleDays.Status.Duty) list.Clear();
			form.AddRange(list);
			;
			int day = schedulelabel.Schedule.Start.Day - 1;
			if (schedulelabel != null)
				if (schedulelabel.Schedule.Description == "1st")
					form.Person = st.Standby[day].First;
				else if (schedulelabel.Schedule.Description == "2nd")
					form.Person = st.Standby[day].Second;
				else if (schedulelabel.Schedule.Description == "3rd")
					form.Person = st.Standby[day].Third;
				else if (schedulelabel.Schedule.Description == "PCI")
					form.Person = st.Standby[day].PCI;
			if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

				if (form.Person != null)
				{
					if (schedulelabel.Schedule.Description == "1st")
					{
						if(st.Standby[day].First != null)
							st.Standby[day].First.FirstCounter--;
						form.Person.FirstCounter++;
						if (schedulelabel.Schedule.Start.DayOfWeek == DayOfWeek.Saturday ||
							schedulelabel.Schedule.Start.DayOfWeek == DayOfWeek.Sunday ||
							!(CalendarControl.GenCalendar.HolidayChecker.Holiday(schedulelabel.Schedule.Start).holiday == CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY))
						{
							if(st.Standby[day].First != null)
								st.Standby[day].First.HolidayCounter--;
							form.Person.HolidayCounter++;
						}
						st.Standby[day].First = form.Person;
						schedulelabel.Schedule.Item = form.Person;
					}
					else if (schedulelabel.Schedule.Description == "2nd")
					{
						if(st.Standby[day].Second != null)
							st.Standby[day].Second.SecondCounter--;
						form.Person.SecondCounter++;
						if (schedulelabel.Schedule.Start.DayOfWeek == DayOfWeek.Saturday ||
							schedulelabel.Schedule.Start.DayOfWeek == DayOfWeek.Sunday ||
							!(CalendarControl.GenCalendar.HolidayChecker.Holiday(schedulelabel.Schedule.Start).holiday == CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY))
						{
							if (st.Standby[day].Second != null)
								st.Standby[day].Second.SecondHolidayCounter--;
							form.Person.SecondHolidayCounter++;
						}
						st.Standby[day].Second = form.Person;
						schedulelabel.Schedule.Item = form.Person;
					}
					else if (schedulelabel.Schedule.Description == "3rd")
					{
						if (st.Standby[day].Third != null)
							st.Standby[day].Third.ThirdCounter--;
						form.Person.ThirdCounter++;
						if (schedulelabel.Schedule.Start.DayOfWeek == DayOfWeek.Saturday ||
							schedulelabel.Schedule.Start.DayOfWeek == DayOfWeek.Sunday ||
							!(CalendarControl.GenCalendar.HolidayChecker.Holiday(schedulelabel.Schedule.Start).holiday == CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY))
						{
							if (st.Standby[day].Third != null)
								st.Standby[day].Third.ThirdHolidayCounter--;
							form.Person.ThirdHolidayCounter++;
						}

						st.Standby[day].Third = form.Person;
						schedulelabel.Schedule.Item = form.Person;
					}
					else if(schedulelabel.Schedule.Description == "PCI")
					{
						if (st.Standby[day].PCI != null)
							st.Standby[day].PCI.PCICounter--;
						form.Person.PCICounter++;
						if (schedulelabel.Schedule.Start.DayOfWeek == DayOfWeek.Saturday ||
							schedulelabel.Schedule.Start.DayOfWeek == DayOfWeek.Sunday ||
							!(CalendarControl.GenCalendar.HolidayChecker.Holiday(schedulelabel.Schedule.Start).holiday == CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY))
						{
							if (st.Standby[day].PCI != null)
								st.Standby[day].PCI.PCIHolidayCounter--;
							form.Person.PCIHolidayCounter++;
						}
						st.Standby[day].PCI = form.Person;
						schedulelabel.Schedule.Item = form.Person;
					}

				}
				PersonsAggregation();
			}

		}
		private void calendar1_ScheduleLabelMouseDoubleClick(object sender, CalendarControl.Calendar.ScheduleLabelMouseEventArgs e)
		{
			ShowSubForm(e.Label);
		}

		private void dgView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		bool calendar_flag = false;
		bool datepicker_flag = false;
		private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
		{
			if (!calendar_flag)
			{
				datepicker_flag = true;
				calendar1.SetDrawDate(dateTimePicker1.Value);
				calendar1.SetDate(dateTimePicker1.Value);
			}
			datepicker_flag = false;
		}

		private void changeEnable()
		{
			if (Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth) == null)
			{
				btnCreate.Enabled = false;
				calendar1.Enabled = false;
				menuGlobalSetting.Enabled = false;
				mnuCreate.Enabled = false;
				mnuPrint.Enabled = false;
			}
			else
			{
				btnCreate.Enabled = true;
				calendar1.Enabled = true;
				menuGlobalSetting.Enabled = true;
				mnuCreate.Enabled = true;
				mnuPrint.Enabled = true;
			}
		}
		private void calendar1_ChangeDrawDate(object sender, EventArgs e)
		{
			changeEnable();
		}

		private void calendar1_ChangeDate(object sender, EventArgs e)
		{
			if (!datepicker_flag)
			{
				calendar_flag = true;
				dateTimePicker1.Value = new DateTime(calendar1.Year, calendar1.Month, calendar1.Day);
			}
			calendar_flag = false;
		}

		private void btnSetting_Click(object sender, EventArgs e)
		{
		}

		private void mnuCreate_Click(object sender, EventArgs e)
		{
			CreateStandby();
		}

		private void Print()
		{
			System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
			pd.PrintPage += pd_PrintPage;
			PrintDialog pdialog = new PrintDialog();
			pdialog.Document = pd;
			pdialog.UseEXDialog = true;
			if(pdialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				pd.Print();
			}
		}
		private void PrintInitialCalendar()
		{
			System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
			pd.PrintPage += pd_PrintCalendarPage;
			PrintDialog pdialog = new PrintDialog();
			pdialog.Document = pd;
			pdialog.UseEXDialog = true;
			if (pdialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				pd.Print();
		}
		void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			var st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth);
			var FooterMessage = "";
			if (st != null)
				FooterMessage = st.FooterMessage;
			else
				FooterMessage = Setting.FooterMessage;

			calendar1.PrintCalendar(e.Graphics, e.MarginBounds, FooterMessage, Setting.DepartmentString);
		}

		private int pagenumber = 0;
		void pd_PrintCalendarPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			var standbylist = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.Year && t.Month == calendar1.Month);
				Person p = standbylist.Persons[pagenumber];

				string[] str = new string[DateTime.DaysInMonth(calendar1.Year, calendar1.Month)];
				for (int i = 0; i < DateTime.DaysInMonth(calendar1.Year, calendar1.Month); i++)
				{
					DateTime dt = new DateTime(calendar1.Year, calendar1.Month, i + 1);
					switch (p[dt])
					{
						case PossibleDays.Status.Duty:
							str[i] = "当直";
							break;
						default:
							str[i] = "";
							break;
					}
				}
				string strfooter = standbylist.FooterMessage_cannot;
				calendar1.PrintInitialCalendar(e.Graphics, e.MarginBounds, "不都合日確認表 " + calendar1.DrawYear.ToString() + "年" + calendar1.DrawMonth + "月 - " + p.Name + "先生", strfooter, str);
				pagenumber++;
				if (pagenumber > standbylist.Persons.Count() - 1)
				{
					e.HasMorePages = false;
					pagenumber = 0;
				}
				else
					e.HasMorePages = true;
			
					//e.HasMorePages = true;
			
			//e.HasMorePages = false;
		}
		private void mnuPrint_Click(object sender, EventArgs e)
		{
			Print();
		}

		private void calendar1_ScheduleLabelKeyDown(object sender, CalendarControl.Calendar.ScheduleLabelKeyEventArgs e)
		{
			if (e.KeyData == Keys.Delete)
				DeleteSchedule();

		}

		private void btnPrevMonth_Click(object sender, EventArgs e)
		{
			calendar1.SetDrawDate(new DateTime(calendar1.DrawYear, calendar1.DrawMonth, 1).AddMonths(-1));
			calendar1.SetDate(new DateTime(calendar1.DrawYear, calendar1.DrawMonth, 1));
			PersonsAggregation();
		}

		private void btnNextMonth_Click(object sender, EventArgs e)
		{
			calendar1.SetDrawDate(new DateTime(calendar1.DrawYear, calendar1.DrawMonth, 1).AddMonths(1));
			calendar1.SetDate(new DateTime(calendar1.DrawYear, calendar1.DrawMonth, 1));
			PersonsAggregation();
		}

		private void menuGlobalSetting_Click(object sender, EventArgs e)
		{
			ShowGlobalSettingForm();
		}

		private void ShowGlobalSettingForm()
		{
			FormGlobalSetting form = new FormGlobalSetting();
			form.StandbyList = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.Year && t.Month == calendar1.Month);
			form.Date = this.dateTimePicker1.Value;
			if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				for (int i = 0; i < DateTime.DaysInMonth(calendar1.Year, calendar1.Month); i++)
				{
					calendar1.GetSchedules().ForEach(t =>
						{
							switch (t.Description)
							{
								case "1st":
									t.BackColor = Setting.FirstColor;
									break;
								case "2nd":
									t.BackColor = Setting.SecondColor;
									break;
								case "3rd":
									t.BackColor = Setting.ThirdColor;
									break;
								case "PCI":
									t.BackColor = Setting.PCIColor;
									break;
							}
						});
				}
				calendar1.Invalidate();
			}
		}

		private void menuNew_Click(object sender, EventArgs e)
		{
			StandbyList.StandbyList st;

			var stold = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth);
			if (stold != null)
				st = stold;
			else
			{
				st = new StandbyList.StandbyList();
				st.SetDate(calendar1.DrawYear, calendar1.DrawMonth);
				var prevst = Core.StandbyLists.LastOrDefault(t => t.Year <= st.Year && t.Month < st.Month);
				if (prevst != null)
				{
					if (MessageBox.Show("以前の設定を引き続き使用しますか？", "確認", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
					{
						st.Persons = prevst.Persons;
						st.FooterMessage = prevst.FooterMessage;
						st.FooterMessage_cannot = prevst.FooterMessage_cannot;
					}
				}
				Core.StandbyLists.Add(st);
				Core.Sort();
				CreateStandbyEmpty();
				//Core.StandbyLists.Add(st);
			}
			calendar1.Enabled = true;
			btnCreate.Enabled = true;
			mnuCreate.Enabled = true;
			mnuPrint.Enabled = true;
			menuGlobalSetting.Enabled = true;
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
		}

		private void LoadData()
		{
			calendar1.RemoveAll();
			Serializer.LoadDatas(Application.StartupPath + @"\datas.dbx");
			Core.StandbyLists.ForEach(t =>
				{
					int Year = t.Year;
					int Month = t.Month;
					for (int i = 0; i < DateTime.DaysInMonth(Year, Month); i++)
					{
						CalendarControl.Schedule item = new Schedule();
						item.Start = new DateTime(Year, Month, i + 1);
						if (t.Standby[i].First != null)
						{
							t.Standby[i].First.FirstCounter++;
							if (item.Start.DayOfWeek == DayOfWeek.Saturday ||
							item.Start.DayOfWeek == DayOfWeek.Sunday ||
							!(CalendarControl.GenCalendar.HolidayChecker.Holiday(item.Start).holiday == CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY))
							{
								t.Standby[i].First.HolidayCounter++;
							}
						}
						item.Item = t.Standby[i].First;
						if (t.Standby[i].First != null && t.Standby[i].First[new DateTime(Year, Month, i + 1)] == PossibleDays.Status.Duty)
							item.ForeColor = Setting.DutyColor;
						item.Description = "1st";
						item.BackColor = Setting.FirstColor;
						item.Alignment = StringAlignment.Center;
						calendar1.AddSchedule(item);
						Schedule item2 = new Schedule();
						item2.Start = new DateTime(Year, Month, i + 1);
						if (t.Standby[i].Second != null)
						{
							t.Standby[i].Second.SecondCounter++;
							if (item.Start.DayOfWeek == DayOfWeek.Saturday ||
							item.Start.DayOfWeek == DayOfWeek.Sunday ||
							!(CalendarControl.GenCalendar.HolidayChecker.Holiday(item.Start).holiday == CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY))
							{
								t.Standby[i].Second.SecondHolidayCounter++;
							}
						}
						item2.Item = t.Standby[i].Second;
						item2.Description = "2nd";
						item2.BackColor = Setting.SecondColor;
						item2.Alignment = StringAlignment.Center;
						calendar1.AddSchedule(item2);
						Schedule item3 = new Schedule();
						item3.Start = new DateTime(Year, Month, i + 1);
						if (t.Standby[i].Third != null)
						{
							t.Standby[i].Third.ThirdCounter++;
							if (item.Start.DayOfWeek == DayOfWeek.Saturday ||
							item.Start.DayOfWeek == DayOfWeek.Sunday ||
							!(CalendarControl.GenCalendar.HolidayChecker.Holiday(item.Start).holiday == CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY))
							{
								t.Standby[i].Third.ThirdHolidayCounter++;
							}
						}
						item3.Item = t.Standby[i].Third;
						item3.Description = "3rd";
						item3.BackColor = Setting.ThirdColor;
						item3.Alignment = StringAlignment.Center;
						calendar1.AddSchedule(item3);
						Schedule item4 = new Schedule();
						item4.Start = new DateTime(Year, Month, i + 1);
						if (t.Standby[i].PCI != null)
						{
							t.Standby[i].PCI.PCICounter++;
							if (item.Start.DayOfWeek == DayOfWeek.Saturday ||
							item.Start.DayOfWeek == DayOfWeek.Sunday ||
							!(CalendarControl.GenCalendar.HolidayChecker.Holiday(item.Start).holiday == CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY))
							{
								t.Standby[i].PCI.PCIHolidayCounter++;
							}
						}
						item4.Item = t.Standby[i].PCI;
						item4.Description = "PCI";
						item4.BackColor = Setting.PCIColor;
						item4.Alignment = StringAlignment.Center;
						calendar1.AddSchedule(item4);
					}
				});
			changeEnable();
			PersonsAggregation();
		}

		private void 予定表印刷ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PrintInitialCalendar();
		}

	}
}
