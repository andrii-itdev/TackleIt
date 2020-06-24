using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ExciteMyLife.EF.Models
{
    public enum QuestionType
    {
        PercentQuestion = 1,
        YesNoQuestion = 2
    }
    public class Question
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Text { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public QuestionType Type { get; set; }
        // Percent Question
        // YesNo Question


        //

        public virtual ICollection<QuestionSuggestion> Suggestions
            { get; set; } = new HashSet<QuestionSuggestion>();

        public virtual ICollection<UserQuestion> UserQuestions
            { get; set; } = new HashSet<UserQuestion>();
    }

    public class InquiryModel
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

        public QuestionType Type { get; set; }
        public double Answer { get; set; }
    }
}
