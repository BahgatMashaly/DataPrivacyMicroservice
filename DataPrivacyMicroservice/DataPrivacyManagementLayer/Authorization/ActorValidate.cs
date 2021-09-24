using DataPrivacyMicroservice.DataPrivacyManagementLayer.DataLimitation;
using DataPrivacyMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataPrivacyMicroservice.DataPrivacyManagementLayer.Authorization
{
    public class ActorValidate
    {
        PersonalDataService PersonalDataServcie = new PersonalDataService();
        public static bool Login(String userName, string password)
        {
            using (DataPrivacyEntities dataPrivacyEntities = new DataPrivacyEntities())
            {
                //  var xxxx = dataPrivacyEntities.Actors.ToList();
                return dataPrivacyEntities.Actors.Any(actor => actor.name.Equals(userName, StringComparison.OrdinalIgnoreCase) && actor.password == password);


                //var user = await userManager.FindByNameAsync(model.Username);
                //if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                //{
                //    var userRoles = await userManager.GetRolesAsync(user);

                //    var authClaims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, user.UserName),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //};

                //    foreach (var userRole in userRoles)
                //    {
                //        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                //    }

                //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                //    var token = new JwtSecurityToken(
                //        issuer: _configuration["JWT:ValidIssuer"],
                //        audience: _configuration["JWT:ValidAudience"],
                //        expires: DateTime.Now.AddHours(3),
                //        claims: authClaims,
                //        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                //       );


            }


        }


            public static ActorDetails ActorDetails(String userName, string password)
            {
                using (DataPrivacyEntities dataPrivacyEntities = new DataPrivacyEntities())
                {
                //  var xxxx = dataPrivacyEntities.Actors.ToList();
                Actor actor= dataPrivacyEntities.Actors.FirstOrDefault(_actor => _actor.name.Equals(userName, StringComparison.OrdinalIgnoreCase) && _actor.password == password);
                string[] userRoles = actor.ActorRoles.Select(ww => ww.Role.name).ToArray();
                ActorDetails actorDetails = new ActorDetails(actor, userRoles);
                return actorDetails;
                //   var user = await userManager.FindByNameAsync(model.Username);
                //if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                //{
                //    var userRoles = await userManager.GetRolesAsync(user);

                //    var authClaims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, user.UserName),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //};

                //    foreach (var userRole in userRoles)
                //    {
                //        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                //    }

                //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                //    var token = new JwtSecurityToken(
                //        issuer: _configuration["JWT:ValidIssuer"],
                //        audience: _configuration["JWT:ValidAudience"],
                //        expires: DateTime.Now.AddHours(3),
                //        claims: authClaims,
                //        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                //        );


            }






            }
    }
}