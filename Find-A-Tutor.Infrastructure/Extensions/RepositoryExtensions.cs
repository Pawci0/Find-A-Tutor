﻿using Find_A_Tutor.Core.Domain;
using Find_A_Tutor.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace Find_A_Tutor.Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<PrivateLesson> GetOrFailAsync(this IPrivateLessonRepository repository, Guid id)
        {
            var privateLesson = await repository.GetAsync(id);
            if (privateLesson == null)
            {
                throw new Exception($"Private lesson with id: '{id}' does not exist.");
            }

            return privateLesson;
        }

        public static async Task<User> GetOrFailAsync(this IUserRepository repository, Guid id)
        {
            var user = await repository.GetAsync(id);
            if (user == null)
            {
                throw new Exception($"User with id: '{id}' does not exist.");
            }

            return user;
        }
    }
}
