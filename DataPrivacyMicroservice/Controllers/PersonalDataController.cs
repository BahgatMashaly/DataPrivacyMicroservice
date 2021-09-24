
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using DataPrivacyMicroservice.Models;
 

 
using DataPrivacyMicroservice.DataPrivacyManagementLayer.DataLimitation;
using DataPrivacyMicroservice.DataPrivacyManagementLayer.Authorization;

namespace DataPrivacyMicroservice.Controllers
{
    [BasicAuthentication]
    public class PersonalDataController : ApiController
    {
        PersonalDataService personalDataService = new PersonalDataService();

        // GET: api/PersonalDatas

        [Authorized(Roles = "ReadWritePersonalData,ReadPersonalData")]
        public dynamic GetPersonalDatas()
        {
            return personalDataService.GetPersonalDatas();

        }


        // GET: api/PersonalDatas/5
        [ResponseType(typeof(PersonalData))]
        public dynamic GetPersonalData(int id)
        {
            var personalData = personalDataService.GetPersonalData(id);

            if (personalData == null)
            {
                return NotFound();
            }

            return Ok(personalData);
        }

        // PUT: api/PersonalDatas/5
        [Authorized(Roles = "WritePersonalData,ReadWritePersonalData")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPersonalData(int id, PersonalData PersonalData)
        { 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != PersonalData.id)
            {
                return BadRequest();
            }
             
            try
            {
                personalDataService.updatePersonalData(id, PersonalData);
                return Ok("The record updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            { 
                    throw; 
            }

           
        }



        // POST: api/PersonalDatas
        [Authorized(Roles = "WritePersonalData,ReadWritePersonalData")]
        [ResponseType(typeof(PersonalData))]
        public async Task<IHttpActionResult> PostPersonalData(PersonalData PersonalData)
        {
         
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            personalDataService.addPersonalData(PersonalData);

            return CreatedAtRoute("DefaultApi", new { id = PersonalData.id }, PersonalData);
        }




        // DELETE: api/PersonalDatas/5
        [Authorized(Roles = "WritePersonalData,ReadWritePersonalData")]
        [ResponseType(typeof(PersonalData))]
        public async Task<IHttpActionResult> DeletePersonalData(int id)
        {
            personalDataService.deletePersonalData(id);
            
            return Ok("The record deleted successfully.");

            
        }

    


      
    }
}