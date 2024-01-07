using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ILoggerService
    {
        void LogInfo(String message);
        void LogWarning(String message);
        void LogError(String message);
        void LogDebug(String message);
    }
}
