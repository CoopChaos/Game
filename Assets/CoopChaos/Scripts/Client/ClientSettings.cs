using System;
using UnityEngine;

namespace CoopChaos
{
    public class ClientSettings
    {
#if DEBUG
        private static Guid guid = Guid.NewGuid();
        public static Guid GetToken()
        {
            return guid;
        }
#else
        public static Guid GetToken()
        {
            if (PlayerPrefs.HasKey("client_token"))
            {
                string g = PlayerPrefs.GetString("client_token");
                return Guid.Parse(g);
            }

            var token = Guid.NewGuid();
            PlayerPrefs.SetString("client_token", token.ToString());
            return token;
        }
#endif
    }
}