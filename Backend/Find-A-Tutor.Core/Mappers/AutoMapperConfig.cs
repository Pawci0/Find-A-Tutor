﻿using AutoMapper;
using Find_A_Tutor.Core.Domain;
using Find_A_Tutor.Core.DTO;

namespace Find_A_Tutor.Core.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrivateLesson, PrivateLessonDTO>()
                    .ForMember(x => x.Subject, m => m.MapFrom(p => p.SchoolSubjectId));
                cfg.CreateMap<User, AccountDto>();
            })
            .CreateMapper();
    }
}
