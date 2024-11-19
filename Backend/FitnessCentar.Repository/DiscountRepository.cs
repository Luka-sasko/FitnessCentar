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


        public async Task<string> DeleteDiscountAsync(Guid discountId,Guid userId)
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

        public Task<PagedList<IDiscount>> GetAllDiscountsAsync(DiscountFilter filter, Sorting sorting, Paging paging)
        {
            throw new NotImplementedException();
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
                                Name= (string)reader["Name"],
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
    }
}
