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
    
    public partial class Event_Registration
    {
        public int Event_Registration_ID { get; set; }
        public Nullable<int> User_Code { get; set; }
        public Nullable<int> Event_ID { get; set; }
        public Nullable<System.DateTime> Event_Date { get; set; }
    
        public virtual Event Event { get; set; }
    }
}
