using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.ViewModels
{
    public class CartComponentViewModel
    {
        public int ItemsInCart { get; set; }
        public decimal TotalCost { get; set; }
        //if no items in cart, the cart will be disabled
        public string Disabled => (ItemsInCart == 0) ? "is-disabled" : "";
    }
}
