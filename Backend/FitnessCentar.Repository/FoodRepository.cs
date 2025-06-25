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
    public class FoodRepository : IFoodRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<string> CreateFoodAsync(IFood newFood)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("INSERT INTO \"Food\" (");
                queryBuilder.AppendLine(" \"Id\", \"Name\", \"Weight\", \"MealId\", ");
                queryBuilder.AppendLine("    \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DatedUpdated\", \"IsActive\"");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("VALUES (");
                queryBuilder.AppendLine("    @Id, @Name, @Weight, @MealId,");
                queryBuilder.AppendLine("    @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive");
                queryBuilder.AppendLine(")");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@Id", newFood.Id);
                    cmd.Parameters.AddWithValue("@Name", newFood.Name);
                    cmd.Parameters.AddWithValue("@Weight", newFood.Weight);
                    cmd.Parameters.AddWithValue("@MealId", newFood.MealId);
                    cmd.Parameters.AddWithValue("@CreatedBy", newFood.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedBy", newFood.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateCreated", newFood.DateCreated);
                    cmd.Parameters.AddWithValue("@DateUpdated", newFood.DateUpdated);
                    cmd.Parameters.AddWithValue("@IsActive", newFood.IsActive);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "Food created!";
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

        public async Task<string> DeleteFoodAsync(Guid foodId, Guid userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("UPDATE \"Food\"");
                queryBuilder.AppendLine("SET");
                queryBuilder.AppendLine("   \"IsActive\" = @IsActive, \"DatedUpdated\" = @DateUpdated, \"UpdatedBy\" = @UpdatedBy ");
                queryBuilder.AppendLine("WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");
                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {

                    cmd.Parameters.AddWithValue("@Id", foodId);
                    cmd.Parameters.AddWithValue("@IsActive", false);
                    cmd.Parameters.AddWithValue("@UpdatedBy", userId);
                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);


                    cmd.CommandText = queryBuilder.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "Food deleted!";
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

        public async Task<PagedList<IFood>> GetAllFoodAsync(FoodFilter filter, Sorting sorting, Paging paging)
        {
            var food = new List<IFood>();
            var itemCount = 0;
            if (filter != null)
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendLine("SELECT food.* FROM \"Food\" food");
                    queryBuilder.AppendLine(" WHERE food.\"IsActive\" = TRUE");

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
                                food.Add(new Food
                                {
                                    Id = (Guid)reader["Id"],
                                    MealId = (Guid)reader["MealId"],
                                    Weight = (decimal)reader["Weight"],
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
            return new PagedList<IFood>(food, paging.PageNumber, paging.PageSize, itemCount);
        }

        public async Task<IFood> GetFoodById(Guid id)
        {
            IFood food = null;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("Select * FROM  \"Food\"");
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
                            food = new Food
                            {
                                Id = (Guid)reader["Id"],
                                Name = (string)reader["Name"],
                                Weight = (decimal)reader["Weight"],
                                MealId = (Guid)reader["MealId"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DatedUpdated"],
                                IsActive = (bool)reader["IsActive"],

                            };
                        }
                    }
                }
                return food;
            }
        }

        public async Task<string> UpdateFoodAsync(IFood updatedFood)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("UPDATE \"Food\"");
                queryBuilder.AppendLine("SET");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    if (updatedFood.Name != null)
                    {
                        queryBuilder.AppendLine("    \"Name\" = @Name,");
                        cmd.Parameters.AddWithValue("@Name", updatedFood.Name);
                    }
                    if (updatedFood.Weight != null)
                    {
                        queryBuilder.AppendLine("    \"Weight\" = @Weight,");
                        cmd.Parameters.AddWithValue("@Weight", updatedFood.Weight);
                    }
                    if (updatedFood.MealId != null)
                    {
                        queryBuilder.AppendLine("    \"MealId\" = @MealId,");
                        cmd.Parameters.AddWithValue("@MealId", updatedFood.MealId);
                    }

                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"DatedUpdated\" = @DateUpdated");
                    queryBuilder.AppendLine(" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                    cmd.Parameters.AddWithValue("@UpdatedBy", updatedFood.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateUpdated", updatedFood.DateUpdated);
                    cmd.Parameters.AddWithValue("@Id", updatedFood.Id);


                    cmd.CommandText = queryBuilder.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "Food updated!";
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

        private async Task<int> GetItemCountAsync(FoodFilter filter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"Food\" food WHERE food.\"IsActive\" = TRUE";
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

        private void ApplyFilter(NpgsqlCommand cmd, FoodFilter filter)
        {
            StringBuilder queryBuilder = new StringBuilder(cmd.CommandText);


            if (filter.MealId != default)
            {
                cmd.Parameters.AddWithValue("@MealId", filter.MealId);
                queryBuilder.AppendLine(" AND food.\"MealId\" = @MealId");
            }

            if (!string.IsNullOrEmpty(filter.SearchQuery))
            {
                cmd.Parameters.AddWithValue("@SearchQuery", $"%{filter.SearchQuery}%");
                queryBuilder.AppendLine(" AND food.\"Name\" ILIKE @SearchQuery");
            }

            cmd.CommandText = queryBuilder.ToString();
        }
    }
}
