using CourseProject.Core.Base;
using CourseProject.Core.GridComponents;
using CourseProject.Core.Local;
using CourseProject.FEM.Assembling;
using CourseProject.FEM.Assembling.Local;
using CourseProject.FEM.Parameters;
using CourseProject.ThreeDimensional.Parameters;

namespace CourseProject.ThreeDimensional.Assembling.Local;

public class LocalAssembler : ILocalAssembler
{
    private readonly ITemplateMatrixProvider _massTemplateProvider;
    private readonly ITemplateMatrixProvider _stiffnessXTemplateProvider;
    private readonly ITemplateMatrixProvider _stiffnessYTemplateProvider;
    private readonly ITemplateMatrixProvider _stiffnessZTemplateProvider;
    private readonly MaterialFactory _materialFactory;
    private readonly IFunctionalParameter _functionalParameter;

    public LocalAssembler(
        ITemplateMatrixProvider massTemplateProvider,
        ITemplateMatrixProvider stiffnessXTemplateProvider,
        ITemplateMatrixProvider stiffnessYTemplateProvider,
        ITemplateMatrixProvider stiffnessZTemplateProvider,
        MaterialFactory materialFactory,
        IFunctionalParameter functionalProvider
    )
    {
        _massTemplateProvider = massTemplateProvider;
        _stiffnessXTemplateProvider = stiffnessXTemplateProvider;
        _stiffnessYTemplateProvider = stiffnessYTemplateProvider;
        _stiffnessZTemplateProvider = stiffnessZTemplateProvider;
        _materialFactory = materialFactory;
        _functionalParameter = functionalProvider;
    }

    public LocalMatrix AssembleStiffnessMatrix(Element element)
    {
        var stiffness = GetStiffnessMatrix(element);
        return new LocalMatrix(element.NodesIndexes, stiffness);
    }

    public LocalMatrix AssembleSigmaMassMatrix(Element element)
    {
        var mass = GetMassMatrix(element);
        var material = _materialFactory.GetById(element.MaterialId);
        return new LocalMatrix(element.NodesIndexes, BaseMatrix.Multiply(material.Sigma, mass));
    }

    public LocalVector AssembleRightSide(Element element, double time)
    {
        var rightPart = GetRightPart(element, time);
        return new LocalVector(element.NodesIndexes, rightPart);
    }

    private BaseMatrix GetStiffnessMatrix(Element element)
    {
        var stiffnessX = _stiffnessXTemplateProvider.GetMatrix();
        var stiffnessY = _stiffnessYTemplateProvider.GetMatrix();
        var stiffnessZ = _stiffnessZTemplateProvider.GetMatrix();

        var stiffness =
            BaseMatrix.Multiply(
                element.Length * element.Width * element.Height / 36d,
                BaseMatrix.Sum(BaseMatrix.Sum(
                    stiffnessX / Math.Pow(element.Length, 2),
                    stiffnessY / Math.Pow(element.Width, 2)),
                    stiffnessZ / Math.Pow(element.Height, 2))
            );

        return stiffness;
    }

    private BaseMatrix GetMassMatrix(Element element)
    {
        var mass = _massTemplateProvider.GetMatrix();

        return element.Length * element.Width * element.Height / 216d * mass;
    }

    private BaseVector GetRightPart(Element element, double time)
    {
        var mass = GetMassMatrix(element);
        var rightPart = new BaseVector(element.NodesIndexes.Length);

        for (var i = 0; i < rightPart.Count; i++)
        {
            rightPart[i] = _functionalParameter.Calculate(element.NodesIndexes[i], time);
        }

        return mass * rightPart;
    }
}