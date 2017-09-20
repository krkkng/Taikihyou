using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CalendarControl.GenCalendar;

namespace StandbyList
{
	public class StandbyList : ICloneable
	{
		public int Year { get; private set; }
		public int Month { get; private set; }
		public string FooterMessage { get; set; }
		public string FooterMessage_cannot { get; set; }
		public List<Person> Persons { get; set; }
		public StandbyPersons[] Standby { get; set; }
		public enum StandbyPosition
		{
			First,
			Second,
			Third,
			PCI,
		}
		public StandbyList()
		{
			Year = 2015;
			Month = 5;
			Person doctor1 = new Person() { Name = "黒木", Attre = Person.Attributes.PCIOperator };
			doctor1.Requirement.FirstCallPossibleTimes = 3;
			doctor1.Requirement.PCIPossibleTimes = 0;
			Person doctor2 = new Person() { Name = "山本", Attre = Person.Attributes.Resident };
			doctor2.Requirement.FirstCallPossibleTimes = 5;
			Person doctor3 = new Person() { Name = "児玉", Attre = Person.Attributes.Resident };
			doctor3.Requirement.FirstCallPossibleTimes = 5;
			Person doctor4 = new Person() { Name = "小山", Attre = Person.Attributes.PCIOperator };
			doctor4.Requirement.FirstCallPossibleTimes = 3;
			Person doctor5 = new Person() { Name = "鬼塚", Attre = Person.Attributes.SeniorPhysician };
			doctor5.Requirement.FirstCallPossibleTimes = 2;
			Person doctor6 = new Person() { Name = "西平", Attre = Person.Attributes.PCIOperator };
			doctor6.Requirement.FirstCallPossibleTimes = 2;
			Person doctor7 = new Person() { Name = "井手口", Attre = Person.Attributes.SeniorPhysician };
			doctor7.Requirement.FirstCallPossibleTimes = 2;
			Person doctor8 = new Person() { Name = "大窪", Attre = Person.Attributes.SeniorPhysician };
			doctor8.Requirement.FirstCallPossibleTimes = 5;
			Person doctor9 = new Person() { Name = "鶴田", Attre = Person.Attributes.SeniorPhysician };
			doctor9.Requirement.FirstCallPossibleTimes = 0;
			Person doctor10 = new Person() { Name = "石川", Attre = Person.Attributes.PCIOperator };
			doctor10.Requirement.FirstCallPossibleTimes = 0;
			Person doctor11 = new Person() { Name = "渡邉", Attre = Person.Attributes.SeniorPhysician };
			doctor11.Requirement.FirstCallPossibleTimes = 2;
			Person doctor12 = new Person() { Name = "西元", Attre = Person.Attributes.SeniorPhysician };
			doctor12.Requirement.FirstCallPossibleTimes = 2;

			PossibleDays d1 = new PossibleDays(Year, Month);
			PossibleDays d2 = new PossibleDays(Year, Month);
			PossibleDays d3 = new PossibleDays(Year, Month);
			PossibleDays d4 = new PossibleDays(Year, Month);
			PossibleDays d5 = new PossibleDays(Year, Month);
			PossibleDays d6 = new PossibleDays(Year, Month);
			PossibleDays d7 = new PossibleDays(Year, Month);
			PossibleDays d8 = new PossibleDays(Year, Month);
			PossibleDays d9 = new PossibleDays(Year, Month);
			PossibleDays d10 = new PossibleDays(Year, Month);
			PossibleDays d11 = new PossibleDays(Year, Month);
			PossibleDays d12 = new PossibleDays(Year, Month);

			doctor1.Possible.Add(d1);
			doctor2.Possible.Add(d2);
			doctor3.Possible.Add(d3);
			doctor4.Possible.Add(d4);
			doctor5.Possible.Add(d5);
			doctor6.Possible.Add(d6);
			doctor7.Possible.Add(d7);
			doctor8.Possible.Add(d8);
			doctor9.Possible.Add(d9);
			doctor10.Possible.Add(d10);
			doctor11.Possible.Add(d11);
			doctor12.Possible.Add(d12);

			Persons = new List<Person>();
			Standby = new StandbyPersons[DateTime.DaysInMonth(Year, Month)];
			Persons.Add(doctor1);
			Persons.Add(doctor2);
			Persons.Add(doctor3);
			Persons.Add(doctor4);
			Persons.Add(doctor5);
			Persons.Add(doctor6);
			Persons.Add(doctor7);
			Persons.Add(doctor8);
			Persons.Add(doctor9);
			Persons.Add(doctor10);
			Persons.Add(doctor11);
			Persons.Add(doctor12);

		}
		public void Create()
		{
			Standby = create();
		}

