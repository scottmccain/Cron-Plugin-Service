﻿using System.Configuration;

namespace CronPluginService.Framework.Configuration
{
    public class CronScheduledJobElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("expression", IsRequired = true)]
        public string Expression
        {
            get { return (string)base["expression"]; }
            set { base["expression"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string TypeString
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("JobParameters", IsRequired = true)]
        public CronServiceParameterCollection Parameters
        {
            get { return (CronServiceParameterCollection)this["JobParameters"]; }
            set { this["JobParameters"] = value; }
        }
    }
}
