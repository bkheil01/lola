namespace LOLAWebsite.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CourseRegistrationModel
    {

        public string Token { get; set; }

        public List<Participant> Participant { get; set; }

        public int NumberOfParticipants { get; set; }

        public Course SelectedCourse { get; set; }
    }
}