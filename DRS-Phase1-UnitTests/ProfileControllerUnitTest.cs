using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using drs_backend_phase1.Models;
using drs_backend_phase1.Controllers;
using System.Web.Http.Results;
using System.Linq;
using Moq;
using System.Data.Entity;
using AutoMapper;
using Profile = drs_backend_phase1.Models.Profile;
using drs_backend_phase1.Models.DTOs;

namespace DRS_Phase1_UnitTests
{
    [TestClass]
    public class ControllerUnitTest
    {
        static List<ProfileProfessional> _testProfessional;
        static IQueryable<ProfileProfessional> _queryProfessionalList;
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

            _testProfessional = new List<ProfileProfessional>
            {
                new ProfileProfessional
                {
                    //  0
                    agencyId = 1,
                    dateCreated = DateTime.Now,
                    gmcNumber="G12345678",
                    ccgId=1,
                    id=1,
                    baseId=1,
                    indemnityExpiryDate = new DateTime(2018,4,13),
                    registeredSurgeryId = 1,
                    teamId = 1,
                    registrarLevelId= 1,
                    nmcNumber = "nmc001",
                    dateModified = DateTime.Now,
                    isDeleted=false,
                    hcpcNumber = "hcpc001",
                    jobTypeId = 1
                },
                new ProfileProfessional
                {
                    //  1
                    agencyId = 1,
                    dateCreated = DateTime.Now,
                    gmcNumber="G12345679",
                    ccgId=1,
                    id=2,
                    baseId=1,
                    indemnityExpiryDate = new DateTime(2018,4,14),
                    registeredSurgeryId = 2,
                    teamId = 1,
                    registrarLevelId= 1,
                    nmcNumber = "nmc002",
                    dateModified = DateTime.Now,
                    isDeleted=false,
                    hcpcNumber = "hcpc002",
                    jobTypeId = 1
                },
                new ProfileProfessional
                {
                    //  2
                    agencyId = 1,
                    dateCreated = DateTime.Now,
                    gmcNumber="G12345680",
                    ccgId=1,
                    id=3,
                    baseId=1,
                    indemnityExpiryDate = new DateTime(2018,4,14),
                    registeredSurgeryId = 2,
                    teamId = 1,
                    registrarLevelId= 1,
                    nmcNumber = "nmc003",
                    dateModified = DateTime.Now,
                    isDeleted=false,
                    hcpcNumber = "hcpc003",
                    jobTypeId = 1
                },
                new ProfileProfessional
                {
                    //  3
                    agencyId = 2,
                    dateCreated = DateTime.Now,
                    gmcNumber="G12345681",
                    ccgId=1,
                    id=4,
                    baseId=1,
                    indemnityExpiryDate = new DateTime(2018,4,14),
                    registeredSurgeryId = 3,
                    teamId = 2,
                    registrarLevelId= 1,
                    nmcNumber = "nmc004",
                    dateModified = DateTime.Now,
                    isDeleted=false,
                    hcpcNumber = "hcpc004",
                    jobTypeId = 2
                },
                new ProfileProfessional
                {
                    //  4
                    agencyId = 2,
                    dateCreated = DateTime.Now,
                    gmcNumber="G12345682",
                    ccgId=1,
                    id=5,
                    baseId=1,
                    indemnityExpiryDate = new DateTime(2018,4,14),
                    registeredSurgeryId = 2,
                    teamId = 3,
                    registrarLevelId= 2,
                    nmcNumber = "nmc005",
                    dateModified = DateTime.Now,
                    isDeleted=false,
                    hcpcNumber = "hcpc005",
                    jobTypeId = 3
                }
            };

