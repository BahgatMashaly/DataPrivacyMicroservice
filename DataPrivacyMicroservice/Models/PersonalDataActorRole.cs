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
    
    public partial class PersonalDataActorRole
    {
        public int personalDataID { get; set; }
        public int actorRoleID { get; set; }
        public Nullable<System.DateTime> validForStartDateTime { get; set; }
        public Nullable<System.DateTime> validForEndDateTime { get; set; }
    
        public virtual ActorRole ActorRole { get; set; }
        public virtual PersonalData PersonalData { get; set; }
    }
}
