<Query Kind="Program" />

void Main()
{
	DateTime date = DateTime.Today;

	do
	{
		DisplayResult(date);
	} while (DateTime.TryParse(Console.ReadLine(), out date));
}

private static void DisplayResult(DateTime date)
{
	DayOfWeek result = GetDayOfWeek(date);
	bool success = result == date.DayOfWeek;
	string errorMessage = success ? "" : "ERROR: ";
	string verb = GetVerb(date);
	string incorrectResult = success ? "" : $", not {result}";
	string output = $"{errorMessage}{date:yyyy-MM-dd} {verb} on {date.DayOfWeek}{incorrectResult}.";
	Console.WriteLine(output);
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

private static DayOfWeek GetCenturyAnchor(int year)
{
	int dayOffset = 5 * (year / 100 % 4) % 7;
	DayOfWeek centuryAnchor = AddOffsetToDayOfWeek(DayOfWeek.Tuesday, dayOffset);

	//or just memorize these two:
	switch (year / 100)
	{
		case 19:
			centuryAnchor = DayOfWeek.Wednesday;
			break;
		case 20:
			centuryAnchor = DayOfWeek.Tuesday;
			break;
	}

	return centuryAnchor;
}

private static DayOfWeek GetDoomsdayForYear(int year)
{
	DayOfWeek centuryAnchor = GetCenturyAnchor(year);

	int twoDigitYear = year % 100;
	int a = twoDigitYear / 12;
	int b = twoDigitYear % 12;
	int c = b / 4;
	int dayOffset = a + b + c;

	DayOfWeek doomsday = AddOffsetToDayOfWeek(centuryAnchor, dayOffset);
	return doomsday;
}

private static List<Doomsday> Doomsdays
{
	get
	{
		List<Doomsday> doomsdays = new List<Doomsday>
		{
			new Doomsday(1, 3, false),
			new Doomsday(1, 4, true),

			//last day of February:
			new Doomsday(2, 28, false),
			new Doomsday(2, 29, true),
			new Doomsday(3, 0),

			new Doomsday(4, 4),
			new Doomsday(6, 6),
			new Doomsday(8, 8),
			new Doomsday(10, 10),
			new Doomsday(12, 12),

			//working 9 to 5 at 7-11:
			new Doomsday(5, 9),
			new Doomsday(7, 11),
			new Doomsday(9, 5),
			new Doomsday(11, 7),

			//holidays:
			new Doomsday(2, 14, false),
			new Doomsday(7, 4),
			new Doomsday(10, 31)
		};

		return doomsdays;
	}
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