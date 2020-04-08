using ANZHFR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANZHFR.Services.Patients
{
    public class TransferHospitalServices : BaseServices
    {
        public List<TransferHospital> GetAll(string name = "")
        {
            var results = Entity.TransferHospitals.ToList();

            if (!string.IsNullOrEmpty(name))
            {
                results = results.Where(x => x.Name.Trim().ToUpper().Contains(name.Trim().ToUpper())).ToList();
            }

            return results;
        }

        public List<TransferHospital> Get(long hospitalID, string name = "")
        {
            var results = Entity.TransferHospitals.Where(x => x.HospitalID == hospitalID).ToList();

            if (!string.IsNullOrEmpty(name))
            {
                results = results.Where(x => x.Name.Trim().ToUpper().Contains(name.Trim().ToUpper())
                       || x.Name.Trim().ToUpper().Contains(name.Trim().ToUpper())).ToList();
            }

            return results;
        }


        public TransferHospital GetTransferHospitalById(long id)
        {
            return Entity.TransferHospitals.Find(id);
        }

        public TransferHospital Insert(long hospitalID, string name, string address1, string address2, string suburb, string city, string state, string postcode, string country, string phone, string adminEmail)
        {
            try
            {
                TransferHospital hospital = new ANZHFR.Data.Models.TransferHospital();

                hospital.HospitalID = hospitalID;
                hospital.Name = name;
                hospital.StreetAddress = address1;
                hospital.StreetAddress2 = address2;
                hospital.Suburb = suburb;
                hospital.City = city;
                hospital.State = state;
                hospital.PostCode = postcode;
                hospital.Country = country;
                hospital.Phone = phone;
                hospital.AdminEmail = adminEmail;

                Entity.TransferHospitals.Add(hospital);
                Entity.SaveChanges();

                return hospital;
            }
            catch
            {
                return null;
            }
        }

        public TransferHospital Update(long id, long hospitalID, string name, string address1, string address2, string suburb, string city, string postcode, string state, string country, string phone, string adminEmail)
        {
            try
            {
                TransferHospital hospital = GetTransferHospitalById(id);
                hospital.HospitalID = hospitalID;
                hospital.Name = name;
                hospital.StreetAddress = address1;
                hospital.StreetAddress2 = address2;
                hospital.Suburb = suburb;
                hospital.City = city;
                hospital.PostCode = postcode;
                hospital.State = state;
                hospital.Country = country;
                hospital.Phone = phone;
                hospital.AdminEmail = adminEmail;

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
                TransferHospital hospital = GetTransferHospitalById(id);
                Entity.TransferHospitals.Remove(hospital);
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
