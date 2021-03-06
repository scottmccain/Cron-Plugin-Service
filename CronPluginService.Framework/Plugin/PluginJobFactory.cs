﻿using System;
using Quartz.Spi;
using Quartz;

namespace CronPluginService.Framework.Plugin
{
    public class PluginJobFactory : IJobFactory
    {
        private readonly Object[] _parameters;
        public PluginJobFactory(Type pluginType, Object [] parameters)
        {
            PluginType = pluginType;
            _parameters = parameters;

            if (!typeof(IPluginJob).IsAssignableFrom(pluginType))
            {
                throw new ArgumentException("Handler must implement the IPluginJob interface.");
            }

            PluginType = pluginType;
        }

        #region IJobFactory Members
        public IJob NewJob(TriggerFiredBundle bundle)
        {
            return new PluginJobHandler(Activator.CreateInstance(PluginType, _parameters) as IPluginJob);
        }
        #endregion

        public Type PluginType
        {
            get; private set;
        }
    }
}
