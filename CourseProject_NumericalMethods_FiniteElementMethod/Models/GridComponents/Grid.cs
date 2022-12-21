namespace CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;

public class Grid
{
    public int AmtByLength { get; set; }
    public int AmtByWidth { get; set; }
    public int AmtByHeight { get; set; }
    public Element[] Elements { get; set; }
    public Node[] Nodes { get; set; }
    public Node[] CornerNodes { get; set; }
    

    public Grid(Node[] nodes, Element[] elements, Node[] cornerNodes, int amtByLength, int amtByWidth, int amtByHeight)
    {
        AmtByLength = amtByLength;
        AmtByWidth = amtByWidth;
        AmtByHeight = amtByHeight;
        Elements = elements;
        Nodes = nodes;
        CornerNodes = cornerNodes;
        
    }
}