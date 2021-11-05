public class Menu {
    Calendar c;

    public Menu(Calendar c) {
        this.c = c;
    }

    public void ShowMenu() {
        System.out.println("\n**-------------------MENU-------------------**\n");
        System.out.println("1. init new calendar");
        System.out.println("2. print calendar");
        System.out.println("3. add event");
        System.out.println("4. remove event");
        System.out.println("5. reschedule event");
        System.out.println("6. exit\n");
    }

    public boolean Launch(int option) {
        try {
            switch (option) {
            case 1:
                this.Init();
                break;
            case 2:
                this.Print();
                break;
            case 3:
                this.Add();
                break;
            case 4:
                this.Remove();
                break;
            case 5:
                this.Reschedule();
                break;
            default:
                return false;
            }
        } catch (Exception e) {
            System.out.println(e.getMessage());
        }
        return true;
    }

    public void Init() {

        System.out.println("\n**--CREATING NEW CALENDAR--**\n");
        System.out.println("Input events. To finish - input keyword stop \n");

        while (true) {
            System.out.print("\nEvent: ");
            String event = System.console().readLine();

            if (event.equals("stop"))
                break;

            System.out.print("Day: ");
            int day = Integer.parseInt(System.console().readLine());

            System.out.print("Month: ");
            int month = Integer.parseInt(System.console().readLine());

            c.Add(day, month, event);
        }

        System.out.print("\n**NEW CALENDAR HAS BEEN SUCCESSFULLY CREATED**\n");
    }

    public void Print() {
        System.out.println("\n**--PRINT CALENDAR--**");
        c.Print();
    }

    public void Add() {
        System.out.println("\n**--ADD EVENT--**\n");

        System.out.print("Event: ");
        String event = System.console().readLine();

        System.out.print("Day: ");
        int day = Integer.parseInt(System.console().readLine());

        System.out.print("Month: ");
        int month = Integer.parseInt(System.console().readLine());

        c.Add(day, month, event);

        System.out.print("\n**NEW EVENT HAS BEEN SUCCESSFULLY ADDED**\n");
        c.Print();
    }

    public void Remove() {
        Event deletedItem = c.Remove();
        System.out.println("\n**EVENT " + deletedItem.event + " HAS BEEN REMOVED**\n");
        c.Print();
    }

    public void Reschedule() {
        System.out.println("\n**RESCHEDULE UPCOMING EVENT**\n");

        System.out.print("New day: ");
        int day = Integer.parseInt(System.console().readLine());

        System.out.print("New month: ");
        int month = Integer.parseInt(System.console().readLine());

        c.Reschedule(day, month);
        c.Print();
    }
}
