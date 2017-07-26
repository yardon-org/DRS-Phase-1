using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using drs_backend_phase1.Models;
using drs_backend_phase1.Models.DTOs;
using log4net.Config;
using Profile = drs_backend_phase1.Models.Profile;

namespace drs_backend_phase1
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.HttpApplication" />
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Applications the start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            Mapper.Initialize(cfg =>
                {
                    cfg.CreateMissingTypeMaps = true;
                    cfg.CreateMap<Profile, ProfileDTO>();
                    cfg.CreateMap<ProfileProfessional, ProfileProfessionalDTO>();
                    cfg.CreateMap<SubType, SubTypeDTO>();
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

                }
            );

            Mapper.AssertConfigurationIsValid();
        }

        /// <summary>
        /// Begin request
        /// </summary>
        protected void Application_BeginRequest()
        {

            // Options added for Ajax stuff
            if (Request.HttpMethod == "OPTIONS")
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                Response.AppendHeader("Access-Control-Allow-Origin", "*");
                Response.AddHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, Authorization");
                Response.AddHeader("Access-Control-Allow-Methods", "GET,PUT,POST,DELETE,PATCH,OPTIONS");
                Response.AppendHeader("Access-Control-Allow-Credentials", "true");

                Response.End();
            }
        }
    }

   


}
