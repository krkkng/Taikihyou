using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar
{
	public static class Setting
	{
		public static Color FirstColor = Color.LightPink;
		public static Color SecondColor = Color.LightSkyBlue;
		public static Color ThirdColor = Color.LightGray;
		public static Color PCIColor = Color.LightSalmon;

		public static int TryCount = 1000;
		public static string FooterMessage = "緊急カテは最下段のドクターもコール。手薄な日はサードコールまで決めています。交代は3年目以下と指導医クラスが必ずペアになるように交代を。";
		public static string FooterMessage_CannotList = "不都合日には”×”を付けて下さい。1日中は無理でも、待機可能な時間帯がある場合は”△”を付けて、可能な時間帯を併せて記入して下さい（特に休日）。記入が完了したら黒木まで提出をお願いします。";
		public static Color DutyColor = Color.Red;
		public static string DepartmentString = "循環器";
	}

	public static class Core
	{
		public static List<StandbyList.StandbyList> StandbyLists { get; set; }
		public static void Sort()
		{
			StandbyLists.Sort((x, y) =>
				{
					var datex = new DateTime(x.Year, x.Month, 1);
					var datey = new DateTime(y.Year, y.Month, 1);
					return datex.CompareTo(datey);
				});
		}
	}
}