            _testProfileList = new List<Profile>
            {
                new Profile
                {
                    //  0
                    address1 = "1 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1960, 1, 1),
                    firstName = "Fred",
                    lastName = "Bloggs",
                    middleNames = "",
                    homeEmail = "me@me.com",
                    postcode = "PO5 T00",
                    id = 1,
                    ProfileProfessional = _testProfessional[0]
                },
                new Profile
                {
                    //  1
                    address1 = "2 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1961, 1, 1),
                    firstName = "Freddy",
                    lastName = "Bloggs",
                    middleNames = "Percy",
                    homeEmail = "him@me.com",
                    postcode = "PO5 T01",
                    id = 2,
                    ProfileProfessional = _testProfessional[1]
                },
                new Profile
                {
                    //  2
                    address1 = "3 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1962, 1, 1),
                    firstName = "Freda",
                    lastName = "Bloggs",
                    middleNames = "",
                    homeEmail = "her@me.com",
                    postcode = "PO5 T02",
                    id = 3,
                    ProfileProfessional = _testProfessional[2]
                },
                new Profile
                {
                    //  3
                    address1 = "4 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1963, 1, 1),
                    firstName = "Deleted",
                    lastName = "Bloggs",
                    middleNames = "",
                    homeEmail = "deleted@me.com",
                    postcode = "PO5 T0",
                    id = 4,
                    isDeleted = true,
                    ProfileProfessional = _testProfessional[3]
                },
                new Profile
                {
                    //  4
                    address1 = "2 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1964, 1, 1),
                    firstName = "Fred",
                    lastName = "Baker",
                    middleNames = "Percy",
                    homeEmail = "baker@me.com",
                    postcode = "PO5 T04",
                    id = 5,
                    ProfileProfessional = _testProfessional[4]
                }
            };
        }

        [TestInitialize]
        public void ProfileSetUp()
        {
            _mockContext = new Mock<DRSEntities>();
            //  The following line is necessary to reset the deleted flag if the "DeleteProfile_Sets_Deleted_Flag_on_Profile" is run before others
            _testProfileList[1].isDeleted = false;

            _queryProfileList = _testProfileList.AsQueryable();
            _queryProfessionalList = _testProfessional.AsQueryable();
        }

        #region "ProfileController"
        [TestMethod]
        public void ReturnAllProfiles_Excludes_Deleted()
        {
            //   Arrange
            SetUpProfileMock();

            var p = new ProfileController(_mockContext.Object);

            //  Act
            dynamic actionResult = p.FetchAllProfiles();
            dynamic content = actionResult.Content;

            // Assert
            Assert.AreEqual(4, content.items.Count);

         }

        [TestMethod]
        public void ReturnAllProfiles_Includes_Deleted()
        {
            //   Arrange
            SetUpProfileMock();

            var p = new ProfileController(_mockContext.Object);

            //  Act
            dynamic actionResult = p.FetchAllProfiles(true);
            dynamic content = actionResult.Content;

            // Assert
            Assert.AreEqual(5, content.items.Count);

        }

        [TestMethod]
        public void FetchProfileById_Returns_Correct_Profile()
        {
            //   Arrange
            SetUpProfileMock();

            var p = new ProfileController(_mockContext.Object);

            //  Act
            var actionResult = p.FetchProfileById(2);
            var contentResult = actionResult as OkNegotiatedContentResult<ProfileDTO>;

            // Assert
            Assert.IsInstanceOfType(contentResult.Content, typeof(ProfileDTO));

            Assert.AreEqual("Freddy", contentResult.Content.firstName);
            Assert.AreEqual(2, contentResult.Content.id);
        }
        [TestMethod]
        public void DeleteProfile_Sets_Deleted_Flag_on_Profile()
        {
            //  Arrange
            var mockSet = SetUpProfileMock();

            var p = new ProfileController(_mockContext.Object);

            //  Act
            var actionResult = p.DeleteProfileById(2);
            var contentResult = actionResult as OkNegotiatedContentResult<Boolean>;

            //  Assert
            Assert.AreEqual(true, contentResult.Content);

            bool prof = (from fp in mockSet.Object
                           where fp.id == 2
                           select fp.isDeleted).Single();

            Assert.AreEqual(true, prof);
        }

