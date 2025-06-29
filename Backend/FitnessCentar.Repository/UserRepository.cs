using FitnessCentar.Model;
using FitnessCentar.Model.Common;
using FitnessCentar.Repository.Common;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<bool> CreateAsync(IUser user)
        {
            int rowChanged;
            byte[] salt = GenerateSalt();
            string hashedPassword = HashPassword(user.Password, salt);

            NpgsqlConnection connection=new NpgsqlConnection(_connectionString);

            using (connection)
            {
                string insertQuery= "INSERT INTO \"User\" (\"Id\", \"Firstname\", \"Lastname\", \"Email\", \"Password\", \"Salt\", \"Contact\",\"Birthdate\",\"Weight\",\"Height\",\"CoachId\",\"SubscriptionId\", \"RoleId\", \"CreatedBy\", \"UpdatedBy\", \"IsActive\") " +
                    "VALUES (@Id, @Firstname, @Lastname, @Email, @Password, @Salt, @Contact, @Birthdate, @Weight, @Height, @CoachId, @SubscriptionId, (SELECT \"Id\" FROM \"Role\" WHERE \"RoleName\" = 'User'), @CreatedBy, @UpdatedBy, @IsActive)";

                NpgsqlCommand insertCommand= new NpgsqlCommand(insertQuery, connection);

                insertCommand.Parameters.Add("@Id", NpgsqlDbType.Uuid).Value = user.Id;
                insertCommand.Parameters.Add("@Firstname", NpgsqlDbType.Text).Value = user.Firstname;
                insertCommand.Parameters.Add("@Lastname", NpgsqlDbType.Text).Value = user.Lastname;
                insertCommand.Parameters.Add("@Email", NpgsqlDbType.Text).Value = user.Email;
                insertCommand.Parameters.Add("@Password", NpgsqlDbType.Text).Value = hashedPassword;
                insertCommand.Parameters.Add("@Salt", NpgsqlDbType.Text).Value = Convert.ToBase64String(salt);
                insertCommand.Parameters.Add("@CreatedBy", NpgsqlDbType.Uuid).Value = user.CreatedBy;
                insertCommand.Parameters.Add("@UpdatedBy", NpgsqlDbType.Uuid).Value = user.UpdatedBy;
                insertCommand.Parameters.Add("@IsActive", NpgsqlDbType.Boolean).Value = user.IsActive;
                insertCommand.Parameters.Add("@Contact", NpgsqlDbType.Text).Value = user.Contact;
                insertCommand.Parameters.Add("@Birthdate",NpgsqlDbType.Date).Value = user.Birthdate;
                insertCommand.Parameters.Add("@Weight",NpgsqlDbType.Double).Value = user.Weight;
                insertCommand.Parameters.Add("@Height", NpgsqlDbType.Double).Value = user.Height;
                insertCommand.Parameters.Add("@CoachId", NpgsqlDbType.Uuid).Value = user.CoachId;
                insertCommand.Parameters.Add("@SubscriptionId",NpgsqlDbType.Uuid).Value= user.SubscriptionId;

                try
                {
                    connection.Open();
                    rowChanged = await insertCommand.ExecuteNonQueryAsync();
                    return rowChanged != 0;
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

        public async Task<bool> DeleteAsync(Guid id,Guid userId,DateTime time)
        {

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string deleteQuery = "UPDATE \"User\" SET \"IsActive\" = false , \"DatedUpdated\" = @DatedUpdated, \"UpdatedBy\" = @UpdatedBy WHERE \"Id\" = @Id";
                NpgsqlCommand deleteCommand = new NpgsqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@Id", id);
                deleteCommand.Parameters.AddWithValue("@DatedUpdated", time);
                deleteCommand.Parameters.AddWithValue("@UpdatedBy", userId);



                try
                {
                    await connection.OpenAsync();

                    int rowsAffected = await deleteCommand.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

        }



        public async Task<IUser> GetByIdAsync(Guid id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                IUser profile = null;
                string commandText = "SELECT * FROM \"User\" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE";
                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(commandText, connection))
                {
                    npgsqlCommand.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        await connection.OpenAsync();

                        using (NpgsqlDataReader reader = await npgsqlCommand.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                profile = new User()
                                {
                                    Id = (Guid)reader["Id"],
                                    Firstname = (String)reader["Firstname"],
                                    Lastname = reader.GetString(reader.GetOrdinal("Lastname")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Password = reader.GetString(reader.GetOrdinal("Password")),
                                    Contact = reader.IsDBNull(reader.GetOrdinal("Contact")) ? null : reader.GetString(reader.GetOrdinal("Contact")),
                                    RoleId = reader.GetGuid(reader.GetOrdinal("RoleId")),
                                    CreatedBy = reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                                    UpdatedBy = reader.GetGuid(reader.GetOrdinal("UpdatedBy")),
                                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                    DatedUpdated = reader.GetDateTime(reader.GetOrdinal("DatedUpdated")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    Weight=reader.GetDouble(reader.GetOrdinal("Weight")),
                                    Height=reader.GetDouble(reader.GetOrdinal("Height")),
                                    Birthdate=reader.GetDateTime(reader.GetOrdinal("Birthdate")),
                                    SubscriptionId=reader.GetGuid(reader.GetOrdinal("SubscriptionId")),
                                    CoachId = reader.IsDBNull(reader.GetOrdinal("CoachId"))
                                    ? (Guid?)null
                                    : reader.GetGuid(reader.GetOrdinal("CoachId"))

                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return profile;
            }
        }

        public async Task<bool> UpdateAsync(Guid id, IUser updatedUser)
        {
            int rowsChanged;
            IUser existingUser = await GetByIdAsync(id);
            if (existingUser == null)
                throw new Exception("No user with such ID in the database!");

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var updateFields = new List<string>();
                var command = new NpgsqlCommand();
                command.Connection = connection;

                if (!string.IsNullOrWhiteSpace(updatedUser.Firstname))
                {
                    updateFields.Add("\"Firstname\" = @Firstname");
                    command.Parameters.AddWithValue("@Firstname", updatedUser.Firstname);
                }

                if (!string.IsNullOrWhiteSpace(updatedUser.Lastname))
                {
                    updateFields.Add("\"Lastname\" = @Lastname");
                    command.Parameters.AddWithValue("@Lastname", updatedUser.Lastname);
                }

                if (!string.IsNullOrWhiteSpace(updatedUser.Contact))
                {
                    updateFields.Add("\"Contact\" = @Contact");
                    command.Parameters.AddWithValue("@Contact", updatedUser.Contact);
                }

                if (updatedUser.Birthdate > DateTime.MinValue)
                {
                    updateFields.Add("\"Birthdate\" = @Birthdate");
                    command.Parameters.AddWithValue("@Birthdate", updatedUser.Birthdate.Date);
                }

                if (updatedUser.Weight > 0)
                {
                    updateFields.Add("\"Weight\" = @Weight");
                    command.Parameters.AddWithValue("@Weight", updatedUser.Weight);
                }

                if (updatedUser.Height > 0)
                {
                    updateFields.Add("\"Height\" = @Height");
                    command.Parameters.AddWithValue("@Height", updatedUser.Height);
                }

                updateFields.Add("\"UpdatedBy\" = @UpdatedBy");
                command.Parameters.AddWithValue("@UpdatedBy", updatedUser.UpdatedBy);

                updateFields.Add("\"DatedUpdated\" = @DatedUpdated");
                command.Parameters.AddWithValue("@DatedUpdated", DateTime.UtcNow);

                string query = $@"
            UPDATE ""User"" SET {string.Join(", ", updateFields)}
            WHERE ""Id"" = @Id AND ""IsActive"" = TRUE";

                command.CommandText = query;
                command.Parameters.AddWithValue("@Id", id);

                rowsChanged = await command.ExecuteNonQueryAsync();
            }

            return rowsChanged > 0;
        }




        public async Task<IUser> ValidateUserAsync(string email, string password)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                IUser user = null;
                string commandText = "SELECT \"User\".\"Id\", \"User\".\"Email\", \"User\".\"Password\", \"User\".\"RoleId\", \"User\".\"Salt\"  FROM \"User\" WHERE \"Email\" = @Email AND \"IsActive\" = TRUE";
                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(commandText, connection))
                {
                    npgsqlCommand.Parameters.AddWithValue("@Email", email);
                    Console.WriteLine($"Email parameter: {email}");
                    try
                    {
                        await connection.OpenAsync();
                        using (NpgsqlDataReader reader = await npgsqlCommand.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                user = new User()
                                {
                                    Id = (Guid)reader["Id"],
                                    RoleId = (Guid)reader["RoleId"],
                                };
                                string storedPassword = (String)reader["Password"];
                                byte[] salt = Convert.FromBase64String((String)reader["Salt"]);

                                string hashedPassword = HashPassword(password, salt);

                                if (hashedPassword != storedPassword)
                                {
                                    user = null;
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return user;
            }
        }

        public async Task<IUser> ValidateUserByPasswordAsync(Guid id, string password)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                IUser user = null;
                string commandText = "SELECT \"User\".\"Id\", \"User\".\"Email\", \"User\".\"Password\", \"User\".\"RoleId\", \"User\".\"Salt\" FROM \"User\" WHERE \"User\".\"Id\" = @id AND \"IsActive\" = TRUE";
                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(commandText, connection))
                {
                    npgsqlCommand.Parameters.AddWithValue("id", id);
                    try
                    {
                        await connection.OpenAsync();
                        using (NpgsqlDataReader reader = await npgsqlCommand.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                user = new User()
                                {
                                    Id = (Guid)reader["Id"],
                                    RoleId = (Guid)reader["RoleId"]
                                };
                                string storedPassword = (String)reader["Password"];
                                byte[] salt = Convert.FromBase64String((String)reader["Salt"]);

                                string hashedPassword = HashPassword(password, salt);

                                if (hashedPassword != storedPassword)
                                {
                                    user = null;
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return user;
            }
        }

        public async Task<bool> UpdatePasswordAsync(Guid id, string passwordNew, string passwordOld)
        {
            int rowsChanged;
            byte[] salt = GenerateSalt();
            string hashedPassword = HashPassword(passwordNew, salt);
            IUser currUser = await ValidateUserByPasswordAsync(id, passwordOld);
            if (currUser == null) { return false; }
            string oldPassword = currUser.Password;
            if (hashedPassword == oldPassword)
            {
                return false;
            }
            IUser profile = await GetByIdAsync(id) ?? throw new Exception("No user with such ID in the database!");

            if (passwordOld != hashedPassword)
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    string updateCommand = $"UPDATE \"User\" SET \"Password\" = @password, \"Salt\"=@salt WHERE \"Id\"=@id AND \"IsActive\" = TRUE";
                    NpgsqlCommand command = new NpgsqlCommand(updateCommand, connection);
                    command.Parameters.AddWithValue("password", hashedPassword);
                    command.Parameters.AddWithValue("salt", Convert.ToBase64String(salt));
                    command.Parameters.AddWithValue("id", id);
                    rowsChanged = await command.ExecuteNonQueryAsync();
                }
                return rowsChanged != 0;
            }
            return false;
        }

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        public static string HashPassword(string password, byte[] salt)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private void AddProfileParameters(NpgsqlCommand command, Guid id, IUser updatedProfile)
        {
            command.Parameters.AddWithValue("@Id", id);
            if (!string.IsNullOrEmpty(updatedProfile.Firstname))
            {
                command.Parameters.AddWithValue("@FirstName", updatedProfile.Firstname);
            }
            if (!string.IsNullOrEmpty(updatedProfile.Lastname))
            {
                command.Parameters.AddWithValue("@LastName", updatedProfile.Lastname);
            }
            if (!string.IsNullOrEmpty(updatedProfile.Email))
            {
                command.Parameters.AddWithValue("@Email", updatedProfile.Email);
            }
            if (!string.IsNullOrEmpty(updatedProfile.Password))
            {
                command.Parameters.AddWithValue("@Password", updatedProfile.Password);
            }
            if (!string.IsNullOrEmpty(updatedProfile.Contact))
            {
                command.Parameters.AddWithValue("@Contact", updatedProfile.Contact);
            }
            if(updatedProfile.Weight != 0.0)
            {
                command.Parameters.AddWithValue("@Weight",updatedProfile.Weight);
            }
            if (updatedProfile.Height != 0.0)
            {
                command.Parameters.AddWithValue("@Height", updatedProfile.Height);
            }
            if(updatedProfile.CoachId != null)
            {
                command.Parameters.AddWithValue("@CoachId", updatedProfile.CoachId);
            }
            if(updatedProfile.SubscriptionId != null)
            {
                command.Parameters.AddWithValue("@SubscriptionId", updatedProfile.SubscriptionId);
            }
         
            if (updatedProfile.RoleId != null)
            {
                command.Parameters.AddWithValue("@RoleId", updatedProfile.RoleId);
            }
            if (updatedProfile.CreatedBy != null)
            {
                command.Parameters.AddWithValue("@CreatedBy", updatedProfile.CreatedBy);
            }
            if (updatedProfile.UpdatedBy != null)
            {
                command.Parameters.AddWithValue("@UpdatedBy", updatedProfile.UpdatedBy);
            }
            if (updatedProfile.DateCreated != null)
            {
                command.Parameters.AddWithValue("@DateCreated", updatedProfile.DateCreated);
            }
            if (updatedProfile.DatedUpdated != null)
            {
                command.Parameters.AddWithValue("@DatedUpdated", updatedProfile.DatedUpdated);
            }
            if (updatedProfile.IsActive != null)
            {
                command.Parameters.AddWithValue("@IsActive", updatedProfile.IsActive);
            }
        }
       
    }
}
