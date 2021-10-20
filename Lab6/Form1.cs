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
            PathsTable.Rows.Clear();
            string verts = vertexesData.Text;
            string edges = edgesData.Text;
            char start = startVertex.Text[0];
            char end = endVertex.Text[0];

            graph = new GraphMatrix(verts, edges);

            string[] Paths = graph.FindAllPaths(start, end);

            foreach (string path in Paths)
                PathsTable.Rows.Add(path);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(graph == null)
                graph = new GraphMatrix(vertexesData.Text, edgesData.Text);
            vis = new Visualization(this.panel1, vertexesData.Text, edgesData.Text, graph!=null ? graph.PerVertexes : null);
        }

        private void PathsTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && vis != null)
            {
                string path = PathsTable.Rows[e.RowIndex].Cells["Path"].Value.ToString();
                vis.HighlightPath(path);
            }
                
        }
    }
}
