using AutoMapper;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class MemberService : IMembersService
    {
        IMembersRepository _membersRepo; 
        private IMapper _autoMapper;

        public MemberService(IMembersRepository repo, IMapper autoMapper)
        {

            _membersRepo = repo;
            _autoMapper = autoMapper;
        }

        public void AddMember(MemberViewModel m)
        {
            Member member = new Member()
            {
                Email = m.Email,
                FirstName = m.FirstName,
                LastName = m.LastName,
                LecturerEmail = m.LecturerEmail,
                PublicKey = m.PublicKey,
                PrivateKey = m.PrivateKey
            };
            _membersRepo.AddMember(member);
        }

        public MemberViewModel GetMember(string email)
        {
            var mem = _membersRepo.GetMember(email);

            if (mem == null)
                return null;
            else
            {
                var res = _autoMapper.Map<MemberViewModel>(mem);
                return res;
            }
        }
    }
}
