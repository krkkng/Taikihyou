using StandbyList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar
{
	public partial class FormSetting : Form
	{
		public Person Person { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public FormSetting()
		{
			InitializeComponent();
		}

		private void calendar1_ChangeDate(object sender, EventArgs e)
		{

		}

		private void calendar1_DoubleClick(object sender, EventArgs e)
		{
			var schedule = calendar1.GetSchedule(calendar1.SelectedDate);
			CalendarControl.Schedule item = new CalendarControl.Schedule();
			item.Start = calendar1.SelectedDate;
			if (schedule.Count() == 0)
			{
				item.Item = "×";
				calendar1.RemoveSchedule(calendar1.SelectedDate);
				calendar1.AddSchedule(item);
			}
			else if (schedule[0].Item.ToString() == "×")
			{
				item.Item = "当直";
				calendar1.RemoveSchedule(calendar1.SelectedDate);
				calendar1.AddSchedule(item);
			}
			else if (schedule[0].Item.ToString() == "当直")
			{
				item.Item = "△";
				calendar1.RemoveSchedule(calendar1.SelectedDate);
				calendar1.AddSchedule(item);
			}
			else
			{
				calendar1.RemoveSchedule(calendar1.SelectedDate);
			}

			
		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void FormSetting_Load(object sender, EventArgs e)
		{
			this.Text = "個人設定";
			txtName.Text = Person.Name;
			if (Person != null)
			{
				num1st.Value = Person.Requirement.FirstCallPossibleTimes;
				num2nd.Value = Person.Requirement.SecondCallPossibleTimes;
				num3rd.Value = Person.Requirement.ThirdCallPossibleTimes;
				numPCI.Value = Person.Requirement.PCIPossibleTimes;
				numHoliday.Value = Person.Requirement.HolidayPossibleTimes;
				num2ndHoliday.Value = Person.Requirement.SecondHolidayPossibleTimes;
				num3rdHoliday.Value = Person.Requirement.ThirdHolidayPossibleTimes;
				numPCIHoliday.Value = Person.Requirement.PCIHolidayPossibleTimes;
				numInterval.Value = Person.Requirement.Interval;
				num2ndInterval.Value = Person.Requirement.SecondInterval;
				num3rdInterval.Value = Person.Requirement.ThirdInterval;
				numPCIInterval.Value = Person.Requirement.PCIInterval;
				calendar1.SetDrawDate(Year, Month);
				
				cmbAttr.Items.Add("指導医");
				cmbAttr.Items.Add("レジデント");
				cmbAttr.Items.Add("PCI術者");
				switch (Person.Attre)
				{
					case StandbyList.Person.Attributes.SeniorPhysician:
						cmbAttr.Text = "指導医";
						break;
					case StandbyList.Person.Attributes.Resident:
						cmbAttr.Text = "レジデント";
						break;
					case StandbyList.Person.Attributes.PCIOperator:
						cmbAttr.Text = "PCI術者";
						break;
				}
				PossibleDays.Status status;
				for(int i = 0; i < DateTime.DaysInMonth(Year, Month); i++)
				{
					status = Person[new DateTime(Year, Month, i + 1)];
					CalendarControl.Schedule item;
					DateTime date = new DateTime(Year, Month, i + 1);
					switch (status)
					{
						case PossibleDays.Status.Affair:
							item = new CalendarControl.Schedule();
							item.Start = date;
							item.Item = "×";
							calendar1.AddSchedule(item);
							break;
						case PossibleDays.Status.Duty:
							item = new CalendarControl.Schedule();
							item.Start = date;
							item.Item = "当直";
							calendar1.AddSchedule(item);
							break;
						case PossibleDays.Status.Limited:
							item = new CalendarControl.Schedule();
							item.Start = date;
							item.Item = "△";
							calendar1.AddSchedule(item);
							break;
						default:
							break;
					}
				}
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (txtName.Text == "")
			{
				MessageBox.Show("名前を入力して下さい");
				return;
			}
			Person.Name = txtName.Text;
			switch(cmbAttr.Text)
			{
				case "指導医":
					Person.Attre = StandbyList.Person.Attributes.SeniorPhysician;
					break;
				case "レジデント":
					Person.Attre = StandbyList.Person.Attributes.Resident;
					break;
				case "PCI術者":
					Person.Attre = StandbyList.Person.Attributes.PCIOperator;
					break;
			}
			Person.Requirement.FirstCallPossibleTimes = (int)num1st.Value;
			Person.Requirement.SecondCallPossibleTimes = (int)num2nd.Value;
			Person.Requirement.ThirdCallPossibleTimes = (int)num3rd.Value;
			Person.Requirement.PCIPossibleTimes = (int)numPCI.Value;
			Person.Requirement.HolidayPossibleTimes = (int)numHoliday.Value;
			Person.Requirement.SecondHolidayPossibleTimes = (int)num2ndHoliday.Value;
			Person.Requirement.ThirdHolidayPossibleTimes = (int)num3rdHoliday.Value;
			Person.Requirement.PCIHolidayPossibleTimes = (int)numPCIHoliday.Value;
			Person.Requirement.Interval = (int)numInterval.Value;
			Person.Requirement.SecondInterval = (int)num2ndInterval.Value;
			Person.Requirement.ThirdInterval = (int)num3rdInterval.Value;
			Person.Requirement.PCIInterval = (int)numPCIInterval.Value;
			if (!Person.Possible.Any(t => t.Year == Year && t.Month == Month))
				Person.Possible.Add(new PossibleDays(Year, Month));
			for(int i = 0; i < DateTime.DaysInMonth(Year, Month); i++)
			{
				DateTime dt = new DateTime(Year, Month, i + 1);
				CalendarControl.Schedule[] schedule = calendar1.GetSchedule(dt);
				if (schedule.Count() != 0 && schedule[0].Item.ToString() == "×")
				{
					Person[new DateTime(Year, Month, i + 1)] = StandbyList.PossibleDays.Status.Affair;
				}
				else if (schedule.Count() != 0 && schedule[0].Item.ToString() == "当直")
				{
					Person[new DateTime(Year, Month, i + 1)] = PossibleDays.Status.Duty;
				}
				else if (schedule.Count() != 0 && schedule[0].Item.ToString() == "△")
				{
					Person[new DateTime(Year, Month, i + 1)] = PossibleDays.Status.Limited;
				}
				else
					Person[new DateTime(Year, Month, i + 1)] = StandbyList.PossibleDays.Status.None;
			}
			DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void calendar1_ChangeDrawDate(object sender, EventArgs e)
		{
			lblCalendar.Text = "不可日：" + calendar1.DrawYear + "年" + calendar1.DrawMonth + "月";
		}

		private void cmbAttr_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(cmbAttr.Text == "PCI術者")
			{
				numPCI.Enabled = true;
				numPCIHoliday.Enabled = true;
				numPCIInterval.Enabled = true;
			}
			else
			{
				numPCI.Enabled = false;
				numPCIHoliday.Enabled = false;
				numPCIInterval.Enabled = false;
			}
		}
	}
}
