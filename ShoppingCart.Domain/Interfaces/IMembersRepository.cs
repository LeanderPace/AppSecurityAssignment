using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IMembersRepository
    {
        Member GetMember(string email);
        void AddMember(Member m);
    }
}
