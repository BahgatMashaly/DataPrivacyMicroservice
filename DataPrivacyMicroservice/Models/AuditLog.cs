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
    
    public partial class AuditLog
    {
        public int id { get; set; }
        public string eventType { get; set; }
        public string tableName { get; set; }
        public string recordID { get; set; }
        public string columnName { get; set; }
        public string originalValue { get; set; }
        public string newValue { get; set; }
        public string createdBy { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
    }
}
