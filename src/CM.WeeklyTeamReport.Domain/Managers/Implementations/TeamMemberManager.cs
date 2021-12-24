using CM.WeeklyTeamReport.Domain.Commands;
using CM.WeeklyTeamReport.Domain.Entities.Interfaces;
using CM.WeeklyTeamReport.Domain.Exceptions;
using CM.WeeklyTeamReport.Domain.Repositories.Dto;
using CM.WeeklyTeamReport.Domain.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CM.WeeklyTeamReport.Domain.Repositories.Managers
{

    public class TeamMemberManager : ITeamMemberManager
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMemberCommands _memberCommands;

        public TeamMemberManager(ITeamMemberRepository teamMemberRepository, ICompanyRepository companyRepository, IMemberCommands memberCommands)
        {
            _teamMemberRepository = teamMemberRepository;
            _companyRepository = companyRepository;
            _memberCommands = memberCommands;
        }

        public ITeamMember Create(TeamMemberDto teamMember)
        {
            CheckIfCompanyExists(teamMember.CompanyId);

            var newTeamMember = _memberCommands.DtoToTeamMember(teamMember);
            return _teamMemberRepository.Create(newTeamMember);
        }

        public void Delete(int companyId, int teamMemberId)
        {
            CheckIfCompanyExists(companyId);
            _teamMemberRepository.Delete(teamMemberId);
        }

        public ICollection<TeamMemberDto> ReadAll(int companyId)
        {
            CheckIfCompanyExists(companyId);
            var teamMembers = _teamMemberRepository.ReadAll(companyId);
            string companyName = _companyRepository.GetCompanyName(companyId);
            var teamMembersDto = teamMembers.Select(el => _memberCommands.TeamMemberToDto(el, companyName)).ToList();

            return teamMembersDto;
        }

        private void CheckIfCompanyExists(int companyId)
        {
            if (_companyRepository.Read(companyId) == null)
                throw new DbRecordNotFoundException($"Company with id {companyId} not found.");
        }

        public TeamMemberDto Read(int companyId, int teamMemberId)
        {
            CheckIfCompanyExists(companyId);
            var teamMember = _teamMemberRepository.Read(companyId, teamMemberId);
            if (teamMember == null)
            {
                return null;
            }
            string companyName = _companyRepository.GetCompanyName(companyId);
            var teamMemberDto = _memberCommands.TeamMemberToDto(teamMember, companyName);

            return teamMemberDto;
        }

        public TeamMemberDto ReadBySub(string sub)
        {
            var teamMember = _teamMemberRepository.ReadBySub(sub);
            if (teamMember == null)
            {
                return null;
            }
            string companyName = _companyRepository.GetCompanyName(teamMember.CompanyId);
            var teamMemberDto = _memberCommands.TeamMemberToDto(teamMember, companyName);

            return teamMemberDto;
        }

        public void Update(TeamMemberDto oldEntity, TeamMemberDto newEntity)
        {
            newEntity.ID = oldEntity.ID;
            newEntity.CompanyId = oldEntity.CompanyId;
            newEntity.Email = oldEntity.Email;
            newEntity.Sub = oldEntity.Sub;
            newEntity.CompanyName = oldEntity.CompanyName;
            newEntity.InviteLink = oldEntity.InviteLink;
            var teamMember = _memberCommands.DtoToTeamMember(newEntity);
            _teamMemberRepository.Update(teamMember);
        }
    }
}
