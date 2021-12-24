﻿using CM.WeeklyTeamReport.Domain;
using CM.WeeklyTeamReport.Domain.Repositories.Dto;
using CM.WeeklyTeamReport.Domain.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CM.WeeklyTeamReport.WebAPI.Controllers.Tests
{
    public class MembersControllerTests
    {
        [Fact]
        public void ShouldReturnAllMembers()
        {
            var fixture = new MembersControllerFixture();
            fixture.TeamMemberManager
                .Setup(x => x.ReadAll(1))
                .Returns(new List<TeamMemberDto>() {
                    GetTeamMemberDto(1),
                    GetTeamMemberDto(2)
                });
            var controller = fixture.GetCompaniesController();
            var teamMembers = (ICollection<TeamMemberDto>)((OkObjectResult)controller.Get(1)).Value;

            teamMembers.Should().NotBeNull();
            teamMembers.Should().HaveCount(2);

            fixture
                .TeamMemberManager
                .Verify(x => x.ReadAll(1), Times.Once);
        }

        [Fact]
        public void ShouldReturnSingleMember()
        {
            var fixture = new MembersControllerFixture();
            fixture.TeamMemberManager
                .Setup(x => x.Read(1, 56))
                .Returns(GetTeamMemberDto(56));
            var controller = fixture.GetCompaniesController();
            var teamMembers = (TeamMemberDto)((OkObjectResult)controller.Get(1, 56)).Value;

            teamMembers.Should().NotBeNull();

            fixture
                .TeamMemberManager
                .Verify(x => x.Read(1, 56), Times.Once);
        }

        [Fact]
        public void ShouldCreateMember()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "auth0|123456"),
                new Claim("https://example.org/email", "mail@example.com"),
                new Claim("https://example.org/email_verified", "true"),
            }, "mock"));

            var fixture = new MembersControllerFixture();
            var teamMember = GetTeamMember();
            var teamMemberDto = GetTeamMemberDto();
            fixture.TeamMemberManager
                .Setup(x => x.Create(teamMemberDto))
                .Returns(teamMember);
            var controller = fixture.GetCompaniesController();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            var returnedTM = (TeamMember)((CreatedResult)controller.Post(1, teamMemberDto)).Value;

            returnedTM.Should().NotBeNull();
            returnedTM.ID.Should().NotBe(0);

            fixture
                .TeamMemberManager
                .Verify(x => x.Create(teamMemberDto), Times.Once);
        }

        [Fact]
        public void ShouldUpdateMember()
        {
            var fixture = new MembersControllerFixture();
            var teamMember = GetTeamMember();
            var teamMemberDto = GetTeamMemberDto();
            var teamMemberDto2 = GetTeamMemberDto();
            fixture.TeamMemberManager
                .Setup(x => x.Read(1, teamMember.ID))
                .Returns(teamMemberDto);
            fixture.TeamMemberManager
                .Setup(x => x.Update(teamMemberDto, teamMemberDto2));
            teamMember.FirstName = "Name 2";
            var controller = fixture.GetCompaniesController();
            var actionResult = controller.Put(teamMemberDto, 1, teamMember.ID);
            actionResult.Should().BeOfType<NoContentResult>();

            fixture
                .TeamMemberManager
                .Verify(x => x.Read(1, teamMember.ID), Times.Once);
            fixture
                .TeamMemberManager
                .Verify(x => x.Update(teamMemberDto, teamMemberDto), Times.Once);
        }

        [Fact]
        public void ShouldDeleteMember()
        {
            var fixture = new MembersControllerFixture();
            var teamMember = GetTeamMember();
            var teamMemberDto = GetTeamMemberDto();
            fixture.TeamMemberManager
                .Setup(x => x.Delete(1, teamMember.ID));
            fixture.TeamMemberManager
                .Setup(x => x.Read(1, teamMember.ID))
                .Returns(teamMemberDto);

            var controller = fixture.GetCompaniesController();
            var actionResult = controller.Delete(1, teamMember.ID);
            actionResult.Should().BeOfType<NoContentResult>();

            fixture
                .TeamMemberManager
                .Verify(x => x.Delete(1, teamMember.ID), Times.Once);
        }
        [Fact]
        public void ShouldReturnNotFoundOnReadAll()
        {
            var fixture = new MembersControllerFixture();
            fixture.TeamMemberManager
                .Setup(x => x.ReadAll(1))
                .Returns((List<TeamMemberDto>)null);
            var controller = fixture.GetCompaniesController();
            var teamMembers = controller.Get(1);

            teamMembers.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public void ShouldReturnNotFoundOnRead()
        {
            var fixture = new MembersControllerFixture();
            fixture.TeamMemberManager
                .Setup(x => x.Read(1, 1))
                .Returns((TeamMemberDto)null);
            var controller = fixture.GetCompaniesController();
            var teamMembers = controller.Get(1);

            teamMembers.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public void ShouldReturnNoContentOnPost()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "auth0|123456"),
                new Claim("https://example.org/email", "mail@example.com"),
                new Claim("https://example.org/email_verified", "true"),
            }, "mock"));

            var fixture = new MembersControllerFixture();
            var teamMemberDto = GetTeamMemberDto();
            fixture.TeamMemberManager
                .Setup(x => x.Create(teamMemberDto))
                .Returns((TeamMember)null);
            var controller = fixture.GetCompaniesController();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            var teamMembers = controller.Post(1, teamMemberDto);

            teamMembers.Should().BeOfType<NoContentResult>();
        }
        [Fact]
        public void ShouldReturnNotFoundOnPut()
        {
            var fixture = new MembersControllerFixture();
            var teamMemberDto = GetTeamMemberDto();
            fixture.TeamMemberManager
                .Setup(x => x.Read(1, 1))
                .Returns((TeamMemberDto)null);
            var controller = fixture.GetCompaniesController();
            var teamMember = controller.Put(teamMemberDto, 1, 1);

            teamMember.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public void ShouldReturnNotFoundOnDelete()
        {
            var fixture = new MembersControllerFixture();
            var teamMemberDto = GetTeamMemberDto();
            fixture.TeamMemberManager
                .Setup(x => x.Read(1, 1))
                .Returns((TeamMemberDto)null);
            var controller = fixture.GetCompaniesController();
            var teamMember = controller.Delete(1, 1);

            teamMember.Should().BeOfType<NotFoundResult>();
        }

        private TeamMember GetTeamMember(int id = 1)
        {
            return new TeamMember
            {
                ID = id,
                FirstName = $"Agent{id}",
                LastName = $"Smith{id}",
                Title = $"Agent{id}",
                Email = $"smith{id}@matrix.org",
                Sub = $"auth0|{id}"
            };
        }
        private TeamMemberDto GetTeamMemberDto(int id = 1)
        {
            return new TeamMemberDto
            {
                ID = id,
                FirstName = $"Agent{id}",
                LastName = $"Smith{id}",
                Title = $"Agent{id}",
                Email = $"smith{id}@matrix.org",
                Sub = $"auth0|{id}"
            };
        }

        public class MembersControllerFixture
        {
            public MembersControllerFixture()
            {
                TeamMemberManager = new Mock<ITeamMemberManager>();
            }

            public Mock<ITeamMemberManager> TeamMemberManager { get; private set; }

            public TeamMembersController GetCompaniesController()
            {
                return new TeamMembersController(TeamMemberManager.Object);
            }
        }
    }
}
