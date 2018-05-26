using System;
using Autofac;
using WheelOfFate.Scheduling.CandidateSelection;
using WheelOfFate.Scheduling.Engineers;
using WheelOfFate.Scheduling.SupportSchedules;

namespace WheelOfFate.Scheduling
{
    public class SchedulingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterTypes(
                    typeof(Calendar),
                    typeof(RandomSelector),
                    typeof(SupportCandidateSelector),
                    typeof(SupportScheduleDateValidator),
                    typeof(DayOffFilter),
                    typeof(ShiftAvailabilityReconciler),
                    typeof(EngineerRepository),
                    typeof(SupportScheduleRepository))
               .AsImplementedInterfaces()
               .SingleInstance();

            builder.RegisterType<SupportScheduler>()
                   .As<IStartable>()
                   .AsSelf()
                   .SingleInstance();
        }
    }
}
