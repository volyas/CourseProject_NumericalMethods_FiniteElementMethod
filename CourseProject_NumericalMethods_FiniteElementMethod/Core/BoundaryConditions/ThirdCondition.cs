using CourseProject.Core.Local;

namespace CourseProject.Core.BoundaryConditions;

public readonly record struct ThirdCondition(LocalMatrix Matrix, LocalVector Vector);