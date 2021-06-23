﻿using LessonMonitor.Core.CoreModels;
using System;

namespace LessonMonitor.Core.Services
{
    public interface IHomeworksService
    {
        bool Create(Homework homework);
        Homework Get();
        bool Delete(int homeworkId);
        bool Update(Homework homework);
    }
}
