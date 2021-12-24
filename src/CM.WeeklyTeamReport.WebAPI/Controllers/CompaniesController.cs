using CM.WeeklyTeamReport.Domain;
using CM.WeeklyTeamReport.Domain.Repositories.Dto;
using CM.WeeklyTeamReport.Domain.Repositories.Interfaces;
using CM.WeeklyTeamReport.Domain.Repositories.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CM.WeeklyTeamReport.WebAPI.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyManager _manager;
        public CompaniesController(ICompanyManager companyManager)
        {
            _manager = companyManager;
        }
        // Get api/<CompanyController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = _manager.ReadAll();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        // Get api/<CompanyController>/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _manager.Read(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        // POST api/<CompanyController>
        [HttpPost]
        public IActionResult Post ([FromBody] CompanyDto companyDto)
        {
            var result = _manager.Create(companyDto);
            if (result == null)
            {
                return NoContent();
            }
            var uriCreatedCompany = $"api/companies/{result.ID}";
            return Created(uriCreatedCompany, result);
        }
        // PUT api/<CompanyController>/id
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put([FromBody] CompanyDto companyDto, int id)
        {
            var updatedCompany = _manager.Read(id);
            if (updatedCompany == null)
            {
                return NotFound();
            }
            _manager.Update(updatedCompany, companyDto);
            return NoContent();
        }
        // DELETE api/<CompanyController>/id
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var result = _manager.Read(id);
            if (result == null)
            {
                return NotFound();
            }
            _manager.Delete(id);
            return NoContent();
        }
    }
}
