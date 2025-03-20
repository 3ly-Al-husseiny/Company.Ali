using AutoMapper;
using Company.Ali.DAL.Models;
using Company.Ali.PL.DTOs;

namespace Company.Ali.PL.Mapping
{
    public class EmployeeProfile : Profile
    {
        // CLR will Create The Object ---> Dependency Injection 
        public EmployeeProfile()
        {
            // CreateMap<CreateEmployeeDto, Employee>().ReverseMap(); To Enable the Reverse Mapping 
            
            CreateMap<CreateEmployeeDto, Employee>()
                .ForMember(destination => destination.Name, dto => dto.MapFrom(e => e.Name));

            CreateMap<Employee, CreateDepartmentDto>();
        }
    }
}
