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
    public class LookupControllerUnitTests
    {
        static List<Agency> _listAgency;
        static List<Bank> _listBank;
        static List<Base> _listBase;
        static List<DocumentType> _ListDocType;
        static List<IndemnityProvider> _listIndemnity;
        static List<PaymentCategory> _listPayCat;
        static List<RegisteredSurgery> _listRegSurgery;
        static List<RegistrarLevel> _listRegLevel;
        static List<Team> _listTeam;

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

            _listAgency = new List<Agency>
            {
                new Agency
                {
                    dateCreated=new DateTime(2016,04,17),
                    dateModified = DateTime.Now,
                    id = 1,
                    name = "Agency 1"
                },
                new Agency
                {
                    dateCreated=new DateTime(2016,05,18),
                    dateModified = DateTime.Now,
                    id = 2,
                    name = "Agency 2"
                },
                new Agency
                {
                    dateCreated=new DateTime(2016,06,19),
                    dateModified = DateTime.Now,
                    id = 3,
                    name = "Agency 3"
                },
                new Agency
                {
                    dateCreated=new DateTime(2016,07,20),
                    dateModified = DateTime.Now,
                    id = 4,
                    name = "Agency 4"
                },
                new Agency
                {
                    dateCreated=new DateTime(2016,08,21),
                    dateModified = DateTime.Now,
                    id = 5,
                    name = "Agency 5"
                }
            };

            _listBank = new List<Bank>
           {
               new Bank
               {
                   dateCreated = new DateTime(2017,01,01),
                   dateModified = DateTime.Now,
                   id = 1,
                   name = "Bank 1"
               },
               new Bank
               {
                   dateCreated = new DateTime(2017,02,02),
                   dateModified = DateTime.Now,
                   id = 2,
                   name = "Bank 2"
               },
               new Bank
               {
                   dateCreated = new DateTime(2017,03,03),
                   dateModified = DateTime.Now,
                   id = 3,
                   name = "Bank 3"
               }
           };

            _listBase = new List<Base>
            {
                new Base
                {
                    dateCreated = new DateTime(2017,4,1),
                    dateModified = DateTime.Now,
                    id= 1,
                    name = "Base 1",
                    teamId = 1
                },
                new Base
                {
                    dateCreated = new DateTime(2017,4,2),
                    dateModified = DateTime.Now,
                    id= 2,
                    name = "Base 2",
                    teamId = 1
                },
                new Base
                {
                    dateCreated = new DateTime(2017,4,3),
                    dateModified = DateTime.Now,
                    id= 3,
                    name = "Base 3",
                    teamId = 2
                },
                new Base
                {
                    dateCreated = new DateTime(2017,4,4),
                    dateModified = DateTime.Now,
                    id= 4,
                    name = "Base 4",
                    teamId = 2
                }
            };

            _ListDocType = new List<DocumentType>
            {
                new DocumentType
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 1,
                    name = "Docv Type 1"
                },
                new DocumentType
                {
                    dateCreated = new DateTime(2017,2,5),
                    dateModified = DateTime.Now,
                    id= 2,
                    name = "Docv Type 2"
                },
                new DocumentType
                {
                    dateCreated = new DateTime(2017,3,5),
                    dateModified = DateTime.Now,
                    id= 3,
                    name = "Docv Type 3"
                },
                new DocumentType
                {
                    dateCreated = new DateTime(2017,4,5),
                    dateModified = DateTime.Now,
                    id= 4,
                    name = "Docv Type 4"
                }
            };

            _listIndemnity = new List<IndemnityProvider>
            {
                new IndemnityProvider
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 1,
                    name = "Indemnity Provider 1"
                },
                new IndemnityProvider
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 2,
                    name = "Indemnity Provider 2"
                },
                new IndemnityProvider
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 3,
                    name = "Indemnity Provider 3"
                },
                new IndemnityProvider
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 4,
                    name = "Indemnity Provider 4"
                }
            };

            _listPayCat = new List<PaymentCategory>
            {
                new PaymentCategory
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 1,
                    name = "Payment Category 1"
                },
                new PaymentCategory
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 2,
                    name = "Payment Category 2"
                },
                new PaymentCategory
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 3,
                    name = "Payment Category 3"
                },
                new PaymentCategory
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 4,
                    name = "Payment Category $1"
                }
            };

            _listRegLevel = new List<RegistrarLevel>
            {
                new RegistrarLevel
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id = 1,
                    name = "Registrar Level 1"
                },
                new RegistrarLevel
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 2,
                    name = "Registrar Level 2"
                },
                new RegistrarLevel
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 3,
                    name = "Registrar Level 3"
                },
                new RegistrarLevel
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 4,
                    name = "Registrar Level 4"
                }
            };

            _listRegSurgery = new List<RegisteredSurgery>
            {
                new RegisteredSurgery
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 1,
                    name = "Registrar Level 1"
                },
               new RegisteredSurgery
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 2,
                    name = "Registrar Level 2"
                },
                new RegisteredSurgery
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 3,
                    name = "Registrar Level 3"
                },
                new RegisteredSurgery
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 4,
                    name = "Registrar Level 4"
                }
            };

            _listTeam = new List<Team>
            {
                new Team
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 1,
                    name = "Team 1"
                },
                new Team
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 2,
                    name = "Team 2"
                },
                new Team
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 3,
                    name = "Team 3"
                },
                new Team
                {
                    dateCreated = new DateTime(2017,1,5),
                    dateModified = DateTime.Now,
                    id= 4,
                    name = "Team 4"
                }
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
