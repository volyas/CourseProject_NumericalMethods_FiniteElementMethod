using CourseProject.Core;
using CourseProject.Core.BoundaryConditions;
using CourseProject.Core.Global;
using CourseProject.Core.GridComponents;
using CourseProject.ThreeDimensional.Assembling;
using CourseProject.ThreeDimensional.Assembling.Boundary;
using CourseProject.ThreeDimensional.Assembling.Global;
using CourseProject.Time.Schemes.Explicit;
using UMF3.SLAE.Solvers;

namespace CourseProject.Time;

public class TimeDisсreditor
{
    public GlobalVector PreviousSolution => TimeSolutions[_currentTimeLayer - 1];
    public GlobalVector TwoLayersBackSolution => TimeSolutions[_currentTimeLayer - 2];
    public GlobalVector ThreeLayersBackSolution => TimeSolutions[_currentTimeLayer - 3];
    public GlobalVector[] TimeSolutions { get; private set; }

    public double CurrentTime => _timeLayers[_currentTimeLayer];
    public double PreviousTime => _timeLayers[_currentTimeLayer - 1];
    public double TwoLayersBackTime => _timeLayers[_currentTimeLayer - 2];
    public double ThreeLayersBackTime => _timeLayers[_currentTimeLayer - 3];

    private int _currentTimeLayer = 0;

    private readonly GlobalAssembler<Node3D> _globalAssembler;
    private readonly double[] _timeLayers;
    private readonly Grid<Node3D> _grid;
    private readonly FirstBoundaryProvider _firstBoundaryProvider;
    private readonly GaussExcluder _gaussExcluder;
    private readonly SecondBoundaryProvider _secondBoundaryProvider;
    private readonly ThirdBoundaryProvider _thirdBoundaryProvider;
    private readonly Inserter _inserter;
    private ThreeLayer _threeLayer;
    private FourLayer _fourLayer;
    private int[]? _firstConditionIndexes;
    private Bound[]? _firstConditionBounds;
    private int[]? _secondConditionIndexes;
    private Bound[]? _secondConditionBounds;
    private int[]? _thirdConditionIndexes;
    private Bound[]? _thirdConditionBounds;
    private double[]? _betas;
    private ISolver<SymmetricSparseMatrix> _solver;

    public TimeDisсreditor
    (
        GlobalAssembler<Node3D> globalAssembler,
        double[] timeLayers,
        Grid<Node3D> grid,
        FirstBoundaryProvider firstBoundaryProvider,
        GaussExcluder gaussExcluder,
        SecondBoundaryProvider secondBoundaryProvider,
        ThirdBoundaryProvider thirdBoundaryProvider,
        Inserter inserter
    )
    {
        _globalAssembler = globalAssembler;
        _timeLayers = timeLayers;
        TimeSolutions = new GlobalVector[_timeLayers.Length];
        _grid = grid;
        _firstBoundaryProvider = firstBoundaryProvider;
        _gaussExcluder = gaussExcluder;
        _secondBoundaryProvider = secondBoundaryProvider;
        _thirdBoundaryProvider = thirdBoundaryProvider;
        _inserter = inserter;

    }

    public TimeDisсreditor SetFirstInitialSolution(Func<Node3D, double, double> u)
    {
        var initialSolution = new GlobalVector(_grid.Nodes.Length);
        var currentTime = CurrentTime;

        for (var i = 0; i < _grid.Nodes.Length; i++)
        {
            initialSolution[i] = u(_grid.Nodes[i], currentTime);
        }

        TimeSolutions[_currentTimeLayer] = initialSolution;
        _currentTimeLayer++;

        return this;
    }

    public TimeDisсreditor SetSecondInitialSolution(Func<Node3D, double, double> u)
    {
        var initialSolution = new GlobalVector(_grid.Nodes.Length);
        var prevTime = PreviousTime;
        var currentTime = CurrentTime;

        for (var i = 0; i < _grid.Nodes.Length; i++)
        {
            initialSolution[i] = PreviousSolution[i] + u(_grid.Nodes[i], prevTime) * (currentTime - prevTime);
        }

        TimeSolutions[_currentTimeLayer] = initialSolution;
        _currentTimeLayer++;

        return this;
    }

    public TimeDisсreditor SetThirdInitialSolution(Func<Node3D, double, double> u)
    {
        var initialSolution = new GlobalVector(_grid.Nodes.Length);

        for (var i = 0; i < _grid.Nodes.Length; i++)
        {
            initialSolution[i] = u(_grid.Nodes[i], CurrentTime);
        }

        TimeSolutions[_currentTimeLayer] = initialSolution;
        _currentTimeLayer++;

        return this;
    }

