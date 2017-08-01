using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using drs_backend_phase1.Models;
using drs_backend_phase1.Controllers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Linq;
using Moq;
using System.Data.Entity;
using AutoMapper;
using Profile = drs_backend_phase1.Models.Profile;
using Microsoft.CSharp;
using drs_backend_phase1.Models.DTOs;

namespace DRS_Phase1_UnitTests
{
    [TestClass]
    public class ControllerUnitTest
    {
        static List<Profile> _testProfileList;
        static IQueryable<Profile> _queryProfileList;
        static Mock<DRSEntities> _mockContext;

        [ClassInitialize]
        public static void ProfileInitialize(TestContext context)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                cfg.CreateMap<Profile, ProfileDTO>();
                cfg.CreateMap<ProfileProfessional, ProfileProfessionalDTO>();
                cfg.CreateMap<List<Profile>, List<ProfileDTO>>();
                cfg.CreateMap<SecurityRole, SecurityRoleDTO>();
                cfg.CreateMap<CCG, CCGDTO>();
                cfg.CreateMap<RegistrarLevel, RegistrarLevelDTO>();
                cfg.CreateMap<Base, BaseDTO>();
                cfg.CreateMap<Team, TeamDTO>();
                cfg.CreateMap<IndemnityProvider, IndemnityProviderDTO>();
                cfg.CreateMap<RegisteredSurgery, RegisteredSurgeryDTO>();
                cfg.CreateMap<Agency, AgencyDTO>();
                cfg.CreateMap<JobType, JobTypeDTO>();
                cfg.CreateMap<ProfileDocument, ProfileDocumentDTO>();
                cfg.CreateMap<ProfileShiftType, ProfileShiftTypeDTO>();
                cfg.CreateMap<ShiftType, ShiftTypeDTO>();
                cfg.CreateMap<SpecialNote, SpecialNoteDTO>();
                cfg.CreateMap<ProfileFinance, ProfileFinanceDTO>();
                cfg.CreateMap<ProfilePaymentCategory, ProfilePaymentCategoryDTO>();
                cfg.CreateMap<PaymentCategory, PaymentCategoryDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Bank, BankDTO>();
                cfg.CreateMap<SecurityPermission, SecurityPermissionDTO>();
                cfg.CreateMap<SecurityGroup, SecurityGroupDTO>();
                cfg.CreateMap<SecurityRight, SecurityRightDTO>();

            });

            Mapper.AssertConfigurationIsValid();



            _testProfileList = new List<Profile>
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
                    firstName = "Freda",
                    lastName = "Bloggs",
                    homeEmail = "her@me.com",
                    postcode = "PO5 T02",
                    id = 3
                },
                new Profile
                {
                    address1 = "4 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1963, 1, 1),
                    firstName = "Deleted",
                    lastName = "Bloggs",
                    homeEmail = "deleted@me.com",
                    postcode = "PO5 T02",
                    id = 4,
                    isDeleted = true
                }
            };
        }

        [TestInitialize]
        public void ProfileSetUp()
        {
            _mockContext = new Mock<DRSEntities>();
            _testProfileList[2].isDeleted = false;
            _queryProfileList = _testProfileList.AsQueryable();
        }

        #region "ProfileController"
        [TestMethod]
        public void ReturnAllProfiles_Excludes_Deleted()
        {
            var mockSet = new Mock<DbSet<Profile>>();

            mockSet.As<IQueryable<Profile>>().Setup(m => m.Provider).Returns(_queryProfileList.Provider);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(_queryProfileList.Expression);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(_queryProfileList.ElementType);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(_queryProfileList.GetEnumerator);

            _mockContext.Setup(c => c.Profiles).Returns(mockSet.Object);

            var p = new ProfileController(_mockContext.Object);

            dynamic actionResult = p.FetchAllProfiles();
            dynamic content = actionResult.Content;

            // Assert

            Assert.AreEqual(3, content.items.Count);

         }

        [TestMethod]
        public void ReturnAllProfiles_Includes_Deleted()
        {
            var mockSet = new Mock<DbSet<Profile>>();

            mockSet.As<IQueryable<Profile>>().Setup(m => m.Provider).Returns(_queryProfileList.Provider);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(_queryProfileList.Expression);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(_queryProfileList.ElementType);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(_queryProfileList.GetEnumerator);

            _mockContext.Setup(c => c.Profiles).Returns(mockSet.Object);

            var p = new ProfileController(_mockContext.Object);

            dynamic actionResult = p.FetchAllProfiles(true);
            dynamic content = actionResult.Content;

            // Assert

            Assert.AreEqual(4, content.items.Count);

        }

        [TestMethod]
        public void FetchProfileById_Returns_Correct_Profile()
        {
            var mockSet = new Mock<DbSet<Profile>>();

            mockSet.As<IQueryable<Profile>>().Setup(m => m.Provider).Returns(_queryProfileList.Provider);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(_queryProfileList.Expression);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(_queryProfileList.ElementType);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(_queryProfileList.GetEnumerator);

            _mockContext.Setup(c => c.Profiles).Returns(mockSet.Object);

            var p = new ProfileController(_mockContext.Object);

            var actionResult = p.FetchProfileById(2);
            var contentResult = actionResult as OkNegotiatedContentResult<ProfileDTO>;

            //dynamic content = actionResult.Content;

            // Assert

            Assert.IsInstanceOfType(contentResult.Content, typeof(ProfileDTO));

            Assert.AreEqual("Freddy", contentResult.Content.firstName);
            Assert.AreEqual(2, contentResult.Content.id);
        }
        [TestMethod]
        public void DeleteProfile_Removes_Profile()
        {

            var mockSet = new Mock<DbSet<Profile>>();

            mockSet.As<IQueryable<Profile>>().Setup(m => m.Provider).Returns(_queryProfileList.Provider);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(_queryProfileList.Expression);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(_queryProfileList.ElementType);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(_queryProfileList.GetEnumerator);

            _mockContext.Setup(c => c.Profiles).Returns(mockSet.Object);

            var p = new ProfileController(_mockContext.Object);

            var actionResult = p.DeleteProfileById(2);
            var contentResult = actionResult as OkNegotiatedContentResult<Boolean>;

            Assert.AreEqual(true, contentResult.Content);

            bool prof = (from fp in mockSet.Object
                       where fp.id == 2
                       select fp.isDeleted).Single();

            Assert.AreEqual(true, prof);
        }
        #endregion
        #region "EMail Controller"
        #endregion
    }    
}
