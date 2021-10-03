using Isu.Models;
using Isu.Services;
using IsuExtra.Models;
using IsuExtra.Services;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class IsuExtraTestsBase
    {
        protected IsuServiceConfiguration IsuServiceConfiguration = null!;
        protected ScheduleServiceConfiguration ScheduleServiceConfiguration = null!;
        
        protected IIsuService IsuService = null!;
        protected IIsuService ForeignIsuService = null!;
        protected IScheduleService ScheduleService = null!;

        [SetUp]
        public void BaseSetup()
        {
            IsuServiceConfiguration = new IsuServiceConfiguration
            {
                MaxStudentCount = 20,
            };
            ScheduleServiceConfiguration = new ScheduleServiceConfiguration
            {
                MaximumExtraStudyCount = 2,
            };
            
            IsuService = IIsuService.Create(IsuServiceConfiguration);
            ForeignIsuService = IIsuService.Create(IsuServiceConfiguration);
            ScheduleService = IScheduleService.Create(ScheduleServiceConfiguration, IsuService);
        }
    }
}