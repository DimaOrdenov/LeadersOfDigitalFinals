using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel.Responses;

namespace LodFinals.Api.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Exercise, ExerciseResponse>();
            CreateMap<Syllabus, SyllabusResponse>();
        }
    }
}
