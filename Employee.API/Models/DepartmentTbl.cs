using System;
using System.Collections.Generic;

namespace Employee.API.Models;

public partial class DepartmentTbl
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual ICollection<DesignationTbl> DesignationTbls { get; set; } = new List<DesignationTbl>();
}
