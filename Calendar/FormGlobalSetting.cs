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
	public partial class FormGlobalSetting : Form
	{
		public StandbyList.StandbyList StandbyList { get; set; }
		private List<StandbyList.Person> tmppersons;
		public DateTime Date { get; set; }
		public FormGlobalSetting()
		{
			InitializeComponent();
		}

		private void FormGlobalSetting_Load(object sender, EventArgs e)
		{
			tmppersons = (StandbyList.Clone() as StandbyList.StandbyList).Persons;
			RefreshDGV();

			cmbColor1st.SelectedItem = Setting.FirstColor;
			cmbColor2nd.SelectedItem = Setting.SecondColor;
			cmbColor3rd.SelectedItem = Setting.ThirdColor;
			cmbColorPCI.SelectedItem = Setting.PCIColor;
			txtFooterMessage.Text = StandbyList.FooterMessage;
			txtFooterMessage_cannot.Text = StandbyList.FooterMessage_cannot;
		}

		private void RefreshDGV()
		{
			dgView.Columns.Clear();
			dgView.Rows.Clear();
			dgView.Columns.Add("名前", "名前");
			dgView.Columns.Add("役職", "役職");
			dgView.Columns.Add("1st可能数", "1st可能数");
			dgView.Columns.Add("2nd可能数", "2nd可能数");
			dgView.Columns.Add("3rd可能数", "3rd可能数");
			dgView.Columns.Add("PCI待機可能数", "PCI待機可能数");
			dgView.Columns.Add("休日可能数", "1stの休日可能数");
			dgView.Columns.Add("2ndの休日可能数", "2ndの休日可能数");
			dgView.Columns.Add("3rdの休日可能数", "3rdの休日可能数");

			tmppersons.ForEach(p =>
			{
				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells(dgView);
				row.Cells[0].Value = p;
				var attr = "";
				switch(p.Attre)
				{
					case global::StandbyList.Person.Attributes.Resident:
						attr = "レジデント";
						break;
					case global::StandbyList.Person.Attributes.SeniorPhysician:
						attr = "指導医";
						break;
					case global::StandbyList.Person.Attributes.PCIOperator:
						attr = "PCI術者";
						break;
				}
				row.Cells[1].Value = attr;
				row.Cells[2].Value = p.Requirement.FirstCallPossibleTimes;
				row.Cells[3].Value = p.Requirement.SecondCallPossibleTimes;
				row.Cells[4].Value = p.Requirement.ThirdCallPossibleTimes;
				row.Cells[5].Value = p.Attre == global::StandbyList.Person.Attributes.PCIOperator ? p.Requirement.PCIPossibleTimes.ToString() : "-";
				row.Cells[6].Value = p.Requirement.HolidayPossibleTimes;
				row.Cells[7].Value = p.Requirement.SecondHolidayPossibleTimes;
				row.Cells[8].Value = p.Requirement.ThirdHolidayPossibleTimes;
				dgView.Rows.Add(row);
			});
		}
		private void btnEditMember_Click(object sender, EventArgs e)
		{
			ShowEditForm();
		}

		private void ShowEditForm()
		{
			if (dgView.SelectedRows.Count == 0) return;
			FormSetting form = new FormSetting { Year = Date.Year, Month = Date.Month, Person = dgView.SelectedRows[0].Cells[0].Value as StandbyList.Person };
			if(form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				int i = dgView.SelectedRows[0].Index;
				RefreshDGV();
				dgView.Rows[i].Selected = true;
			}
		}
		private void btnAddMember_Click(object sender, EventArgs e)
		{
			FormSetting form = new FormSetting { Year = Date.Year, Month = Date.Month, Person = new StandbyList.Person() };
			if(form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				tmppersons.Add(form.Person);
				RefreshDGV();
			}
		}

		private void btnDeleteMember_Click(object sender, EventArgs e)
		{
			if (dgView.SelectedRows.Count == 0) return;
			StandbyList.Person person = dgView.SelectedRows[0].Cells[0].Value as StandbyList.Person;
			if (person != null)
				tmppersons.Remove(person);
			RefreshDGV();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			StandbyList.Persons = tmppersons;
			DialogResult = System.Windows.Forms.DialogResult.OK;

			Setting.FirstColor = (Color)cmbColor1st.SelectedItem;
			Setting.SecondColor = (Color)cmbColor2nd.SelectedItem;
			Setting.ThirdColor = (Color)cmbColor3rd.SelectedItem;
			Setting.PCIColor = (Color)cmbColorPCI.SelectedItem;
			StandbyList.FooterMessage = this.txtFooterMessage.Text;
			StandbyList.FooterMessage_cannot = this.txtFooterMessage_cannot.Text;
			//Setting.FooterMessage = this.txtFooterMessage.Text;
			Setting.DepartmentString = this.txtDepartment.Text;
			DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void dgView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			ShowEditForm();
		}

		private void btnAllSchedules_Click(object sender, EventArgs e)
		{
			FormAllSchedules form = new FormAllSchedules() { Year = Date.Year, Month = Date.Month, StandbyList = this.StandbyList.Clone() as StandbyList.StandbyList };
			if(form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				tmppersons = form.StandbyList.Persons;
				RefreshDGV();
			}
		}
	}
}
