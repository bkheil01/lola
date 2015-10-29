namespace LOLAWebsite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public class EventRegistrationModel
    {
        [Required]
        public string Token { get; set; }

        public List<Participant> Participant { get; set; }

        public int NumberOfParticipants { get; set; }

        public Event SelectedEvent { get; set; }
    }
}