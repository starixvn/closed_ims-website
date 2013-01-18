// --------------------------------------------------------------------------
// <copyright file="BaseWorker.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.12.17">Created</change>
// </history>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Fanex.Logging;

namespace Fanex.TicketControl.Core.Services.Impl.Worker
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class BaseWorker
    {
        private Thread _workerThread;

        protected string ThreadName { get; set; }

        protected bool IsStop { get; set; }

        public BaseWorker(string threadName)
        {
            _workerThread = new Thread(Run);
            ThreadName = threadName;
        }

        /// <summary>
        /// Run main function
        /// </summary>
        protected virtual void Run()
        {
        }

        /// <summary>
        /// Start Worker
        /// </summary>
        public void Start()
        {
            if (_workerThread != null)
            {
                _workerThread.Start();
                IsStop = false;
            }
            else
            {
                Logger.Log.Error(string.Format("Thread {0} was stoped!", ThreadName), new Exception());
            }
        }

        /// <summary>
        /// Set Stop Worker
        /// </summary>
        public void Stop()
        {
            IsStop = true;
        }
    }
}
