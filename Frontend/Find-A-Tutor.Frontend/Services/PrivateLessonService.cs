﻿using Find_A_Tutor.Frontend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Find_A_Tutor.Frontend.Services
{
    public class PrivateLessonService : IPrivateLessonService
    {
        readonly private string UrlBasePath;
        readonly private string Route = "/privatelesson/";
        private readonly IHttpContextAccessor _accessor;

        public PrivateLessonService(IConfiguration config, IHttpContextAccessor accessor)
        {
            UrlBasePath = config.GetValue<string>("UrlBasePath");
            _accessor = accessor;
        }

        public async Task<Result<IEnumerable<PrivateLesson>>> GetAll()
        {
            var url = UrlBasePath + Route;

            using (var response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<ResultSimple<IEnumerable<PrivateLesson>>>();

                    return Result<IEnumerable<PrivateLesson>>.Ok(result.Value);
                }
                else
                {
                    var result = await response.Content.ReadAsAsync<ResultSimple<IEnumerable<PrivateLesson>>>();

                    return Result<IEnumerable<PrivateLesson>>.Error(result.Errors.ToArray());
                }
            }
        }

        public async Task<Result<PrivateLesson>> Get(Guid privateLessonId)
        {
            var url = UrlBasePath + Route + privateLessonId;

            using (var response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<ResultSimple<PrivateLesson>>();

                    return Result<PrivateLesson>.Ok(result.Value);
                }
                else
                {
                    var result = await response.Content.ReadAsAsync<ResultSimple<IEnumerable<PrivateLesson>>>();

                    return Result<PrivateLesson>.Error(result.Errors.ToArray());
                }
            }
        }

        public async Task<Result> Post(PrivateLesson privateLesson)
        {
            var url = UrlBasePath + Route;
            var token = _accessor.HttpContext.Session.GetString("token");

            ApiHelper.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using (var response = await ApiHelper.ApiClient.PostAsJsonAsync(url, privateLesson))
            {
                if (response.IsSuccessStatusCode)
                {
                    return Result.Ok();
                }
                else
                {
                    var result = await response.Content.ReadAsAsync<ResultSimple>();

                    return Result.Error(result.Errors.ToArray());
                }
            }
        }
    }
}
