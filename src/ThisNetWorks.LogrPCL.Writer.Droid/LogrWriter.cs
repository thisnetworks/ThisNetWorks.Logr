﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ThisNetWorks.LogrPCL.Abstractions;
using ThisNetWorks.LogrPCL.Writer.Shared;

using Android.Util;
using System.IO;
using Android.App;
using System.Text;

namespace ThisNetWorks.LogrPCL.Writer
{
	public class LogrWriter : LogrWriterBase, ILogrWriter
	{
		private string _tag = Application.Context.ApplicationInfo.LoadLabel(Application.Context.PackageManager);

		protected override void LogDebug(string message)
		{
			Log.Debug(_tag, message);
		}

		protected override void LogInfo(string message)
		{
			Log.Info(_tag, message);
		}
		protected override void LogWarn(string message)
		{
			Log.Warn(_tag, message);
		}

		protected override void LogError(string message, Exception e)
		{
            LogrException logrException = null;

			if (e == null)
				logrException = new LogrException(message);
            else 
                logrException = new LogrException(message, e);


			Log.Error(_tag, message);
			if (e != null && e.StackTrace != null)
				Log.Error(_tag, e.StackTrace);

			ReportToInsights(logrException, message, InsightsSeverity.Error);
            ReportToMobileCenter(_tag, logrException, message);
		}

		protected override string FormatMessage(string message, params object[] args)
		{
			var messageFormatted = string.Format(message, args);
			string messageWithTime = FormatMessageWithTime(messageFormatted);

			return messageWithTime;
		}

		protected override void AddLogFileToInsights(System.Collections.Generic.IDictionary<string, string> dict)
		{
			var appLog = ReadLog(string.Empty);
			dict.Add(_tag, appLog);
			return;
		}

        protected override string AddLogFileToMobileCenter(string message)
		{
			var appLog = ReadLog(string.Empty);
			StringBuilder builder = new StringBuilder();
            builder.Append(message);
            builder.AppendLine();
            builder.AppendLine("Console log dump");
            builder.Append(appLog);
            return builder.ToString();

        }
		private string ReadLog(string tag)
		{
			var cmd = "logcat -d";
			if (!string.IsNullOrEmpty(tag)) cmd += " -s " + tag;

			var process = Java.Lang.Runtime.GetRuntime().Exec(cmd);
			using (var sr = new StreamReader(process.InputStream)) {
				var log = sr.Tail(Settings.Insights.LogNumberOfLines);

				return log;
			}
		}
	}


}


