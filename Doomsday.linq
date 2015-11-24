<Query Kind="Program" />

void Main()
{
	DateTime date = DateTime.Today;

	do
	{
		DisplayResult(date);
	} while (DateTime.TryParse(Console.ReadLine(), out date));
}

#region Methods

private static void DisplayResult(DateTime date)
{
	DayOfWeek result = GetDayOfWeek(date);
	string output = $"{date:yyyy-MM-dd} {GetVerb(date)} on {date.DayOfWeek}";

	if (result != date.DayOfWeek)
	{
		output = $"ERROR: {output}, not {result}";
	}

	Console.WriteLine($"{output}.");
}

private static DayOfWeek GetCenturyAnchor(int year)
{
	//https://en.wikipedia.org/wiki/Doomsday_rule#Finding_a_century.27s_anchor_day

	int century = year / 100;

	DayOfWeek centuryAnchor;

	switch (century)
	{
		case 19:
			centuryAnchor = DayOfWeek.Wednesday;
			break;
		case 20:
			centuryAnchor = DayOfWeek.Tuesday;
			break;
		default:
			//This calculation works for 19 and 20 also, but it's easier to just memorize those.
			int dayOffset = 5 * (century % 4) % 7;
			centuryAnchor = AddOffsetToDayOfWeek(DayOfWeek.Tuesday, dayOffset);
			break;
	}

	return centuryAnchor;
}

private static DayOfWeek GetDoomsdayForYear(int year)
{
	//https://en.wikipedia.org/wiki/Doomsday_rule#Finding_a_year.27s_Doomsday

	DayOfWeek centuryAnchor = GetCenturyAnchor(year);

	int twoDigitYear = year % 100; //y in algorithm
	int yearDividedBy12 = twoDigitYear / 12; //a in algorithm
	int remainder = twoDigitYear % 12; //b in algorithm
	int remainderDividedBy4 = remainder / 4; //c in algorithm
	int dayOffset = yearDividedBy12 + remainder + remainderDividedBy4; //d in algorithm

	DayOfWeek doomsday = AddOffsetToDayOfWeek(centuryAnchor, dayOffset);
	return doomsday;
}

private static Doomsday GetDoomsdayForMonth(DateTime date)
{
	Doomsday doomsdayForMonth = Doomsdays.First(d => d.Month == date.Month
		&& (d.IsLeapYear == null || d.IsLeapYear == DateTime.IsLeapYear(date.Year)));

	return doomsdayForMonth;
}

private static DayOfWeek GetDayOfWeek(DateTime date)
{
	DayOfWeek doomsdayForYear = GetDoomsdayForYear(date.Year);
	Doomsday doomsdayForMonth = GetDoomsdayForMonth(date);

	int dayOffset = date.Day - doomsdayForMonth.Day;
	DayOfWeek dayOfWeek = AddOffsetToDayOfWeek(doomsdayForYear, dayOffset);

	return dayOfWeek;
}

#region Helpers

private static DayOfWeek AddOffsetToDayOfWeek(DayOfWeek input, int dayOffset)
{
	int dayIndex = ((int)input + dayOffset) % 7;

	if (dayIndex < 0)
	{
		dayIndex += 7;
	}

	DayOfWeek output = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayIndex.ToString());
	return output;
}

private static string GetVerb(DateTime date)
{
	string verb = string.Empty;

	switch (date.CompareTo(DateTime.Today))
	{
		case -1:
			verb = "was";
			break;
		case 0:
			verb = "is";
			break;
		case 1:
			verb = "will be";
			break;
	}

	return verb;
}

#endregion

#endregion

#region Properties

private static List<Doomsday> Doomsdays
{
	get
	{
		//https://en.wikipedia.org/wiki/Doomsday_rule#Memorable_dates_that_always_land_on_Doomsday

		List<Doomsday> doomsdays = new List<Doomsday>
		{
			//the 3rd 3 years in 4 and the 4th in the 4th
			new Doomsday(1, 3, isLeapYear: false),
			new Doomsday(1, 4, isLeapYear: true),

			//last day of February
			new Doomsday(2, 28, isLeapYear: false),
			new Doomsday(2, 29, isLeapYear: true),
			new Doomsday(3, 0),
			//isLeapYear = null means it doesn't matter whether it's a leap year, which is true for months other than January and February

			//4, 6, 8, 10, 12
			new Doomsday(4, 4),
			new Doomsday(6, 6),
			new Doomsday(8, 8),
			new Doomsday(10, 10),
			new Doomsday(12, 12),

			//working 9 to 5 at 7-11
			new Doomsday(5, 9),
			new Doomsday(7, 11),
			new Doomsday(9, 5),
			new Doomsday(11, 7),

			//holidays
			new Doomsday(2, 14, isLeapYear: false), //Valentine's Day is on doomsday in common years
			new Doomsday(7, 4), //Independence Day is always on doomsday
			new Doomsday(10, 31), //Halloween is always on doomsday
			new Doomsday(12, 26) //Christmas is always the day before doomsday
		};

		return doomsdays;
	}
}

#endregion

#region Classes

class Doomsday
{
	public int Month { get; private set; }
	public int Day { get; private set; }
	public bool? IsLeapYear { get; private set; }

	public Doomsday(int month, int day, bool? isLeapYear = null)
	{
		Month = month;
		Day = day;
		IsLeapYear = isLeapYear;
	}
}

#endregion
