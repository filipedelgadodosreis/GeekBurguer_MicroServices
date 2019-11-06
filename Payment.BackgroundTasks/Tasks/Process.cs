using Autofac;

namespace Payment.BackgroundTasks.Tasks
{
    public class Process
    {
        private ILifetimeScope _container;

        private string _teste;

        public Process(ILifetimeScope container , string teste)
        {
            _teste = teste;
            _container = container;
        }
    }
}
