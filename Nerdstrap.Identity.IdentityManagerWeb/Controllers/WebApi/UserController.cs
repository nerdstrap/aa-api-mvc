using AutoMapper;
using log4net;
using System;
using System.Web.Http;

namespace Nerdstrap.Identity.IdentityManagerWeb.Controllers.WebApi
{
    [Authorize]
    [RoutePrefix("api/user")]
    public partial class UserController : BaseWebApiController, IUserController
    {
        private static ILog _logger;
        public ILog Logger
        {
            get { return _logger ?? (_logger = LogManager.GetLogger(typeof(UserController))); }
            set { _logger = value; }
        }

        private ILog _auditLogger;
        public ILog AuditLogger
        {
            get { return _auditLogger ?? (_auditLogger = LogManager.GetLogger("AuditLogger")); }
            set { _auditLogger = value; }
        }

        //private BusinessLogic.Interfaces.IUserManagementBusinessLogic UserManagementBusinessLogic;
        private IMapper Mapper;

        //public UserController(BusinessLogic.Interfaces.IUserManagementBusinessLogic userManagementBusinessLogic, IMapper mapper)
        public UserController(IMapper mapper)
        {
            //if (userManagementBusinessLogic == null)
            //{
            //    throw new ArgumentNullException(nameof(userManagementBusinessLogic));
            //}
            //UserManagementBusinessLogic = userManagementBusinessLogic;

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            Mapper = mapper;
        }
    }
}
