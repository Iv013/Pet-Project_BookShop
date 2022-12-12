using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShop_Models.Models
{
    public class OrderHistory
    {
        [Key]
        public int HistoryId { get; set; }

        public int Id { get; set; }
 
        [Required]
        public double FinalOrderTotal { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Region { get; set; }
        [Required]
        public string OrderStatus { get; set; }
        [Required]
        public DateTime DateStartState { get; set; }
        [Required]
        public DateTime DateEndState { get; set; }
  
    }
}
