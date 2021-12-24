using CM.WeeklyTeamReport.Domain.Dto;
using CM.WeeklyTeamReport.Domain.Entities.Interfaces;
using CM.WeeklyTeamReport.Domain.Repositories.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.WeeklyTeamReport.Domain.Repositories.Interfaces
{
    public interface ITeamMemberManager
    {
        public ICollection<TeamMemberDto> ReadAll(int companyId);
        public TeamMemberDto Read(int companyId, int teamMemberId);
        public TeamMemberDto ReadBySub(string sub);
        public ITeamMember Create(TeamMemberDto newTeamMember);
        public void Update(TeamMemberDto oldEntity, TeamMemberDto newEntity);
        public void Delete(int companyId, int entityIdy);
    }
}