    public TimeDisсreditor SetFirstConditions(int[] elementsIndexes, Bound[] bounds)
    {
        _firstConditionIndexes = elementsIndexes;
        _firstConditionBounds = bounds;

        return this;
    }

    public TimeDisсreditor SetFirstConditions(int elementsByLength, int elementsByWidth, int elementsByHeight)
    {
        (_firstConditionIndexes, _firstConditionBounds) =
            _firstBoundaryProvider.GetConditions(elementsByLength, elementsByWidth, elementsByHeight);

        return this;
    }

    public TimeDisсreditor SetSecondConditions(int[] elementsIndexes, Bound[] bounds)
    {
        _secondConditionIndexes = elementsIndexes;
        _secondConditionBounds = bounds;

        return this;
    }

    public TimeDisсreditor SetThirdConditions(int[] elementsIndexes, Bound[] bounds, double[] betas)
    {
        _thirdConditionIndexes = elementsIndexes;
        _thirdConditionBounds = bounds;
        _betas = betas;

        return this;
    }

    public TimeDisсreditor SetSolver(ISolver<SymmetricSparseMatrix> solver)
    {
        _solver = solver;

        return this;
    }

    public GlobalVector[] GetSolutions()
    {
        var stiffness = _globalAssembler.AssembleStiffnessMatrix(_grid);
        var sigmaMass = _globalAssembler.AssembleSigmaMassMatrix(_grid);
        var timeDeltasCalculator = new TimeDeltasCalculator();

        _threeLayer = new ThreeLayer(stiffness, sigmaMass, timeDeltasCalculator);
        _fourLayer = new FourLayer(stiffness, sigmaMass, timeDeltasCalculator);

        while (_currentTimeLayer < _timeLayers.Length)
        {
            var equation = TimeSolutions[2] != null ? UseFourLayerScheme() : UseThreeLayerScheme();

            if (_secondConditionIndexes != null && _secondConditionBounds != null)
            {
                var secondConditions = _secondBoundaryProvider.GetConditions(_secondConditionIndexes,
                    _secondConditionBounds, CurrentTime);
                ApplySecondConditions(equation, secondConditions);
            }
            if (_thirdConditionIndexes != null && _thirdConditionBounds != null && _betas != null)
            {
                var thirdConditions = _thirdBoundaryProvider.GetConditions(_thirdConditionIndexes,
                    _thirdConditionBounds, _betas, CurrentTime);
                ApplyThirdConditions(equation, thirdConditions);
            }
            if (_firstConditionIndexes != null && _firstConditionBounds != null)
            {
                var firstConditions = _firstBoundaryProvider.GetConditions(_firstConditionIndexes,
                    _firstConditionBounds, CurrentTime);
                ApplyFirstConditions(equation, firstConditions);
            }

            TimeSolutions[_currentTimeLayer] = _solver.Solve(equation);
            _currentTimeLayer++;

        }

        return TimeSolutions;
    }

    private Equation<SymmetricSparseMatrix> UseThreeLayerScheme()
    {
        var equation = _threeLayer
            .BuildEquation
            (
                _globalAssembler.AssembleRightPart(_grid, PreviousTime),
                PreviousSolution,
                TwoLayersBackSolution,
                CurrentTime,
                PreviousTime,
                TwoLayersBackTime
            );

        return equation;
    }

    private Equation<SymmetricSparseMatrix> UseFourLayerScheme()
    {
        var equation = _fourLayer
            .BuildEquation
            (
                _globalAssembler.AssembleRightPart(_grid, PreviousTime),
                PreviousSolution,
                TwoLayersBackSolution,
                ThreeLayersBackSolution,
                CurrentTime,
                PreviousTime,
                TwoLayersBackTime,
                ThreeLayersBackTime
            );

        return equation;
    }

    private void ApplyFirstConditions(Equation<SymmetricSparseMatrix> equation, FirstCondition[] firstConditions)
    {
        foreach (var firstCondition in firstConditions)
        {
            _gaussExcluder.Exclude(equation, firstCondition);
        }
    }

    private void ApplySecondConditions(Equation<SymmetricSparseMatrix> equation, SecondCondition[] secondConditions)
    {
        foreach (var secondCondition in secondConditions)
        {
            _inserter.InsertVector(equation.RightSide, secondCondition.Vector);
        }
    }

    private void ApplyThirdConditions(Equation<SymmetricSparseMatrix> equation, ThirdCondition[] thirdConditions)
    {
        foreach (var thirdCondition in thirdConditions)
        {
            _inserter.InsertMatrix(equation.Matrix, thirdCondition.Matrix);
            _inserter.InsertVector(equation.RightSide, thirdCondition.Vector);
        }
    }
}