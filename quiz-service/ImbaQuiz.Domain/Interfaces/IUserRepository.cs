using ImbaQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Domain.Interfaces
{ 
    public interface IUserRepository : IRepository<User, string>
    { 
    }

}
