using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;

namespace MediatorChatRoom
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class Person
    {
        public string Name;
        public ChatRoom room;
        private List<string> chatLog = new List<string>();

        public Person(string name)
        {
            Name = name;
        }

        public void Say(string message)
        {
            room.Broadcast(Name, message);
        }

        public void PrivateMessage(string who, string message)
        {
            room.Message(Name, who, message);
        }

        public void Receive(string sender, string message)
        {
            string s = $"{sender}: {message}";
            chatLog.Add(s);
            Console.WriteLine($"[{Name}'s chat session] {s}");
        }
        
    }

    public class ChatRoom
    {
        private List<Person> people = new List<Person>();

        public void Join(Person p)
        {
            string joinMsg = $"{p.Name} has joined the room";
            Broadcast("room", joinMsg);
        }

        public void Broadcast(string source, string message)
        {
            foreach (var p in people)
            {
                if (p.Name != source)
                    p.Receive(source, message);
            }
        }

        public void Message(string source, string destination,  string message)
        {
            people.FirstOrDefault(p=> p.Name == destination)?.Receive(source, message);
        }
    }
}
