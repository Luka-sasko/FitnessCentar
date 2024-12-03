using FitnessCentar.Common;
using FitnessCentar.Model;
using FitnessCentar.Model.Common;
using FitnessCentar.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository
{
    public class WorkoutPlanExerciseRepository:IWorkoutPlanExerciseRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<PagedList<IWorkoutPlanExercise>> GetAllAsync(WorkoutPlanExerciseFilter filter, Sorting sorting, Paging paging)
        {
            var workoutPlanExercies= new List<IWorkoutPlanExercise>();
            var itemCount = 0;
            if (filter != null)
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var queryBuilder=new StringBuilder();
                    queryBuilder.AppendLine("SELECT workoutPlanExercise.* FROM \"WorkoutPlanExercise\" workoutPlanExercise");
                    queryBuilder.AppendLine(" WHERE workoutPlanExercise.\"IsActive\" = TRUE");

                    using(var cmd=new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText=queryBuilder.ToString();

                        ApplyFilter(cmd, filter);
                        ApplySorting(cmd, sorting);

                        itemCount = await GetItemCountAsync(filter);
                        ApplyPaging(cmd, paging, itemCount);

                        using(var reader=await cmd.ExecuteReaderAsync())
                        {
                            while(await reader.ReadAsync())
                            {
                                workoutPlanExercies.Add(new WorkoutPlanExercise
                                {
                                    Id=(Guid)reader["Id"],
                                    WorkoutPlanId = (Guid)reader["WorkoutPlanId"],
                                    ExerciseId=(Guid)reader["ExerciseId"],
                                    Exercisenumber = (int)reader["ExerciseNumber"],
                                    CreatedBy = (Guid)reader["CreatedBy"],
                                    UpdatedBy = (Guid)reader["UpdatedBy"],
                                    DateCreated = (DateTime)reader["DateCreated"],
                                    DatedUpdated = (DateTime)reader["DatedUpdated"],
                                    IsActive = (bool)reader["IsActive"],

                                });
                            }
                        }
                     
                    }
                }
            }
            return new PagedList<IWorkoutPlanExercise>(workoutPlanExercies, paging.PageNumber, paging.PageSize, itemCount);
           
        }

        public async Task<IWorkoutPlanExercise> GetByIdAsync(Guid id)
        {
            IWorkoutPlanExercise workoutPlanExercise = null;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("SELECT * FROM \"WorkoutPlanExercise\"");
                queryBuilder.AppendLine("WHERE \"Id\" = @Id AND \"IsActive\" = True");

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = queryBuilder.ToString();
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            workoutPlanExercise = new WorkoutPlanExercise
                            {
                                Id = (Guid)reader["Id"],
                                WorkoutPlanId = (Guid)reader["WorkoutPlanId"],
                                ExerciseId = (Guid)reader["ExerciseId"],
                                Exercisenumber = (int)reader["Exercisenumber"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                DatedUpdated = (DateTime)reader["DatedUpdated"],
                                IsActive = (bool)reader["IsActive"],
                            };

                        }

                    }
                }
                return workoutPlanExercise;

            }
        }

        public async Task<string> DeleteAsync(Guid workoutPlanExerciseId, Guid userId, DateTime time)
        {
           using(var connection=new NpgsqlConnection(_connectionString))
            {

                await connection.OpenAsync();
                var queryBuilder=new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"WorkoutPlanExercise\"");
                queryBuilder.AppendLine("SET");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"IsActive\" = @IsActive,");
                    queryBuilder.AppendLine("    \"DatedUpdated\" = @DateUpdated");
                    queryBuilder.AppendLine(" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                    cmd.Parameters.AddWithValue("@UpdatedBy", userId);
                    cmd.Parameters.AddWithValue("@DateUpdated", time);
                    cmd.Parameters.AddWithValue("@Id", workoutPlanExerciseId);
                    cmd.Parameters.AddWithValue("@IsActive", false);

                    cmd.CommandText=queryBuilder.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "WorkoutPlanExercise deleted!";
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public async Task<string> CreateAsync(IWorkoutPlanExercise newworkoutPlanExercise)
        {
            using(var connection=new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder=new StringBuilder();

                queryBuilder.AppendLine("INSERT INTO \"WorkoutPlanExercise\" (");
                queryBuilder.AppendLine(" \"Id\", \"ExerciseNumber\", \"WorkoutPlanId\", \"ExerciseId\",");
                queryBuilder.AppendLine("    \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DatedUpdated\", \"IsActive\"");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("VALUES (");
                queryBuilder.AppendLine("    @Id, @ExerciseNumber, @WorkoutPlanId, @ExerciseId,");
                queryBuilder.AppendLine("    @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive");
                queryBuilder.AppendLine(")");

                using(var cmd=new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@Id",newworkoutPlanExercise.Id);
                    cmd.Parameters.AddWithValue("@ExerciseNumber", newworkoutPlanExercise.Exercisenumber);
                    cmd.Parameters.AddWithValue("@WorkoutPlanId", newworkoutPlanExercise.WorkoutPlanId);
                    cmd.Parameters.AddWithValue("@ExerciseId", newworkoutPlanExercise.ExerciseId);
                    cmd.Parameters.AddWithValue("@CreatedBy", newworkoutPlanExercise.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedBy", newworkoutPlanExercise.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateCreated", newworkoutPlanExercise.DateCreated);
                    cmd.Parameters.AddWithValue("@DateUpdated", newworkoutPlanExercise.DatedUpdated);
                    cmd.Parameters.AddWithValue("@IsActive", newworkoutPlanExercise.IsActive);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "WorkoutPlanExercise created!";
                    }
                   catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally { connection.Close(); }
                }
            }
        }

        public async Task<string> UpdateAsync(IWorkoutPlanExercise updatedWorkoutPlanExercise)
        {
            using(var connection=new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder= new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"WorkoutPlanExercise\"");
                queryBuilder.AppendLine("SET");

                using(var cmd=new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    if(updatedWorkoutPlanExercise.WorkoutPlanId != null)
                    {
                        queryBuilder.AppendLine("   \"WorkoutPlanId\" = @WorkoutPlanId,");
                        cmd.Parameters.AddWithValue("@WorkoutPlanId", updatedWorkoutPlanExercise.WorkoutPlanId);
                    }
                    if (updatedWorkoutPlanExercise.ExerciseId != null)
                    {
                        queryBuilder.AppendLine("   \"ExerciseId\" = @ExerciseId,");
                        cmd.Parameters.AddWithValue("@ExerciseId", updatedWorkoutPlanExercise.ExerciseId);
                    }
                    if (updatedWorkoutPlanExercise.Exercisenumber > 0)
                    {
                        queryBuilder.AppendLine("   \"ExerciseNumber\" = @Exercisenumber,");
                        cmd.Parameters.AddWithValue("@Exercisenumber", updatedWorkoutPlanExercise.Exercisenumber);
                    }
                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"DatedUpdated\" = @DateUpdated");
                    queryBuilder.AppendLine(" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                    cmd.Parameters.AddWithValue("@UpdatedBy", updatedWorkoutPlanExercise.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateUpdated", updatedWorkoutPlanExercise.DatedUpdated);
                    cmd.Parameters.AddWithValue("@Id", updatedWorkoutPlanExercise.Id);

                    cmd.CommandText = queryBuilder.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "WorkoutPlanExercise updated!";
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
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


        private async Task<int> GetItemCountAsync(WorkoutPlanExerciseFilter filter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"WorkoutPlanExercise\" workoutPlanExercise WHERE workoutPlanExercise.\"IsActive\" = TRUE";
                ApplyFilter(command, filter);
                command.Connection = connection;
                try
                {
                    await connection.OpenAsync();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    return reader.GetInt32(0);
                }
                catch (Exception e)
                {
                    return 0;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }

        private void ApplySorting(NpgsqlCommand cmd, Sorting sorting)
        {
            StringBuilder commandText = new StringBuilder(cmd.CommandText);
            commandText.Append(" ORDER BY \"");
            commandText.Append(sorting.SortBy).Append("\" ");
            commandText.Append(sorting.SortOrder == "ASC" ? "ASC" : "DESC");
            cmd.CommandText = commandText.ToString();
        }

        private void ApplyFilter(NpgsqlCommand cmd, WorkoutPlanExerciseFilter filter)
        {
            StringBuilder queryBuilder = new StringBuilder(cmd.CommandText);

            if (filter.WorkoutPlanId != null)
            {
                cmd.Parameters.AddWithValue("@WorkoutPlanId", filter.WorkoutPlanId);
                queryBuilder.AppendLine(" AND workoutPlanExercise.\"WorkoutPlanId\" = @WorkoutPlanId");
            }
            if (filter.ExerciseId != null)
            {
                cmd.Parameters.AddWithValue("@ExerciseId", filter.ExerciseId);
                queryBuilder.AppendLine(" AND workoutPlanExercise.\"ExerciseId\" = @ExerciseId");
            }

            if(filter.ExerciseNumber!=0)
            {
                cmd.Parameters.AddWithValue("@ExerciseNumber", filter.ExerciseNumber);
                queryBuilder.AppendLine(" AND workoutPlanExercise.\"ExerciseNumber\" = @ExerciseNumber");
            }

            cmd.CommandText = queryBuilder.ToString();
        }

       
    }
}
