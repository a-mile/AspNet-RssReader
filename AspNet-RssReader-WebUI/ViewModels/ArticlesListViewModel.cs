﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet_RssReader_WebUI.ViewModels
{
    public class ArticlesListViewModel
    {
        public IEnumerable<ArticleViewModel> ArticleViewModels { get; set; }
    }
}