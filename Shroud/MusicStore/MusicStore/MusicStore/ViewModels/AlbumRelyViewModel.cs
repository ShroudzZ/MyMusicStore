using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStoreEntity;

namespace MusicStore.ViewModels
{
    public class AlbumRelyViewModel
    {
        public Album album { get; set; }
        public  List<Reply> replys { get; set; }
    }
}