using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using drs_backend_phase1.Models;
using drs_backend_phase1.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using Profile = drs_backend_phase1.Models.Profile;
using drs_backend_phase1.Models.DTOs;
using Moq;
using System.Data.Entity;
using System.Web.Http.Results;

namespace DRS_Phase1_UnitTests
{
    [TestClass]
    public class JobTypeControllerUnitTests
    {
        static List<JobType> _testJobTypeList;
        static Mock<DRSEntities> _mockContext;
        static IQueryable<JobType> _queryJobTypeList;

        [ClassInitialize]
        public static void JobTypeInitialize(TestContext context)
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

            _testJobTypeList = new List<JobType>
            {
                new JobType
                {
                    id = 1,
                    name = "Doctor",
                    isClinical = true,
                    isGmcRequired = true,
                    isNmcRequired = false,
                    isHcpcRequired = false,
                    dateCreated = new DateTime(2017,4,11),
                    dateModified = DateTime.Now
                },
                new JobType
                {
                    id = 2,
                    name = "ANP",
                    isClinical = true,
                    isGmcRequired = false,
                    isNmcRequired = true,
                    isHcpcRequired = false,
                    dateCreated = new DateTime(2017,4,12),
                    dateModified = DateTime.Now
                },
                new JobType
                {
                    id = 3,
                    name = "UCP (Paramedic)",
                    isClinical = true,
                    isGmcRequired = false,
                    isNmcRequired = false,
                    isHcpcRequired = true,
                    dateCreated = new DateTime(2017,4,11),
                    dateModified = DateTime.Now
                },
                new JobType
                {
                    id = 4,
                    name = "UCP",
                    isClinical = true,
                    isGmcRequired = false,
                    isNmcRequired = true,
                    isHcpcRequired = false,
                    dateCreated = new DateTime(2017,4,11),
                    dateModified = DateTime.Now
                },
                new JobType
                {
                    id = 5,
                    name = "PP",
                    isClinical = true,
                    isGmcRequired = false,
                    isNmcRequired = false,
                    isHcpcRequired = false,
                    dateCreated = new DateTime(2017,4,11),
                    dateModified = DateTime.Now
                },
                new JobType
                {
                    id = 6,
                    name = "Driver/receptionist",
                    isClinical = false,
                    isGmcRequired = false,
                    isNmcRequired = false,
                    isHcpcRequired = false,
                    dateCreated = new DateTime(2017,4,11),
                    dateModified = DateTime.Now
                },
                new JobType
                {
                    id = 7,
                    name = "Receptionist",
                    isClinical = false,
                    isGmcRequired = false,
                    isNmcRequired = false,
                    isHcpcRequired = false,
                    dateCreated = new DateTime(2017,4,12),
                    dateModified = DateTime.Now
                },
                new JobType
                {
                    id = 8,
                    name = "Registrar",
                    isClinical = true,
                    isGmcRequired = true,
                    isNmcRequired = false,
                    isHcpcRequired = false,
                    dateCreated = new DateTime(2017,4,12),
                    dateModified = DateTime.Now
                },
                new JobType
                {
                    id = 9,
                    name = "Driver",
                    isClinical = false,
                    isGmcRequired = false,
                    isNmcRequired = false,
                    isHcpcRequired = false,
                    dateCreated = new DateTime(2017,4,12),
                    dateModified = DateTime.Now
                }
            };
        }
        [TestInitialize]
        public void JobTypeTestsSetup()
        {
            _mockContext = new Mock<DRSEntities>();
            _queryJobTypeList = _testJobTypeList.AsQueryable();
        }
        [TestMethod]
        public void FetchAllJobTypes_Returns_Jobtypes()
        {
            //   Arrange
            SetUpJobTypeMock();

            var p = new JobTypeController(_mockContext.Object);

            //  Act
            //var actionResult = p.GetJobTypesOData();
            //var contentResult = actionResult as OkNegotiatedContentResult<List<JobType>>;

            dynamic actionResult = p.FetchAllJobTypes();
            dynamic content = actionResult.Content;

            // Assert
            Assert.AreEqual(9, content.items.Count);

        }
        [TestMethod]
        public void FetchJobTypeById_Returns_Expected_JobType()
        {
            //   Arrange
            SetUpJobTypeMock();

            var p = new JobTypeController(_mockContext.Object);

            //  Act
            var actionResult = p.FetchJobTypeById(4);
            var contentResult = actionResult as OkNegotiatedContentResult<JobTypeDTO>;

            //dynamic actionResult = p.FetchJobTypeById(4);
            //dynamic content = actionResult.Content;

            // Assert
            Assert.AreEqual("UCP", contentResult.Content.name);
        }
        #region "Test Helpers"
        private Mock<DbSet<JobType>> SetUpJobTypeMock()
        {
            var mockSet = new Mock<DbSet<JobType>>();

            mockSet.As<IQueryable<JobType>>().Setup(m => m.Provider).Returns(_queryJobTypeList.Provider);
            mockSet.As<IQueryable<JobType>>().Setup(m => m.Expression).Returns(_queryJobTypeList.Expression);
            mockSet.As<IQueryable<JobType>>().Setup(m => m.ElementType).Returns(_queryJobTypeList.ElementType);
            mockSet.As<IQueryable<JobType>>().Setup(m => m.GetEnumerator()).Returns(_queryJobTypeList.GetEnumerator);

            _mockContext.Setup(c => c.JobTypes).Returns(mockSet.Object);

            return mockSet;
        }
        #endregion  

    }
}
