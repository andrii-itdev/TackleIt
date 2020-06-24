using ExciteMyLife.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExciteMyLife.Models
{
    public class SuggestionValue
    {
        public Suggestion Suggestion { get; set; }

        [Key]
        public int Id { get { return Suggestion.Id; } set { Suggestion.Id = value; } }
        public string Text { get { return Suggestion.Text; } set { Suggestion.Text = value; } }
        public string Description { get { return Suggestion.Description; } set { Suggestion.Description = value; } }

        public double? Value { get; set; }
        public double ValueRounded { get { return Math.Round(Value.GetValueOrDefault(), 5); } }

        public SuggestionStatus? Status { get; set; }
    }
}