using Employee.API.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeMasterController : ControllerBase
    {
        private readonly EmployeeManageDbContext _context;

        public EmployeeMasterController(EmployeeManageDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeMaster/GetAllEmployees
        [HttpGet("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            try
            {
                var employees = _context.EmployeeTbls
                    .Select(e => new EmployeeListDTO
                    {
                        EmployeeId = e.EmployeeId,
                        Name = e.Name,
                        ContactNo = e.ContactNo,
                        City = e.City,
                        State = e.State,
                        Pincode = e.Pincode,
                        AltContactNo = e.AltContactNo,
                        Address = e.Address,
                        DesignationId = e.DesignationId,
                        CreatedDate = e.CreatedDate,
                        ModifiedDate = e.ModifiedDate,
                        Email = e.Email,
                        Role = e.Role,
                        Designation = e.Designation.DesignationName,
                        Department = e.Designation.Department.DepartmentName
                    }
                    )
                    .ToList();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetEmployeeById/{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            try
            {
                var employee = _context.EmployeeTbls
                    .Where(e => e.EmployeeId == id)
                    .Select(e => new EmployeeListDTO
                    {
                        EmployeeId = e.EmployeeId,
                        Name = e.Name,
                        ContactNo = e.ContactNo,
                        City = e.City,
                        State = e.State,
                        Pincode = e.Pincode,
                        AltContactNo = e.AltContactNo,
                        Address = e.Address,
                        DesignationId = e.DesignationId,
                        CreatedDate = e.CreatedDate,
                        ModifiedDate = e.ModifiedDate,
                        Email = e.Email,
                        Role = e.Role,
                        Designation = e.Designation.DesignationName,
                        Department = e.Designation.Department.DepartmentName
                    })
                    .FirstOrDefault();

                if (employee == null)
                {
                    return NotFound(new { message = "Employee not found" });
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/EmployeeMaster/GetEmployees
        // Supports filtering by name, sorting, and pagination
        [HttpGet("GetEmployees")]
        public IActionResult GetEmployees(
            string? search = null,
            string? sortBy = null,
            bool desc = false,
            int page = 1,
            int pageSize = 10)
        {
            try
            {
                var query = _context.EmployeeTbls.Include(e => e.Designation).AsQueryable();

                // Filtering
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(e =>
                        e.Name.Contains(search) ||
                        e.City.Contains(search) ||
                        e.State.Contains(search) ||
                        e.ContactNo.Contains(search) ||
                        (e.Email != null && e.Email.Contains(search))
                    );
                }

                // Sorting
                query = sortBy?.ToLower() switch
                {
                    "name" => desc ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name),
                    "city" => desc ? query.OrderByDescending(e => e.City) : query.OrderBy(e => e.City),
                    "state" => desc ? query.OrderByDescending(e => e.State) : query.OrderBy(e => e.State),
                    "createddate" => desc ? query.OrderByDescending(e => e.CreatedDate) : query.OrderBy(e => e.CreatedDate),
                    _ => query.OrderBy(e => e.EmployeeId)
                };

                // Pagination
                var totalRecords = query.Count();
                var employees = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new
                {
                    TotalRecords = totalRecords,
                    Page = page,
                    PageSize = pageSize,
                    Data = employees
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/EmployeeMaster/AddEmployee
        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee([FromBody] EmployeeTbl employee)
        {
            if (employee == null)
                return BadRequest(new { message = "Employee is required." });

            if (string.IsNullOrWhiteSpace(employee.Name))
                return BadRequest(new { message = "Name is required." });

            if (string.IsNullOrWhiteSpace(employee.ContactNo))
                return BadRequest(new { message = "ContactNo is required." });

            if (string.IsNullOrWhiteSpace(employee.Email))
                return BadRequest(new { message = "Email is required." });

            // Unique validation
            var exists = _context.EmployeeTbls.Any(e =>
                e.ContactNo == employee.ContactNo || e.Email == employee.Email);

            if (exists)
                return BadRequest(new { message = "ContactNo and Email must be unique." });

            try
            {
                employee.CreatedDate = DateTime.UtcNow;
                _context.EmployeeTbls.Add(employee);
                _context.SaveChanges();
                return Ok(new { message = "Employee added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/EmployeeMaster/UpdateEmployee
        [HttpPut("UpdateEmployee")]
        public IActionResult UpdateEmployee([FromBody] EmployeeTbl employee)
        {
            if (employee == null)
                return BadRequest(new { message = "Employee is required." });

            if (employee.EmployeeId <= 0)
                return BadRequest(new { message = "Valid EmployeeId is required." });

            if (string.IsNullOrWhiteSpace(employee.Name))
                return BadRequest(new { message = "Name is required." });

            if (string.IsNullOrWhiteSpace(employee.ContactNo))
                return BadRequest(new { message = "ContactNo is required." });

            if (string.IsNullOrWhiteSpace(employee.Email))
                return BadRequest(new { message = "Email is required." });

            // Unique validation (exclude self)
            var exists = _context.EmployeeTbls.Any(e =>
                (e.ContactNo == employee.ContactNo || e.Email == employee.Email) &&
                e.EmployeeId != employee.EmployeeId);

            if (exists)
                return BadRequest(new { message = "ContactNo and Email must be unique." });

            try
            {
                var existing = _context.EmployeeTbls.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
                if (existing == null)
                    return NotFound(new { message = "Employee not found." });

                existing.Name = employee.Name;
                existing.ContactNo = employee.ContactNo;
                existing.Email = employee.Email;
                existing.City = employee.City;
                existing.State = employee.State;
                existing.Pincode = employee.Pincode;
                existing.AltContactNo = employee.AltContactNo;
                existing.Address = employee.Address;
                existing.DesignationId = employee.DesignationId;
                existing.ModifiedDate = DateTime.UtcNow;

                _context.SaveChanges();
                return Ok(new { message = "Employee updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/EmployeeMaster/DeleteEmployee/5
        [HttpDelete("DeleteEmployee/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                var employee = _context.EmployeeTbls.FirstOrDefault(e => e.EmployeeId == id);
                if (employee == null)
                    return NotFound(new { message = "Employee not found." });

                _context.EmployeeTbls.Remove(employee);
                _context.SaveChanges();

                return Ok(new { message = "Employee deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.ContactNo))
                return BadRequest(new { Message = "Email and Password are required." });

            try
            {
                var employee = _context.EmployeeTbls
                    .FirstOrDefault(e => e.Email == request.Email && e.ContactNo == request.ContactNo);

                if (employee == null)
                    return Unauthorized(new { Message = "Invalid email or password." });

                // Optionally, you can return a DTO or token here
                return Ok(new
                {
                    Message = "Login has been successful.",
                    employee.EmployeeId,
                    employee.Name,
                    employee.Email,
                    employee.DesignationId,
                    employee.Role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
