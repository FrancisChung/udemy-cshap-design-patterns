using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace MediatorEventBroker
{
    public class Actor
    {
        protected EventBroker broker;

        public Actor(EventBroker broker)
        {
            this.broker = broker ?? throw new ArgumentNullException(paramName: nameof(broker));
        }
    }

    public class FootballPlayer : Actor
    {

        public string Name { get; set; }

        public FootballPlayer(EventBroker broker, string name) : base(broker)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));

            broker.OfType<PlayerScoredEvent>()
                .Where(ps => !ps.Name.Equals(name))
                .Subscribe(
                pe =>
                {
                    Console.WriteLine($"{name} : Nicely done {pe.Name}, it's your {pe.GoalsScored}");
                }
            );

            broker.OfType<PlayerSentOffEvent>()
                .Where(ps => !ps.Name.Equals(name))
                .Subscribe(
                    pe =>
                    {
                        Console.WriteLine($"{name} : See you in the lockers, {pe.Name}");
                    }
                );
        }
    }

    public class FootballCoach : Actor
    {
        public FootballCoach(EventBroker broker) : base(broker)
        {
            broker.OfType<PlayerScoredEvent>()
                .Subscribe(
                    pe =>
                    {
                        if (pe.GoalsScored < 3)
                            Console.WriteLine($"Coach : Well done, {pe.Name}!");
                    }
                );

            broker.OfType<PlayerSentOffEvent>()
                .Subscribe(
                    pe =>
                    {
                        if (pe.Reason == "Violence")
                            Console.WriteLine($"Coach : How could you, {pe.Name}?!?!");
                    }
                );
        }
    }

    public class PlayerEvent
    {
        public string Name { get; set; }
    }

    public class PlayerScoredEvent : PlayerEvent
    {
        public int GoalsScored { get; set; }
    }

    public class PlayerSentOffEvent : PlayerEvent
    {
        public string Reason { get; set; }        
    }

    public class EventBroker : IObservable<PlayerEvent>
    {
        Subject<PlayerEvent> subscriptions = new Subject<PlayerEvent>();
        public IDisposable Subscribe(IObserver<PlayerEvent> observer)
        {
            return subscriptions.Subscribe(observer);
        }

        public void Publish(PlayerEvent playerEvent)
        {
            subscriptions.OnNext(playerEvent);
        }

    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
