using DataPrivacyMicroservice.Models;
 using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DataPrivacyMicroservice.DataPrivacyManagementLayer.DataLimitation
{
    public class PersonalDataService
    {
        private DataPrivacyEntities db = new DataPrivacyEntities();
      private  PersonalDataLimitation personalDataLimitation = new PersonalDataLimitation();

        public dynamic GetPersonalDatas()
        {
            return personalDataLimitation.peronsalDataListRowColumnsValidation();

        }

        public dynamic GetPersonalData(int id)
        {
            return personalDataLimitation.peronsalDataObjectRowColumnsValidation(id);
         
        }

      public dynamic  peronsalDataWithRowColumnsValidation(int id)
        {
            
            return personalDataLimitation.peronsalDataObjectRowColumnsValidation(id);
        }

        public bool actorHasWriteAccessToAllFields(PersonalData PersonalData)
        {
            return personalDataLimitation.actorHasWriteAccessToAllFields(PersonalData);
        }

        public void updatePersonalData(int id , PersonalData PersonalData)
        {

            if (peronsalDataWithRowColumnsValidation(id) == null)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "No recored to update or you don't have the privilege to update this record!" };
                throw new HttpResponseException(msg);
            }


            if (actorHasWriteAccessToAllFields(PersonalData) == false)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "You don't have the privilege to update all fields of this record!" };
                throw new HttpResponseException(msg);
            }
            db.Entry(PersonalData).State = EntityState.Modified;

               db.SaveChanges();
             
        }


        public void addPersonalData(PersonalData PersonalData)
        {

            if (personalDataLimitation.actorHasWriteAccessToAllFields(PersonalData) == false)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "You don't have the privilege to update all fields of this record!" };
                throw new HttpResponseException(msg);
            }

            db.PersonalDatas.Add(PersonalData);
             db.SaveChangesAsync(); 
        }

        public void deletePersonalData(int id)
        {
            PersonalData PersonalData = personalDataLimitation.peronsalDataWithRowValidation(id);

            if (PersonalData == null)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "No recored to delete or you don't have the privilege to delete this record!" };
                throw new HttpResponseException(msg);
            }


            if (personalDataLimitation.actorHasWriteAccessToAllFields(PersonalData) == false)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "You don't have the privilege to delete all fields of this record!" };
                throw new HttpResponseException(msg);
            }
            using (var DataPrivacyEntities = new DataPrivacyEntities())
            {
                var p = DataPrivacyEntities.PersonalDatas.SingleOrDefault(x => x.id == id);
                if (p == null)
                {
                    var msg = new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "You don't have the privilege to delete all fields of this record!" };
                    throw new HttpResponseException(msg);
                }

                DataPrivacyEntities.PersonalDatas.Remove(p);
                DataPrivacyEntities.SaveChanges();
                return ;
            } 
         
        }


        private bool PersonalDataExists(int id)
        {
            if (personalDataLimitation.peronsalDataWithRowValidation(id) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
            // return db.PersonalDatas.Count(e => e.id == id) > 0;
        }


        protected   void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
           
        }

    }
}