using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShop_Models.Models.VIewModel
{
    public class OrderDetailsHistoryVM
    {
        public OrderHistory SelectedOrderHistory { get; set; }
        public List<OrderDetails> detailList { get; set; }
       
    }
}
