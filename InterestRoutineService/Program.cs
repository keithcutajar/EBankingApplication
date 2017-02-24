using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace InterestRoutineService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            var routineService = new RoutineService();
            routineService.OnDebug();
            // Keeps the service going countinously TODO RUN ONCE A DAY 
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
            var servicesToRun = new ServiceBase[]
            {
                new RoutineService(), 
            };
            ServiceBase.Run(servicesToRun);
#endif
        }
    }
}
