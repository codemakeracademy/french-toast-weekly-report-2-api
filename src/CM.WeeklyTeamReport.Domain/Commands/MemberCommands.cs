﻿using CM.WeeklyTeamReport.Domain.Entities.Interfaces;
using CM.WeeklyTeamReport.Domain.Repositories.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.WeeklyTeamReport.Domain.Commands
{
    public class MemberCommands : IMemberCommands
    {
        public TeamMemberDto TeamMemberToDto(ITeamMember teamMember, string company)
        {
            var teamMemberDto = new TeamMemberDto();
            teamMemberDto.ID = teamMember?.ID;
            teamMemberDto.FirstName = teamMember.FirstName;
            teamMemberDto.LastName = teamMember.LastName;
            teamMemberDto.Title = teamMember.Title;
            teamMemberDto.Email = teamMember.Email;
            teamMemberDto.Sub = teamMember.Sub;
            teamMemberDto.CompanyName = company;
            teamMemberDto.CompanyId = teamMember.CompanyId;
            teamMemberDto.InviteLink = teamMember?.InviteLink;

            return teamMemberDto;
        }
        public ITeamMember DtoToTeamMember(TeamMemberDto teamMemberDto)
        {
            var teamMember = new TeamMember();
            teamMember.ID = (int)teamMemberDto.ID;
            teamMember.FirstName = teamMemberDto.FirstName;
            teamMember.LastName = teamMemberDto.LastName;
            teamMember.Email = teamMemberDto.Email;
            teamMember.Sub = teamMemberDto.Sub;
            teamMember.Title = teamMemberDto.Title;
            teamMember.CompanyId = (int)teamMemberDto.CompanyId;
            teamMember.InviteLink = teamMemberDto.InviteLink;

            return teamMember;
        }
    }
}
