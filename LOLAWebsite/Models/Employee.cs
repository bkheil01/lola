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
    
    public partial class Employee
    {
        public int Employee_ID { get; set; }
        public string Id { get; set; }
        public Nullable<int> SSN { get; set; }
        public string Job_Description { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
