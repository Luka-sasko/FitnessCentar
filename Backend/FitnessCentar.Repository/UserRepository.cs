using FitnessCentar.Model.Common;
using FitnessCentar.Repository.Common;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
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
                string insertQuery= "INSERT INTO \"User\" (\"Id\", \"FirstName\", \"LastName\", \"Email\", \"Password\", \"Salt\", \"Contact\",\"Birthdate\",\"Weight\",\"Height\",\"CoachId\",\"SubscriptionId\", \"RoleId\", \"CreatedBy\", \"UpdatedBy\", \"IsActive\") " +
                    "VALUES (@Id, @FirstName, @LastName, @Email, @Password, @Salt, @Contact, @Birthdate, @Weight, @Height, @CoachId, @SubscriptionId, (SELECT \"Id\" FROM \"Role\" WHERE \"Name\" = 'User'), @CreatedBy, @UpdatedBy, @IsActive)";

                NpgsqlCommand insertCommand= new NpgsqlCommand(insertQuery, connection);

                insertCommand.Parameters.Add("@Id", NpgsqlDbType.Uuid).Value = user.Id;
                insertCommand.Parameters.Add("@FirstName", NpgsqlDbType.Text).Value = user.FirstName;
                insertCommand.Parameters.Add("@LastName", NpgsqlDbType.Text).Value = user.LastName;
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

        public Task<bool> DeleteAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Guid Id, IUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> ValidateUserAsync(string username, string password)
        {
            throw new NotImplementedException();
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
    }
}
