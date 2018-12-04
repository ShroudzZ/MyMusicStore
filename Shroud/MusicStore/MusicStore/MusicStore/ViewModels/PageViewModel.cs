using MusicStoreEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.ViewModels
{
    public class PageViewModel
    {
        public IEnumerable<Album> Items{get;set;}
        public PageInfo PageInfo { get; set; }
    }
}