		public void CreateEmpty()
		{
			Standby = createempty();
		}
		public List<LimitedPerson> PossibleList(DateTime dt, StandbyPersons[] sp, StandbyPosition pos)
		{
			List<LimitedPerson> tmp = new List<LimitedPerson>();
			int date = dt.Day - 1;
			switch (pos)
			{
				case StandbyPosition.First:
					var duty = Persons.FirstOrDefault(t => t[dt] == PossibleDays.Status.Duty);
					if (duty != null)
					{
						LimitedPerson limitp = new LimitedPerson(duty);
						tmp.Add(limitp);
					}
					else
					{
						Persons.ForEach(p =>
							{
								if (p != sp[date].First && p != sp[date].Second && p != sp[date].Third)
								{
									LimitedPerson limitp = new LimitedPerson(p);
									if (p[dt] == PossibleDays.Status.Affair)
									{
										limitp.LimitStatus = LimitedPerson.Limit.Cannot;
										limitp.Messages.Add("不都合日です");
									}
									else if (sp[date].Second != null && sp[date].Second.Attre == Person.Attributes.Resident && p.Attre == Person.Attributes.Resident)
									{
										limitp.LimitStatus = LimitedPerson.Limit.Cannot;
										limitp.Messages.Add("1stと2ndのレジデント同士の待機は不可です");
									}
									else
									{
										if(p[dt] == PossibleDays.Status.Limited)
										{
											limitp.LimitStatus = LimitedPerson.Limit.Limited;
											limitp.Messages.Add("待機可能時間に制限があります");
										}
										if ((dt.DayOfWeek == DayOfWeek.Saturday ||
											dt.DayOfWeek == DayOfWeek.Sunday ||
											CalendarControl.GenCalendar.HolidayChecker.Holiday(dt).holiday != CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY) &&
											p.HolidayCounter >= p.Requirement.HolidayPossibleTimes)
										{
											limitp.LimitStatus = LimitedPerson.Limit.Limited;
											limitp.Messages.Add("1stの休日可能数を越えています");
										}
										if (p.FirstCounter >= p.Requirement.FirstCallPossibleTimes)
										{
											limitp.LimitStatus = LimitedPerson.Limit.Limited;
											limitp.Messages.Add("1stの制限数を超えています");
										}
										if (checkInterval(p, dt, sp, StandbyPosition.First))
										{
											limitp.LimitStatus = LimitedPerson.Limit.Limited;
											limitp.Messages.Add("待機の間隔が制限を越えています");
										}
									}
									tmp.Add(limitp);
								}
							});
					}
					break;
				case StandbyPosition.Second:
					Persons.ForEach(p =>
						{
							if (p != sp[date].First && p != sp[date].Second && p != sp[date].Third)
							{
								LimitedPerson limitp = new LimitedPerson(p);
								if (p[dt] == PossibleDays.Status.Affair)
								{
									limitp.LimitStatus = LimitedPerson.Limit.Cannot;
									limitp.Messages.Add("不都合日です");
								}
								else if (sp[date].First != null && sp[date].First.Attre == Person.Attributes.Resident && p.Attre == Person.Attributes.Resident)
								{
									limitp.LimitStatus = LimitedPerson.Limit.Cannot;
									limitp.Messages.Add("1stと2ndのレジデント同士の待機は不可です");
								}
								else
								{
									if (p[dt] == PossibleDays.Status.Limited)
									{
										limitp.LimitStatus = LimitedPerson.Limit.Limited;
										limitp.Messages.Add("待機可能時間に制限があります");
									}
									if ((dt.DayOfWeek == DayOfWeek.Saturday ||
										dt.DayOfWeek == DayOfWeek.Sunday ||
										CalendarControl.GenCalendar.HolidayChecker.Holiday(dt).holiday != CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY) &&
										p.SecondHolidayCounter >= p.Requirement.SecondHolidayPossibleTimes)
									{
										limitp.LimitStatus = LimitedPerson.Limit.Limited;
										limitp.Messages.Add("2ndの休日可能数を越えています");
									}
									if (p.SecondCounter >= p.Requirement.SecondCallPossibleTimes)
									{
										limitp.LimitStatus = LimitedPerson.Limit.Limited;
										limitp.Messages.Add("2ndの制限数を超えています");
									}
									if (checkInterval(p, dt, sp, StandbyPosition.Second))
									{
										limitp.LimitStatus = LimitedPerson.Limit.Limited;
										limitp.Messages.Add("2ndの待機の間隔が制限を越えています");
									}
								}
								tmp.Add(limitp);
							}
						});
					break;
				case StandbyPosition.Third:
					Persons.ForEach(p =>
					{
						if (p != sp[date].First && p != sp[date].Second && p != sp[date].Third)
						{
							LimitedPerson limitp = new LimitedPerson(p);
							if (p[dt] == PossibleDays.Status.Affair)
							{
								limitp.LimitStatus = LimitedPerson.Limit.Cannot;
								limitp.Messages.Add("不都合日です");
							}
							else
							{
								if (p[dt] == PossibleDays.Status.Limited)
								{
									limitp.LimitStatus = LimitedPerson.Limit.Limited;
									limitp.Messages.Add("待機可能時間に制限があります");
								}
								if ((dt.DayOfWeek == DayOfWeek.Saturday ||
									dt.DayOfWeek == DayOfWeek.Sunday ||
									CalendarControl.GenCalendar.HolidayChecker.Holiday(dt).holiday != CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY) &&
									p.ThirdHolidayCounter >= p.Requirement.ThirdHolidayPossibleTimes)
								{
									limitp.LimitStatus = LimitedPerson.Limit.Limited;
									limitp.Messages.Add("3rdの休日可能数を越えています");
								}
								if (p.ThirdCounter >= p.Requirement.ThirdCallPossibleTimes)
								{
									limitp.LimitStatus = LimitedPerson.Limit.Limited;
									limitp.Messages.Add("3rdの制限数を超えています");
								}
								if (checkInterval(p, dt, sp, StandbyPosition.Third))
								{
									limitp.LimitStatus = LimitedPerson.Limit.Limited;
									limitp.Messages.Add("3rdの待機の間隔が制限を越えています");
								}
							}
							tmp.Add(limitp);
						}
					});
					break;
				case StandbyPosition.PCI:
					Persons.ForEach(p =>
					{
						if (p != sp[date].PCI && p.Attre == Person.Attributes.PCIOperator)
						{
							LimitedPerson limitp = new LimitedPerson(p);
							if (p[dt] == PossibleDays.Status.Affair)
							{
								limitp.LimitStatus = LimitedPerson.Limit.Cannot;
								limitp.Messages.Add("不都合日です");
							}
							else
							{
								if (p[dt] == PossibleDays.Status.Limited)
								{
									limitp.LimitStatus = LimitedPerson.Limit.Limited;
									limitp.Messages.Add("待機可能時間に制限があります");
								}
								if ((dt.DayOfWeek == DayOfWeek.Saturday ||
									dt.DayOfWeek == DayOfWeek.Sunday ||
									CalendarControl.GenCalendar.HolidayChecker.Holiday(dt).holiday != CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY) &&
									p.PCIHolidayCounter >= p.Requirement.PCIHolidayPossibleTimes)
								{
									limitp.LimitStatus = LimitedPerson.Limit.Limited;
									limitp.Messages.Add("PCI待機の休日可能数を越えています");
								}
								if (p.PCICounter >= p.Requirement.PCIPossibleTimes)
								{
									limitp.LimitStatus = LimitedPerson.Limit.Limited;
									limitp.Messages.Add("PCI待機の制限数を超えています");
								}
								if (checkInterval(p, dt, sp, StandbyPosition.PCI))
								{
									limitp.LimitStatus = LimitedPerson.Limit.Limited;
									limitp.Messages.Add("PCI待機の間隔が制限を越えています");
								}
							}
							tmp.Add(limitp);
						}
					});
					break;
			}
			return tmp;
		}
		public StandbyPersons[] createempty()
		{
			StandbyPersons[] result = new StandbyPersons[DateTime.DaysInMonth(Year, Month)];
			int maxday = DateTime.DaysInMonth(Year, Month);
			Persons.ForEach(p =>
				{
					p.FirstCounter = 0;
					p.SecondCounter = 0;
					p.ThirdCounter = 0;
					p.HolidayCounter = 0;
					p.SecondHolidayCounter = 0;
					p.ThirdHolidayCounter = 0;
					p.PCICounter = 0;
					p.PCIHolidayCounter = 0;
				});	// カウンターの初期化

			for (int i = 0; i < maxday; i++)
			{
				result[i] = new StandbyPersons();
				result[i].First = null;
				result[i].Second = null;
				result[i].Third = null;
				result[i].PCI = null;
			}
			return result;
		}
		/*public StandbyPersons[] create2()
		{
			StandbyPersons[] result = new StandbyPersons[DateTime.DaysInMonth(Year, Month)];
			Random rand1 = new Random(Environment.TickCount);
			int maxday = DateTime.DaysInMonth(Year, Month);
			Persons.ForEach(p =>
				{
					p.FirstCounter = 0;
					p.SecondCounter = 0;
					p.ThirdCounter = 0;
					p.HolidayCounter = 0;
					p.SecondHolidayCounter = 0;
					p.ThirdHolidayCounter = 0;
					p.PCICounter = 0;
					p.PCIHolidayCounter = 0;
				});	// カウンターの初期化
			// まずは1stを埋める
			List<Person>[] possiblepersons = new List<Person>[maxday];
			for (int i = 0; i < maxday; i++)
			{
				result[i] = new StandbyPersons();
				DateTime date = new DateTime(Year, Month, i + 1);
				List<Person> tmp = PossibleList(date, result, StandbyPosition.First).Where(p => p.LimitStatus == LimitedPerson.Limit.None).Select(t => t.Person).ToList();
				possiblepersons[i] = tmp;
			}
			possiblepersons.ToList().Sort((x, y) => x.Count - y.Count);   // 待機可能な候補者が少ない日順に並び替える。
			for(int i = 0; i < maxday; i++)
			{
				Person select = null;
				DateTime date = new DateTime(Year, Month, i + 1);
				if(possiblepersons[i].Count == 0)
				{
					result[i].First = select;
				}
				else if (possiblepersons[i].Count == 1)
				{
					select = possiblepersons[i][0];
					select.FirstCounter++;
					if (HolidayChecker.IsHoliday(date))
						select.HolidayCounter++;
					result[i].First = select;
				}
				else
				{
					List<Person> duplicatelist = new List<Person>();
					for(int j = 0; j < possiblepersons[i].Count; j++)
					{
						for (int k = 0; k < possiblepersons[i][j].Requirement.FirstCallPossibleTimes - possiblepersons[i][j].FirstCounter; k++)
							duplicatelist.Add(possiblepersons[i][j]);
					}
					if(result[i].First == null)
					{
						int con = rand1.Next(duplicatelist.Count);
						select = duplicatelist[con];
						select.FirstCounter++;
						if (HolidayChecker.IsHoliday(date))
							select.HolidayCounter++;
						result[i].First = select;
					}
				}

			}
			// 2ndを決める
			possiblepersons = new List<Person>[maxday];
			for(int i = 0; i < maxday; i++)
			{
				DateTime date = new DateTime(Year, Month, i + 1);
				List<Person> tmp = PossibleList(date, result, StandbyPosition.Second).Where(p => p.LimitStatus == LimitedPerson.Limit.None).Select(t => t.Person).ToList();
				possiblepersons[i] = tmp;
			}
			possiblepersons.ToList().Sort((x, y) => x.Count - y.Count);
			for(int i = 0; i < maxday; i++)
			{
				Person select = null;
				DateTime date = new DateTime(Year, Month, i + 1);
				if(possiblepersons[i].Count == 0)
				{
					result[i].Second = null;
				}
				else
				{
					List<Person> duplicatelist = new List<Person>();
					for(int j = 0; j < possiblepersons[i].Count; j++)
					{
						for (int k = 0; k < possiblepersons[i][j].Requirement.SecondCallPossibleTimes - possiblepersons[i][j].SecondCounter; k++)
							duplicatelist.Add(possiblepersons[i][j]);
					}
					select = duplicatelist[rand1.Next(duplicatelist.Count)];
					select.SecondCounter++;
					if (HolidayChecker.IsHoliday(date))
						select.SecondHolidayCounter++;
					result[i].Second = select;
				}
			}
			// PCIを決める
			possiblepersons = new List<Person>[maxday];
			for(int i = 0; i < maxday; i++)
			{
				DateTime date = new DateTime(Year, Month, i + 1);
				List<Person> tmp = PossibleList(date, result, StandbyPosition.PCI).Where(p => p.LimitStatus == LimitedPerson.Limit.None).Select(t => t.Person).ToList();
				possiblepersons[i] = tmp;
			}
			possiblepersons.ToList().Sort((x, y) => x.Count - y.Count);
			for(int i = 0; i < maxday; i++)
			{
				Person select = null;
				DateTime date = new DateTime(Year, Month, i + 1);
				if (possiblepersons[i].Count == 0)
				{
					result[i].PCI = null;
				}
				else
				{ 
					if(possiblepersons[i].Any(p => p == result[i].First))
						select = result[i].First;
					else if(possiblepersons[i].Any(p => p == result[i].Second))
						select = result[i].Second;
					else
					{
						List<Person> duplicatelist = new List<Person>();
						for (int j = 0; j < possiblepersons[i].Count; j++)
						{
							for (int k = 0; k < possiblepersons[i][j].Requirement.PCIPossibleTimes - possiblepersons[i][j].PCICounter; k++)
								duplicatelist.Add(possiblepersons[i][j]);
						}
						select = duplicatelist[rand1.Next(duplicatelist.Count)];
						select.PCICounter++;
						if (HolidayChecker.IsHoliday(date))
							select.PCIHolidayCounter++;
						result[i].PCI = select;
					}
				}
			}
			// Thirdを決める
			possiblepersons = new List<Person>[maxday];
			for (int i = 0; i < maxday; i++ )
			{
				List<Person> tmp = new List<Person>();
				if (result[i].First != result[i].PCI || result[i].Second != result[i].PCI) // PCI術者を含めて3人確保できていればThirdは必要なし。
				{
					DateTime date = new DateTime(Year, Month, i + 1);
					tmp = PossibleList(date, result, StandbyPosition.Third).Where(p => p.LimitStatus == LimitedPerson.Limit.None).Select(t => t.Person).ToList();
					possiblepersons[i] = tmp;
				}
			}
			possiblepersons.ToList().Sort((x, y) => x.Count - y.Count);
			for (int i = 0; i < maxday; i++)
			{
				Person select = null;
				DateTime date = new DateTime(Year, Month, i + 1);
				if (possiblepersons[i].Count == 0)
					result[i].PCI = null;
				else
				{
					List<Person> duplicatelist = new List<Person>();
					for(int j = 0; j < possiblepersons[i].Count(); j++)
					{
						for (int k = 0; k < possiblepersons[i][j].Requirement.ThirdCallPossibleTimes - possiblepersons[i][j].ThirdCounter; k++)
							duplicatelist.Add(possiblepersons[i][j]);
					}
					select = duplicatelist[rand1.Next(duplicatelist.Count)];
					select.ThirdCounter++;
					if (HolidayChecker.IsHoliday(date))
						select.ThirdHolidayCounter++;
					result[i].Third = select;

				}
			}


			return result;

		}
		*/

