﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LessonMonitor.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        //public UsersController()
        //{

        //}

        [HttpGet]
        public User[] Get(string userName)
        {
            var rand = new Random();
            var users = new List<User>();
            for (int i = 0; i < 11; i++)
            {
                var user = new User();
                user.Age = rand.Next(1, 101);
                user.Name = userName + i;
                users.Add(user);
            }
            return users.ToArray();
        }

        [HttpGet("model")]
        public void GetModel([FromQuery] User user)
        {
            var model = user.GetType();

            foreach (var property in model.GetProperties())
            {
                foreach (var customAttribute in property.CustomAttributes)
                {
                    if (customAttribute.AttributeType.Name == "RequiredAttribute")
                    {
                        var value = property.GetValue(user);
                        var specifiedValue = Convert.ChangeType(value, property.PropertyType);

                        if (value is DateTime dateValue && dateValue == default(DateTime))
                        {
                            throw new Exception($"{property.Name}: {value}");
                        }
                        if (value is int intValue && intValue == default(int))
                        {
                            throw new Exception($"{property.Name}: {value}");
                        }
                        if (value == null)
                        {
                            throw new Exception($"{property.Name}: {value}");
                        }
                    }
                }
                var rangeAttribute = property.GetCustomAttribute<RangeAttribute>();
                if (rangeAttribute != null)
                {
                    var value = property.GetValue(user);

                    var isValueInRange = value is int intValue &&
                                         intValue >= rangeAttribute.MinValue &&
                                         intValue <= rangeAttribute.MaxValue;
                    if (!isValueInRange)
                    {
                        throw new Exception($"{property.Name}: {value} - not in Range ({rangeAttribute.MinValue} {rangeAttribute.MaxValue})");
                    }
                }
            }

        }
    }
}
