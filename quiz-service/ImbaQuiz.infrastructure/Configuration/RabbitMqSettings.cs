﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.infrastructure.Configuration
{
    public class RabbitMqSettings
    {
        public const string SectionName = "RabbitMqSettings"; 
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
    }
}
