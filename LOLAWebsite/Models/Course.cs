//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LOLAWebsite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Course
    {
        public Course()
        {
            this.Course_Feedback = new HashSet<Course_Feedback>();
            this.Course_Registration = new HashSet<Course_Registration>();
        }

        public int Course_ID { get; set; }

        [Display(Name = "Name")]
        public string Course_Type { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Course_Desc { get; set; }

        [Display(Name = "Teacher")]
        public Nullable<int> Teacher_ID { get; set; }

        [Display(Name = "Max Students")]
        public Nullable<int> Course_Max_Size { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Course_Start_Date { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Course_End_Date { get; set; }

        [Display(Name = "Start Time")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public Nullable<System.TimeSpan> Course_Time_Start { get; set; }

        [Display(Name = "End Time")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public Nullable<System.TimeSpan> Course_Time_End { get; set; }

        [Display(Name = "Cost")]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true)]
        public Nullable<float> Course_Cost { get; set; }

        [Display(Name = "Location")]
        public string Course_Location { get; set; }

        [Display(Name = "Special Notes")]
        [DataType(DataType.MultilineText)]
        public string Course_Notes { get; set; }


        public Nullable<int> Participating_Students { get; set; }

        public virtual ICollection<Course_Feedback> Course_Feedback { get; set; }
        public virtual ICollection<Course_Registration> Course_Registration { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
