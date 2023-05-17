using CourseProject.Calculus;
using CourseProject.Core.GridComponents;
using CourseProject.FEM;
using CourseProject.GridGenerator;
using CourseProject.GridGenerator.Area.Core;
using CourseProject.GridGenerator.Area.Splitting;
using CourseProject.SLAE.Preconditions.LLT;
using CourseProject.SLAE.Solvers;
using CourseProject.ThreeDimensional;
using CourseProject.ThreeDimensional.Assembling;
using CourseProject.ThreeDimensional.Assembling.Boundary;
using CourseProject.ThreeDimensional.Assembling.Global;
using CourseProject.ThreeDimensional.Assembling.Local;
using CourseProject.ThreeDimensional.Assembling.MatrixTemplates;
using CourseProject.ThreeDimensional.Parameters;
using CourseProject.Time;
using System.Globalization;

Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

var gridBuilder3D = new GridBuilder3D();
var grid = gridBuilder3D
    .SetXAxis(new AxisSplitParameter(
            new[] { 1d, 3d },
            new UniformSplitter(1)
        )
    )
    .SetYAxis(new AxisSplitParameter(
            new[] { 1d, 3d },
            new UniformSplitter(1)
        )
    )
    .SetZAxis(new AxisSplitParameter(
            new[] { 1d, 3d },
            new UniformSplitter(1)
        )
    )
    .Build();

var materialFactory = new MaterialFactory
(
    new List<double> { 1d },
    new List<double> { 1d }
);

var localBasisFunctionsProvider = new LocalBasisFunctionsProvider(grid, new LinearFunctionsProvider());

var f = new RightPartParameter((p, t) => p.X, grid);

var derivativeCalculator = new DerivativeCalculator();

var localAssembler = new LocalAssembler(new MassMatrixTemplateProvider(), new StiffnessXMatrixTemplateProvider(),
    new StiffnessYMatrixTemplateProvider(), new StiffnessZMatrixTemplateProvider(), materialFactory, f);

var inserter = new Inserter();
var globalAssembler = new GlobalAssembler<Node3D>(new MatrixPortraitBuilder(), localAssembler, inserter);

var timeLayers = new UniformSplitter(4)
    .EnumerateValues(new Interval(1, 1 + 4e-8))
    .ToArray();

var secondConditionTemplate = new SecondConditionMatrixTemplateProvider();
var firstBoundaryProvider = new FirstBoundaryProvider(grid, (p, t) => p.X * t);
var secondBoundaryProvider = new SecondBoundaryProvider(grid, (p, t) => p.X * t, materialFactory, secondConditionTemplate, derivativeCalculator);
var thirdBoundaryProvider = new ThirdBoundaryProvider(grid, (p, t) => p.X * t, materialFactory, secondConditionTemplate, derivativeCalculator);

var lltPreconditioner = new LLTPreconditioner();
var solver = new MCG(lltPreconditioner, new LLTSparse(lltPreconditioner));

var timeDiscreditor = new TimeDisсreditor(globalAssembler, timeLayers, grid, firstBoundaryProvider,
    new GaussExcluder(), secondBoundaryProvider, thirdBoundaryProvider, inserter);

var solutions =
    timeDiscreditor
        .SetFirstInitialSolution((p, t) => p.X * t)
        //.SetSecondInitialSolution((p, t) => p.X * t)
        //.SetThirdInitialSolution((p, t) => p.X * t)
        .SetSecondConditions
        (
            new[] { 0, 0, 0 },
            new[] { Bound.Left, Bound.Right, Bound.Upper }
        )
        .SetThirdConditions
        (
            new[] { 0, 0 },
            new[] { Bound.Front, Bound.Back },
            new[] { 1d, 1d }
        )
        //.SetFirstConditions
        //(
        //    new[] { 0, 0, 1, 1, 2, 2, 3, 3 },
        //    new[] { Bound.Lower, Bound.Left, Bound.Lower, Bound.Right, Bound.Left, Bound.Upper, Bound.Upper, Bound.Right }
        //)
        .SetFirstConditions
        (
            new[] { 0 },
            new[] { Bound.Lower }
        )
        .SetSolver(solver)
        .GetSolutions();

var femSolution = new FEMSolution(grid, solutions, timeLayers, localBasisFunctionsProvider);
//femSolution.Calculate(new Node3D(2d, 2d, 2d), 1 + 2.2e-5);
var error = femSolution.CalcError((p, t) => p.X * t, 1 + 4e-8);

Console.WriteLine(error);