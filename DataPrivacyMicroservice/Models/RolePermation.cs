//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataPrivacyMicroservice.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RolePermation
    {
        public int id { get; set; }
        public int roleID { get; set; }
        public int permationID { get; set; }
    
        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}
