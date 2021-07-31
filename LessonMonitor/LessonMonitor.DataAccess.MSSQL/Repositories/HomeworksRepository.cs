using LessonMonitor.Core;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LessonMonitor.Core.Repositories;

namespace LessonMonitor.DataAccess.MSSQL.Repositories
{
    public class HomeworksRepository : IHomeworksRepository
    {
        private readonly LessonMonitorDbContext _context;

        public HomeworksRepository(LessonMonitorDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(Homework newHomework)
        {
            if (newHomework is null)
                throw new ArgumentNullException(nameof(newHomework));

            var newHomeworkEntity = new Entities.Homework
            {
                Title = newHomework.Title,
                Description = newHomework.Description,
                Link = newHomework.Link
            };

            await _context.Homeworks.AddAsync(newHomeworkEntity);
            await _context.SaveChangesAsync();

            return newHomeworkEntity.LessonId;
        }

        public async Task Delete(int homeworkId)
        {
            var command = _context.Homeworks
                .FromSqlRaw("DELETE FROM Homeworks WHERE Id = @homeworkId", homeworkId)
                .CreateDbCommand();

            //_context.Homeworks.Remove(new Entities.Homework { Id = homeworkId });
            await command.ExecuteNonQueryAsync();
        }
    }
}
