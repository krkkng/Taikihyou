using CalendarControl;
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
	public partial class SubForm : Form
	{
		public Person Person { get; set; }

		public SubForm()
		{
			InitializeComponent();
		}


		public void AddRange(List<LimitedPerson> p)
		{
			dgView.Columns.Clear();
			dgView.Columns.Add("医師名", "医師名");
			dgView.Columns.Add("交代", "交代");
			dgView.Columns.Add("メッセージ", "メッセージ");
			dgView.Columns[0].Width = 100;
			dgView.Columns[1].Width = 60;
			dgView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dgView.Columns[2].Width = 200;
			p.Sort((x, y) => x.LimitStatus - y.LimitStatus);
			DataGridViewRow[] row = new DataGridViewRow[p.Count];
			for(int i = 0; i < p.Count; i++)
			{
				row[i] = new DataGridViewRow();
				row[i].CreateCells(dgView);
				row[i].Cells[0].Value = p[i].Person;
				switch (p[i].LimitStatus)
				{
					case LimitedPerson.Limit.None:
						row[i].Cells[1].Value = "可";
						break;
					case LimitedPerson.Limit.Limited:
						row[i].Cells[1].Value = "制限あり";
						break;
					case LimitedPerson.Limit.Cannot:
						row[i].Cells[1].Value = "不可";
						break;
				}
				row[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
				row[i].Cells[2].Value = "";
				p[i].Messages.ForEach(t => row[i].Cells[2].Value = row[i].Cells[2].Value + "/" + t);
				row[i].Cells[2].Value = row[i].Cells[2].Value.ToString().TrimStart('/');
				row[i].Cells[2].Value = row[i].Cells[2].Value.ToString().TrimEnd('/');
				if (p[i].LimitStatus == LimitedPerson.Limit.Limited)
					row[i].DefaultCellStyle.ForeColor = Color.Gray;
				else if (p[i].LimitStatus == LimitedPerson.Limit.Cannot)
					row[i].DefaultCellStyle.ForeColor = Color.LightGray;
			}
			dgView.Rows.AddRange(row);
		}

		private void SubForm_Load(object sender, EventArgs e)
		{
		}

		private void dgView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (dgView.SelectedRows[0].Index != -1)
				Person = dgView.SelectedRows[0].Cells[0].Value as Person;
			else
				return;
			if (dgView.SelectedRows[0].Cells[1].Value.ToString() == "不可")
			{
				MessageBox.Show(Person.ToString() + "さんとは交代できません");
				return;
			}

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}
	}
}
