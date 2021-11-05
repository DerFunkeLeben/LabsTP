public class Event {
    int day;
    int month;
    String event;

    public Event(int month, String event) {
        this.event = event;
        this.month = month;
    }

    public Event withDay(int day) {

        switch (this.month) {
        case 1:
        case 3:
        case 5:
        case 7:
        case 8:
        case 10:
        case 12:
            this.day = (day <= 31) ? day : 0;
            break;

        case 4:
        case 6:
        case 9:
        case 11:
            this.day = (day <= 30) ? day : 0;
            break;

        case 2:
            this.day = (day <= 28) ? day : 0;
            break;

        default:
            if (this.month < 1 || this.month > 12)
                throw new IllegalArgumentException(
                        this.month + " is not a valid month. Month must be between 1 and 12");
            break;
        }
        if (this.day <= 0)
            throw new IllegalArgumentException(day + " is not a valid day for month " + this.month);
        return this;
    }
}
