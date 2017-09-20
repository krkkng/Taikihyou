using StandbyList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Calendar
{
	public static class Serializer
	{
		const string SETTINGS = "Settings";
		const string COLOR1ST = "Color1st";
		const string COLOR2ND = "Color2nd";
		const string COLOR3RD = "Color3rd";
		const string COLORPCI = "PCIColor";
		const string TRYCOUNT = "Try_Count";
		const string FOOTERMESSAGE = "Footer_m";
		const string FOOTERMESSAGE_CANNOT = "Footer_m_cannot";
		const string DATAS = "Datas";
		const string YEAR_MONTH = "Year_Month";
		const string DATE_YEAR = "Year";
		const string DATE_MONTH = "Month";
		const string PERSONS = "Persons";
		const string PERSON = "Person";
		const string NAME = "Name";
		const string ATTRIBUTE = "Attribute";
		const string _1ST_COUNT = "_1st_count";
		const string _2ND_COUNT = "_2nd_count";
		const string _3RD_COUNT = "_3rd_count";
		const string _PCI_COUNT = "_pci_count";
		const string _1ST_HOLIDAYCOUNT = "_1st_holidaycount";
		const string _2ND_HOLIDAYCOUNT = "_2nd_holidaycount";
		const string _3RD_HOLIDAYCOUNT = "_3rd_holidaycount";
		const string _PCI_HOLIDAYCOUNT = "_pci_holidaycount";
		const string _1ST_INTERVAL = "_1st_interval";
		const string _2ND_INTERVAL = "_2nd_interval";
		const string _3RD_INTERVAL = "_3rd_interval";
		const string _PCI_INTERVAL = "_pci_interval";
		const string POSSIBLES = "Possibles";
		const string STANDBYLISTS = "StandbyLists";
		public static void SaveDatas(string filename)
		{
			var xml = new XDocument(new XDeclaration("1.0", "utf-8", "true"), new XComment("待機表くん ver1.0"), new XElement("Main"));

			var SettingElem = new XElement(SETTINGS,
				new XElement(COLOR1ST, Setting.FirstColor.Name),
				new XElement(COLOR2ND, Setting.SecondColor.Name),
				new XElement(COLOR3RD, Setting.ThirdColor.Name),
				new XElement(COLORPCI, Setting.PCIColor.Name),
				new XElement(TRYCOUNT, Setting.TryCount));
			xml.Root.Add(SettingElem);

			var DataElem = new XElement(DATAS);

			Core.StandbyLists.ForEach(t =>
			{
				var DateElem = new XElement(YEAR_MONTH);
				DataElem.Add(DateElem);
				var Date_YearElem = new XElement(DATE_YEAR, t.Year);
				var Date_MonthElem = new XElement(DATE_MONTH, t.Month);
				DateElem.Add(Date_YearElem);
				DateElem.Add(Date_MonthElem);
				var FooterMessage = new XElement(FOOTERMESSAGE, t.FooterMessage);
				var FooterMessage_cannot = new XElement(FOOTERMESSAGE_CANNOT, t.FooterMessage_cannot);
				DateElem.Add(FooterMessage);
				DateElem.Add(FooterMessage_cannot);

				var PersonsElem = new XElement(PERSONS);
				for (int i = 0; i < t.Persons.Count; i++)
				{
					var PersonElem = new XElement(PERSON);
					PersonElem.Add(new XElement(NAME, t.Persons[i].Name));
					PersonElem.Add(new XElement(ATTRIBUTE, (int)t.Persons[i].Attre));
					PersonElem.Add(new XElement(_1ST_COUNT, t.Persons[i].Requirement.FirstCallPossibleTimes));
					PersonElem.Add(new XElement(_2ND_COUNT, t.Persons[i].Requirement.SecondCallPossibleTimes));
					PersonElem.Add(new XElement(_3RD_COUNT, t.Persons[i].Requirement.ThirdCallPossibleTimes));
					PersonElem.Add(new XElement(_PCI_COUNT, t.Persons[i].Requirement.PCIPossibleTimes));
					PersonElem.Add(new XElement(_1ST_HOLIDAYCOUNT, t.Persons[i].Requirement.HolidayPossibleTimes));
					PersonElem.Add(new XElement(_2ND_HOLIDAYCOUNT, t.Persons[i].Requirement.SecondHolidayPossibleTimes));
					PersonElem.Add(new XElement(_3RD_HOLIDAYCOUNT, t.Persons[i].Requirement.ThirdHolidayPossibleTimes));
					PersonElem.Add(new XElement(_PCI_HOLIDAYCOUNT, t.Persons[i].Requirement.PCIHolidayPossibleTimes));
					PersonElem.Add(new XElement(_1ST_INTERVAL, t.Persons[i].Requirement.Interval));
					PersonElem.Add(new XElement(_2ND_INTERVAL, t.Persons[i].Requirement.SecondInterval));
					PersonElem.Add(new XElement(_3RD_INTERVAL, t.Persons[i].Requirement.ThirdInterval));
					PersonElem.Add(new XElement(_PCI_INTERVAL, t.Persons[i].Requirement.PCIInterval));

					var PossibleElem = new XElement("Possibles");
					for (int j = 0; j < DateTime.DaysInMonth(t.Year, t.Month); j++)
						PossibleElem.Add(new XElement("_" + (j + 1).ToString(), (int)t.Persons[i][new DateTime(t.Year, t.Month, j + 1)]));
					PersonElem.Add(PossibleElem);
					PersonsElem.Add(PersonElem);
				}
				DateElem.Add(PersonsElem);
				var StandbyListElem = new XElement("StandbyLists");
				for (int i = 0; i < DateTime.DaysInMonth(t.Year, t.Month); i++)
					StandbyListElem.Add(new XElement("_" + (i + 1).ToString(),
						new XElement("_1st", t.Standby[i].First),
						new XElement("_2nd", t.Standby[i].Second),
						new XElement("_3rd", t.Standby[i].Third),
						new XElement("_PCI", t.Standby[i].PCI)));
				DateElem.Add(StandbyListElem);
			});
			xml.Root.Add(DataElem);


			try
			{
				using (StreamWriter sw = new StreamWriter(new FileStream(filename, FileMode.Create), Encoding.Unicode))
				{
					sw.Write(xml);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "エラー");
			}
		}

		public static void LoadDatas(string filename)
		{
			if (string.IsNullOrEmpty(filename)) throw new FileNotFoundException();
			Core.StandbyLists = new List<StandbyList.StandbyList>();

			try
			{
				using (XmlReader reader = XmlReader.Create(filename))
				{
					while (reader.Read())
					{
						if (reader.NodeType == XmlNodeType.Element)
						{
							switch (reader.LocalName)
							{
								case COLOR1ST:
									Setting.FirstColor = Color.FromName(reader.ReadString());
									break;
								case COLOR2ND:
									Setting.SecondColor = Color.FromName(reader.ReadString());
									break;
								case COLOR3RD:
									Setting.ThirdColor = Color.FromName(reader.ReadString());
									break;
								case COLORPCI:
									Setting.PCIColor = Color.FromName(reader.ReadString());
									break;
								case TRYCOUNT:
									Setting.TryCount = int.Parse(reader.ReadString());
									break;
								case YEAR_MONTH:
									reader.Read();
									StandbyList.StandbyList st = new StandbyList.StandbyList();
									st.Persons.Clear();
									int year = 1999;
									int month = 1;
									while (!(reader.NodeType == XmlNodeType.EndElement && reader.LocalName == YEAR_MONTH))
									{
										if (reader.NodeType == XmlNodeType.Element)
										{
											switch (reader.LocalName)
											{
												case DATE_YEAR:
													year = int.Parse(reader.ReadString());
													break;
												case DATE_MONTH:
													month = int.Parse(reader.ReadString());
													st.SetDate(year, month);
													break;
												case FOOTERMESSAGE:
													st.FooterMessage = reader.ReadString();
													break;
												case FOOTERMESSAGE_CANNOT:
													st.FooterMessage_cannot = reader.ReadString();
													break;

												case PERSONS:
													reader.Read();
													while (!(reader.NodeType == XmlNodeType.EndElement && reader.LocalName == PERSONS))
													{
														Person person = new Person();
														while (!(reader.NodeType == XmlNodeType.EndElement && reader.LocalName == PERSON))
														{
															if (reader.NodeType == XmlNodeType.Element)
															{
																switch (reader.LocalName)
																{
																	case NAME:
																		person.Name = reader.ReadString();
																		break;
																	case ATTRIBUTE:
																		person.Attre = (Person.Attributes)int.Parse(reader.ReadString());
																		break;
																	case _1ST_COUNT:
																		person.Requirement.FirstCallPossibleTimes = int.Parse(reader.ReadString());
																		break;
																	case _2ND_COUNT:
																		person.Requirement.SecondCallPossibleTimes = int.Parse(reader.ReadString());
																		break;
																	case _3RD_COUNT:
																		person.Requirement.ThirdCallPossibleTimes = int.Parse(reader.ReadString());
																		break;
																	case _PCI_COUNT:
																		person.Requirement.PCIPossibleTimes = int.Parse(reader.ReadString());
																		break;
																	case _1ST_HOLIDAYCOUNT:
																		person.Requirement.HolidayPossibleTimes = int.Parse(reader.ReadString());
																		break;
																	case _2ND_HOLIDAYCOUNT:
																		person.Requirement.SecondHolidayPossibleTimes = int.Parse(reader.ReadString());
																		break;
																	case _3RD_HOLIDAYCOUNT:
																		person.Requirement.ThirdHolidayPossibleTimes = int.Parse(reader.ReadString());
																		break;
																	case _PCI_HOLIDAYCOUNT:
																		person.Requirement.PCIHolidayPossibleTimes = int.Parse(reader.ReadString());
																		break;
																	case _1ST_INTERVAL:
																		person.Requirement.Interval = int.Parse(reader.ReadString());
																		break;
																	case _2ND_INTERVAL:
																		person.Requirement.SecondInterval = int.Parse(reader.ReadString());
																		break;
																	case _3RD_INTERVAL:
																		person.Requirement.ThirdInterval = int.Parse(reader.ReadString());
																		break;
																	case _PCI_INTERVAL:
																		person.Requirement.PCIInterval = int.Parse(reader.ReadString());
																		break;

																	case POSSIBLES:
																		reader.Read();
																		while (!(reader.NodeType == XmlNodeType.EndElement && reader.LocalName == POSSIBLES))
																		{
																			if (reader.NodeType == XmlNodeType.Element)
																			{
																				int day = int.Parse(reader.LocalName.TrimStart('_'));
																				person[new DateTime(year, month, day)] = (PossibleDays.Status)int.Parse(reader.ReadString());
																			}
																			reader.Read();
																		}
																		break;

																}
															}
															reader.Read();
														}

														st.Persons.Add(person);
														reader.Read();
														reader.Read();
													}
													break;
												case STANDBYLISTS:
													reader.Read();
													StandbyPersons[] stanby = new StandbyPersons[DateTime.DaysInMonth(year, month)];
													while (!(reader.NodeType == XmlNodeType.EndElement && reader.LocalName == STANDBYLISTS))
													{
														if (reader.NodeType == XmlNodeType.Element)
														{
															int day = int.Parse(reader.LocalName.TrimStart('_'));
															stanby[day - 1] = new StandbyPersons();
															reader.Read();
															while (!(reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "_" + day.ToString()))
															{
																if (reader.NodeType == XmlNodeType.Element)
																{
																	string tmp = reader.ReadString();
																	Person person = st.Persons.FirstOrDefault(t => t.Name == tmp);
																	switch (reader.LocalName)
																	{
																		case "_1st":
																			stanby[day - 1].First = person ?? new Person() { Name = tmp };
																			break;
																		case "_2nd":
																			stanby[day - 1].Second = person ?? new Person() { Name = tmp };
																			break;
																		case "_3rd":
																			stanby[day - 1].Third = person ?? new Person() { Name = tmp };
																			break;
																		case "_PCI":
																			stanby[day - 1].PCI = person ?? new Person() { Name = tmp };
																			break;
																	}
																}
																reader.Read();
															}
														}
														reader.Read();
													}
													st.Standby = stanby;
													break;
											}
											//reader.Read();
										}
										reader.Read();
									}
									Core.StandbyLists.Add(st);
									break;
							}
						}

					}
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "ファイルの読み込みに失敗しました。");
			}
		}

	}
}
