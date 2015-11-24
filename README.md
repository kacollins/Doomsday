# Doomsday
LINQPad program example using the Doomsday algorithm:  https://en.wikipedia.org/wiki/Doomsday_rule

DayOfWeek is a property of DateTime, so this program is not intended to be used to calculate the day of the week.  The goals are to make the algorithm easier to memorize, and to provide an example of how to use LINQPad for debugging.

To run the program:
* Download LINQPad at http://www.linqpad.net/Download.aspx
* Press F5 or the play button. The first result will be for today's date.
* Continue entering dates. If the input is not a valid date, the program will end, so you can press enter to quit.

To calculate the weekday for a given date:
* Get the weekday of the anchor for the century. (ex: Tuesday for the 2000s)
* Get the weekday of the doomsday for the year. (ex: Saturday for 2015)
* Get the day of the month of a doomsday for the month. (ex: the 7th for November)
* Use the difference between the date's day of the month and the result of the previous step to get the date's weekday.
