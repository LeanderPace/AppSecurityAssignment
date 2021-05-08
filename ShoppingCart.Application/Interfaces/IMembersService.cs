using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IMembersService
    {
        MemberViewModel GetMember(string email);
        void AddMember(MemberViewModel m);
    }
}