		public struct DayCounter
		{
			public int Counter;
			public DateTime Date;
		}
		public StandbyPersons[] create()
		{
			StandbyPersons[] result = new StandbyPersons[DateTime.DaysInMonth(Year, Month)];
			Random rand1 = new Random(Environment.TickCount);
			int maxday = DateTime.DaysInMonth(Year, Month);
			Persons.ForEach(p =>
				{
					p.FirstCounter = 0;
					p.SecondCounter = 0;
					p.ThirdCounter = 0;
					p.HolidayCounter = 0;
					p.SecondHolidayCounter = 0;
					p.ThirdHolidayCounter = 0;
					p.PCICounter = 0;
					p.PCIHolidayCounter = 0;
				});	// カウンターの初期化
			// 優先的に担当を決める順番を決める
			DayCounter[] day = new DayCounter[maxday];
			for (int i = 0; i < maxday; i++ )
			{
				DateTime date = new DateTime(Year, Month, i + 1);
				day[i] = new DayCounter() { Date = date, Counter = 0 };
				Persons.ForEach(p =>
					{
						if (p[date] != PossibleDays.Status.Affair || p[date] != PossibleDays.Status.Limited)
							day[i].Counter++;
					});
			}
			Array.Sort(day, (x, y) => x.Counter - y.Counter);
			//day.ToList().Sort((x, y) => x.Counter - y.Counter);
			for (int i = 0; i < maxday; i++)
			{
				DateTime date = day[i].Date; //new DateTime(Year, Month, i + 1);
				result[date.Day - 1] = new StandbyPersons();
				Persons.ForEach(p =>
					{
						if (p[date] == PossibleDays.Status.Duty)
						{
							result[date.Day - 1].First = p;
							//p.FirstCounter++; dutyの場合はFirstCounterをカウントしない。
							if (p.Attre == Person.Attributes.PCIOperator)
							{
								result[date.Day - 1].PCI = p;
								p.PCICounter++;
							}
							if (HolidayChecker.IsHoliday(date))
							{
								p.HolidayCounter++;
								p.PCIHolidayCounter++;
							}
						}

					});
			}
			for (int i = 0; i < maxday; i++)
			{
				List<Person> tmp = new List<Person>();
				DateTime date = day[i].Date;
				Person select = null;
				if (result[date.Day - 1].First == null)
				{
					// 1stが可能な人のリストを作る
					tmp = PossibleList(date, result, StandbyPosition.First).Where(p => p.LimitStatus == LimitedPerson.Limit.None).Select(t => t.Person).ToList();
					if (tmp.Count == 0)
					{
						result[date.Day - 1].First = null;
						//continue;
					}
					else if (tmp.Count == 1)
					{
						select = tmp[0];
						select.FirstCounter++;
						if (HolidayChecker.IsHoliday(date))
							select.HolidayCounter++;
						result[date.Day - 1].First = select;
					}
					else
					{
						List<Person> duplicatelist = new List<Person>();
						for (int j = 0; j < tmp.Count(); j++)
						{
							for (int k = 0; k < tmp[j].Requirement.FirstCallPossibleTimes - tmp[j].FirstCounter; k++)
								duplicatelist.Add(tmp[j]);

						}
						if (result[date.Day - 1].First == null)
						{
							// 可能な人の中からFirstを一人選ぶ
							int con = rand1.Next(duplicatelist.Count);
							select = duplicatelist[con];
							select.FirstCounter++;
							if (HolidayChecker.IsHoliday(date))
								select.HolidayCounter++;
							result[date.Day - 1].First = select;
						}
						else
						{
							select = result[date.Day - 1].First;
						}
						if(select.Attre == Person.Attributes.PCIOperator)
						{
							result[date.Day - 1].PCI = select;
							select.PCICounter++;
							if (HolidayChecker.IsHoliday(date))
								select.PCIHolidayCounter++;
						}
					}
				}
				// Secondの候補者リストを作成
				List<Person> tmp2;
				tmp2 = PossibleList(date, result, StandbyPosition.Second).Where(p => p.LimitStatus == LimitedPerson.Limit.None).Select(t => t.Person).ToList();
				// 残りの人の中からSecondを一人選ぶ
				if (tmp2.Count == 0)
				{
					result[date.Day - 1].Second = null;
				}
				else
				{
					List<Person> duplicatelist = new List<Person>();
					if (result[date.Day - 1].First != null && result[date.Day - 1].First.Attre == Person.Attributes.PCIOperator)
					{
						List<Person> tmpnotpci = tmp2.Where(p => p.Attre != Person.Attributes.PCIOperator).ToList();
						if (tmpnotpci.Count > 0)
							tmp2 = tmpnotpci;
					}
					for (int j = 0; j < tmp2.Count(); j++)
					{
						for (int k = 0; k < tmp2[j].Requirement.SecondCallPossibleTimes - tmp2[j].SecondCounter; k++)
						{
							duplicatelist.Add(tmp2[j]);
						}

					}
					select = duplicatelist[rand1.Next(duplicatelist.Count)];
					select.SecondCounter++;
					if (date.DayOfWeek == DayOfWeek.Saturday ||
						date.DayOfWeek == DayOfWeek.Sunday ||
						CalendarControl.GenCalendar.HolidayChecker.Holiday(date).holiday != CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY)
						select.SecondHolidayCounter++;

					result[date.Day - 1].Second = select;
				}
				if (result[date.Day - 1].PCI == null)
				{
					if (select != null && select.Attre == Person.Attributes.PCIOperator)
					{
						result[date.Day - 1].PCI = select;
						select.PCICounter++;
						if (HolidayChecker.IsHoliday(date))
							select.PCIHolidayCounter++;
					}
				}
				// PCI待機の候補者リストを作成
				if (result[date.Day - 1].PCI == null)
				{
					List<Person> tmp4;
					tmp4 = PossibleList(date, result, StandbyPosition.PCI).Where(p => p.LimitStatus == LimitedPerson.Limit.None).Select(t => t.Person).ToList();
					if (tmp4.Count == 0)
					{
						result[date.Day - 1].PCI = new Person() { Name = "×" };
					}
					else
					{
						if (tmp4.Any(p => p == result[date.Day - 1].First))
							select = result[date.Day - 1].First;
						else if (tmp4.Any(p => p == result[date.Day - 1].Second))
							select = result[date.Day - 1].Second;
						else
						{
							List<Person> duplicatelist = new List<Person>();
							for (int j = 0; j < tmp4.Count(); j++)
							{
								for (int k = 0; k < tmp4[j].Requirement.PCIPossibleTimes - tmp4[j].PCICounter; k++)
								{
									duplicatelist.Add(tmp4[j]);
								}
							}
							select = duplicatelist[rand1.Next(duplicatelist.Count)];
						}
						select.PCICounter++;
						if (date.DayOfWeek == DayOfWeek.Saturday ||
							date.DayOfWeek == DayOfWeek.Sunday ||
							CalendarControl.GenCalendar.HolidayChecker.Holiday(date).holiday != CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY)
							select.PCIHolidayCounter++;
						result[date.Day - 1].PCI = select;
					}
				}
				// Thirdの候補者リストを作成
				if (result[date.Day - 1].First == result[date.Day - 1].PCI || result[date.Day - 1].Second == result[date.Day - 1].PCI)
				{
					List<Person> tmp3;
					tmp3 = PossibleList(date, result, StandbyPosition.Third).Where(p => p.LimitStatus == LimitedPerson.Limit.None).Select(t => t.Person).ToList();
					if (tmp3.Count == 0)
					{
						result[date.Day - 1].Third = new Person() { Name = "×" };
					}
					else
					{
						List<Person> duplicatelist = new List<Person>();
						for (int j = 0; j < tmp3.Count(); j++)
						{
							for (int k = 0; k < tmp3[j].Requirement.SecondCallPossibleTimes - tmp3[j].SecondCounter; k++)
							{
								duplicatelist.Add(tmp3[j]);
							}

						}
						select = duplicatelist[rand1.Next(duplicatelist.Count)];
						select.ThirdCounter++;
						if (date.DayOfWeek == DayOfWeek.Saturday ||
							date.DayOfWeek == DayOfWeek.Sunday ||
							CalendarControl.GenCalendar.HolidayChecker.Holiday(date).holiday != CalendarControl.GenCalendar.HolidayChecker.HolidayInfo.HOLIDAY.WEEKDAY)
							select.ThirdHolidayCounter++;

						result[date.Day - 1].Third = select;
					}
				}
				else
					result[date.Day - 1].Third = new Person() { Name = "-" };
			}
			return result;
		}
		/// <summary>
		/// 指定した前後のインターバルに担当になっている日がないかをチェックします。
		/// </summary>
		/// <param name="p"></param>
		/// <param name="dt"></param>
		/// <param name="sp"></param>
		/// <returns>担当がある true, 担当がない false</returns>
		private bool checkInterval(Person p, DateTime dt, StandbyPersons[] sp, StandbyPosition pos)
		{
			int counter = 0;
			switch (pos)
			{
				case StandbyPosition.First:
					counter = p.Requirement.Interval;
					break;
				case StandbyPosition.Second:
					counter = p.Requirement.SecondInterval;
					break;
				case StandbyPosition.Third:
					counter = p.Requirement.ThirdInterval;
					break;
				case StandbyPosition.PCI:
					counter = p.Requirement.PCIInterval;
					break;
				default:
					counter = 0;
					break;
			}

			for (int i = 0; i < counter; i++)
			{
				int check_day = dt.Day - (i + 1) - 1;
				if (check_day >= 0)
				{
					if (sp[check_day] != null)
					{
						switch (pos)
						{
							case StandbyPosition.First:
								if (sp[check_day].First != null && sp[check_day].First == p)
									return true;
								break;
							case StandbyPosition.Second:
								if (sp[check_day].Second != null && sp[check_day].Second == p)
									return true;
								break;
							case StandbyPosition.Third:
								if (sp[check_day].Third != null && sp[check_day].Third == p)
									return true;
								break;
							case StandbyPosition.PCI:
								if (sp[check_day].PCI != null && sp[check_day].PCI == p)
									return true;
								break;
						}
					}
				}
				check_day = dt.Day + (i + 1) - 1;
				if (check_day < DateTime.DaysInMonth(Year, Month))
				{
					if (sp[check_day] != null)
					{
						switch (pos)
						{
							case StandbyPosition.First:
								if (sp[check_day].First != null && sp[check_day].First == p)
									return true;
								break;
							case StandbyPosition.Second:
								if (sp[check_day].Second != null && sp[check_day].Second == p)
									return true;
								break;
							case StandbyPosition.Third:
								if (sp[check_day].Third != null && sp[check_day].Third == p)
									return true;
								break;
							case StandbyPosition.PCI:
								if (sp[check_day].PCI != null && sp[check_day].PCI == p)
									return true;
								break;
						}
					}
				}
			}
			return false;
		}
		public void SetDate(int year, int month)
		{
			if (Year < DateTime.MinValue.Year && Year > DateTime.MaxValue.Year &&
				Month <= 0 && Month > 12)
				throw new ArgumentOutOfRangeException();
			Year = year;
			Month = month;
			Persons.ForEach(p =>
				{
					PossibleDays pd = new PossibleDays(year, month);
					p.Possible.Add(pd);
				});
		}
		public Tuple<double, double> get1stVariance(Person person)
		{
			List<double> dayinterval = new List<double>();
			int counter = 0;
			if (Standby != null)
			{
				for (int i = 0; i < Standby.Count(); i++)
				{
					counter++;
					if (Standby[i] != null && Standby[i].First != null && Standby[i].First.Name == person.Name)
					{
						dayinterval.Add(counter);
						counter = 0;
					}
				}
			}
			if (dayinterval.Count == 0) return Tuple.Create(0d, 0d);
			dayinterval.Remove(dayinterval[0]);
			if (dayinterval.Count == 0) return Tuple.Create(0d, 0d);
			return Tuple.Create(dayinterval.Min(), MathNet.Numerics.Statistics.Statistics.Variance(dayinterval));
		}

