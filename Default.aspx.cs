using System;
using System.Collections.Concurrent;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;

namespace ADPASSWD3
{
    public partial class _Default : Page
    {
        private static HttpClient client;
        private static readonly ConcurrentDictionary<string, bool> cache = new ConcurrentDictionary<string, bool>();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Force TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (client == null)
            {
                var handler = new HttpClientHandler()
                {
                    Proxy = new WebProxy("http://10.84.128.220:8080", false),
                    UseProxy = true
                };

                client = new HttpClient(handler)
                {
                    Timeout = TimeSpan.FromSeconds(20)
                };

                client.DefaultRequestHeaders.Add("User-Agent", "ADPasswordResetTool");
            }
        }

        protected async void btnResetPassword_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();

            if (!IsValidPassword(newPassword))
            {
                ShowMessage("The new password does not meet complexity requirements.", "text-danger");
                return;
            }

            try
            {
                if (!await CheckPwnedApiConnectivity())
                {
                    ShowMessage("Unable to connect to the Pwned Passwords API. Please try again later.", "text-danger");
                    return;
                }

                if (await IsPasswordPwned(newPassword))
                {
                    ShowMessage("This password has been found in data breaches. Please choose a different password.", "text-danger");
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error checking password: " + ex.Message, "text-danger");
                return;
            }

            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                {
                    UserPrincipal user = UserPrincipal.FindByIdentity(context, username);

                    if (user != null)
                    {
                        if (context.ValidateCredentials(username, oldPassword))
                        {
                            user.ChangePassword(oldPassword, newPassword);
                            ShowMessage("Password has been reset successfully.", "text-success");
                        }
                        else
                        {
                            ShowMessage("Old password is incorrect.", "text-danger");
                        }
                    }
                    else
                    {
                        ShowMessage("User not found.", "text-danger");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error resetting password: " + ex.Message, "text-danger");
            }
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = cssClass;
        }

        private bool IsValidPassword(string password, string username = "")
        {
            // Password length check
            if (password.Length < 10) return false;

            // Basic checks for lower, upper, numbers, and special characters
            if (!Regex.IsMatch(password, "[a-z]")) return false;
            if (!Regex.IsMatch(password, "[A-Z]")) return false;
            if (!Regex.IsMatch(password, "[0-9]")) return false;
            if (!Regex.IsMatch(password, "[@$!%*?&]")) return false;

            // Check for repeated characters like "aa", "11", "bb"
            if (Regex.IsMatch(password, @"(.)\1")) return false;

            // Check if password contains username
            if (!string.IsNullOrEmpty(username) && password.ToLower().Contains(username.ToLower())) return false;

            // Common passwords check
            string[] commonPasswords = { "password", "p@ssw0rd", "123456", "abc123", "admin123", "letmein", "qwerty", "welcome", "abc12345", "qazwsx" };
            if (commonPasswords.Any(p => password.ToLower().Contains(p))) return false;

            // Avoid simple patterns like "1234", "abcd", etc.
            if (Regex.IsMatch(password, @"(1234|2345|3456|abcd|bcde|cdef)", RegexOptions.IgnoreCase)) return false;

            return true;
        }

        private async Task<bool> CheckPwnedApiConnectivity()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.pwnedpasswords.com/range/00000");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> IsPasswordPwned(string password)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToUpperInvariant();
                string prefix = hash.Substring(0, 5);
                string suffix = hash.Substring(5);

                if (cache.TryGetValue(hash, out bool isPwned))
                {
                    return isPwned;
                }

                try
                {
                    HttpResponseMessage response = await client.GetAsync($"https://api.pwnedpasswords.com/range/{prefix}");
                    response.EnsureSuccessStatusCode();

                    string content = await response.Content.ReadAsStringAsync();
                    isPwned = content.Contains(suffix);

                    cache[hash] = isPwned;
                    return isPwned;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error connecting to Pwned Passwords API: " + ex.Message);
                    throw;
                }
            }
        }
    }
}
