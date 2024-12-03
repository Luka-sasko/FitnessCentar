using FitnessCentar.Common;
using FitnessCentar.Model;
using FitnessCentar.Model.Common;
using FitnessCentar.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository
{
    public class WorkoutPlanRepository : IWorkoutPlanRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public async Task<PagedList<IWorkoutPlan>> GetAllAsync(WorkoutPlanFilter filter, Sorting sorting, Paging paging)
        {
            var workoutPlan=new List<IWorkoutPlan>();
            var itemCount = 0;
            if(filter != null)
            {
                using(var connection=new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendLine("SELECT workoutPlan.* FROM \"WorkoutPlan\" workoutPlan");
                    queryBuilder.AppendLine(" WHERE workoutPlan.\"IsActive\" = TRUE");

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText=queryBuilder.ToString();

                        ApplyFilter(cmd,filter);
                        ApplySorting(cmd,sorting);

                        itemCount = await GetItemCountAsync(filter);
                        ApplyPaging(cmd,paging,itemCount);

                        using(var reader=await cmd.ExecuteReaderAsync())
                        {
                            while(await reader.ReadAsync())
                            {
                                workoutPlan.Add(new WorkoutPlan
                                {
                                    Id = (Guid)reader["Id"],
                                    Name = (string)reader["Name"],
                                    Desc= (string)reader["Description"],
                                    UserId= (Guid)reader["UserId"],
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
            return new PagedList<IWorkoutPlan>(workoutPlan,paging.PageNumber,paging.PageSize,itemCount);
        }

        public async Task<IWorkoutPlan> GetByIdAsync(Guid id)
        {
            IWorkoutPlan workoutPlan = null;
            using(var connection=new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("SELECT * FROM \"WorkoutPlan\"");
                queryBuilder.AppendLine("WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                using(var cmd=new NpgsqlCommand())
                {
                    cmd.CommandText=queryBuilder.ToString();
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@Id", id);

                    using(var reader=await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            workoutPlan = new WorkoutPlan
                            {
                                Id = (Guid)reader["Id"],
                                Name= (string)reader["Name"],
                                Desc= (string)reader["Description"],
                                UserId = (Guid)reader["UserId"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                DatedUpdated = (DateTime)reader["DatedUpdated"],
                                IsActive = (bool)reader["IsActive"],

                            };
                        }
                    }

                }
                return workoutPlan;
            }
        }

        public async Task<string> DeleteAsync(Guid workoutPlanId, Guid userId, DateTime time)
        {
            using(var connection=new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("UPDATE \"WorkoutPlan\"");
                queryBuilder.AppendLine("SET");
                queryBuilder.AppendLine("    \"IsActive\" = @IsActive,");
                queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                queryBuilder.AppendLine("    \"DatedUpdated\" = @DatedUpdated");
                queryBuilder.AppendLine(" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                using (var cmd=new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@IsActive", false);
                    cmd.Parameters.AddWithValue("@UpdatedBy", userId);
                    cmd.Parameters.AddWithValue("@DatedUpdated", time);
                    cmd.Parameters.AddWithValue("@Id", workoutPlanId);
                    cmd.CommandText=queryBuilder.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "WorkoutPlan deleted!";
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

        public async Task<string> CreateAsync(IWorkoutPlan newworkoutPlan)
        {
            using(var connection=new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("INSERT INTO \"WorkoutPlan\"(");
                queryBuilder.AppendLine(" \"Id\", \"Name\", \"Description\", \"UserId\",");
                queryBuilder.AppendLine("    \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DatedUpdated\", \"IsActive\"");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("VALUES (");
                queryBuilder.AppendLine("    @Id, @Name, @Description, @UserId,");
                queryBuilder.AppendLine("    @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive");
                queryBuilder.AppendLine(")");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@Id", newworkoutPlan.Id);
                    cmd.Parameters.AddWithValue("@Name", newworkoutPlan.Name);
                    cmd.Parameters.AddWithValue("@Description",newworkoutPlan.Desc);
                    cmd.Parameters.AddWithValue("@UserId", newworkoutPlan.UserId);
                    cmd.Parameters.AddWithValue("@CreatedBy", newworkoutPlan.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedBy", newworkoutPlan.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateCreated", newworkoutPlan.DateCreated);
                    cmd.Parameters.AddWithValue("@DateUpdated", newworkoutPlan.DatedUpdated);
                    cmd.Parameters.AddWithValue("@IsActive", newworkoutPlan.IsActive);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "WorkoutPlan created!";
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

        public async Task<string> UpdateAsync(IWorkoutPlan updatedWorkoutPlan)
        {
            using(var connection=new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder= new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"WorkoutPlan\"");
                queryBuilder.AppendLine("SET");

                using(var cmd=new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    if (updatedWorkoutPlan.Name != null)
                    {
                        queryBuilder.AppendLine("   \"Name\" = @Name,");
                        cmd.Parameters.AddWithValue("@Name", updatedWorkoutPlan.Name);
                    }
                    if (updatedWorkoutPlan.UserId != null)
                    {
                        queryBuilder.AppendLine("    \"UserId\" = @UserId,");
                        cmd.Parameters.AddWithValue("@UserId", updatedWorkoutPlan.UserId);
                    }
                    if(updatedWorkoutPlan.Desc != null)
                    {
                        queryBuilder.AppendLine("   \"Description\" = @Desc,");
                        cmd.Parameters.AddWithValue("@Desc", updatedWorkoutPlan.Desc);
                    }

                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"DatedUpdated\" = @DateUpdated");
                    queryBuilder.AppendLine(" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");
                    cmd.Parameters.AddWithValue("@UpdatedBy", updatedWorkoutPlan.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateUpdated", updatedWorkoutPlan.DatedUpdated);
                    cmd.Parameters.AddWithValue("@Id", updatedWorkoutPlan.Id);
                    
                    cmd.CommandText=queryBuilder.ToString();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "WorkoutPlan updated!";
                    }
                    catch(Exception ex)
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

        private void ApplyFilter(NpgsqlCommand cmd, WorkoutPlanFilter filter)
        {
            StringBuilder queryBuilder = new StringBuilder(cmd.CommandText);

            if (!string.IsNullOrEmpty(filter.SearchQuery))
            {
                cmd.Parameters.AddWithValue("@SearchQuery", $"%{filter.SearchQuery}%");
                queryBuilder.AppendLine(" AND workoutPlan.\"Name\" ILIKE @SearchQuery");
            }

            if (filter.UserId != null)
            {
                cmd.Parameters.AddWithValue("@UserId", filter.UserId);
                queryBuilder.AppendLine(" AND workoutPlan.\"UserId\" = @UserId");
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

        private async Task<int> GetItemCountAsync(WorkoutPlanFilter filter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"WorkoutPlan\" workoutPlan WHERE workoutPlan.\"IsActive\" = TRUE";
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

        
    }
}
