import java.awt.Color;

public class Vertex {

    int x;
    int y;
    char title;
    Color c;

    Vertex(int x, int y, char title, Color c) {
        this.x = x;
        this.y = y;
        this.title = title;
        this.c = c;
    }

    Vertex(int x, int y) {
        this.x = x;
        this.y = y;
    }
}
