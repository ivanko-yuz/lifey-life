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

        public async Task<User> FindByName(string userName)
        {
            const string query = $@"SELECT 
                            uuid as  @{nameof(User.Uuid)},
                            default_language as @{nameof(User.DefaultLanguage)}::language,
                            first_name as @{nameof(User.FirstName)},
                            last_name as  @{nameof(User.LastName)},
                            user_name as  @{nameof(User.UserName)},
                            email as  @{nameof(User.Email)},
                            password_hash as  @{nameof(User.PasswordHash)},
                            current_level as  @{nameof(User.CurrentLevel)},
                            current_experience as  @{nameof(User.CurrentExperience)}
                        FROM public.user
                        WHERE user_name =@{nameof(User.UserName)};";

            return await _dbAdapter.GetSingle<User>(query, new { userName });
        }

        public async Task<User> FindById(Guid uuid)
        {
            const string query = $@"SELECT 
                            uuid as  @{nameof(User.Uuid)},
                            default_language as  @{nameof(User.DefaultLanguage)}::language,
                            first_name as @{nameof(User.FirstName)},
                            last_name as  @{nameof(User.LastName)},
                            user_name as  @{nameof(User.UserName)},
                            email as  @{nameof(User.Email)},
                            password_hash as  @{nameof(User.PasswordHash)},
                            current_level as  @{nameof(User.CurrentLevel)},
                            current_experience as  @{nameof(User.CurrentExperience)}
                        FROM public.user
                        WHERE uuid =@{nameof(User.Uuid)};";

            return await _dbAdapter.GetSingle<User>(query, new { uuid });
        }

        public Task<User> Update(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateUser(User user)
        {
            const string query = $@"INSERT INTO public.user
                        (
                             uuid,
                             default_language,
                             first_name,
                             last_name,
                             user_name,
                             email,
                             password_hash,
                             current_level,
                             current_experience
                         ) VALUES (
                                @{nameof(User.Uuid)},
                                @{nameof(User.DefaultLanguage)}::language,
                                @{nameof(User.FirstName)},
                                @{nameof(User.LastName)},
                                @{nameof(User.UserName)},
                                @{nameof(User.Email)},
                                @{nameof(User.PasswordHash)},
                                @{nameof(User.CurrentLevel)},
                                @{nameof(User.CurrentExperience)}
                        );";

            var result = await _dbAdapter.ExecuteCommand(query, new
            {
                user.Uuid,
                DefaultLanguage = user.DefaultLanguage.ToString(),
                user.FirstName,
                user.LastName,
                UserName = KeyNormalizer.NormalizeName(user.UserName),
                Email = KeyNormalizer.NormalizeEmail(user.Email),
                user.PasswordHash,
                user.CurrentLevel,
                user.CurrentExperience
            });

            return result > 0;
        }

        public Task<string> GetToken(User user, string loginProvider, string tokenName)
        {
            throw new NotImplementedException();
        }

        public async Task SetToken(User user, string loginProvider, string tokenName, string tokenValue)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveToken(User user, string loginProvider, string tokenName)
        {
            throw new NotImplementedException();
        }

        // public async Task SetPasswordHashAsync(User user, string? hash)
        // {
        //     const string query = $@"UPDATE public.user
        //                 SET
        //                        password_hash = @{nameof(User.PasswordHash)} 
        //                 WHERE uuid =  @{nameof(User.Uuid)};";
        //
        //     await _dbAdapter.GetSingle<string>(query, new { user.Uuid, user.PasswordHash });
        // }

        public async Task<string> GetPasswordHash(User user)
        {
            const string query = $@"SELECT 
                               password_hash as {nameof(User.PasswordHash)} 
                        FROM public.user
                        WHERE uuid =@{nameof(User.Uuid)};";

            return await _dbAdapter.GetSingle<string>(query, new { user.Uuid });
        }
    }
}