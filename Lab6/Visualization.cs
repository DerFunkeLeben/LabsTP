using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Lab6
{
    class Point
    {
        double x;
        double y;
        string name;
        Color clr;
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Color Clr
        {
            get { return Clr; }
            set { Clr = value; }
        }

        public double X
        {
            get { return x; }
        }

        public double Y
        {
            get { return y; }
        }


    }
   
    class Edge
    {
        Point start;
        Point end;

        public Edge(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }

        public Point Start
        {
            get { return start; }
        }

        public Point End
        {
            get { return end; }
        }
    }
    
    class Visualization
    {
        private Graphics g;
        private Point Center;
        private int radius;
        private int VertexR = 14;
        private Point[] coordsV;
        private Edge[] coordsE;
        private char[] perVerts;

        public Visualization(System.Windows.Forms.Panel panel1, string verts, string edges, char[] perVerts)
        {
            this.perVerts = perVerts;
            g = panel1.CreateGraphics();
            g.Clear(Color.White);
            SetDimentions(panel1);
            coordsV = GetVertsCoords(verts);
            coordsE = GetEdgesCoords(edges, coordsV);
            DrawEdges(coordsE, Color.Indigo);
            DrawVerts(coordsV);
        }

        private void SetDimentions(System.Windows.Forms.Panel panel1)
        {
            Center = new Point(panel1.Width / 2, panel1.Height / 2);
            radius = panel1.Height / 2 - 30;
        }

        private Point[] GetVertsCoords(string verts)
        {
            char[] separators = new char[] { ' ', '\r', '\n', ',' };
            string[] vertexes = verts.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            int verts_count = vertexes.Length;
            double angle = 2 * Math.PI / verts_count;
            Point[] coords = new Point[verts_count];

            for (int i = 0; i < verts_count; ++i)
            {
                coords[i] = new Point(Center.X + radius * Math.Sin(angle * i), Center.Y - radius * Math.Cos(angle * i));
                coords[i].Name = vertexes[i];
            }
            return coords;

        }

        private Edge[] GetEdgesCoords(string edgesStr, Point[] vertsCoords)
        {
            char[] separators = new char[] { ' ', '\r', '\n', ',' };
            string[] edges = edgesStr.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            int edges_count = edges.Length;
            Edge[] coords = new Edge[edges_count];

            for (int i = 0; i < edges_count; ++i)
            {
                Point start = GetCoordsByName(vertsCoords, edges[i][0].ToString());
                Point end = GetCoordsByName(vertsCoords, edges[i][1].ToString());
                coords[i] = new Edge(start, end);
            }
            return coords;

        }

        private Point GetCoordsByName(Point[] coords, string Name)
        {
            foreach(Point coord in coords)
                if (coord.Name == Name) return coord;
            return null;
        }

        private void DrawVerts(Point[] coords)
        {
            Brush vertexColor;
            Brush fontColor = new SolidBrush(Color.Navy);
            Font font = new Font("Verdana", 12);
            StringFormat stringFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            foreach (Point coord in coords)
            {
                if(perVerts!=null && Array.IndexOf(perVerts, coord.Name[0]) != -1)
                    vertexColor = new SolidBrush(Color.Yellow);
                else vertexColor = new SolidBrush(Color.Beige);
                g.FillEllipse(vertexColor, (int)coord.X, (int)coord.Y, VertexR * 2, VertexR * 2);
                g.DrawString(coord.Name, font, fontColor, new RectangleF((float)coord.X, (float)coord.Y, VertexR * 2, VertexR * 2), stringFormat);
            }

        }

        private void DrawEdges(Edge[] coords, Color clr)
        {
            Pen EdgeColor = new Pen(clr, 3);
          
            foreach (Edge coord in coords)
                g.DrawLine(EdgeColor, (int)coord.Start.X + VertexR, (int)coord.Start.Y + VertexR, (int)coord.End.X + VertexR, (int)coord.End.Y + VertexR);
        }

        public void HighlightPath(string path)
        {
            Edge[] highlightedEdges = new Edge[path.Length - 1];
            for (int i = 0; i < path.Length - 1; ++i)
            {
                Point start = GetCoordsByName(coordsV, path[i].ToString());
                Point end = GetCoordsByName(coordsV, path[i + 1].ToString());
                highlightedEdges[i] = new Edge(start, end);
            }
            DrawEdges(coordsE, Color.Indigo);
            DrawEdges(highlightedEdges, Color.Red);
            DrawVerts(coordsV);
        }
    }
}
