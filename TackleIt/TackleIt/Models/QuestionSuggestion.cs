using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExciteMyLife.EF.Models
{
    public class QuestionSuggestion
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Suggestion")]
        public int SuggestionId { get; set; }

        //

        public virtual Question Question { get; set; }
        public virtual Suggestion Suggestion { get; set; }
    }
}
