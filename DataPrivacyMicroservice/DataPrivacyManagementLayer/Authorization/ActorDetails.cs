using DataPrivacyMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataPrivacyMicroservice.DataPrivacyManagementLayer.Authorization
{
    public class ActorDetails
    {

        public ActorDetails(Actor _actorx, string[] _actorRolsx)
        {
            actor = _actorx;
            actorRols = _actorRolsx;
        }
        
          private Actor _actor=new Actor();

        public Actor actor
        {
            get { return _actor; }
            set { _actor = value; }
        }




        private string[] _actorRols;
        public string[] actorRols
        {
            get { return _actorRols; }
            set { _actorRols = value; }
        }


    }
}