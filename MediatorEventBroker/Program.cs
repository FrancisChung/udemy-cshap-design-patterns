using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

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
        public FootballPlayer(EventBroker broker) : base(broker)
        {
            
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
