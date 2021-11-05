public class App {
    public static void main(String[] args) throws Exception {
        
        Calendar c = new Calendar();
        Menu menu = new Menu(c);

        int option;
        do {
            menu.ShowMenu();
            option = Integer.parseInt(System.console().readLine());
        } while (menu.Launch(option));

    }
}