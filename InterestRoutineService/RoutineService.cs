using System.ServiceProcess;
using BusinessLogic;

namespace InterestRoutineService
{
    public class RoutineService : ServiceBase
    {
        protected override void OnStart(string[] args)
        {
            new InterestRatesBl().InterestsRoutine();
        }

        protected override void OnStop()
        {
        }

        public void OnDebug()
        {
            OnStart(null);
        }
    }
}