		public int Compare(StandbyList st)
		{
			int resultthis = 0;
			int resultother = 0;
			st.Persons.ForEach(p =>
			{
				double nthis = get1stVariance(p).Item1;
				double pother = st.get1stVariance(p).Item1;
				if (nthis > pother)
					resultthis++;
				else if (nthis < pother)
					resultother++;
			});
			return resultthis - resultother;
		}

		#region ICloneable メンバー

		public object Clone()
		{
			StandbyList csbl = new StandbyList();
			csbl.Persons.Clear();
			csbl.Month = this.Month;
			csbl.Year = this.Year;
			this.Persons.ForEach(p => csbl.Persons.Add(p.Clone() as Person));
			csbl.Standby = new StandbyPersons[this.Standby.Count()];
			for(int i = 0; i < this.Standby.Count(); i++)
			{
				csbl.Standby[i] = this.Standby[i].Clone() as StandbyPersons;
			}
			return csbl;
		}

		#endregion

	}

	public class StandbyPersons : ICloneable
	{
		public Person First { get; set; }
		public Person Second { get; set; }
		public Person Third { get; set; }
		public Person PCI { get; set; }

		public bool Judge()
		{
			if (First != null && Second != null)
				return true;
			else
				return false;
		}
		public int NullCount()
		{
			int counter = 0;
			if (First == null)
				counter++;
			if (Second == null)
				counter++;
			return counter;
		}

