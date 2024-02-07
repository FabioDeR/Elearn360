using Blazored.LocalStorage;
using elearn.Data.ViewModel.AccountVM;
using eLearn360.Data.VM.AccountVM;
using eLearn360.Service.Auth;
using eLearn360.Service.Interface;
using Microsoft.AspNetCore.Components.Authorization;
using Sotsera.Blazor.Toaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eLearn360.Service
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        protected IToaster Toaster { get; set; }

        public AuthService(HttpClient httpClient,
                         AuthenticationStateProvider authenticationStateProvider,
                         ILocalStorageService localStorage,
                         IToaster toaster)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
            Toaster = toaster;
        }
        public async Task<LoginResultVM> Login(LoginVM login)
        {
            try
            {
                var logininfo = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");

                LoginResultVM result = await JsonSerializer.DeserializeAsync<LoginResultVM>(
                await _httpClient.PostAsync("api/Auth/login", logininfo).Result.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


                if (result.Success)
                {
                    await _localStorage.SetItemAsync("authToken", result.Token);
                    ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(result.Token);
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", result.Token);
                    return result;
                }

                return result;
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task RefreshTokenAuthorize(RefreshTokenVM refreshTokenVM)
        {
            try
            {
                await _localStorage.RemoveItemAsync("authToken");
                var registerjson = new StringContent(JsonSerializer.Serialize(refreshTokenVM), Encoding.UTF8, "application/json");

                RefreshTokenResultVM newToken = await JsonSerializer.DeserializeAsync<RefreshTokenResultVM>(
                       await _httpClient.PostAsync("api/Auth/refreshToken", registerjson).Result.Content.ReadAsStreamAsync(),
                       new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                if (newToken.Success)
                {
                    await _localStorage.SetItemAsync("authToken", newToken.RefreshToken);
                    ((ApiAuthenticationStateProvider)_authenticationStateProvider).RefreshToken(newToken.RefreshToken);
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", newToken.RefreshToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<RegisterResultVM> Register(RegisterVM register)
        {
            try
            {
                var registerjson = new StringContent(JsonSerializer.Serialize(register), Encoding.UTF8, "application/json");
                Toaster.Success("Un email vous a été envoyé !");
                return await JsonSerializer.DeserializeAsync<RegisterResultVM>(
                       await _httpClient.PostAsync("api/Auth/register", registerjson).Result.Content.ReadAsStreamAsync(),
                       new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Toaster.Error("Une erreur est apparue !");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        #region Change Password
        public async Task<HttpResponseMessage> ChangePassword(AccountChangePasswordVM changePassword)
        {
            try
            {
                var ChangePasswordjson = new StringContent(JsonSerializer.Serialize(changePassword), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PutAsync("api/auth/changepassword", ChangePasswordjson);
                Toaster.Success("Le mot de passe à correctement été modifié !");
                return result;

            }
            catch (Exception err)
            {
                Toaster.Error("L'ancien mot de passe n'est pas correct");
                Console.WriteLine(err);
                throw;
            }
        }
        #endregion

        #region Reset Password By guid
        public async Task<HttpResponseMessage> ResetPasswordByGuid(Guid userid)
        {
            try
            {
                var result = await _httpClient.GetAsync($"api/Auth/passwordefaultresetbyguid?userid={userid}");
                Toaster.Success($"Un email avec le nouveau mot de passe a été envoyé à l'utilisateur !");
                return result;
            }
            catch (Exception err)
            {
                Toaster.Error("Une erreur est apparue !");
                Console.WriteLine(err);
                throw;
            }
        }
        #endregion

        #region Reset Password By Mail
        public async Task<HttpResponseMessage> ResetPasswordByMail(string mail)
        {
            try
            {
                var result = await _httpClient.GetAsync($"api/Auth/passwordefaultresetbymail?mail={mail}");

                return result;
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
            }
        }
        #endregion
    }
}
