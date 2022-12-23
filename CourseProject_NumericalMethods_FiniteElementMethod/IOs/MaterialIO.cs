using CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;

namespace CourseProject_NumericalMethods_FiniteElementMethod.IOs;

public class MaterialIO
{
    private readonly string _path;

    public MaterialIO(string path)
    {
        _path = path;
    }

    public void ReadMaterialsParametersFromFile(string fileName, out List<double> lambdasList, out List<double> gammasList)
    {
        using var streamReader = new StreamReader(_path + fileName);
        var materialsParameters = streamReader.ReadToEnd().Replace('.', ',').Split("\r\n");
        lambdasList = new List<double>();
        gammasList = new List<double>();
        var i = 0;
        foreach (var materialParameters in materialsParameters)
        {
            var materialData = materialParameters.Split(' ');
            lambdasList.Add(double.Parse(materialData[0]));
            gammasList.Add(double.Parse(materialData[1]));
        }
    }

    public Material[] ReadMaterialsFromFile(string fileName)
    {
        using var streamReader = new StreamReader(_path + fileName);
        var materialsParameters = streamReader.ReadToEnd().Replace('.', ',').Split("\r\n");
        var materials = new Material[materialsParameters.Length];
        var i = 0;
        foreach (var materialParameters in materialsParameters)
        {
            var materialData = materialParameters.Split(' ');
            var id = int.Parse(materialData[0]);
            var lambdas = double.Parse(materialData[1]);
            var gamma = double.Parse(materialData[^1]);

            materials[i++] = new Material(id, lambdas, gamma);
        }

        return materials;
    }
}