		#region ICloneable メンバー

		public object Clone()
		{
			StandbyPersons csbp = new StandbyPersons();
			csbp.First = this.First != null ? this.First.Clone() as Person : null;
			csbp.Second = this.Second != null ? this.Second.Clone() as Person : null;
			csbp.Third = this.Third != null ? this.Third.Clone() as Person : null;
			csbp.PCI = this.PCI != null ? this.PCI.Clone() as Person : null;
			return csbp;
		}

		#endregion
	}

	public class PossibleDays : ICloneable
	{
		public int Year { get; private set; }
		public int Month { get; private set; }
		public Status[] PossibleDay { get; set; }
		public void SetDate(int year, int month)
		{
			if (Year < DateTime.MinValue.Year && Year > DateTime.MaxValue.Year &&
				Month <= 0 && Month > 12)
				throw new ArgumentOutOfRangeException();
			Year = year;
			Month = month;
			PossibleDay = new PossibleDays.Status[DateTime.DaysInMonth(year, month)];
			for (int i = 0; i < PossibleDay.Count(); i++)
				PossibleDay[i] = Status.None;
		}
		public PossibleDays(int year, int month)
		{
			SetDate(year, month);
		}
		public enum Status
		{
			None,
			Affair,
			Duty,
			Limited
		}

