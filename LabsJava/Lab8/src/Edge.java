import java.awt.Color;

public class Edge {

    Vertex start;
    Vertex end;
    String title;
    Color c;

    Edge(Vertex start, Vertex end, Color c) {
        this.start = start;
        this.end = end;
        this.title = "" + start.title + end.title;
        this.c = c;
    }

    public boolean eq(Vertex A, Vertex B) {
        if (this.start.x == A.x && this.start.y == A.y && this.end.x == B.x && this.end.y == B.y)
            return true;
        if (this.end.x == A.x && this.end.y == A.y && this.start.x == B.x && this.start.y == B.y)
            return true;
        return false;
    }

    public boolean eq(char A, char B) {
        if (this.title.charAt(0) == A && this.title.charAt(1) == B)
            return true;
        if (this.title.charAt(1) == A && this.title.charAt(0) == B)
            return true;
        return false;

    }
}
