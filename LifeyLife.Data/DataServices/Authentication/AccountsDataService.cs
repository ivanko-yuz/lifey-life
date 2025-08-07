using LifeyLife.Core.Contracts.Authentication;
using LifeyLife.Core.Models;
using LifeyLife.Core.Utils;

namespace LifeyLife.Data.DataServices.Authentication
{
    public class AccountsDataService : IAccountsDataService
    {
        private readonly IDbAdapter _dbAdapter;

        public AccountsDataService(IDbAdapter dbAdapter)
        {
            _dbAdapter = dbAdapter;
        }

        public async Task<User> FindByName(string email)
        {
            const string query = @"SELECT 
                            uuid as Uuid,
                            email as Email,
                            password_hash as PasswordHash,
                            preferred_language as PreferredLanguage
                        FROM public.user
                        WHERE email = @Email;";

            return await _dbAdapter.GetSingle<User>(query, new { Email = email });
        }

        public async Task<User> FindById(Guid uuid)
        {
            const string query = @"SELECT 
                            uuid as Uuid,
                            email as Email,
                            password_hash as PasswordHash,
                            preferred_language as PreferredLanguage
                        FROM public.user
                        WHERE uuid = @Uuid;";

            return await _dbAdapter.GetSingle<User>(query, new { Uuid = uuid });
        }

        public async Task<bool> CreateUser(User user)
        {
            const string query = @"INSERT INTO public.user
                        (
                            uuid,
                            email,
                            password_hash,
                            preferred_language
                        ) VALUES (
                            @Uuid,
                            @Email,
                            @PasswordHash,
                            @PreferredLanguage
                        );";

            var result = await _dbAdapter.ExecuteCommand(query, new
            {
                user.Uuid,
                user.Email,
                user.PasswordHash,
                PreferredLanguage = user.PreferredLanguage.ToString()
            });

            return result > 0;
        }

        public async Task<bool> UpdateUser(User user)
        {
            const string query = @"UPDATE public.user
                        SET 
                            email = @Email,
                            preferred_language = @PreferredLanguage
                        WHERE uuid = @Uuid;";

            var result = await _dbAdapter.ExecuteCommand(query, new
            {
                user.Uuid,
                user.Email,
                PreferredLanguage = user.PreferredLanguage.ToString()
            });

            return result > 0;
        }

        public async Task<string> GetPasswordHash(User user)
        {
            const string query = @"SELECT 
                               password_hash as PasswordHash 
                        FROM public.user
                        WHERE uuid = @Uuid;";

            return await _dbAdapter.GetSingle<string>(query, new { user.Uuid });
        }
    }
}