using System;
using System.Collections.Generic;

namespace Employee.API.Models;

public partial class DesignationTbl
{
    public int DesignationId { get; set; }

    public int DepartmentId { get; set; }

    public string? DesignationName { get; set; }

    public virtual DepartmentTbl? Department { get; set; } = null!;

    public virtual ICollection<EmployeeTbl> EmployeeTbls { get; set; } = new List<EmployeeTbl>();
}

public class DesignationDTO
{
    public int DesignationId { get; set; }

    public int DepartmentId { get; set; }

    public string DesignationName { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;
}

public class DesignationCreateDTO
{
    public int DepartmentId { get; set; }
    public string DesignationName { get; set; } = null!;
}