		#region ICloneable メンバー

		public object Clone()
		{
			PossibleDays cpd = new PossibleDays(Year, Month);
			for (int i = 0; i < PossibleDay.Count(); i++)
				cpd.PossibleDay[i] = PossibleDay[i];
			return cpd;
		}

		#endregion
	}
	public class Person : ICloneable
	{
		public string Name { get; set; }
		public List<PossibleDays> Possible { get; set; }
		public Attributes Attre { get; set; }
		public Requirement Requirement { get; set; }
		public int FirstCounter { get; set; }
		public int SecondCounter { get; set; }
		public int ThirdCounter { get; set; }
		public int HolidayCounter { get; set; }
		public int PCICounter { get; set; }
		public int SecondHolidayCounter { get; set; }
		public int ThirdHolidayCounter { get; set; }
		public int PCIHolidayCounter { get; set; }
		public enum Attributes
		{
			SeniorPhysician,
			Resident,
			PCIOperator
		}

		public Person()
		{
			Requirement = new Requirement();
			Possible = new List<PossibleDays>();
		}

		public override string ToString()
		{
			return this.Name;
		}

		public PossibleDays.Status this[DateTime dt]
		{
			get
			{
				if (!Possible.Any(t => t.Year == dt.Year && t.Month == dt.Month))
				{
					PossibleDays pd = new PossibleDays(dt.Year, dt.Month);
					Possible.Add(pd);
					return pd.PossibleDay[dt.Day - 1];
				}

				return Possible.First(t => t.Year == dt.Year && t.Month == dt.Month).PossibleDay[dt.Day - 1];
			}
			set
			{
				if (!Possible.Any(t => t.Year == dt.Year && t.Month == dt.Month))
				{
					PossibleDays pd = new PossibleDays(dt.Year, dt.Month);
					pd.PossibleDay[dt.Day - 1] = value;
					Possible.Add(pd);
				}
				else
					Possible.First(t => t.Year == dt.Year && t.Month == dt.Month).PossibleDay[dt.Day - 1] = value;
			}
		}



