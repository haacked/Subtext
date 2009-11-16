using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Subtext.Identicon
{
    /// <summary>
    /// Code borrowed from http://identicon.codeplex.com/
    /// </summary>
    public class IdenticonUtil
    {
        /// <summary>Sets or returns current IP address mask. Default is 0xffffffff.</summary>
        public static int Mask = unchecked((int)0xffffffff);

        /// <summary>Sets or returns current salt string value.</summary>
        public static String Salt;

        /// <summary>
        /// Returns Identicon code for given IP address as an integer.
        /// </summary>
        public static int Code(string ipAddress)
        {
            if(ipAddress == null)
            {
                throw new ArgumentNullException("ipAddress", "Must specify a non-null ip address.");
            }

            if(Salt == null)
            {
                // if not set manually, salt is automatically set to some machine-specific stuff 
                // Removed Environment.ProcessorCount because it requires elevated privileges.
                Salt = Environment.MachineName;
            }

            byte[] ip = GetAddressBytes(ipAddress);

            var s = new StringBuilder();
            /// Current implementation uses first four bytes of SHA1(int(mask(ip))+salt)
            /// where mask(ip) uses inetMask to remove unwanted bits from IP address.
            s.Append((((ip[0] & 0xFF) << 24) | ((ip[1] & 0xFF) << 16) | ((ip[2] & 0xFF) << 8) | (ip[3] & 0xFF)) & Mask);
            s.Append('+');
            /// Also, since salt is a string for convenience sake, int(mask(ip)) is
            /// converetd into a string and combined with inetSalt prior to hashing.
            s.Append(Salt);

            SHA1 md = new SHA1CryptoServiceProvider();
            byte[] hashedIp = md.ComputeHash(new UTF8Encoding().GetBytes(s.ToString()));

            return ((hashedIp[0] & 0xFF) << 24) | ((hashedIp[1] & 0xFF) << 16) | ((hashedIp[2] & 0xFF) << 8) | (hashedIp[3] & 0xFF);
        }

        /// <summary>
        /// Translates IP string into 4-byte array
        /// </summary>
        private static byte[] GetAddressBytes(string ipAddress)
        {
            var b = new byte[] {0, 0, 0, 0};
            if(!String.IsNullOrEmpty(ipAddress))
            {
                string s = Regex.Match(ipAddress, @"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}").ToString();
                int i = 0;
                foreach(Match m in Regex.Matches(s, @"\d+"))
                {
                    Byte.TryParse(m.ToString(), out b[i]);
                    i++;
                }
            }
            return b;
        }

        /// <summary>
        /// returns unique string tag for an Identicon code at a specific size. 
        /// Used to track browser caching of specific images
        /// </summary>		
        public static String ETag(int code, int size)
        {
            return "W/\"" + Convert.ToString(code, 16) + "@" + size + "\"";
        }
    }
}