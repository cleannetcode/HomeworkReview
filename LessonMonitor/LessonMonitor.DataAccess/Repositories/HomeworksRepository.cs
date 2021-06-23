﻿using System;
using LessonMonitor.Core.CoreModels;
using LessonMonitor.Core.Helper;
using LessonMonitor.Core.Repositories;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LessonMonitor.DataAccess.Repositories
{
    public class HomeworksRepository : IHomeworksRepository
	{
		private string _connectionString;
		public HomeworksRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public int Add(Homework newHomework)
		{
			if (newHomework is null)
				throw new ArgumentNullException(nameof(newHomework));

			var newHomeworkEntity = new Entities.Homework
			{
				TopicId = newHomework.TopicId,
				Name = newHomework.Name,
				Link = newHomework.Link,
				Grade = newHomework.Grade
			};

			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				var command = new SqlCommand(@"
					INSERT INTO Homeworks (TopicId, Name, Link, Grade, CreatedDate, UpdatedDate)
					VALUES (@TopicId, @Name, @Link, @Grade, @CreatedDate, @UpdatedDate);
					SET @Id = scope_identity();",
				connection);

				command.Parameters.AddWithValue("@TopicId", newHomeworkEntity.TopicId);
				command.Parameters.AddWithValue("@Name", newHomeworkEntity.Name);
				command.Parameters.AddWithValue("@Link", newHomeworkEntity.Link);
				command.Parameters.AddWithValue("@Grade", newHomeworkEntity.Grade);
				command.Parameters.AddWithValue("@CreatedDate", newHomeworkEntity.CreatedDate);
				command.Parameters.AddWithValue("@UpdatedDate", newHomeworkEntity.UpdatedDate);

				var resultParameter = new SqlParameter
				{
					Direction = System.Data.ParameterDirection.Output,
					SqlDbType = System.Data.SqlDbType.Int,
					ParameterName = "@Id"
				};

				command.Parameters.Add(resultParameter);

				command.ExecuteNonQuery();

				if (command.Parameters["@Id"].Value is int homeworkId)
				{
					return homeworkId;
				}
				else
				{
					throw new InvalidOperationException($"value id cannot be converted: {command.Parameters["@Id"].Value}");
				}
			}
		}

		public void Delete(int homeworkId)
		{
			if (homeworkId <= 0)
				throw new ArgumentException(nameof(homeworkId));

			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				var command = new SqlCommand(@"
					UPDATE Homeworks
					SET DeletedDate = @DeletedDate
					WHERE Id = @Id",
				connection);

				command.Parameters.AddWithValue("@Id", homeworkId);
				command.Parameters.AddWithValue("@DeletedDate", DateTime.Now);

				command.ExecuteNonQuery();
			}
		}
		public Homework Get(int homeworkId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				var command = new SqlCommand(@"
						SELECT 
						h.Id,
						h.TopicId,
						h.Name,
						h.Link, 
						h.Grade,
						t.Id as ""Topic.Id"",
						t.Theme as ""Topic.Theme"",
						u.Id as ""User.Id"",
						u.Name as ""User.Name"",
						u.Nicknames as ""User.Nicknames"",
						u.Email as ""User.Email""
						FROM Homeworks h
						INNER JOIN UsersHomeworks uh on h.Id = uh.HomeworkId
						INNER JOIN Users u on uh.UserId = u.Id
						INNER JOIN Topics t on h.TopicId = t.Id
						WHERE h.DeletedDate IS NULL AND h.Id = @Id",
				connection);

				command.Parameters.AddWithValue("@Id", homeworkId);

				var reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						return ModelMapper.CreateOf<Homework>(reader);
					}
				}
			}

			return null;
		}

		public Homework[] Get()
		{
			var homeworks = new List<Homework>();

			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				var command = new SqlCommand(@"
						SELECT 
						h.Id,
						h.TopicId,
						h.Name,
						h.Link, 
						h.Grade,
						t.Id as ""Topic.Id"",
						t.Theme as ""Topic.Theme"",
						u.Id as ""User.Id"",
						u.Name as ""User.Name"",
						u.Nicknames as ""User.Nicknames"",
						u.Email as ""User.Email""
						FROM Homeworks h
						INNER JOIN UsersHomeworks uh on h.Id = uh.HomeworkId
						INNER JOIN Users u on uh.UserId = u.Id
						INNER JOIN Topics t on h.TopicId = t.Id
						WHERE h.DeletedDate IS NULL",
				connection);

				var reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						homeworks.Add(ModelMapper.CreateOf<Homework>(reader));
					}
				}
			}

			return homeworks.ToArray();
		}


		public void Update(Homework homework)
		{
			if (homework is null)
				throw new ArgumentNullException(nameof(homework));

			var updatedHomeworkEntity = new Entities.Homework
			{
				Id = homework.Id,
				TopicId = homework.TopicId,
				Name = homework.Name,
				Link = homework.Link,
				Grade = homework.Grade
			};

			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				var command = new SqlCommand(@"
					UPDATE Homeworks
					SET 
						TopicId = @TopicId,
						Name = @Name, 
						Link = @Link,
						Grade = @Grade, 
						UpdatedDate = @UpdatedDate
					WHERE Id = @Id",
				connection);

                command.Parameters.AddWithValue("@Id", updatedHomeworkEntity.Id);
                command.Parameters.AddWithValue("@TopicId", updatedHomeworkEntity.TopicId);
                command.Parameters.AddWithValue("@Name", updatedHomeworkEntity.Name);
                command.Parameters.AddWithValue("@Link", updatedHomeworkEntity.Link);
                command.Parameters.AddWithValue("@Grade", updatedHomeworkEntity.Grade);
                command.Parameters.AddWithValue("@UpdatedDate", updatedHomeworkEntity.UpdatedDate);

                command.ExecuteNonQuery();
			}
		}

		public void CleanTable()
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				var command = new SqlCommand(@"
					DELETE FROM Homeworks
					WHERE Id NOT IN (SELECT TOP 10 Id FROM Homeworks)",
				connection);

				command.ExecuteNonQuery();
			}
		}
	}
}
