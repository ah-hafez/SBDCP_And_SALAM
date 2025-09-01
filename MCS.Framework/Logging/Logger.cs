using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using System;
using System.Diagnostics;
using System.Text;

namespace MCS.Framework.Logging
{
    /// <summary>
    /// An enumeration with all available logging priorities that can be used with <see cref="Logger"/> class.
    /// </summary>
    public enum LoggingPriority
    {
        /// <summary>
        /// Low priority, e.g. Information
        /// </summary>
        Low = 1,

        /// <summary>
        /// Meduim priority, e.g. Warning
        /// </summary>
        Medium = 3,

        /// <summary>
        /// High / Critial priority, e.g. Error
        /// </summary>
        High = 5
    }

    /// <summary>
    /// An enumeration with all event Ids that can be used with <see cref="Logger"/> class
    /// </summary>
    public enum LoggingEventId
    {
        /// <summary>
        /// EventId for information severity
        /// </summary>
        Information = 100,

        /// <summary>
        /// EventId for warning serverity
        /// </summary>
        Warning = 101,

        /// <summary>
        /// EventId for error serverity
        /// </summary>

        Error = 102,

        /// <summary>
        /// EventId for debug serverity
        /// </summary>
        Debug = 103
    }

    /// <summary>
    /// A numeration with all logging categories that can be used with <see cref="Logger"/> class and
    /// need to be mapped in the configuration section that pertain to logging application bolck.
    /// </summary>
    public enum LoggingCategory
    {
        /// <summary>
        /// Category for error logs
        /// </summary>
        Error,

        /// <summary>
        /// Category for warning logs
        /// </summary>
        Warning,
        /// <summary>
        /// Category for information logs
        /// </summary>
        /// 
        Information,

        /// <summary>
        /// Generic category for all other logs
        /// </summary>
        General
    }

    public static class Logger
    {
        /// <summary>
        /// Write a log entry to the log destination with as follows:
        /// <see cref="LoggingCategory"/> is Information, severity is <see cref="TraceEventType.Information"/>
        /// and <see cref="LoggingPriority"/> is Low.
        /// </summary>
        /// <param name="message">The message to write</param>
        public static void WriteInformation(string message)
        {
            WriteInformation(message, LoggingCategory.Information);
        }

        ///<summary>
        /// Write a log entry to the log destination with as follows:
        /// <see cref="LoggingCategory"/> is custom, severity is <see cref="TraceEventType.Information"/> 
        /// and <see cref="LoggingPriority"/> is Low.
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="category">The custom <see cref="LoggingCategory"/> to use</param>
        public static void WriteInformation(string message, LoggingCategory category)
        {
            try
            {
                if (!CanLog(category, LoggingPriority.Low))
                    return;

                //create Error log entry
                LogEntry entry = new LogEntry();

                entry.EventId = (int)LoggingEventId.Information;
                entry.Categories.Add(category.ToString());
                entry.Priority = (int)LoggingPriority.Low;
                entry.Severity = TraceEventType.Information;

                entry.Message = message;

                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(entry);
            }
            catch (Exception)
            {

                throw;
            }
        }

        ///<summary>
        /// Write a log entry to the log destination as follows:
        /// <see cref="LoggingCategory"/> is Error, severity is <see cref="TraceEventType.Error"/> 
        /// and <see cref="LoggingPriority"/> is High.
        /// </summary>
        /// <param name="message">The message to write</param>
        public static void WriteError(string message)
        {
            WriteError(message, LoggingCategory.Error);
        }

