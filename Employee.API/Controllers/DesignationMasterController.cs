using Employee.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationMasterController : ControllerBase
    {
        private readonly EmployeeManageDbContext _context;

        public DesignationMasterController(EmployeeManageDbContext context)
        {
            _context = context;
        }

        // GET: api/DesignationMaster/GetAllDesignations
        [HttpGet("GetAllDesignations")]
        public IActionResult GetAllDesignations()
        {
            try
            {
                var designations = _context.DesignationTbls
                    .Select(d => new DesignationDTO
                    {
                        DesignationId = d.DesignationId,
                        DesignationName = d.DesignationName,
                        DepartmentId = d.DepartmentId,
                        DepartmentName = d.Department.DepartmentName
                    })
                    .ToList();
                return Ok(designations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/DesignationMaster/GetDesignationById/5
        [HttpGet("GetDesignationById/{id}")]
        public IActionResult GetDesignationById(int id)
        {
            try
            {
                var designation = _context.DesignationTbls
                    .Include(d => d.Department)
                    .FirstOrDefault(d => d.DesignationId == id);

                if (designation == null)
                    return NotFound(new { message = "Designation not found." });

                return Ok(designation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/DesignationMaster/AddDesignation
        [HttpPost("AddDesignation")]
        public IActionResult AddDesignation([FromBody] DesignationTbl designation)
        {
            if (designation == null)
                return BadRequest(new { message = "Designation is required." });

            if (designation.DesignationName == null || designation.DesignationName.Length == 0)
                return BadRequest(new { message = "Designation name is required." });

            if (designation.DepartmentId <= 0)
                return BadRequest(new { message = "Valid DepartmentId is required." });

            var exists = _context.DesignationTbls
                .Any(d => d.DesignationName.ToLower() == designation.DesignationName.ToLower());

            if (exists)
            {
                return BadRequest(new { message = "Designation name must be unique." });
            }

            try
            {
                _context.DesignationTbls.Add(designation);
                _context.SaveChanges();
                return Ok(new { message = "Designation added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/DesignationMaster/UpdateDesignation
        [HttpPut("UpdateDesignation")]
        public IActionResult UpdateDesignation([FromBody] DesignationTbl designation)
        {
            if (designation == null)
                return BadRequest(new { message = "Designation is required." });

            if (designation.DesignationId <= 0)
                return BadRequest(new { message = "Valid DesignationId is required." });

            if (designation.DesignationName == null || designation.DesignationName.Length == 0)
                return BadRequest(new { message = "Designation name is required." });

            if (designation.DepartmentId <= 0)
                return BadRequest(new { message = "Valid DepartmentId is required." });

            try
            {
                var existing = _context.DesignationTbls.FirstOrDefault(d => d.DesignationId == designation.DesignationId);
                if (existing == null)
                    return NotFound(new { message = "Designation not found." });

                existing.DesignationName = designation.DesignationName;
                existing.DepartmentId = designation.DepartmentId;
                _context.SaveChanges();

                return Ok(new { message = "Designation updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/DesignationMaster/DeleteDesignation/5
        [HttpDelete("DeleteDesignation/{id}")]
        public IActionResult DeleteDesignation(int id)
        {
            try
            {
                var designation = _context.DesignationTbls.FirstOrDefault(d => d.DesignationId == id);
                if (designation == null)
                    return NotFound(new { message = "Designation not found." });

                _context.DesignationTbls.Remove(designation);
                _context.SaveChanges();

                return Ok(new { message = "Designation deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // FILTER: api/DesignationMaster/FilterByDepartment/3
        [HttpGet("FilterByDepartment/{departmentId}")]
        public IActionResult FilterByDepartment(int departmentId)
        {
            try
            {
                var designations = _context.DesignationTbls
                    .Where(d => d.DepartmentId == departmentId)
                    .Include(d => d.Department)
                    .ToList();

                return Ok(designations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
