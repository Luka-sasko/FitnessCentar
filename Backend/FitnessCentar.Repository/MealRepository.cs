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

    public class MealRepository : IMealRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public async Task<string> CreateAsync(IMeal newMeal)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("INSERT INTO \"Meal\" (");
                queryBuilder.AppendLine(" \"Id\", \"Name\",");
                queryBuilder.AppendLine("    \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DatedUpdated\", \"IsActive\"");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("VALUES (");
                queryBuilder.AppendLine("    @Id, @Name,");
                queryBuilder.AppendLine("    @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive");
                queryBuilder.AppendLine(")");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@Id", newMeal.Id);
                    cmd.Parameters.AddWithValue("@Name", newMeal.Name);
                    cmd.Parameters.AddWithValue("@CreatedBy", newMeal.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedBy", newMeal.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateCreated", newMeal.DateCreated);
                    cmd.Parameters.AddWithValue("@DateUpdated", newMeal.DateUpdated);
                    cmd.Parameters.AddWithValue("@IsActive", newMeal.IsActive);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "Meal created!";
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

        public async Task<string> DeleteAsync(Guid mealId, Guid userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("UPDATE \"Meal\"");
                queryBuilder.AppendLine("SET");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {

                    queryBuilder.AppendLine("   \"IsActive\" = @IsActive,");
                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"DatedUpdated\" = @DateUpdated");
                    queryBuilder.AppendLine(" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                    cmd.Parameters.AddWithValue("@IsActive", false);
                    cmd.Parameters.AddWithValue("@UpdatedBy", userId);
                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@Id", mealId);


                    cmd.CommandText = queryBuilder.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "Meal deleted!";
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

        public async Task<PagedList<IMeal>> GetAllAsync(MealFilter filter, Sorting sorting, Paging paging)
        {
            var meal = new List<IMeal>();
            var itemCount = 0;
            if (filter != null)
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendLine("SELECT meal.* FROM \"Meal\" meal");
                    queryBuilder.AppendLine(" WHERE meal.\"IsActive\" = TRUE");

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
                                meal.Add(new Meal
                                {
                                    Id = (Guid)reader["Id"],
                                    Name = (string)reader["Name"],
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
            return new PagedList<IMeal>(meal, paging.PageNumber, paging.PageSize, itemCount);
        }

        public async  Task<IMeal> GetByIdAsync(Guid id)
        {
            IMeal meal = null;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("Select * FROM  \"Meal\"");
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
                            meal = new Meal
                            {
                                Id = (Guid)reader["Id"],
                                Name = (string)reader["Name"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DatedUpdated"],
                                IsActive = (bool)reader["IsActive"],

                            };
                        }
                    }
                }
                return meal;
            }
        }

        public async  Task<string> UpdateAsync(IMeal updatedMeal)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("UPDATE \"Meal\"");
                queryBuilder.AppendLine("SET");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    if (updatedMeal.Name != null)
                    {
                        queryBuilder.AppendLine("    \"Name\" = @Name,");
                        cmd.Parameters.AddWithValue("@Name", updatedMeal.Name);
                    }

                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"DatedUpdated\" = @DateUpdated");
                    queryBuilder.AppendLine(" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                    cmd.Parameters.AddWithValue("@UpdatedBy", updatedMeal.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateUpdated", updatedMeal.DateUpdated);
                    cmd.Parameters.AddWithValue("@Id", updatedMeal.Id);


                    cmd.CommandText = queryBuilder.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "Meal updated!";
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

        private async Task<int> GetItemCountAsync(MealFilter filter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"Meal\" meal WHERE meal.\"IsActive\" = TRUE";
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

        private void ApplyFilter(NpgsqlCommand cmd, MealFilter filter)
        {
            StringBuilder queryBuilder = new StringBuilder(cmd.CommandText);

            if (!string.IsNullOrEmpty(filter.SearchQuery))
            {
                cmd.Parameters.AddWithValue("@SearchQuery", $"%{filter.SearchQuery}%");
                queryBuilder.AppendLine(" AND meal.\"Name\" ILIKE @SearchQuery");
            }

            cmd.CommandText = queryBuilder.ToString();
        }
    }
}
