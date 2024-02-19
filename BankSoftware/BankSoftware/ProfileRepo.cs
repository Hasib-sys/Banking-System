using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankSoftware.Models;

namespace BankSoftware
{
    public class ProfileRepo
    {
        public int Add(ProfileModel model, string loggedInUser)
        {
            using (var context = new BankEntities())
            {
                Profile s = new Profile();

                s.FullName = loggedInUser;
                s.DOB = model.DOB;
                s.PhoneNo = model.PhoneNo;
                s.Address = model.Address;
                s.Occupation = model.Occupation;

                context.Profile.Add(s);
                context.SaveChanges();

                return s.Profile_ID;
            }
        }

        public List<ProfileModel> GetAllData(string loggedInUser)
        {
            using (var context = new BankEntities())
            {
                var result = context.Profile
                   .Where(x => x.FullName == loggedInUser)
                   .Select(x => new ProfileModel() 

              // var result = context.Profile.Select(x => new ProfileModel()
                {
                    Profile_ID = x.Profile_ID,
                    DOB = x.DOB,
                    PhoneNo = x.PhoneNo,
                    Address = x.Address,
                    Occupation = x.Occupation,
                    FullName = x.FullName
                }
                ).ToList();

                return result;
            }
        }
       

        /*
        public ProfileModel GetDetails(int id)
        {
            using (var context = new BankEntities())
            {
                var result = context.Profile.Where(x => x.Profile_ID == id).Select(x => new ProfileModel()
                {
                    Profile_ID = x.Profile_ID,
                    DOB = x.DOB,
                    PhoneNo = x.PhoneNo,
                    Address = x.Address,
                    Occupation = x.Occupation,
                    FullName = x.FullName
                }
                ).FirstOrDefault();

                return result;
            }
        }

 
        public bool UpdateData(int id, ProfileModel model)
        {
            using (var context = new BankEntities())
            {
                var M = context.Profile.FirstOrDefault(x => x.Profile_ID == id);
                if (M != null)
                {

                    M.DOB = model.DOB;
                    M.PhoneNo = model.PhoneNo;
                    M.Address = model.Address;
                    M.Occupation = model.Occupation;
                    M.FullName = model.FullName;

                }

                context.SaveChanges();

                return true;
            }
        }
        */

        public bool DeleteData(int id)
        {
            using (var context = new BankEntities())
            {
                var M = context.Profile.FirstOrDefault(x => x.Profile_ID == id);
                if (M != null)
                {
                    context.Profile.Remove(M);
                    context.SaveChanges();
                    return true;
                }

                return false;
            }
        }
    }
}