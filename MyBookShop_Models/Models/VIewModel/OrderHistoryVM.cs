using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShop_Models.Models.VIewModel
{
    public class OrderHistoryVM
    {
        public IEnumerable<OrderHistory> OrderHistory { get; set; }
        public IEnumerable<SelectListItem> statusList { get; set; }
        public string Status { get; set; }
    }
}
