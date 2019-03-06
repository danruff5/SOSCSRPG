using Engine.Factories;
using System;
using System.ComponentModel;

namespace ConsoleApp1
{
    class Program
    {
        public static void Main(string[] args)
        {
            Foo f = BaseFactory.Create<FooFactory, Foo>();
            f.PropertyChanged += OnPropertyChanged_Foo;

            f.Name = "Hello World!!";
            Console.WriteLine(f);

            f.PropertyChanged -= OnPropertyChanged_Foo;

            f.Name = "Bar";
            Console.WriteLine(f);
        }

        private static void OnPropertyChanged_Foo(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine("Foo changed!!");
        }
    }
}
