using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace CalendarControl
{
	public class Schedule
	{
		public struct Repeater
		{
			public enum RepeatFlag
			{
				None,
				Day,
				Week,
				Month,
				Year
			}
			public int Times { get; set; }
			public RepeatFlag Flag { get; set; }
		}
		private object item;
		public event EventHandler ItemChanged;
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public object Item
		{
			get { return item; }
			set { 
				item = value;
				OnItemChanged(EventArgs.Empty);
			}
		}

		protected virtual void OnItemChanged(EventArgs e)
		{
			if(ItemChanged != null)
			{
				ItemChanged(this, e);
			}
		}
		public string Description { get; set; }
		public Repeater RepeatSetting { get; set; }
		public Color ForeColor { get; set; }
		public Font Font { get; set; }
		public Color BackColor { get; set; }
		public StringAlignment Alignment { get; set; }
		public StringAlignment LineAlignment { get; set; }

		public Schedule()
		{
			ForeColor = Color.Black;
			BackColor = Color.LightPink;
			Alignment = StringAlignment.Near;
			LineAlignment = StringAlignment.Center;

			Font = new Font("MS UI Gothic", 9);
			Start = DateTime.MinValue;
			End = DateTime.MinValue;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			Schedule other = obj as Schedule;
			if (other == null) return false;
			return this.Item == other.Item &&
				this.Start == other.Start &&
				this.End == other.End &&
				this.Description == other.Description;
		}

		public override int GetHashCode()
		{
			return Item.GetHashCode() ^ Start.GetHashCode() ^ End.GetHashCode() ^ Description.GetHashCode();
		}

		public static bool operator == (Schedule a, Schedule b)
		{
			if (ReferenceEquals(a, b)) return true;
			if ((object)a == null || (object)b == null) return false;
			return a.Equals(b);
		}

		public static bool operator != (Schedule a, Schedule b)
		{
			if (ReferenceEquals(a, b)) return false;
			return !a.Equals(b);
		}
	}
}
