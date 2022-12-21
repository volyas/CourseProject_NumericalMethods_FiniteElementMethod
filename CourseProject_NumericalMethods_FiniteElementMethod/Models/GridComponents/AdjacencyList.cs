namespace CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;

public class AdjacencyList
{
    public List<SortedSet<int>> List { get; set; }

    private readonly Grid _grid;

    public AdjacencyList(Grid grid)
    {
        _grid = grid;
    }

    public void CreateAdjacencyList()
    {
        var elements = _grid.Elements;

        List = new List<SortedSet<int>>(_grid.Nodes.Length);

        for (var i = 0; i < _grid.Nodes.Length; i++)
        {
            List.Add(new SortedSet<int>());
        }

        foreach (var element in elements)
        {
            var globalNodesNumbers = element.GlobalNodesNumbers;
            foreach (var currentNodeNumber in globalNodesNumbers)
            {
                foreach (var globalNodeNumber in globalNodesNumbers)
                {
                    if (currentNodeNumber > globalNodeNumber)
                        List[currentNodeNumber].Add(globalNodeNumber);
                }
            }
        }
    }
}