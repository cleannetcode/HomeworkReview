﻿using LessonMonitor.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LessonMonitor.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoadMapController : ControllerBase
    {
        [HttpGet]
        public Skill[] Get()
        {
            var random = new Random();
            var skills = new List<Skill>();

            for (int i = 1; i <= 3; i++)
            {
                var skill = new Skill();

                skill.Id = i;
                skill.ParentId = 0;
                skill.SkillName = $"Skill_{i}";

                skills.Add(skill);
                for (int j = 1; j <= random.Next(3,8); j++)
                {
                    var child_skill = new Skill();
                    child_skill.Id = i*100+j;
                    child_skill.ParentId = i;
                    child_skill.SkillName = $"Skill_{i}_{j}";

                    skills.Add(child_skill);
                }
            }

            return skills.ToArray();
        }

        [HttpGet("model")]
        public RoadMapModel[] GetModel()
        {
            string selectedNamespace = "LessonMonitor.Api.Models";
            var selectedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => t.IsClass && t.Namespace == selectedNamespace)
                .ToArray();

            var result = new RoadMapModel[selectedTypes.Length];

            for (int i = 0; i < selectedTypes.Length; i++)
            {
                
                result[i] = new RoadMapModel
                {
                    ClassName = selectedTypes[i].Name,
                    Properties = selectedTypes[i].GetProperties()
                                    .Select(prop => new PropertyModel { 
                                        Name = prop.Name,
                                        Type = prop.PropertyType,
                                        Description = prop.GetCustomAttribute<DescriptionAttribute>()?.Description
                                    }).ToArray()
                };
            }

            return result;
        }
    }
}
