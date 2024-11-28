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
        public async Task<PagedList<IExercise>> GetAllExercisesAsync(ExerciseFilter filter, Sorting sorting, Paging paging)
        {
            var exercise = new List<IExercise>();
            var itemCount = 0;
            if (filter != null)
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendLine("SELECT exercise.* FROM \"Exercises\" exercise");
                    queryBuilder.AppendLine("WHERE exercise.\"IsActive\" = TRUE");

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = queryBuilder.ToString();

                        ApplyFilter(cmd, filter);
                        ApplySorting(cmd, sorting);

                        itemCount = await GetItemCountAsync(filter);
                        ApplyPaging(cmd, paging, itemCount);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                exercise.Add(new Exercise
                                {
                                    Id = (Guid)reader["Id"],
                                    Name = (string)reader["Name"],
                                    Desc= (string)reader["Desc"],
                                    Reps = (int)reader["Reps"],
                                    Sets= (int)reader["Sets"],
                                    RestPeriod = (int)reader["RestPeriod"],
                                    CreatedBy = (Guid)reader["CreatedBy"],
                                    UpdatedBy = (Guid)reader["UpdatedBy"],
                                    DateCreated = (DateTime)reader["DateCreated"],
                                    DatedUpdated = (DateTime)reader["DatedUpdated"],
                                    IsActive = (bool)reader["IsActive"]
                                });

                            }

                        }
                    }

                }
            }
            return new PagedList<IExercise>(exercise,paging.PageNumber,paging.PageSize, itemCount);
        }

        public async Task<IExercise> GetExerciseById(Guid id)
        {
            IExercise exercise = null;
            var connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("SELECT * FROM \"Exercises\"");
                queryBuilder.AppendLine("WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = queryBuilder.ToString();
                    cmd.Parameters.AddWithValue("@Id", id);

                    using(var reader=await cmd.ExecuteReaderAsync())
                    {
                        if(await reader.ReadAsync())
                        {
                            exercise = new Exercise
                            {
                                Id = (Guid)reader["Id"],
                                Name = (string)reader["Name"],
                                Desc= (string)reader["Desc"],
                                Reps = (int)reader["Reps"],
                                Sets = (int)reader["Sets"],
                                RestPeriod = (int)reader["RestPeriod"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                DatedUpdated = (DateTime)reader["DatedUpdated"],
                                IsActive = (bool)reader["IsActive"]
                            };
                        }
                    }
                }
                return exercise;
            }    
            
        }

        public async Task<string> DeleteExerciseAsync(Guid exerciseId, Guid userId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
               await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"Exercises\"");
                queryBuilder.AppendLine("SET");
                queryBuilder.AppendLine("   \"IsActive\" = @IsActive, \"DatedUpdated\" = @DatedUpdated, \"UpdatedBy\" = @UpdatedBy ");
                queryBuilder.AppendLine("WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                using(var cmd=new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@Id",exerciseId);
                    cmd.Parameters.AddWithValue("@IsActive", false);
                    cmd.Parameters.AddWithValue("@UpdatedBy",userId);
                    cmd.Parameters.AddWithValue("@DatedUpdated",DateTime.UtcNow);

                    cmd.CommandText=queryBuilder.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "Food deleted";
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally { connection.Close(); }
                }
            }

        }

        public async Task<int> GetItemCountAsync(ExerciseFilter filter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"Exercises\" exercise WHERE exercise.\"IsActive\" = TRUE";
                ApplyFilter(command, filter);
                command.Connection = connection;
                try
                {
                    await connection.OpenAsync();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    return reader.GetInt32(0);
                }
                catch (Exception ex)
                {
                    return 0;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }

        private void ApplyFilter(NpgsqlCommand cmd, ExerciseFilter filter)
        {
            StringBuilder queryBuilder = new StringBuilder(cmd.CommandText);

            if (!string.IsNullOrEmpty(filter.SearchQuery))
            {
                cmd.Parameters.AddWithValue("@SearchQuery", $"%{filter.SearchQuery}%");
                queryBuilder.AppendLine(" AND exercise.\"Name\" ILIKE @SearchQuery");
            }

            cmd.CommandText = queryBuilder.ToString();
        }

        private void ApplySorting(NpgsqlCommand cmd, Sorting sorting)
        {
            StringBuilder commandText = new StringBuilder(cmd.CommandText);
            commandText.Append(" ORDER BY \"");
            commandText.Append(sorting.SortBy).Append("\" ");
            commandText.Append(sorting.SortOrder == "ASC" ? "ASC" : "DESC");
            cmd.CommandText = commandText.ToString();
        }

        private void ApplyPaging(NpgsqlCommand cmd, Paging paging, int itemCount)
        {
            StringBuilder commandText = new StringBuilder(cmd.CommandText);
            int currentItem = (paging.PageNumber - 1) * paging.PageSize;
            if (currentItem >= 0 && currentItem < itemCount)
            {
                commandText.Append(" LIMIT ").Append(paging.PageSize).Append(" OFFSET ").Append(currentItem);
                cmd.CommandText = commandText.ToString();
            }
            else
            {
                commandText.Append(" LIMIT 10");
                cmd.CommandText = commandText.ToString();
            }
        }

        
    }
}
