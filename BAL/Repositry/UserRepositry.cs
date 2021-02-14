using BAL.IRepositry;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataAccessLayer;
using System.Threading.Tasks;

namespace BAL.Repositry
{
    public class UserRepositry : GenerateRepositry<User>, IUserRepositry
    {
        private readonly TechnicalContext taskContext;

        public UserRepositry(TechnicalContext dbContext) : base(dbContext)
        {
            taskContext = dbContext;
        }

        public User FindUser(User model)
        {
            var user = taskContext.Users.FirstOrDefault(x => x.Password == model.Password && x.UserName == model.UserName&&x.active==true);

            return user;
        }
        //public List<User> getbydate(DateTime date)
        //{
        //    var x = (from y in taskContext.Users
        //             where y.Birthdate == date
        //             select y).ToList();
        //    return x;
        //}
       public User GetUserByObjectId(string ObjectName, string ObjectId)
        {
            var x = (from table in taskContext.Users
                     where table.ObjectName == ObjectName && table.ObjectId == ObjectId
                     select table).FirstOrDefault();
            return x;
        }
    }
}
