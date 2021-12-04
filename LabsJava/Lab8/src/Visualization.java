import javax.swing.*;
import java.awt.*;
import java.awt.Color;
import java.awt.event.*;
import java.awt.geom.Line2D;

public class Visualization extends JFrame implements MouseListener, MouseMotionListener {
    JPanel panel;
    final int ASCII_SHIFT = 65;
    final int radius = 15;
    Stack<Vertex> verts;
    Stack<Edge> edges;
    Vertex start, end;
    boolean pickStart, pickEnd;
    Vertex pathsStart, pathsEnd;
    String[] Paths;
    GraphMatrix graph;
    int currPathIndex = 0;

    Visualization() {
        verts = new Stack<Vertex>();
        edges = new Stack<Edge>();
        start = null;
        end = null;
        pathsStart = null;
        pathsEnd = null;
        pickStart = false;
        pickEnd = false;

        this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        this.setSize(1024, 768);
        this.setLayout(null);

        initPanel();
        initMenu();

        this.setVisible(true);
    }

    void initPanel() {
        panel = new JPanel();
        panel.setBounds(10, 20, 985, 668);
        panel.setBackground(Color.LIGHT_GRAY);
        panel.setOpaque(true);
        panel.addMouseListener(this);
        panel.addMouseMotionListener(this);
        this.add(panel);
    }

    void initMenu() {
        MenuBar mb = new MenuBar();
        setMenuBar(mb);

        Menu file = new Menu("Выбрать");
        MenuItem menuStart = new MenuItem("Стартовую вершину");
        file.add(menuStart);
        MenuItem menuEnd = new MenuItem("Конечную вершину");
        file.add(menuEnd);
        mb.add(file);

        Menu paths = new Menu("Граф");
        MenuItem menuPaths = new MenuItem("Найти все пути");
        paths.add(menuPaths);
        MenuItem menuNext = new MenuItem("Следующий путь..");
        paths.add(menuNext);
        MenuItem menuPerVerts = new MenuItem("Показать периферийные вершины");
        paths.add(menuPerVerts);
        mb.add(paths);

        menuStart.addActionListener(new menuStartHandler());
        menuEnd.addActionListener(new menuEndHandler());
        menuPaths.addActionListener(new menuPathsHandler(this));
        menuNext.addActionListener(new menuNextHandler());
        menuPerVerts.addActionListener(new menuPerVertsHandler());
    }

    class menuStartHandler implements ActionListener {
        @Override
        public void actionPerformed(ActionEvent e) {
            pickStart = !pickStart;
        }
    }

    class menuEndHandler implements ActionListener {
        @Override
        public void actionPerformed(ActionEvent e) {
            pickEnd = !pickEnd;
        }
    }

    class menuPerVertsHandler implements ActionListener {
        @Override
        public void actionPerformed(ActionEvent e) {
            char[] perVerts = graph.perVertexes;

            for (char perVert : perVerts)
                FindVertex(perVert).c = Color.MAGENTA;

            repaint();

        }
    }

    class menuPathsHandler implements ActionListener {
        JFrame jf;

        menuPathsHandler(JFrame jf) {
            this.jf = jf;
        }

        @Override
        public void actionPerformed(ActionEvent e) {
            graph = new GraphMatrix(vertsToString(), edgesToString());
            if (pathsStart != null && pathsEnd != null)
                Paths = graph.FindAllPaths(pathsStart.title, pathsEnd.title);

            ParamDialog d = new ParamDialog(jf, "Все пути, проходящие хотя бы через одну периферийную вершину");
            d.setVisible(true);

            for (String path : Paths)
                System.out.println(path);

            repaintEdges(Paths[0]);
            repaint();
        }
    }

    class menuNextHandler implements ActionListener {
        @Override
        public void actionPerformed(ActionEvent e) {
            currPathIndex++;
            if (currPathIndex < Paths.length)
                repaintEdges(Paths[currPathIndex]);
            repaint();
        }
    }

    class ParamDialog extends Dialog {
        String lbl1 = "Путей с заданным условием не найдено";
        String lbl2 = "Пути из " + pathsStart.title + " в " + pathsEnd.title + ": ";
        JLabel text;

        ParamDialog(JFrame parent, String title) {
            super(parent, title, true);
            addWindowListener(new WindowAdapter() {
                public void windowClosing(WindowEvent we) {
                    dispose();
                    setVisible(false);
                }
            });
            setLayout(new FlowLayout());
            setSize(500, 300);
            JButton btOk = new JButton("OK");

            if (Paths.length == 0) {
                text = new JLabel(lbl1);
                add(text);
            } else {
                text = new JLabel(lbl2);
                add(text);
                for (String path : Paths) {
                    text = new JLabel(path);
                    add(text);
                }
            }
            add(btOk);
            btOk.addActionListener(new OkListener(this));
        }

        class OkListener implements ActionListener {
            ParamDialog pd;

            OkListener(ParamDialog pard) {
                pd = pard;
            }

            public void actionPerformed(ActionEvent ae) {
                pd.dispose();
                pd.setVisible(false);
            }
        }
    }

    public String vertsToString() {
        Stack<Vertex> vertsCopy = verts.Reverse();
        String res = "";
        while (!vertsCopy.IsEmpty()) {
            Vertex curr = vertsCopy.Pop();
            res += curr.title + " ";
        }
        return res;
    }

