using System.ComponentModel;

namespace InvestmentsApplication.Notifier
{
    public abstract class Decorator : Notifier
    {
        protected Notifier Notifier;

        protected Decorator(Notifier notifier)
        {
            Notifier = notifier;
        }

        public void SetNotifier(Notifier notifier)
        {
            Notifier = notifier;
        }

        public override void Notify(string message)
        {
            Notifier?.Notify(message);
        }
    }
}