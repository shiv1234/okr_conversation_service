using OkrConversationService.Domain.Common;
using System;
using Xunit;

namespace OkrConversationService.Application.Tests.Common
{
    public class ApplicationEnumsTest
    {
        [Theory]
        [InlineData(Modules.General)]
        [InlineData(Modules.Organization)]
        [InlineData(Modules.UserManagement)]
        [InlineData(Modules.RoleManagement)]
        [InlineData(Modules.CoachFeature)]
        public void TestModules(Modules number)
        {
            Assert.True((Convert.ToInt32(number) > 0));
        }

        [Theory]
        [InlineData(Permissions.CreateOkrs)]
        [InlineData(Permissions.EditOkrs)]
        [InlineData(Permissions.AssignOkr)]
        [InlineData(Permissions.AllowtoaddContributorforOkr)]
        [InlineData(Permissions.Feedbackmodule)]
        [InlineData(Permissions.OneToOneModule)]
        [InlineData(Permissions.ViewOrganizationManagementPage)]
        [InlineData(Permissions.CreateTeams)]
        [InlineData(Permissions.ViewRoleManagement)]
        [InlineData(Permissions.AddNewRole)]
        [InlineData(Permissions.DeleteUsersFrom)]
        [InlineData(Permissions.EditUsersFrom)]
        [InlineData(Permissions.AddNewUsers)]
        [InlineData(Permissions.ViewUserManagementPage)]
        [InlineData(Permissions.DeleteTeams)]
        [InlineData(Permissions.EditTeams)]
        [InlineData(Permissions.EditMainOrganization)]
        [InlineData(Permissions.AllowCreateOkrsOnBehalfOfAnotherPerson)]
        [InlineData(Permissions.DeleteRole)]
        [InlineData(Permissions.EditExistingRole)]
        public void TestPermissions(Permissions number)
        {
            Assert.True((Convert.ToInt32(number) > 0));
        }


    }
}