		#region ICloneable メンバー

		public object Clone()
		{
			Person cperson = new Person();
			cperson.Name = Name;
			Possible.ForEach(t => cperson.Possible.Add(t.Clone() as PossibleDays));
			cperson.Attre = Attre;
			cperson.Requirement = Requirement.Clone() as Requirement;
			cperson.FirstCounter = FirstCounter;
			cperson.SecondCounter = SecondCounter;
			cperson.ThirdCounter = ThirdCounter;
			cperson.HolidayCounter = HolidayCounter;
			cperson.PCICounter = PCICounter;
			cperson.SecondHolidayCounter = SecondHolidayCounter;
			cperson.ThirdHolidayCounter = ThirdHolidayCounter;
			cperson.PCIHolidayCounter = PCIHolidayCounter;

			return cperson;
		}

		#endregion
	}

	public class Requirement : ICloneable
	{
		public int FirstCallPossibleTimes { get; set; }
		public int SecondCallPossibleTimes { get; set; }
		public int ThirdCallPossibleTimes { get; set; }
		public int PCIPossibleTimes { get; set; }
		public int HolidayPossibleTimes { get; set; }
		public int SecondHolidayPossibleTimes { get; set; }
		public int ThirdHolidayPossibleTimes { get; set; }
		public int PCIHolidayPossibleTimes { get; set; }
		public int Interval { get; set; }
		public int SecondInterval { get; set; }
		public int ThirdInterval { get; set; }
		public int PCIInterval { get; set; }
		public Requirement()
		{

			Interval = 1;
			SecondInterval = 1;
			ThirdInterval = 1;
			PCIInterval = 1;
			FirstCallPossibleTimes = 5;
			SecondCallPossibleTimes = 5;
			ThirdCallPossibleTimes = 5;
			PCIPossibleTimes = 20;
			HolidayPossibleTimes = 2;
			SecondHolidayPossibleTimes = 2;
			ThirdHolidayPossibleTimes = 2;
			PCIHolidayPossibleTimes = 20;
		}


