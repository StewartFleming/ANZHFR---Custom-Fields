using ANZHFR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANZHFR.Services.Patients
{
    public class HospitalServices : BaseServices
    {
        public List<Hospital> GetAll(string name = "")
        {
            var results = from h in Entity.Hospitals
                          select h;

            if (!string.IsNullOrEmpty(name))
            {
                results = results.Where(h => h.HName.Contains(name));
            }

            var hospitals = results.OrderBy(h => h.HName).ToList();

            return hospitals;
        }

        public Hospital GetHospitalById(long id)
        {
            return Entity.Hospitals.Find(id);
        }

        public String GetHospitalNameById(long id)
        {
            return Entity.Hospitals.Find(id).HName;
        }

        public Hospital GetHospitalByName(string hospitalName)
        {
            hospitalName = hospitalName.Replace("-", "").Replace(" ", "");

            return (from h in Entity.Hospitals
                    where (h.HName.Replace(" ", "") == hospitalName)
                    select h).FirstOrDefault();
        }

        public Hospital Insert(string name, string address1, string address2, string suburb, string city, string state, string postcode, string country, string phone, string adminEmail)
        {
            try
            {
                Hospital hospital = new ANZHFR.Data.Models.Hospital();

                hospital.HName = name;
                hospital.HStreetAddress1 = address1;
                hospital.HStreetAddress2 = address2;
                hospital.HSuburb = suburb;
                hospital.HCity = city;
                hospital.HState = state;
                hospital.HPostCode = postcode;
                hospital.HCountry = country;
                hospital.HPhone = phone;
                hospital.HAdminEmail = adminEmail;

                Entity.Hospitals.Add(hospital);
                Entity.SaveChanges();

                return hospital;
            }
            catch
            {
                return null;
            }
        }

        public Hospital Update(long id, string name, string address1, string address2, string suburb, string city, string state, string postcode, string country, string phone, string adminEmail)
        {
            try
            {
                Hospital hospital = GetHospitalById(id);
                hospital.HName = name;
                hospital.HStreetAddress1 = address1;
                hospital.HStreetAddress2 = address2;
                hospital.HSuburb = suburb;
                hospital.HCity = city;
                hospital.HState = state;
                hospital.HPostCode = postcode;
                hospital.HCountry = country;
                hospital.HPhone = phone;
                hospital.HAdminEmail = adminEmail;

                Entity.SaveChanges();

                return hospital;
            }
            catch
            {
                return null;
            }
        }

        public bool Delete(long id)
        {
            try
            {
                Hospital hospital = GetHospitalById(id);
                Entity.Hospitals.Remove(hospital);
                Entity.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}