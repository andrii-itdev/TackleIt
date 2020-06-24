using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExciteMyLife.Models;

namespace ExciteMyLife.EF.Models
{
    public enum SuggestionStatus
    {
        Undefined = 0,
        Completed = 1,
        NotToday = 2,
        NeverAsk = 3
    }
    public class UserSuggestion
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        [Key, Column(Order = 1)]
        public int SuggestionId { get; set; }

        public SuggestionStatus Status { get; set; }
        
        public DateTime DTAdded { get; set; } = DateTime.Now;
        //

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("SuggestionId")]
        public virtual Suggestion Suggestion { get; set; }
    }
}