		#region ICloneable メンバー

		public object Clone()
		{
			Requirement rcopy = new Requirement();
			rcopy.FirstCallPossibleTimes = FirstCallPossibleTimes;
			rcopy.SecondCallPossibleTimes = SecondCallPossibleTimes;
			rcopy.ThirdCallPossibleTimes = ThirdCallPossibleTimes;
			rcopy.PCIPossibleTimes = PCIPossibleTimes;
			rcopy.HolidayPossibleTimes = HolidayPossibleTimes;
			rcopy.SecondHolidayPossibleTimes = SecondHolidayPossibleTimes;
			rcopy.ThirdHolidayPossibleTimes = ThirdHolidayPossibleTimes;
			rcopy.PCIHolidayPossibleTimes = PCIHolidayPossibleTimes;
			rcopy.Interval = Interval;
			rcopy.SecondInterval = SecondInterval;
			rcopy.ThirdInterval = ThirdInterval;
			rcopy.PCIInterval = PCIInterval;

			return rcopy;

		}

		#endregion
	}

	public class LimitedPerson
	{
		public enum Limit
		{
			None,
			Limited,
			Cannot,
		}
		public Person Person { get; set; }
		public Limit LimitStatus { get; set; }
		public List<string> Messages { get; set; }
		public LimitedPerson(Person p)
		{
			Person = p;
			LimitStatus = Limit.None;
			Messages = new List<string>();
		}
	}
}
