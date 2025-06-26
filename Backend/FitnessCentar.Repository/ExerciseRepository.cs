using FitnessCentar.Common;
using FitnessCentar.Model;
using FitnessCentar.Model.Common;
using FitnessCentar.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
      

        public async Task<Guid> CreateExerciseAsync(IExercise newExercise)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"INSERT INTO ""Exercises""
                    (""Id"", ""Name"", ""Desc"", ""Reps"", ""Sets"", ""RestPeriod"", ""CreatedBy"", ""UpdatedBy"", ""DateCreated"", ""DatedUpdated"", ""IsActive"", ""UserId"")
                    VALUES
                    (@Id, @Name, @Desc, @Reps, @Sets, @RestPeriod, @CreatedBy, @UpdatedBy, @DateCreated, @DatedUpdated, @IsActive, @UserId)";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", newExercise.Id);
                    cmd.Parameters.AddWithValue("@Name", newExercise.Name);
                    cmd.Parameters.AddWithValue("@Desc", newExercise.Desc);
                    cmd.Parameters.AddWithValue("@Reps", newExercise.Reps);
                    cmd.Parameters.AddWithValue("@Sets", newExercise.Sets);
                    cmd.Parameters.AddWithValue("@RestPeriod", newExercise.RestPeriod);
                    cmd.Parameters.AddWithValue("@CreatedBy", newExercise.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedBy", newExercise.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateCreated", newExercise.DateCreated);
                    cmd.Parameters.AddWithValue("@DatedUpdated", newExercise.DatedUpdated);
                    cmd.Parameters.AddWithValue("@IsActive", newExercise.IsActive);
                    cmd.Parameters.AddWithValue("@UserId", newExercise.UserId);
                    await cmd.ExecuteNonQueryAsync();
                }
                return newExercise.Id;
            }
        }

        public async Task<string> DeleteExerciseAsync(Guid exerciseId, Guid userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"UPDATE ""Exercises"" SET ""IsActive"" = FALSE, ""UpdatedBy"" = @UpdatedBy, ""DatedUpdated"" = @DatedUpdated WHERE ""Id"" = @Id";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", exerciseId);
                    cmd.Parameters.AddWithValue("@UpdatedBy", userId);
                    cmd.Parameters.AddWithValue("@DatedUpdated", DateTime.UtcNow);
                    await cmd.ExecuteNonQueryAsync();
                }

                var query2 = @"UPDATE ""WorkoutPlanExercise"" SET ""IsActive"" = FALSE, ""UpdatedBy"" = @UpdatedBy, ""DatedUpdated"" = @DatedUpdated WHERE ""ExerciseId"" = @ExerciseId";
                using (var cmd2 = new NpgsqlCommand(query2, connection))
                {
                    cmd2.Parameters.AddWithValue("@ExerciseId", exerciseId);
                    cmd2.Parameters.AddWithValue("@UpdatedBy", userId);
                    cmd2.Parameters.AddWithValue("@DatedUpdated", DateTime.UtcNow);
                    await cmd2.ExecuteNonQueryAsync();
                }

                return "Exercise and all related WorkoutPlanExercises soft deleted.";
            }
        }


        public async Task<string> UpdateExerciseAsync(IExercise updatedExercise)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"UPDATE ""Exercises"" SET
                    ""Name"" = @Name,
                    ""Desc"" = @Desc,
                    ""Reps"" = @Reps,
                    ""Sets"" = @Sets,
                    ""RestPeriod"" = @RestPeriod,
                    ""UpdatedBy"" = @UpdatedBy,
                    ""DatedUpdated"" = @DatedUpdated
                    WHERE ""Id"" = @Id";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", updatedExercise.Id);
                    cmd.Parameters.AddWithValue("@Name", updatedExercise.Name);
                    cmd.Parameters.AddWithValue("@Desc", updatedExercise.Desc);
                    cmd.Parameters.AddWithValue("@Reps", updatedExercise.Reps);
                    cmd.Parameters.AddWithValue("@Sets", updatedExercise.Sets);
                    cmd.Parameters.AddWithValue("@RestPeriod", updatedExercise.RestPeriod);
                    cmd.Parameters.AddWithValue("@UpdatedBy", updatedExercise.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DatedUpdated", updatedExercise.DatedUpdated);
                    var affected = await cmd.ExecuteNonQueryAsync();
                    return affected > 0 ? "Exercise updated." : null;
                }
            }
        }

        public async Task<PagedList<IExercise>> GetAllExercisesAsync(ExerciseFilter filter, Sorting sorting, Paging paging)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine(@"SELECT * FROM ""Exercises"" WHERE ""IsActive"" = TRUE");

                if (filter.UserId.HasValue)
                {
                    queryBuilder.AppendLine("AND (\"UserId\" = @UserId OR \"UserId\" IS NULL)");
                }
                if (!string.IsNullOrEmpty(filter.SearchQuery))
                {
                    queryBuilder.AppendLine("AND LOWER(\"Name\") LIKE @SearchQuery");
                }

                // TODO: Dodaj sortiranje i paginaciju

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    if (filter.UserId.HasValue)
                        cmd.Parameters.AddWithValue("@UserId", filter.UserId.Value);
                    if (!string.IsNullOrEmpty(filter.SearchQuery))
                        cmd.Parameters.AddWithValue("@SearchQuery", "%" + filter.SearchQuery.ToLower() + "%");

                    var reader = await cmd.ExecuteReaderAsync();
                    var exercises = new List<IExercise>();
                    while (await reader.ReadAsync())
                    {
                        exercises.Add(new Exercise
                        {
                            Id = (Guid)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Desc = reader["Desc"].ToString(),
                            Reps = (int)reader["Reps"],
                            Sets = (int)reader["Sets"],
                            RestPeriod = (int)reader["RestPeriod"],
                            CreatedBy = (Guid)reader["CreatedBy"],
                            UpdatedBy = (Guid)reader["UpdatedBy"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            DatedUpdated = (DateTime)reader["DatedUpdated"],
                            IsActive = (bool)reader["IsActive"],
                            UserId = (Guid)reader["UserId"]
                        });
                    }
                    // TODO: Implement pagination logic here
                    return new PagedList<IExercise>(exercises, 1, exercises.Count, exercises.Count);
                }
            }
        }

        public async Task<IExercise> GetExerciseById(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT * FROM ""Exercises"" WHERE ""Id"" = @Id AND ""IsActive"" = TRUE";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        return new Exercise
                        {
                            Id = (Guid)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Desc = reader["Desc"].ToString(),
                            Reps = (int)reader["Reps"],
                            Sets = (int)reader["Sets"],
                            RestPeriod = (int)reader["RestPeriod"],
                            CreatedBy = (Guid)reader["CreatedBy"],
                            UpdatedBy = (Guid)reader["UpdatedBy"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            DatedUpdated = (DateTime)reader["DatedUpdated"],
                            IsActive = (bool)reader["IsActive"],
                            UserId = (Guid)reader["UserId"]
                        };
                    }
                    return null;
                }
            }
        }
    }
}
