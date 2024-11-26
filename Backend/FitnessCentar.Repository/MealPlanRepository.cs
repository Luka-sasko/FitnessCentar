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
    public class MealPlanRepository : IMealPlanRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<string> CreateAsync(IMealPlan newMealPlan)
        {

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("INSERT INTO \"MealPlan\" (");
                queryBuilder.AppendLine(" \"Id\", \"Name\", \"UserId\",");
                queryBuilder.AppendLine("    \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DatedUpdated\", \"IsActive\"");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("VALUES (");
                queryBuilder.AppendLine("    @Id, @Name, @UserId,");
                queryBuilder.AppendLine("    @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive");
                queryBuilder.AppendLine(")");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@Id", newMealPlan.Id);
                    cmd.Parameters.AddWithValue("@UserId", newMealPlan.UserId);
                    cmd.Parameters.AddWithValue("@Name", newMealPlan.Name);
                    cmd.Parameters.AddWithValue("@CreatedBy", newMealPlan.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedBy", newMealPlan.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateCreated", newMealPlan.DateCreated);
                    cmd.Parameters.AddWithValue("@DateUpdated", newMealPlan.DateUpdated);
                    cmd.Parameters.AddWithValue("@IsActive", newMealPlan.IsActive);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "MealPlan created!";
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

        public async Task<string> DeleteAsync(Guid mealPlanId, Guid userId, DateTime time)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("UPDATE \"MealPlan\"");
                queryBuilder.AppendLine("SET");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    queryBuilder.AppendLine("    \"IsActive\" = @IsActive,");
                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"DatedUpdated\" = @DateUpdated");
                    queryBuilder.AppendLine(" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                    cmd.Parameters.AddWithValue("@IsActive", false);
                    cmd.Parameters.AddWithValue("@UpdatedBy", userId);
                    cmd.Parameters.AddWithValue("@DateUpdated", time);
                    cmd.Parameters.AddWithValue("@Id",mealPlanId);


                    cmd.CommandText = queryBuilder.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "MealPlan deleted!";
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

        public async Task<PagedList<IMealPlan>> GetAllAsync(MealPlanFilter filter, Sorting sorting, Paging paging)
        {
            var mealPlan = new List<IMealPlan>();
            var itemCount = 0;
            if (filter != null)
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendLine("SELECT mealPlan.* FROM \"MealPlan\" mealPlan");
                    queryBuilder.AppendLine(" WHERE mealPlan.\"IsActive\" = TRUE");

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
                                mealPlan.Add(new MealPlan
                                {
                                    Id = (Guid)reader["Id"],
                                    Name = (string)reader["Name"],
                                    UserId = (Guid)reader["UserId"],
                                    CreatedBy = (Guid)reader["CreatedBy"],
                                    UpdatedBy = (Guid)reader["UpdatedBy"],
                                    DateCreated = (DateTime)reader["DateCreated"],
                                    DateUpdated = (DateTime)reader["DatedUpdated"],
                                    IsActive = (bool)reader["IsActive"],

                                });
                            }
                        }
                    }
                }
            }
            return new PagedList<IMealPlan>(mealPlan, paging.PageNumber, paging.PageSize, itemCount);
        }

        public async Task<IMealPlan> GetByIdAsync(Guid id)
        {
            IMealPlan mealPlan = null;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("Select * FROM  \"MealPlan\"");
                queryBuilder.AppendLine("WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");


                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = queryBuilder.ToString();
                    cmd.Parameters.AddWithValue("@Id", id);


                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            mealPlan = new MealPlan
                            {
                                Id = (Guid)reader["Id"],
                                Name = (string)reader["Name"],
                                UserId = (Guid)reader["UserId"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DatedUpdated"],
                                IsActive = (bool)reader["IsActive"],

                            };
                        }
                    }
                }
                return mealPlan;
            }
        }

        public async Task<string> UpdateAsync(IMealPlan updatedMealPlan)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("UPDATE \"MealPlan\"");
                queryBuilder.AppendLine("SET");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    if (updatedMealPlan.Name != null)
                    {
                        queryBuilder.AppendLine("    \"Name\" = @Name,");
                        cmd.Parameters.AddWithValue("@Name", updatedMealPlan.Name);
                    }
                    
                    if (updatedMealPlan.UserId != null)
                    {
                        queryBuilder.AppendLine("    \"UserId\" = @UserId,");
                        cmd.Parameters.AddWithValue("@UserId", updatedMealPlan.UserId);
                    }

                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"DatedUpdated\" = @DateUpdated");
                    queryBuilder.AppendLine(" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                    cmd.Parameters.AddWithValue("@UpdatedBy", updatedMealPlan.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateUpdated", updatedMealPlan.DateUpdated);
                    cmd.Parameters.AddWithValue("@Id", updatedMealPlan.Id);


                    cmd.CommandText = queryBuilder.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "MealPlan updated!";
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

        private async Task<int> GetItemCountAsync(MealPlanFilter filter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"MealPlan\" mealPlan WHERE mealPlan.\"IsActive\" = TRUE";
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

        private void ApplyFilter(NpgsqlCommand cmd, MealPlanFilter filter)
        {
            StringBuilder queryBuilder = new StringBuilder(cmd.CommandText);

            if (!string.IsNullOrEmpty(filter.SearchQuery))
            {
                cmd.Parameters.AddWithValue("@SearchQuery", $"%{filter.SearchQuery}%");
                queryBuilder.AppendLine(" AND mealPlan.\"Name\" ILIKE @SearchQuery");
            }
            
            if (filter.UserId != null)
            {
                cmd.Parameters.AddWithValue("@UserId", filter.UserId);
                queryBuilder.AppendLine(" AND mealPlan.\"UserId\" = @UserId");
            }

            cmd.CommandText = queryBuilder.ToString();
        }
    }
}
