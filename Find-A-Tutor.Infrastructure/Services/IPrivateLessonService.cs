﻿using Find_A_Tutor.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Find_A_Tutor.Infrastructure.DTO.PrivateLessonDTO;

namespace Find_A_Tutor.Infrastructure.Services
{
    public interface IPrivateLessonService
    {
        Task<PrivateLessonDTO> GetAsync(Guid id);
        Task<PrivateLessonDTO> GetAsyncBySubject(SchoolSubjectDTO name);
        Task<IEnumerable<PrivateLessonDTO>> GetForUserAsync(Guid userId);
        Task<IEnumerable<PrivateLessonDTO>> BrowseAsync(string description = "");
        Task CreateAsync(Guid id, Guid studnetId, DateTime relevantTo, string description, SchoolSubjectDTO subject);
        Task UpdateAsync(Guid id, string description);
        Task DeleteAsync(Guid id);
        Task AssignTutor(Guid id, Guid tutorId);
        Task RemoveAssignedTutor(Guid id);
    }
}
