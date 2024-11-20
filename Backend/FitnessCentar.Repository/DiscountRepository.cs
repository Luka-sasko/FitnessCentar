using FitnessCentar.Common;
using FitnessCentar.Model;
using FitnessCentar.Model.Common;
using FitnessCentar.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository
{
    public class DiscountRepository : IDiscountRepository
    {

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<PagedList<IDiscount>> GetAllDiscountsAsync(DiscountFilter filter, Sorting sorting, Paging paging)
        {
            var discounts = new List<IDiscount>();
            var itemCount = 0;
            if (filter != null)
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendLine("SELECT discount.* FROM \"Discount\" discount");
                    queryBuilder.AppendLine(" WHERE discount.\"IsActive\" = TRUE");

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
                                discounts.Add(new Discount
                                {
                                    Id = (Guid)reader["Id"],
                                    Amount = (int)reader["Amount"],
                                    StartDate = (DateTime)reader["StartDate"],
                                    EndDate = (DateTime)reader["EndDate"],
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
            return new PagedList<IDiscount>(discounts, paging.PageNumber, paging.PageSize, itemCount);
        }

        public async Task<IDiscount> GetDiscountByIdAsync(Guid id)
        {
            IDiscount discount = null;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM \"Discount\" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            discount = new Discount
                            {
                                Id = (Guid)reader["Id"],
                                Amount = (int)reader["Amount"],
                                StartDate = (DateTime)reader["StartDate"],
                                EndDate = (DateTime)reader["EndDate"],
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
                return discount;

            }
        }

        public async Task<string> CreateDiscountAsync(IDiscount newDiscount)
        {

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("INSERT INTO \"Discount\" (");
                queryBuilder.AppendLine("    \"Id\", \"StartDate\", \"EndDate\", \"Amount\", \"Name\", ");
                queryBuilder.AppendLine("    \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DatedUpdated\", \"IsActive\"");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("VALUES (");
                queryBuilder.AppendLine("    @Id, @StartDate, @EndDate, @Amount, @Name,");
                queryBuilder.AppendLine("    @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive");
                queryBuilder.AppendLine(")");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@Id", newDiscount.Id);
                    cmd.Parameters.AddWithValue("@StartDate", newDiscount.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", newDiscount.EndDate);
                    cmd.Parameters.AddWithValue("@CreatedBy", newDiscount.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedBy", newDiscount.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateCreated", newDiscount.DateCreated);
                    cmd.Parameters.AddWithValue("@DateUpdated", newDiscount.DateUpdated);
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    cmd.Parameters.AddWithValue("@Amount", newDiscount.Amount);
                    cmd.Parameters.AddWithValue("@Name", newDiscount.Name ?? "Name");

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "Discount created!";
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


        public async Task<string> DeleteDiscountAsync(Guid discountId, Guid userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"Discount\"");
                queryBuilder.AppendLine("SET");



                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    queryBuilder.AppendLine("    \"IsActive\" = @IsActive,");
                    cmd.Parameters.AddWithValue("@IsActive", false);

                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"DatedUpdated\" = @DateUpdated");
                    queryBuilder.AppendLine(" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                    cmd.Parameters.AddWithValue("@UpdatedBy", userId);
                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@Id", discountId);

                    cmd.CommandText = queryBuilder.ToString();

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return "Discount deleted!";
        }



        public async Task<string> UpdateDiscountAsync(IDiscount discountUpdated)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"Discount\"");
                queryBuilder.AppendLine("SET");

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    if (discountUpdated.Name != null)
                    {
                        queryBuilder.AppendLine("    \"Name\" = @Name,");
                        cmd.Parameters.AddWithValue("@Name", discountUpdated.Name);
                    }
                    if (discountUpdated.Amount != null)
                    {
                        queryBuilder.AppendLine("    \"Amount\" = @Amount,");
                        cmd.Parameters.AddWithValue("@Amount", discountUpdated.Amount);
                    }
                    if (discountUpdated.StartDate != null)
                    {
                        queryBuilder.AppendLine("    \"StartDate\" = @StartDate,");
                        cmd.Parameters.AddWithValue("@StartDate", discountUpdated.StartDate);
                    }
                    if (discountUpdated.EndDate != null)
                    {
                        queryBuilder.AppendLine("    \"EndDate\" = @EndDate,");
                        cmd.Parameters.AddWithValue("@EndDate", discountUpdated.EndDate);
                    }
                    if (discountUpdated.IsActive != null)
                    {
                        queryBuilder.AppendLine("    \"IsActive\" = @IsActive,");
                        cmd.Parameters.AddWithValue("@IsActive", discountUpdated.IsActive);
                    }

                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"DatedUpdated\" = @DateUpdated");
                    queryBuilder.AppendLine(" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                    cmd.Parameters.AddWithValue("@UpdatedBy", discountUpdated.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateUpdated", discountUpdated.DateUpdated);
                    cmd.Parameters.AddWithValue("@Id", discountUpdated.Id);

                    cmd.CommandText = queryBuilder.ToString();

                    await cmd.ExecuteNonQueryAsync();

                }
            }
            return "Discount updated!";
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

        private async Task<int> GetItemCountAsync(DiscountFilter filter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"Discount\" discount WHERE discount.\"IsActive\" = TRUE";
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

        private void ApplyFilter(NpgsqlCommand cmd, DiscountFilter filter)
        {
            StringBuilder queryBuilder = new StringBuilder(cmd.CommandText);

            if (filter.StartDate != default && filter.EndDate != default)
            {
                cmd.Parameters.AddWithValue("@StartDate", filter.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", filter.EndDate);
                queryBuilder.AppendLine(" AND NOT (discount.\"StartDate\" >= @StartDate AND discount.\"EndDate\" <= @EndDate)");
            }
            else if (filter.StartDate != default)
            {
                cmd.Parameters.AddWithValue("@StartDate", filter.StartDate);
                queryBuilder.AppendLine(" AND discount.\"StartDate\" >= @StartDate");
            }
            else if (filter.EndDate != default)
            {
                cmd.Parameters.AddWithValue("@EndDate", filter.EndDate);
                queryBuilder.AppendLine(" AND discount.\"EndDate\" <= @EndDate");
            }

            if (!string.IsNullOrEmpty(filter.SearchQuery))
            {
                cmd.Parameters.AddWithValue("@SearchQuery", $"%{filter.SearchQuery}%");
                queryBuilder.AppendLine(" AND discount.\"Name\" ILIKE @SearchQuery");
            }

            if (filter.Amount != null && filter.Amount > 0 && filter.Amount <= 100)
            {
                cmd.Parameters.AddWithValue("@Amount", filter.Amount);
                queryBuilder.AppendLine(" AND discount.\"Amount\" >= @Amount");
            }

            cmd.CommandText = queryBuilder.ToString();
        }
    }
}
