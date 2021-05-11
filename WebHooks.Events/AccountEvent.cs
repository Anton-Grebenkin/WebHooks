using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebHooks.Events
{

    public class AccountEvent : Event
    {
        public int SiteID { get; set; } //id аккаунта

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Integrations { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Login { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PartnerName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

    }
}
