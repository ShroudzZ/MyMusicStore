using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStoreEntity;

namespace MusicStore.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartList { get; set; }
        public decimal AllPrice { get; set; }
        
    }
}