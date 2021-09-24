using DataPrivacyMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Net.Http;
using System.Text;
using System.Net;
using System.Web.Http;

namespace DataPrivacyMicroservice.DataPrivacyManagementLayer.DataLimitation
{
    public class PersonalDataLimitation
    {
        private DataPrivacyEntities db = new DataPrivacyEntities();
        public string accessFieldsLimitation()
        {
            //   "new(PersonalDatas.id,PersonalDatas.firstName,PersonalDatas.middleName,PersonalDatas.lastName,PersonalDatas.emailAddress,PersonalDatas.dateOfBirth,PersonalDatas.mobile)"
            var fields = db.ActorRolePersonalDataFields.Join(db.PersonalDataFields,
                    arpdf => arpdf.personalDataFieldID,
                    pdf => pdf.id,
                    (arpdf, pdf) => new { ActorRolePersonalDataFields = arpdf, PersonalDataFields = pdf })
                    .Where(record => record.ActorRolePersonalDataFields.ActorRole.actorID.ToString() == HttpContext.Current.User.Identity.Name)
                    .Select("PersonalDataFields.dataFieldName").ToListAsync().Result;
            if (fields.Count <= 0)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Actor doesn't have access to view these fields!" };
                throw new HttpResponseException(msg);
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("new(");
            foreach (var field in fields)
            {
                sb.Append("PersonalDatas." + field + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(")");
            return sb.ToString();
        }


        public bool actorHasWriteAccessToAllFields(PersonalData personalData)
        {
            var fields = db.ActorRolePersonalDataFields.Join(db.PersonalDataFields,
                    arpdf => arpdf.personalDataFieldID,
                    pdf => pdf.id,
                    (arpdf, pdf) => new { ActorRolePersonalDataFields = arpdf, PersonalDataFields = pdf })
                    .Where(record => record.ActorRolePersonalDataFields.ActorRole.actorID.ToString() == HttpContext.Current.User.Identity.Name)
                    .Select("PersonalDataFields.dataFieldName").ToListAsync().Result;

            foreach (var field in fields)
            {
                if (typeof(PersonalData).GetProperties().Select(x => x.Name).ToArray().Contains(field) == false)
                {
                    return false;
                }
            }

            return true;
        }


        public PersonalData peronsalDataWithRowValidation(int id)
        {
            return db.PersonalDatas.Join(db.PersonalDataActorRoles,
            pd => pd.id,
            pdr => pdr.personalDataID,
            (pd, pdr) => new { PersonalDatas = pd, PersonalDataActorRoles = pdr })
            .Where(record => record.PersonalDataActorRoles.actorRoleID.ToString() == HttpContext.Current.User.Identity.Name
            && record.PersonalDataActorRoles.validForStartDateTime < DateTime.Now
            && record.PersonalDataActorRoles.validForEndDateTime > DateTime.Now
            && record.PersonalDatas.id == id).Distinct().FirstOrDefault().PersonalDatas;

        }

        public dynamic peronsalDataObjectRowColumnsValidation(int id)
        {
            return db.PersonalDatas.Join(db.PersonalDataActorRoles,
            pd => pd.id,
            pdr => pdr.personalDataID,
            (pd, pdr) => new { PersonalDatas = pd, PersonalDataActorRoles = pdr })
            .Where(record => record.PersonalDataActorRoles.actorRoleID.ToString() == HttpContext.Current.User.Identity.Name
            && record.PersonalDataActorRoles.validForStartDateTime < DateTime.Now
            && record.PersonalDataActorRoles.validForEndDateTime > DateTime.Now
            && record.PersonalDatas.id == id)
            .Select(accessFieldsLimitation()).Distinct();

         
        }

        public dynamic peronsalDataListRowColumnsValidation()
        {
            return db.PersonalDatas.Join(db.PersonalDataActorRoles,
               pd => pd.id,
               pdr => pdr.personalDataID,
               (pd, pdr) => new { PersonalDatas = pd, PersonalDataActorRoles = pdr })
               .Where(record => record.PersonalDataActorRoles.actorRoleID.ToString() == HttpContext.Current.User.Identity.Name
               && record.PersonalDataActorRoles.validForStartDateTime < DateTime.Now
               && record.PersonalDataActorRoles.validForEndDateTime > DateTime.Now)
               .Select(accessFieldsLimitation()).Distinct();
        }


    }
}