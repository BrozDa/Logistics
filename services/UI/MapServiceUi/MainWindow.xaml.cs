using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System.Windows;

namespace MapServiceUi
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GViewer viewer = new GViewer()
            {
                ToolBarIsVisible = false,
                Dock = System.Windows.Forms.DockStyle.Fill,
                BackColor = System.Drawing.Color.Red, // outer viewer background
                
            };
     
            Graph graph = new Graph("graph");
            graph.Attr.BackgroundColor = Microsoft.Msagl.Drawing.Color.Transparent;
            graph.AddNode("A");
            graph.AddNode("B");
            graph.AddNode("C");
            graph.AddNode("D");
            graph.AddEdge("A", "B");
            graph.AddEdge("A", "C");
            graph.AddEdge("B", "C");
            graph.AddEdge("B", "D");


            foreach (var node in graph.Nodes) 
            { 
                node.Attr.Shape = Shape.Circle;
                node.Attr.Padding = 20;
            }
            foreach(var edge in graph.Edges)
            {
                edge.Attr.Length = 1500;
            }
           

            viewer.Graph = graph;
            viewer.Click += GraphObject_Click;
            formsHost.Child = viewer; 
        }

        private void GraphObject_Click(object? sender, EventArgs e)
        {
            GViewer viewer = sender as GViewer;

            foreach (var gNode in viewer.Graph.Nodes)
            {
                gNode.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
            }

            if (viewer.SelectedObject is Node node)
            {
                
                node.Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                
            }

        }
    }
}