        [TestMethod]
        public void FetchManyByTeamId_Returns_Many()
        {
            //  Arrange
            SetUpProfileMock();

            var p = new ProfileController(_mockContext.Object);

            //  Act
            dynamic actionResult = p.FetchManyByTeamId(1);
            dynamic content = actionResult.Content;

            // Assert
            Assert.AreEqual(3, content.items.Count);

        }

        [TestMethod]
        public void FetchManyByTeamId__Does_Not_Return_Deleted()
        {
            //  Arrange

            SetUpProfileMock();

            var p = new ProfileController(_mockContext.Object);

            //  Act
            dynamic actionResult = p.FetchManyByTeamId(2);
            dynamic content = actionResult.Content;

            // Assert
            Assert.AreEqual(0, content.items.Count);
        }

        [TestMethod]
        public void SearchProfiles_Returns_All_Profiles_With_SpecifiedFisrtname()
        {
            //  Arrange

            SetUpProfileMock();

            var p = new ProfileController(_mockContext.Object);

            //  Act
            dynamic actionResult = p.SearchProfiles("Fred");
            dynamic content = actionResult.Content;

            // Assert
            Assert.AreEqual(4, content.items.Count);
        }
        [TestMethod]
        public void SearchProfiles_Returns_All_Profiles_With_SpecifiedLastname()
        {
            //  Arrange

            SetUpProfileMock();

            var p = new ProfileController(_mockContext.Object);

            //  Act
            dynamic actionResult = p.SearchProfiles("Bloggs");
            dynamic content = actionResult.Content;

            // Assert
            Assert.AreEqual(3, content.items.Count);
        }
        [TestMethod]
        public void SearchProfiles_Returns_All_Profiles_With_SpecifiedLastname_Including_Deleted()
        {
            //  Arrange

            SetUpProfileMock();

            var p = new ProfileController(_mockContext.Object);

            //  Act
            dynamic actionResult = p.SearchProfiles("Bloggs", true);
            dynamic content = actionResult.Content;

            // Assert
            Assert.AreEqual(4, content.items.Count);
        }
        [TestMethod]
        public void SearchProfiles_Returns_All_Profiles_With_SpecifiedMiddlename()
        {
            //  Arrange

            SetUpProfileMock();

            var p = new ProfileController(_mockContext.Object);

            //  Act
            dynamic actionResult = p.SearchProfiles("Percy");
            dynamic content = actionResult.Content;

            // Assert
            Assert.AreEqual(2, content.items.Count);
        }
        [TestMethod]
        public void UpdateProfile_Updates_and_Returns_Update()
        {
            ProfileDTO testDTO = new ProfileDTO
            {
                address1 = "New Address Line 1",
                address2 = "New Address Line 2",
                dateOfBirth = new DateTime(1970, 11, 11),
                id = 2,
                firstName = "Johnny",
                lastName = "Depp",
                homeEmail = "test@test.com",
                isDeleted = false,
                postcode = "NR0 0OO",
                profileProfessionalId = 1
            };


            SetUpProfileMock();

            var p = new ProfileController(_mockContext.Object);

            //  Act
            dynamic actionResult = p.UpdateProfile(testDTO);
            dynamic content = actionResult.Content;

            // Assert
            Assert.AreEqual(2, content.items.Count);

        }
        #endregion
        #region "EMail Controller"
        #endregion

        #region "Test Helpers"
        private Mock<DbSet<Profile>> SetUpProfileMock()
        {
            var mockSet = new Mock<DbSet<Profile>>();

            mockSet.As<IQueryable<Profile>>().Setup(m => m.Provider).Returns(_queryProfileList.Provider);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(_queryProfileList.Expression);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(_queryProfileList.ElementType);
            mockSet.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(_queryProfileList.GetEnumerator);

            _mockContext.Setup(c => c.Profiles).Returns(mockSet.Object);

            return mockSet;
        }
        #endregion  
    }    
}
