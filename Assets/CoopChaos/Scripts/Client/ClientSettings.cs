using System;
using UnityEngine;

namespace CoopChaos
{
    public class ClientSettings
    {
        public static Guid GetToken()
        {
            if (PlayerPrefs.HasKey("client_token"))
            {
                return Guid.Parse(PlayerPrefs.GetString("client_token"));
            }

            var token = Guid.NewGuid();
            PlayerPrefs.SetString("client_token", token.ToString());
            return token;
        }
    }
}