    public String edgesToString() {
        Stack<Edge> edgesCopy = edges.Reverse();
        String res = "";

        Edge curr = edgesCopy.Pop();
        res += "" + curr.start.title + "" + curr.end.title;

        while (!edgesCopy.IsEmpty()) {
            curr = edgesCopy.Pop();
            res += " " + curr.start.title + "" + curr.end.title;
        }
        return res;
    }

    Vertex FindVertex(int x, int y) {
        Stack<Vertex> vertsCopy = verts.Reverse();
        while (!vertsCopy.IsEmpty()) {
            Vertex curr = vertsCopy.Pop();
            if (curr.x - radius < x && x < curr.x + radius && curr.y - radius < y && y < curr.y + radius)
                return curr;
        }
        return null;
    }

    Vertex FindVertex(char title) {
        Stack<Vertex> vertsCopy = verts.Reverse();
        while (!vertsCopy.IsEmpty()) {
            Vertex curr = vertsCopy.Pop();
            if (curr.title == title)
                return curr;
        }
        return null;
    }

    Edge FindEdge(Vertex start, Vertex end) {
        Stack<Edge> edgesCopy = edges.Reverse();
        while (!edgesCopy.IsEmpty()) {
            Edge curr = edgesCopy.Pop();
            if (curr.eq(start, end))
                return curr;
        }
        return null;
    }

    Edge FindEdge(char start, char end) {
        Stack<Edge> edgesCopy = edges.Reverse();
        while (!edgesCopy.IsEmpty()) {
            Edge curr = edgesCopy.Pop();
            if (curr.eq(start, end))
                return curr;
        }
        return null;
    }

    void repaintEdges(String path) {
        Stack<Edge> edgesCopy = edges.Reverse();
        while (!edgesCopy.IsEmpty())
            edgesCopy.Pop().c = Color.BLUE;

        System.out.println(path);
        for (int i = 0; i < path.length() - 1; i++)
            FindEdge(path.charAt(i), path.charAt(i + 1)).c = Color.PINK;

    }

    public void drawCircle(int x, int y, char title, Color clr) {
        Graphics g = panel.getGraphics();
        g.setColor(clr);

        g.fillOval(x - radius, y - radius, 2 * radius, 2 * radius);
        g.setColor(Color.BLUE);
        g.setFont(new Font("Verdana", Font.PLAIN, 18));
        g.drawString(title + "", x - radius / 3, y + radius / 3);
    }

    public void drawLine(int x1, int y1, int x2, int y2, Color clr) {
        Graphics2D g2 = (Graphics2D) panel.getGraphics();
        g2.setColor(clr);
        g2.setStroke(new BasicStroke(4));
        g2.draw(new Line2D.Float(x1, y1, x2, y2));
    }

    public void drawAllVertexes() {
        Stack<Vertex> vertsCopy = verts.Reverse();
        while (!vertsCopy.IsEmpty()) {
            Vertex curr = vertsCopy.Pop();
            drawCircle(curr.x, curr.y, curr.title, curr.c);
        }
    }

    public void drawAllEdges() {
        Stack<Edge> edgesCopy = edges.Reverse();
        while (!edgesCopy.IsEmpty()) {
            Edge curr = edgesCopy.Pop();
            drawLine(curr.start.x, curr.start.y, curr.end.x, curr.end.y, curr.c);
        }
    }

    public void paint(Graphics g) {
        super.paint(g);
        drawAllEdges();
        if (start != null && end != null)
            drawLine(start.x, start.y, end.x, end.y, Color.BLUE);
        drawAllVertexes();
    }

    @Override
    public void mouseClicked(MouseEvent e) {
        int x = e.getX();
        int y = e.getY();

        char title = (char) (verts.Count() + ASCII_SHIFT);
        Vertex v = new Vertex(x, y, title, Color.orange);

        Vertex foundVertex = FindVertex(x, y);
        if (foundVertex == null) {
            verts.Push(v);
            drawCircle(x, y, title, Color.orange);
        } else if (pickStart && pathsStart == null) {
            pathsStart = foundVertex;
            foundVertex.c = Color.GREEN;
            pickStart = false;
            repaint();
        } else if (pickEnd && pathsEnd == null) {
            pathsEnd = foundVertex;
            foundVertex.c = Color.CYAN;
            pickEnd = false;
            repaint();
        }

    }

    @Override
    public void mousePressed(MouseEvent e) {
        int x = e.getX();
        int y = e.getY();
        start = FindVertex(x, y);
    }

    @Override
    public void mouseReleased(MouseEvent e) {
        int x = e.getX();
        int y = e.getY();
        end = FindVertex(x, y);

        if (start != null && end != null)
            if (start.x != end.x && start.y != end.y) {
                Edge foundEdge = FindEdge(start, end);
                if (foundEdge == null) {
                    Edge ed = new Edge(start, end, Color.BLUE);
                    edges.Push(ed);
                }
            }
        repaint();
        start = null;
        end = null;
    }

    @Override
    public void mouseEntered(MouseEvent e) {

    }

    @Override
    public void mouseExited(MouseEvent e) {

    }

    @Override
    public void mouseDragged(MouseEvent e) {
        int x = e.getX();
        int y = e.getY();
        end = new Vertex(x, y);
        if (start != null)
            repaint();
    }

    @Override
    public void mouseMoved(MouseEvent e) {

    }

}
