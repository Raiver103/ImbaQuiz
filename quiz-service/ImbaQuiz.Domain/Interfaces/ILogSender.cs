﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Domain.Interfaces
{
    public interface ILogSender
    {
        void SendLog(string message);
    }
}
