using ANZHFR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANZHFR.Services.Auth
{
    public class UserServices : BaseServices

    {
        public string myglobalerror;

        public bool IsValidUser(string email, string password)
        {
            try
            {
                var user = GetUserByEmail(email);

                if (user.UPassword.Trim().Equals(password))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public List<User> Get(string hospitalID, string name, string email, string state)
        {
            var results = from u in Entity.Users
                          select u;

            if (!string.IsNullOrWhiteSpace(hospitalID))
            {
                long HID = 0;
                if (long.TryParse(hospitalID, out HID) && HID > 0)
                {
                    results = results.Where(u => u.UHospitalID == HID);
                }
            }

            if (!String.IsNullOrEmpty(name))
            {
                results = results.Where(u => u.UFirstName.ToUpper().Contains(name.ToUpper()) || u.USurname.ToUpper().Contains(name.ToUpper()));
            }

            if (!String.IsNullOrEmpty(email))
            {
                results = results.Where(u => u.UEmail.ToUpper().Contains(email.ToUpper()));
            }

            if (!String.IsNullOrEmpty(state))
            {
                results = results.Where(u => u.Hospital.HState.ToUpper().Contains(state.ToUpper()));
            }

            return results.ToList();
        }

        public bool IsUserExists(string email)
        {
            try
            {
                var user = GetUserByEmail(email);

                if (user != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public User GetUserByEmail(string email)
        {
            myglobalerror = "";
            try
            {
                return Entity.Users.Where(x => x.UEmail.Trim().Equals(email.Trim())).SingleOrDefault();
            }
            catch (Exception ex)
            {
                myglobalerror = ex.Message;
                return null;
            }
        }

        public User GetUserById(long id)
        {
            return Entity.Users.Find(id);
        }

        public User Insert(string email, string firstname, string surname, string password, long hospitalId, int accessLevel, string AdminNotes, byte Active, string position)
        {
            try
            {
                var user = new ANZHFR.Data.Models.User();
                user.UEmail = email;
                user.UFirstName = firstname;
                user.USurname = surname;
                user.UPassword = password;
                user.UHospitalID = hospitalId;
                user.UAccessLevel = accessLevel;
                user.AdminNotes = AdminNotes;
                user.UActive = Active;
                user.UPosition = position;

                Entity.Users.Add(user);
                Entity.SaveChanges();

                return user;
            }
            catch
            {
                return null;
            }
        }

        public User Update(long id, string firstname, string surname, string password, long hospitalId, int accessLevel, string AdminNotes, byte Active, string position)
        {
            try
            {
                var user = GetUserById(id);
                user.UFirstName = firstname;
                user.USurname = surname;
                user.UPassword = (string.IsNullOrWhiteSpace(password) ? user.UPassword : password);
                user.UHospitalID = hospitalId;
                user.UAccessLevel = accessLevel;
                user.AdminNotes = AdminNotes;
                user.UActive = Active;
                user.UPosition = position;

                Entity.SaveChanges();

                return user;
            }
            catch
            {
                return null;
            }
        }

        public bool ChangePassword(long id, string password)
        {
            try
            {
                var user = GetUserById(id);
                user.UPassword = password;

                Entity.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public int GetUserRole(long id)
        {
            try
            {
                var user = GetUserById(id);
                return user.UAccessLevel;
            }
            catch
            {
                return 0;
            }
        }
        public bool Delete(long id)
        {
            try
            {
                var user = GetUserById(id);
                Entity.Users.Remove(user);
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