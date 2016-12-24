using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNet_RssReader_Domain.Entities;
using Microsoft.AspNet.Identity;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class BaseController : Controller
    {     
        public ApplicationUser CurrentUser => _currentUser ??
                                              (_currentUser =
                                                  DbContext.Set<ApplicationUser>().Find(User.Identity.GetUserId()));

        protected readonly DbContext DbContext;
        private ApplicationUser _currentUser;

        public BaseController(DbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}