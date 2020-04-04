using System;
using System.Collections.Generic;

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
        { }

        public void PrivateMessage(string who, string message)
        { }

        public void Receive(string sender, string message)
        {
            string s = $"{sender}: {message}";
            chatLog.Add(s);
            Console.WriteLine($"[{Name}'s chat session] {s}");
        }
        
    }

    public class ChatRoom
    {

    }
}
