using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenza.Core.Settings
{
    public sealed class AzureOpenAISettings
    {
        public string Endpoint { get; set; } = "";
        public string ApiKey { get; set; } = "";
        public string ChatDeployment { get; set; } = "";
    }
}

