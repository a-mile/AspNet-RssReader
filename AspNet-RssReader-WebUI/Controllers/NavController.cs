﻿using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AspNet_RssReader_Domain.Abstract;
using AspNet_RssReader_Domain.Entities;
using AspNet_RssReader_WebUI.ViewModels;
using Microsoft.AspNet.Identity;

namespace AspNet_RssReader_WebUI.Controllers
{
    public class NavController : Controller
    {
        private readonly DbContext _dbContext;
        public NavController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public PartialViewResult Menu(string sourceName = null)
        {
            string userId = User.Identity.GetUserId();
            NavMenuViewModel sourcesList = new NavMenuViewModel
            {
                Sources = _dbContext.Set<Source>().Where(x=>x.ApplicationUserId == userId),
                CurrentSourceName = sourceName,
                Categories = _dbContext.Set<Category>().Where(x=>x.ApplicationUserId == userId)
            };
            return PartialView(sourcesList);
        }
    }
}