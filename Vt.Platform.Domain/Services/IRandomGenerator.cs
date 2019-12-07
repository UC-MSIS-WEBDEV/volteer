using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Vt.Platform.Domain.Services
{
    public interface IRandomGenerator
    {
        Task<string> GetEventCode();
        Task<string> GetSigninToken();
        Task<string> GetParticipantCode();
    }

    public class RandomGenerator : IRandomGenerator
    {
        public async Task<string> GetEventCode()
        {
            await Task.CompletedTask;
            int tokenLength = 6;
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToArray();
            var rnd = new RNGCryptoServiceProvider();
            byte[] arr = new byte[75];
            rnd.GetNonZeroBytes(arr);
            var res = new string(Convert.ToBase64String(arr).Where(c => chars.Contains(c)).Take(tokenLength).ToArray());
            return res;
        }

        public async Task<string> GetSigninToken()
        {
            await Task.CompletedTask;
            return Guid.NewGuid().ToString("N");
        }

        public async Task<string> GetParticipantCode()
        {
            await Task.CompletedTask;
            int tokenLength = 6; 
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToArray();
            var rnd = new RNGCryptoServiceProvider();
            byte[] arr = new byte[75];
            rnd.GetNonZeroBytes(arr);
            var res = new string(Convert.ToBase64String(arr).Where(c => chars.Contains(c)).Take(tokenLength).ToArray());
            return res;
        }
    }
}
