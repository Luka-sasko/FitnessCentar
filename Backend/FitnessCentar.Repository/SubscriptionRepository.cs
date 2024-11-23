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
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<string> CreateSubscriptionAsync(ISubscription newSubscription)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("INSERT INTO \"Subscription\" (");
                queryBuilder.AppendLine("    \"Id\", \"StartDate\", \"Description\", \"Name\", \"DiscountId\", \"Price\", \"Duration\",  ");
                queryBuilder.AppendLine("    \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DatedUpdated\", \"IsActive\"");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("VALUES (");
                queryBuilder.AppendLine("    @Id, @StartDate, @Description, @Name, @DiscountId, @Price::money, @Duration, ");
                queryBuilder.AppendLine("    @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive");
                queryBuilder.AppendLine(")");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@Id", newSubscription.Id);
                    cmd.Parameters.AddWithValue("@StartDate", newSubscription.StartDate);
                    cmd.Parameters.AddWithValue("@CreatedBy", newSubscription.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedBy", newSubscription.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateCreated", newSubscription.DateCreated);
                    cmd.Parameters.AddWithValue("@DateUpdated", newSubscription.DateUpdated);
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    cmd.Parameters.AddWithValue("@Description", newSubscription.Description);
                    cmd.Parameters.AddWithValue("@Name", newSubscription.Name ?? "Name");
                    cmd.Parameters.AddWithValue("@DiscountId", newSubscription.DiscountId);
                    cmd.Parameters.AddWithValue("@Price", newSubscription.Price);
                    cmd.Parameters.AddWithValue("@Duration", newSubscription.Duration);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "Subscription created!";
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

        public async Task<string> DeleteSubscriptionAsync(Guid id, Guid userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"Subscription\"");
                queryBuilder.AppendLine("SET");

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;


                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"DatedUpdated\" = @DateUpdated,");
                    queryBuilder.AppendLine("    \"IsActive\" = FALSE");
                    queryBuilder.AppendLine(" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                    cmd.Parameters.AddWithValue("@UpdatedBy", userId);
                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@Id", id);


                    cmd.CommandText = queryBuilder.ToString();

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return "Subscription deleted!";
        }

        public async Task<PagedList<ISubscription>> GetAllSubscriptionAsync(SubscriptionFilter filter, Sorting sorting, Paging paging)
        {
            var subscriptions = new List<ISubscription>();
            var itemCount = 0;
            if (filter != null)
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendLine("SELECT subscription.* FROM \"Subscription\" subscription");
                    queryBuilder.AppendLine(" WHERE subscription.\"IsActive\" = TRUE");

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
                                subscriptions.Add(new Subscription
                                {
                                    Id = (Guid)reader["Id"],
                                    Description = (string)reader["Description"],
                                    StartDate = (DateTime)reader["StartDate"],
                                    DiscountId = (Guid)reader["DiscountId"],
                                    Duration = (int)reader["Duration"],
                                    Name = (string)reader["Name"],
                                    CreatedBy = (Guid)reader["CreatedBy"],
                                    UpdatedBy = (Guid)reader["UpdatedBy"],
                                    DateCreated = (DateTime)reader["DateCreated"],
                                    DateUpdated = (DateTime)reader["DatedUpdated"],
                                    IsActive = (bool)reader["IsActive"],
                                    Price = (decimal)reader["Price"],

                                });
                            }
                        }
                    }
                }
            }
            return new PagedList<ISubscription>(subscriptions, paging.PageNumber, paging.PageSize, itemCount);
        }


        public async Task<ISubscription> GetSubscriptionByIdAsync(Guid id)
        {
            ISubscription subscription = null;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("Select * FROM  \"Subscription\"");
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
                            subscription = new Subscription
                            {
                                Id = (Guid)reader["Id"],
                                Description = (string)reader["Description"],
                                StartDate = (DateTime)reader["StartDate"],
                                DiscountId = (Guid)reader["DiscountId"],
                                Duration = (int)reader["Duration"],
                                Name = (string)reader["Name"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DatedUpdated"],
                                IsActive = (bool)reader["IsActive"],
                                Price = (decimal)reader["Price"],

                            };
                        }
                    }
                }
                return subscription;
            }
        }
        public async Task<string> UpdateSubscriptionAsync(ISubscription updatedSubscription)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"Subscription\"");
                queryBuilder.AppendLine("SET");

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    if (updatedSubscription.Name != null)
                    {
                        queryBuilder.AppendLine("    \"Name\" = @Name,");
                        cmd.Parameters.AddWithValue("@Name", updatedSubscription.Name);
                    }
                    if (updatedSubscription.Price != null)
                    {
                        queryBuilder.AppendLine("    \"Price\" = @Price,");
                        cmd.Parameters.AddWithValue("@Price", updatedSubscription.Price);
                    }
                    if (updatedSubscription.Duration != null)
                    {
                        queryBuilder.AppendLine("    \"Duration\" = @Duration,");
                        cmd.Parameters.AddWithValue("@Duration", updatedSubscription.Duration);
                    }
                    if (updatedSubscription.StartDate != null)
                    {
                        queryBuilder.AppendLine("    \"StartDate\" = @StartDate,");
                        cmd.Parameters.AddWithValue("@StartDate", updatedSubscription.StartDate);
                    }
                    if (updatedSubscription.Description != null)
                    {
                        queryBuilder.AppendLine("    \"Description\" = @Description,");
                        cmd.Parameters.AddWithValue("@Description", updatedSubscription.Description);
                    }if (updatedSubscription.DiscountId != null)
                    {
                        queryBuilder.AppendLine("    \"DiscountId\" = @DiscountId,");
                        cmd.Parameters.AddWithValue("@DiscountId", updatedSubscription.DiscountId);
                    }

                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"DatedUpdated\" = @DateUpdated");
                    queryBuilder.AppendLine(" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                    cmd.Parameters.AddWithValue("@UpdatedBy", updatedSubscription.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateUpdated", updatedSubscription.DateUpdated);
                    cmd.Parameters.AddWithValue("@Id", updatedSubscription.Id);


                    cmd.CommandText = queryBuilder.ToString();

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return "Subscription updated!";
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

        private async Task<int> GetItemCountAsync(SubscriptionFilter filter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"Subscription\" subscription WHERE subscription.\"IsActive\" = TRUE";
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

        private void ApplyFilter(NpgsqlCommand cmd, SubscriptionFilter filter)
        {
            StringBuilder queryBuilder = new StringBuilder(cmd.CommandText);

            
            if (filter.StartDate != default)
            {
                cmd.Parameters.AddWithValue("@StartDate", filter.StartDate);
                queryBuilder.AppendLine(" AND subscription.\"StartDate\" >= @StartDate");
            }
        
            if (!string.IsNullOrEmpty(filter.SearchQuery))
            {
                cmd.Parameters.AddWithValue("@SearchQuery", $"%{filter.SearchQuery}%");
                queryBuilder.AppendLine(" AND subscription.\"Name\" ILIKE @SearchQuery");
            }

            if (filter.Price != default && filter.Price >= 0)
            {
                cmd.Parameters.AddWithValue("@Price", filter.Price.Value);
                queryBuilder.AppendLine(" AND subscription.\"Price\"::numeric >= @Price");
            }

            if (filter.Duration != default && filter.Duration >= 0)
            {
                cmd.Parameters.AddWithValue("@Duration", filter.Duration);
                queryBuilder.AppendLine(" AND subscription.\"Duration\" >= @Duration");
            }


            cmd.CommandText = queryBuilder.ToString();
        }
    }
    
}
