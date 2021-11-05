public class Calendar {
    public class InnerCalendar {
        public Event[] events;
        public int Count;

        public InnerCalendar() {
            events = new Event[100];
            this.Count = 0;
        }
    }

    InnerCalendar calendar;

    public Calendar() {
        calendar = new InnerCalendar();
    }

    public void Add(int day, int month, String event) {

        Event newEvent = new Event(month, event).withDay(day);

        int i;
        for (i = 0; i < calendar.Count; i++) {
            if (month > calendar.events[i].month)
                continue;
            if (month == calendar.events[i].month)
                if (day > calendar.events[i].day)
                    continue;
                else
                    break;
            else
                break;
        }

        for (int j = calendar.Count - 1; j >= i; j--)
            calendar.events[j + 1] = calendar.events[j];

        calendar.events[i] = newEvent;

        calendar.Count++;
    }

    public Event Remove() {
        if (calendar.Count == 0)
            throw new IllegalArgumentException("Calendar is empty");

        Event removingEvent = calendar.events[0];
        for (int j = 0; j < calendar.Count - 1; j++)
            calendar.events[j] = calendar.events[j + 1];

        calendar.Count--;
        return removingEvent;
    }

    public void Reschedule(int day, int month) {
        Event removingEvent = this.Remove();
        this.Add(day, month, removingEvent.event);
    }

    public void Print() {
        if (calendar.Count == 0) {
            System.out.println("\n*---EMPTY---*");
            return;
        }

        for (int i = 0; i < calendar.Count; i++) {
            String outputDate = String.format("\nDate: %d.%d.2021", calendar.events[i].day, calendar.events[i].month);

            System.out.print(outputDate);
            System.out.println(" Event: " + calendar.events[i].event);

        }
    }
}
