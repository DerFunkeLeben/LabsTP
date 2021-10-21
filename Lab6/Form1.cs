using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Form1 : Form
    {
        Visualization vis;
        GraphMatrix graph;


        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                string fileText = System.IO.File.ReadAllText(filename);

                string[] data = fileText.Split(';');

                this.vertexesData.Text = data[0];
                this.edgesData.Text = data[1];
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                string data = vertexesData.Text + ';' + edgesData.Text;
                System.IO.File.WriteAllText(filename, data);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (FieldsValid())
            {
                PathsTable.Rows.Clear();
                string verts = vertexesData.Text;
                string edges = edgesData.Text;
                char start = startVertex.Text[0];
                char end = endVertex.Text[0];

                graph = new GraphMatrix(verts, edges);

                string[] Paths = graph.FindAllPaths(start, end);

                if (Paths.Length == 0) MessageBox.Show("No paths on such conditions!");

                foreach (string path in Paths)
                    PathsTable.Rows.Add(path);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FieldsValid())
            {
                graph = new GraphMatrix(vertexesData.Text, edgesData.Text);
                vis = new Visualization(this.panel1, vertexesData.Text, edgesData.Text, graph != null ? graph.PerVertexes : null);
            }
        }

        private void PathsTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && vis != null)
            {
                string path = PathsTable.Rows[e.RowIndex].Cells["Path"].Value.ToString();
                vis.HighlightPath(path);
            }
                
        }
    
        private bool FieldsValid()
        {
            string verts = vertexesData.Text;
            string edges = edgesData.Text;
            char start = startVertex.Text[0];
            char end = endVertex.Text[0];

            char[] separators = new char[] { ' ', '\r', '\n', ',' };
            
            try
            {
                foreach (char vert in verts)
                    if (!isSeparator(separators, vert) && (vert < 'A' || vert > 'Z'))
                        throw new InvalidOperationException("Invalid vertexes construction");

                foreach (char edgeSymb in edges)
                    if (!isSeparator(separators, edgeSymb))
                        if(!verts.Contains(edgeSymb))
                            throw new InvalidOperationException("There is no vertex for the one of the edges");

                if (!verts.Contains(start))
                    throw new InvalidOperationException("There is no such start vertex");

                if (!verts.Contains(end))
                    throw new InvalidOperationException("There is no such end vertex");

                string[] edges_arr = edges.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (var edge in edges_arr)
                    if (edge.Length != 2)
                        throw new InvalidOperationException("Inappropriate edges' construction");

            }
            catch (InvalidOperationException msg)
            {
                MessageBox.Show(msg.Message);
                return false;
            }
            return true;
        }

        private bool isSeparator(char[] separators, char item)
        {
            foreach (char lex in separators)
                if (lex == item) return true;
            return false;
        }

    }
}
