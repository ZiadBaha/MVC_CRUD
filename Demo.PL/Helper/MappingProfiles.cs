using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            CreateMap<DepartmentViewModel,Department>().ReverseMap();
            CreateMap<ApplicationUser,UserViewModel>().ReverseMap();
            CreateMap<IdentityRole, RoleViewModel>()
                .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Name))
                .ReverseMap();

        }
    }
}
