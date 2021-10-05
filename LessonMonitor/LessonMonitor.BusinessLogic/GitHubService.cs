﻿using LessonMonitor.DataAccess;
using Octokit;
using System.Net.Http.Headers;

namespace LessonMonitor.BusinessLogic
{
    public class GitHubService : IGitHubService
    {
        public GitHubUser GetUserByLogin(string login)
        {
            var github = new GitHubClient(new Octokit.ProductHeaderValue("MyApp"));
            var user = github.User.Get(login).Result;

            GitHubUser gitHubUser = new GitHubUser() {
                Login = user.Login,
                Name = user.Name,
                CreatedAt = user?.CreatedAt.DateTime,
                Url = user.Url
            };

            return gitHubUser;
        }
    }
}
