using System;
using System.Collections.Generic;

namespace Employee.API.Models;

public partial class EmployeeTbl
{
    public int EmployeeId { get; set; }

    public string Name { get; set; } = null!;

    public string ContactNo { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Pincode { get; set; } = null!;

    public string? AltContactNo { get; set; }

    public string Address { get; set; } = null!;

    public int DesignationId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string Email { get; set; } = null!;

    public string? Role { get; set; }

    public virtual DesignationTbl? Designation { get; set; } = null!;
}


public class LoginRequestDTO
{
    public string Email { get; set; } = null!;
    public string ContactNo { get; set; } = null!;
}

public class EmployeeListDTO
{
    public int EmployeeId { get; set; }

    public string Name { get; set; } = null!;

    public string ContactNo { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Pincode { get; set; } = null!;

    public string? AltContactNo { get; set; }

    public string Address { get; set; } = null!;

    public int DesignationId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string Email { get; set; } = null!;

    public string? Role { get; set; }

    public string Designation { get; set; } = null!;

    public string Department { get; set; } = null!;
}