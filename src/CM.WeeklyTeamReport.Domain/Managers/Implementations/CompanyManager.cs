using CM.WeeklyTeamReport.Domain.Commands;
using CM.WeeklyTeamReport.Domain.Entities.Interfaces;
using CM.WeeklyTeamReport.Domain.Repositories.Dto;
using CM.WeeklyTeamReport.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.WeeklyTeamReport.Domain.Repositories.Managers
{
    public class CompanyManager : ICompanyManager
    {
        private readonly ICompanyRepository _repository;
        private readonly ICompanyCommand _companyCommand;

        public CompanyManager(ICompanyRepository companyRepository, ICompanyCommand companyCommand)
        {
            _repository = companyRepository;
            _companyCommand = companyCommand;
        }

        public ICompany Create(CompanyDto companyDto)
        {
            var newCompany = _companyCommand.DtoToCompany(companyDto);
            newCompany.CreationDate = DateTime.Today;
            return _repository.Create(newCompany);
        }
        public CompanyDto Read(int entityId)
        {
            var company = _repository.Read(entityId);
            if (company == null)
            {
                return null;
            }
            var companyDto = _companyCommand.CompanyToDto(company);
            return companyDto;
        }
        public ICollection<CompanyDto> ReadAll()
        {
            var companies = _repository.ReadAll();
            var companiesDto = companies.Select(el => _companyCommand.CompanyToDto(el)).ToList();
            return companiesDto;
        }
        public void Update(CompanyDto oldEntity, CompanyDto newEntity)
        {
            newEntity.ID = oldEntity.ID;
            var company = _companyCommand.DtoToCompany(newEntity);
            _repository.Update(company);
        }
        public void Delete(int entityId)
        {
            _repository.Delete(entityId);
        }
    }
}
