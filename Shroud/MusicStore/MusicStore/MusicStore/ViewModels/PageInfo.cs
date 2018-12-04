using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.ViewModels
{
    public class PageInfo
    {
        public int Itemall { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling((decimal)Itemall / PageSize);
            }
        }
    }
}