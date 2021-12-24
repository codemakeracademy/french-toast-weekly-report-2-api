using CM.WeeklyTeamReport.Domain.Entities.Interfaces;
using CM.WeeklyTeamReport.Domain.Repositories.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.WeeklyTeamReport.Domain.Repositories.Interfaces
{
    public interface ICompanyManager
    {
        public ICompany Create(CompanyDto companyDto);
        public CompanyDto Read(int entityId);
        public ICollection<CompanyDto> ReadAll();
        public void Update(CompanyDto oldEntity, CompanyDto newEntity);
        public void Delete(int entityId);
    }
}
