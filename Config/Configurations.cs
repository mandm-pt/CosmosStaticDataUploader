using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosStaticDataUploader.Config
{
    internal class Configurations
    {
        public IEnumerable<Environment> Environments { get; set; }
    }

    internal class Environment
    {
        public string Name { get; set; }
        public string Endpoint { get; set; }
        public string AuthorizationKey { get; set; }
    }
}
