﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.Reflection;
using log4net;
using System.IO;
using System.Configuration;
using System.Data;
using System.ComponentModel.Composition;
using CronPluginService.Framework.Plugin;
using DailyProcessingJobs.Model;
using DailyProcessingJobs.Data;
using System.Net.Mail;
using CronPluginService.Framework.Communication;

namespace DailyProcessingJobs
{
    [PluginMetaData(JobKey = "DailyActivityReport")]
    public class GenerateManagementReportJob : PluginBase
    {
        private static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string _reportPath;
        private readonly string _reportName;
        private readonly string _worksheet;
        private readonly string _recipients;

        public GenerateManagementReportJob()
            : base()
        {
        }

        public GenerateManagementReportJob(string ReportPath, string ReportName, string Worksheet, string Recipients)
            : base()
        {
            _reportPath = ReportPath;
            _reportName = ReportName;
            _worksheet = Worksheet;
            _recipients = Recipients;
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Execute(PluginContext context)
        {
            try
            {
                ReportGenerator generator = new ReportGenerator();
                Report report = ReportRepository.Instance.GetDailyActivityReport();

                string reportName = string.Format("{0}_{1:MMM}_{1:yyyy}.xlsx", _reportName, DateTime.Now);

                Log.DebugFormat("Creating {0} at {1}", reportName, _reportPath);

                string reportPath = Path.Combine(_reportPath, reportName);

                InitializePath(_reportPath);

                generator.GenerateReport(reportPath, string.Format("{0}_{1:MM_dd_yyyy}", _worksheet, DateTime.Now), report, true, true);

                SendReport(reportPath);

                Log.DebugFormat("GenerateManagementReport Job ended @ {0}", DateTime.Now);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        private void SendReport(string reportPath)
        {
            string[] recipients = _recipients.Split(';');
            MailMessage message = CommunicationManager.Instance.BuildMessage(
                recipients, _reportName, "");

            CommunicationManager.Instance.SendAttachment(reportPath, message);
        }

        private void InitializePath(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                }
            }
        }

    }

}
