using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using drs_backend_phase1.Models;
using System.Collections.Generic;
using drs_backend_phase1.Models.DTOs.SuperSlim;
using Telerik.JustMock.Helpers;
using drs_backend_phase1.Controllers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Linq;

namespace DRS_Phase1_UnitTests
{
    [TestClass]
    public class ProfileControllerUnitTest
    {
        drs_backend_phase1.Paging.IPagedList<SlimProfileDTO> fp;

        List<SlimProfileDTO> spo = new List<SlimProfileDTO>
        {
            new SlimProfileDTO
            {
                firstName = "Fred",
                lastName = "Bloggs",
                homeEmail = "me@me.com",
                id = 1,
                middleNames = "",
                isComplete = false
            },
            new SlimProfileDTO
            {
                firstName = "Fred",
                lastName = "Bloggs",
                homeEmail = "me@me.com",
                id = 1,
                middleNames = "",
                isComplete = false
            },
            new SlimProfileDTO
            {
                firstName = "Fred",
                lastName = "Bloggs",
                homeEmail = "her@me.com",
                id = 1,
                middleNames = "",
                isComplete = false
            }
        };
        //PagedList<SlimProfileDTO> f = new PagedList<SlimProfileDTO>(spo, 1, 20)
        //{

        //}

        List<Profile> fakeProf = new List<Profile>
            {
                new Profile {
                    address1 = "1 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1960,1,1),
                    firstName ="Fred",
                    lastName = "Bloggs",
                    homeEmail = "me@me.com",
                    postcode = "PO5 T00",
                    id = 1
                },
                new Profile {
                    address1 = "2 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1961,1,1),
                    firstName ="Fred",
                    lastName = "Bloggs",
                    homeEmail = "him@me.com",
                    postcode = "PO5 T01",
                    id = 2
                },
                new Profile {
                    address1 = "3 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1962,1,1),
                    firstName ="Fred",
                    lastName = "Bloggs",
                    homeEmail = "her@me.com",
                    postcode = "PO5 T02",
                    id = 3
                }
            };


        [TestMethod]
        public void GetAllProfiles_Returns_AllProfiles()
        {
            ProfileController p;
            var drsMock = Mock.Create<DRSEntities>();

            List<Profile> lp = new List<Profile>
            {
                new Profile
                {
                    address1 = "1 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1960, 1, 1),
                    firstName = "Fred",
                    lastName = "Bloggs",
                    homeEmail = "me@me.com",
                    postcode = "PO5 T00",
                    id = 1
                },
                new Profile
                {
                    address1 = "2 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1961, 1, 1),
                    firstName = "Freddy",
                    lastName = "Bloggs",
                    homeEmail = "him@me.com",
                    postcode = "PO5 T01",
                    id = 2
                },
                new Profile
                {
                    address1 = "3 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1962, 1, 1),
                    firstName = "Frederick",
                    lastName = "Bloggs",
                    homeEmail = "her@me.com",
                    postcode = "PO5 T02",
                    id = 3
                }
            };

            IQueryable<Profile> fProf = lp.AsQueryable();

            drsMock.Profiles.AddRange(fProf);


            //drsMock.Profiles.Add(
            //    new Profile
            //    {
            //        address1 = "3 Fake Street",
            //        address2 = "FakeVille",
            //        address4 = "FakeShire",
            //        dateCreated = DateTime.Now,
            //        dateOfBirth = new DateTime(1962, 1, 1),
            //        firstName = "Fred",
            //        lastName = "Bloggs",
            //        homeEmail = "her@me.com",
            //        postcode = "PO5 T02",
            //        id = 3
            //    });

            drsMock.SaveChanges();
            
            //Mock.Arrange(() => drsMock.Profiles).Returns(fakeProf);

            // Act
            p = new ProfileController(drsMock);
            var actionResult = p.FetchAllProfiles();
            var contentResult = actionResult as OkNegotiatedContentResult<SlimProfileDTO>;

            // Assert
            Assert.IsNotNull(contentResult);
            var prof = contentResult.Content;

            
        }

    }
}
