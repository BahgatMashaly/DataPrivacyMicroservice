using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace DataPrivacyMicroservice.Models
{
    public partial class DataPrivacyEntities : DbContext
    {

    


    public override int SaveChanges()
        {

            //// Get all Added/Deleted/Modified entities (not Unmodified or Detached)
            //foreach (var ent in this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
            //{
            //    // For each changed record, get the audit record entries and add them
            //    foreach (DataPrivacyMicroservice.Models.AuditLog x in GetAuditRecordsForChange(ent, HttpContext.Current.User.Identity.Name ))
            //    {
            //        this.logs.Add(x);
            //    }
            //}

            // Call the original SaveChanges(), which will save both the changes made and the audit records
              return base.SaveChanges();
        }

        private List<AuditLog> GetAuditRecordsForChange(DbEntityEntry dbEntry, string userId)
        {
            List<DataPrivacyMicroservice.Models.AuditLog> result = new List<DataPrivacyMicroservice.Models.AuditLog>();
            if(dbEntry.Entity.GetType().Name!= "PersonalData")
            {
                return result;
            }
            DateTime changeTime = DateTime.Now;

            // Get the Table() attribute, if one exists
            TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;

            // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
            string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

            // Get primary key value (If you have more than one key column, this will need to be adjusted)
            string keyName ="id";

            if (dbEntry.State == EntityState.Added)
            {
                // For Inserts, just add the whole record
                // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()
                result.Add(new  AuditLog()
                {
                     
                    eventType = "Added", // Added
                    tableName = tableName,
                    recordID = dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),  // Again, adjust this if you have a multi-column key
                    columnName = "*ALL",    // Or make it nullable, whatever you want
                    newValue =   dbEntry.CurrentValues.ToObject().ToString(),
                    createdBy = HttpContext.Current.User.Identity.Name,
                    createdDate = changeTime
                }
                    );
            }
            else if (dbEntry.State == EntityState.Deleted)
            {
                // Same with deletes, do the whole record, and use either the description from Describe() or ToString()
                result.Add(new AuditLog()
                {
                     
                    eventType = "Deleted", // Deleted
                    tableName = tableName,
                    recordID = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                    columnName = "*ALL",
                    newValue =  dbEntry.OriginalValues.ToObject().ToString(),
                    createdBy = HttpContext.Current.User.Identity.Name,
                    createdDate = changeTime
                }
                    );
            }
            else if (dbEntry.State == EntityState.Modified)
            {
                foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                {
            
                    // For updates, we only want to capture the columns that actually changed
                    if (!object.Equals(dbEntry.GetDatabaseValues().GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                    {
                        result.Add(new AuditLog()
                        { 
                            eventType = "Modified",    // Modified
                            tableName = tableName,
                            recordID = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                            columnName = propertyName,
                            originalValue = dbEntry.GetDatabaseValues().GetValue<object>(propertyName) == null ? null : dbEntry.GetDatabaseValues().GetValue<object>(propertyName).ToString(),
                            newValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString(),
                            createdBy = HttpContext.Current.User.Identity.Name,
                            createdDate = changeTime
                            
                        }
                            );
                    }
                }
            }
            // Otherwise, don't do anything, we don't care about Unchanged or Detached entities

            return result;
        }

        public DbSet<AuditLog> logs { get; set; }
    }
}