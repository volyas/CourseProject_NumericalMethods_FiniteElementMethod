using CourseProject_NumericalMethods_FiniteElementMethod.Factories;
using CourseProject_NumericalMethods_FiniteElementMethod.IOs;
using CourseProject_NumericalMethods_FiniteElementMethod.Models.BoundaryConditions;
using CourseProject_NumericalMethods_FiniteElementMethod.Models.GlobalSolutionComponents;
using CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;
using CourseProject_NumericalMethods_FiniteElementMethod.SLAESolution;
using CourseProject_NumericalMethods_FiniteElementMethod.Tools;
using CourseProject_NumericalMethods_FiniteElementMethod.Tools.Producers;

var parametersI = new ParametersIO("../CourseProject_NumericalMethods_FiniteElementMethod/Input/");
var gridI = new GridIO("../CourseProject_NumericalMethods_FiniteElementMethod/Input/Grid/");
var materialI = new MaterialIO("../CourseProject_NumericalMethods_FiniteElementMethod/Input/Materials/");
var globalVectorI = new GlobalVectorIO("../CourseProject_NumericalMethods_FiniteElementMethod/Input/GlobalVectors/");
var boundaryConditionI = new BoundaryConditionIO("../CourseProject_NumericalMethods_FiniteElementMethod/Input/BoundaryConditions/");
var pointI = new PointIO();

materialI.ReadMaterialsParametersFromFile("MaterialsParameters.txt", out var lambdasList, out var gammasList);

var materialFactory = new MaterialFactory(lambdasList, gammasList);
var linearFunctionsProducer = new LinearFunctionsProducer();
var gridComponentsProducer = new GridComponentsProducer(materialFactory, linearFunctionsProducer);
var gridFactory = new GridFactory(gridComponentsProducer);

gridI.ReadParametersFromFile("GridParameters.txt", out var cornerNodes, out var amtByLength, out var amtByWidth, out var amtByHeight);
var grid = gridFactory.CreateGrid(cornerNodes, amtByLength, amtByWidth, amtByHeight);

var adjacencyList = new AdjacencyList(grid);
adjacencyList.CreateAdjacencyList();

var globalMatrix = new GlobalMatrix(adjacencyList);
var globalVector = new GlobalVector(globalMatrix.N);

var nodeFinder = new NodeFinder(grid);
var gx = MatrixProducer.Gx;
var gy = MatrixProducer.Gy;
var gz = MatrixProducer.Gz;
var m = MatrixProducer.M;
var pComponentsProvider =
    new PComponentsProducer((x, y, z) => x * x - 2, nodeFinder);

foreach (var element in grid.Elements)
{
    element.CalcStiffnessMatrix(nodeFinder, gx, gy, gz);
    element.CalcMassMatrix(nodeFinder, m);
    element.CalcRightPart(pComponentsProvider);
    element.CalcAMatrix();
    globalMatrix.PlaceLocalMatrix(element.LocalMatrixA, element.GlobalNodesNumbers);
    globalVector.PlaceLocalVector(element.RightPart, element.GlobalNodesNumbers);
}

boundaryConditionI.ReadFirstCondition("FirstBoundaryCondition.txt", out var globalNodesNumbersList1, out var usList1);
boundaryConditionI.ReadSecondCondition("SecondBoundaryCondition.txt", out var globalNodesNumbersList2, out var thetasList);
boundaryConditionI.ReadThirdCondition("ThirdBoundaryCondition.txt", out var globalNodesNumbersList3, out var betasList, out var usList2);


var firstBoundaryConditionArray = new FirstBoundaryCondition[globalNodesNumbersList1.Count];
var secondBoundaryConditionArray = new SecondBoundaryCondition[globalNodesNumbersList2.Count];
var thirdBoundaryConditionArray = new ThirdBoundaryCondition[globalNodesNumbersList3.Count];

for (var i = 0; i < firstBoundaryConditionArray.Length; i++)
{
    firstBoundaryConditionArray[i] = new FirstBoundaryCondition(globalNodesNumbersList1[i], usList1[i]);
}

for (var i = 0; i < secondBoundaryConditionArray.Length; i++)
{
    secondBoundaryConditionArray[i] = new SecondBoundaryCondition(globalNodesNumbersList2[i], thetasList[i]);
}

for (var i = 0; i < thirdBoundaryConditionArray.Length; i++)
{
    thirdBoundaryConditionArray[i] = new ThirdBoundaryCondition(globalNodesNumbersList3[i], betasList[i], usList2[i]);
}

var boundaryConditionsApplicator = new BoundaryConditionsApplicator(nodeFinder);

foreach (var secondBoundaryCondition in secondBoundaryConditionArray)
{
    boundaryConditionsApplicator.ApplySecondCondition(globalVector, secondBoundaryCondition);
}

//foreach (var thirdBoundaryCondition in thirdBoundaryConditionArray)
//{
//    boundaryConditionsApplicator.ApplyThirdCondition(globalMatrix, globalVector, thirdBoundaryCondition);
//}

foreach (var firstBoundaryCondition in firstBoundaryConditionArray)
{
    boundaryConditionsApplicator.ApplyFirstCondition(globalMatrix, globalVector, firstBoundaryCondition);
}

var startVector = globalVectorI.Read("StartVector.txt");
var (eps, maxIter) = parametersI.ReadMethodParameters("MCGParameters.txt");

var choleskyMCG = new MCGCholesky();

var qVector = choleskyMCG.Solve(globalMatrix, startVector, globalVector, eps, maxIter);

var globalVectorO = new GlobalVectorIO("../CourseProject_NumericalMethods_FiniteElementMethod/Output/GlobalVector/");
globalVectorO.Write("QVector.txt", qVector);

var solutionFinder = new SolutionFinder(grid, qVector, nodeFinder);

while (true)
{
    var point = pointI.ReadNodeFromConsole();

    if (solutionFinder.CheckArea(point))
    {
        var result = solutionFinder.FindSolution(point);

        CourseHolder.WriteSolution(point, result);
    }
    else
    {
        CourseHolder.WriteAreaInfo();
    }
}