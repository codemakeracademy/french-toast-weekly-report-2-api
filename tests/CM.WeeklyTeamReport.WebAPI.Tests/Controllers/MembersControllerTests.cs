﻿using CM.WeeklyTeamReport.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            fixture.MemberRepository
                .Setup(x => x.ReadAll())
                .Returns(new List<TeamMember>() {
                    GetTeamMember(1),
                    GetTeamMember(2)
                });
            var controller = fixture.GetCompaniesController();
            var teamMembers = (ICollection<TeamMember>)((OkObjectResult)controller.Get()).Value;

            teamMembers.Should().NotBeNull();
            teamMembers.Should().HaveCount(2);

            fixture
                .MemberRepository
                .Verify(x => x.ReadAll(), Times.Once);
        }

        [Fact]
        public void ShouldReturnSingleMember()
        {
            var fixture = new MembersControllerFixture();
            fixture.MemberRepository
                .Setup(x => x.Read(56))
                .Returns(GetTeamMember(56));
            var controller = fixture.GetCompaniesController();
            var teamMembers = (TeamMember)((OkObjectResult)controller.GetSingle(1, 56)).Value;

            teamMembers.Should().NotBeNull();

            fixture
                .MemberRepository
                .Verify(x => x.Read(56), Times.Once);
        }

        [Fact]
        public void ShouldCreateMember()
        {
            var fixture = new MembersControllerFixture();
            var tm = GetTeamMember();
            fixture.MemberRepository
                .Setup(x => x.Create(tm))
                .Returns(tm);
            var controller = fixture.GetCompaniesController();
            var returnedTM = (TeamMember)((CreatedResult)controller.Create(1, tm)).Value;

            returnedTM.Should().NotBeNull();
            returnedTM.ID.Should().NotBe(0);

            fixture
                .MemberRepository
                .Verify(x => x.Create(tm), Times.Once);
        }

        [Fact]
        public void ShouldUpdateMember()
        {
            var fixture = new MembersControllerFixture();
            var tm = GetTeamMember();
            fixture.MemberRepository
                .Setup(x => x.Read(tm.ID))
                .Returns(tm);
            fixture.MemberRepository
                .Setup(x => x.Update(tm));
            tm.FirstName = "Name 2";
            var controller = fixture.GetCompaniesController();
            var actionResult = controller.Put(1, tm.ID, tm);
            actionResult.Should().BeOfType<OkObjectResult>();

            fixture
                .MemberRepository
                .Verify(x => x.Read(tm.ID), Times.Once);
            fixture
                .MemberRepository
                .Verify(x => x.Update(tm), Times.Once);
        }

        [Fact]
        public void ShouldDeleteMember()
        {
            var fixture = new MembersControllerFixture();
            var tm = GetTeamMember();
            fixture.MemberRepository
                .Setup(x => x.Delete(tm.ID));
            fixture.MemberRepository
                .Setup(x => x.Read(tm.ID))
                .Returns(tm);

            var controller = fixture.GetCompaniesController();
            var actionResult = controller.Delete(1, tm.ID);
            actionResult.Should().BeOfType<NoContentResult>();

            fixture
                .MemberRepository
                .Verify(x => x.Delete(tm.ID), Times.Once);
        }

        private TeamMember GetTeamMember(int id = 1)
        {
            return new TeamMember
            {
                ID = id,
                FirstName = $"Agent{id}",
                LastName = $"Smith{id}",
                Title = $"Agent{id}",
                Email = $"smith{id}@matrix.org"
            };
        }

        public class MembersControllerFixture
        {
            public MembersControllerFixture()
            {
                MemberRepository = new Mock<IRepository<TeamMember>>();
            }

            public Mock<IRepository<TeamMember>> MemberRepository { get; private set; }

            public MembersController GetCompaniesController()
            {
                return new MembersController(MemberRepository.Object);
            }
        }
    }
}