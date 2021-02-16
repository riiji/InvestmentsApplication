using System;
using System.Threading.Tasks;
using Quartz;

namespace InvestmentsApplication.Notifier
{
    public class DecoratorJob : IJob
    {
        private readonly Decorator decorator;
        private readonly string message;

        public DecoratorJob(Decorator decorator, string message)
        {
            this.decorator = decorator;
            this.message = message;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() => decorator.Notify(message));
        }
    }
}