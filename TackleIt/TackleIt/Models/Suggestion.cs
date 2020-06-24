using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExciteMyLife.EF.Models
{
    public class SuggestionResponse
    {
        public SuggestionResponse()
        {
        }

        public SuggestionResponse(Suggestion suggestion)
        {
            this.Id = suggestion.Id;
            this.Text = suggestion.Text;
            this.Description = suggestion.Description;
            this.Status = SuggestionStatus.Undefined;
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public SuggestionStatus Status { get; set; }

    }
    public class Suggestion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Text { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        //

        public virtual ICollection<QuestionSuggestion> Suggestions
        { get; set; } = new HashSet<QuestionSuggestion>();

        public virtual ICollection<UserSuggestion> UserSuggestions
        { get; set; } = new HashSet<UserSuggestion>();
    }
}