        /// <summary>
        /// Write a log entry to the log destination as follows:
        /// <see cref="LoggingCategory"/> is custom, severity is <see cref="TraceEventType.Error"/>
        /// and <see cref="LoggingPriority"/> is High.
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="category">The custom <see cref="LoggingCategory"/> to be used</param>
        public static void WriteError(string message, LoggingCategory category)
        {
            try
            {
                if (!CanLog(category, LoggingPriority.High))
                    return;

                //create Error log entry
                LogEntry entry = new LogEntry();

                entry.EventId = (int)LoggingEventId.Error;
                entry.Categories.Add(category.ToString());
                entry.Priority = (int)LoggingPriority.High;
                entry.Severity = TraceEventType.Error;

                entry.Message = message;

                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(entry);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Write a log entry to the log destination with as follows:
        /// <see cref="LoggingCategory"/> is Warning, severity is <see cref="TraceEventType.Warning"/>
        /// <see cref="LoggingPriority"/> is Meduim.
        /// </summary>
        /// <param name="message">The message to write</param>
        public static void WriteWarning(string message)
        {
            WriteWarning(message, LoggingCategory.Warning);
        }

        /// <summary>
        /// Write a log entry to the log destination with as follows:
        /// <see cref="LoggingCategory"/> is custom, severity is <see cref="TraceEventType.Warning"/>
        /// <see cref="LoggingPriority"/> is Meduim.
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="category">The custom <see cref="LoggingCategory"/> to be used</param>
        public static void WriteWarning(string message, LoggingCategory category)
        {
            try
            {
                if (!CanLog(category, LoggingPriority.Medium))
                    return;

                //create Error log entry
                LogEntry entry = new LogEntry();

                entry.EventId = (int)LoggingEventId.Warning;
                entry.Categories.Add(category.ToString());
                entry.Priority = (int)LoggingPriority.Medium;
                entry.Severity = TraceEventType.Warning;

                entry.Message = message;

                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(entry);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Write the exception information to the log destination as Error.
        /// </summary>
        /// <param name="exception">The exception information to be written</param>
        public static void WriteException(Exception exception)
        {
            if (!Microsoft.Practices.EnterpriseLibrary.Logging.Logger.IsLoggingEnabled())
                return;

            StringBuilder message = new StringBuilder();

            Exception inner = exception;

            // write the exception first and then any inner exceptions related..
            while (inner != null)
            {
                message.Append(inner.ToString());

                message.Append("\n-------------- InnerException -----------");

                inner = inner.InnerException;
            }

            message.Append("\n-------------- StackTrace -----------\n");

            message.Append(exception.StackTrace);

            WriteError(message.ToString());
        }

        public static void WriteExceptionWithMessage(Exception exception, string notes)
        {
            if (!Microsoft.Practices.EnterpriseLibrary.Logging.Logger.IsLoggingEnabled())
                return;

            StringBuilder message = new StringBuilder();

            Exception inner = exception;

            // write the exception first and then any inner exceptions related..
            while (inner != null)
            {
                message.Append(notes);
                message.Append(inner.ToString());

                message.Append("\n-------------- InnerException -----------");

                inner = inner.InnerException;
            }

            message.Append("\n-------------- StackTrace -----------\n");

            message.Append(exception.StackTrace);

            WriteError(message.ToString());
        }

        /// <summary>
        /// Simply writes tracing message to the configured destinations.
        /// </summary>
        /// <param name="message">Message to write.</param>
        public static void Write(string message)
        {
            try
            {

                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Return whether a given <see cref="LoggingCategory"/> and <see cref="LoggingPriority"/> can be 
        /// logged to the log destination and whether the logging feature is enabled.
        /// A given priority can be disabled through the priority filter in the configuration file.
        /// A given category can be disabled through the category filter in the configuration file.
        /// Logging as a whole can be disabled through the logging filter in the configuration file.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static bool CanLog(LoggingCategory category, LoggingPriority priority)
        {
            if (!Microsoft.Practices.EnterpriseLibrary.Logging.Logger.IsLoggingEnabled())
                return false;

            if (!Microsoft.Practices.EnterpriseLibrary.Logging.Logger.GetFilter<PriorityFilter>().ShouldLog((int)priority))
                return false;

            if (!Microsoft.Practices.EnterpriseLibrary.Logging.Logger.GetFilter<CategoryFilter>().ShouldLog(category.ToString()))
                return false;

            return true;
        }
    }
}
