﻿using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Processors;

namespace Nelibur.ServiceModel.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public sealed class SoapServicePerCall : ISoapService
    {
        /// <summary>
        ///     Process message with response.
        /// </summary>
        /// <param name="message">Request message.</param>
        /// <returns>Response message.</returns>
        public Message Process(Message message)
        {
            return NeliburSoapService.Process(message);
        }

        /// <summary>
        ///     Process message without response.
        /// </summary>
        /// <param name="message">Request message.</param>
        public void ProcessOneWay(Message message)
        {
            NeliburSoapService.ProcessOneWay(message);
        }
    }
}
