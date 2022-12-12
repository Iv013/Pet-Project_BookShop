using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShop_Models.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }
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
        public string StreetAddress { get; set; }
        [Required]
        public string OrderStatus { get; set; }

        [Required]
        public DateTime DateStartState { get; set; }
        [Required]
        public DateTime DateEndState { get; set; }
        public bool active_state { get; set; }
    }
}
