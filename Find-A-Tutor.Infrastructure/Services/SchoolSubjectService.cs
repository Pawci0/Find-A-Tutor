﻿using Find_A_Tutor.Core.Domain;
using Find_A_Tutor.Core.Repositories;
using Find_A_Tutor.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Find_A_Tutor.Infrastructure.Services
{
    public class SchoolSubjectService : ISchoolSubjectService
    {
        private readonly ISchoolSubjectRepository _schoolSubjectRepository;
        public SchoolSubjectService(ISchoolSubjectRepository schoolSubjectRepository)
        {
            _schoolSubjectRepository = schoolSubjectRepository;
        }

        public async Task<SchoolSubject> GetAsync(Guid id)
        {
            return await _schoolSubjectRepository.GetAsync(id);
        }

        public async Task<SchoolSubject> GetAsync(string name)
        {
            return await _schoolSubjectRepository.GetAsync(name);
        }
        public async Task<IEnumerable<SchoolSubject>> BrowseAsync(string name = "")
        {
            return await _schoolSubjectRepository.BrowseAsync(name);
        }

        public async Task CreateAsync(Guid id, string name)
        {
            var schoolSubject = await _schoolSubjectRepository.GetAsync(name);
            if (schoolSubject != null)
            {
                throw new Exception($"School subject already exists.");
            }

            schoolSubject = new SchoolSubject(id, name);
            await _schoolSubjectRepository.AddAsync(schoolSubject);
        }

        public async Task UpdateAsync(Guid id, string name)
        {
            var schoolSubjectById = await _schoolSubjectRepository.GetOrFailAsync(id); //czy jest o takim id
            //teraz sprawdzmy czy nazwa się nie zdubluje
            var schoolSubjectByName = await _schoolSubjectRepository.GetAsync(name);
            if (schoolSubjectByName != null)
            {
                throw new Exception("School subject with that name already exists.");
            }

            schoolSubjectById.SetName(name);

            await _schoolSubjectRepository.UpdateAsync(schoolSubjectById);
        }

        public async Task DeleteAsync(Guid id)
        {
            var schoolSubject = await _schoolSubjectRepository.GetOrFailAsync(id);
            await _schoolSubjectRepository.DeleteAsync(schoolSubject);
        }
    }
}
