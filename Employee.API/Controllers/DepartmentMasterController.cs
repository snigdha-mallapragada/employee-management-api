using Employee.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentMasterController : ControllerBase
    {
        private readonly EmployeeManageDbContext _context;

        public DepartmentMasterController(EmployeeManageDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllDepartment")]
        public IActionResult GetAllDepartment()
        {
            var deptList = _context.DepartmentTbls.ToList();
            return Ok(deptList);
        }

        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment([FromBody] DepartmentTbl department)
        {
            if (string.IsNullOrWhiteSpace(department.DepartmentName))
            {
                return BadRequest(new { message = "Department name is required." });
            }

            var exists = _context.DepartmentTbls
                .Any(d => d.DepartmentName.ToLower() == department.DepartmentName.ToLower());

            if (exists)
            {
                return BadRequest(new { message = "Department name must be unique." });
            }

            _context.DepartmentTbls.Add(department);
            _context.SaveChanges();
            return Ok(new { message = "Department Added Successfully" });
        }


        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] DepartmentTbl department)
        {
            var dept = _context.DepartmentTbls.Where(x => x.DepartmentId == department.DepartmentId).FirstOrDefault();
            if (dept != null)
            {
                dept.DepartmentName = department.DepartmentName;
                dept.IsActive = department.IsActive;
                _context.SaveChanges();
                return Ok(new { message = "Department Updated Successfully" });
            }
            else
            {
                return NotFound(new { message = "Department Not Found" });
            }
        }

        [HttpDelete("DeleteDepartment/{id}")]
        public IActionResult DeleteDepartment([FromRoute(Name = "id")] int departmentId)
        {
            var dept = _context.DepartmentTbls.Where(x => x.DepartmentId == departmentId).FirstOrDefault();
            if (dept != null)
            {
                _context.DepartmentTbls.Remove(dept);
                _context.SaveChanges();
                return Ok(new { message = "Department Deleted Successfully" });
            }
            else
            {
                return NotFound(new { message = "Department Not Found" });
            }
        }
    }
}
