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
	public partial class AggregationForm : Form
	{
		public DataGridView dgView { get { return dgView1; } }
		public int Year { get; set; }
		public int Month { get; set; }
		public AggregationForm()
		{
			InitializeComponent();

			Year = DateTime.Now.Year;
			Month = DateTime.Now.Month;
		}

		private void dgView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex == -1) return;
			FormSetting form = new FormSetting { Year = this.Year, Month = this.Month, Person = dgView.SelectedRows[0].Cells[0].Value as StandbyList.Person };
			form.ShowDialog();
		}
	}
}
