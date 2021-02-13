using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.IRepositry
{
    public interface IUserRepositry:IGenericRepositry<User>
    {
        User FindUser(User model);
        //List<User> getbydate(DateTime date);
        User GetUserByObjectId(string ObjectName, string ObjectId);
    }
}
