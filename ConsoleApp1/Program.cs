using Castle.DynamicProxy;
using Engine.Attributes;
using Engine.Factories;
using System;
using System.ComponentModel;

namespace ConsoleApp1
{
    public class Program
    {
        public virtual Foo foo
        {
            get;
            [HandleEvents(nameof(Foo.PropertyChanged), nameof(OnPropertyChanged_Foo))]
            set;
        }

        public static void Main(string[] args)
        {
            Program p = BaseFactory.Create<ProgramFactory, Program>();
            p.foo = BaseFactory.Create<FooFactory, Foo>();

            p.foo.Name = "Hello World!!";
            Console.WriteLine(p.foo);

            p.foo.Name = "Bar";
            Console.WriteLine(p.foo);
        }

        public void OnPropertyChanged_Foo(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine("Foo changed!!");
        }
    }

    class ProgramFactory : BaseFactory, IFactory
    {
        public T CreateType<T>(params object[] ctorArguments) where T : class
        {
            Aspect<Program> a = new Aspect<Program>();
            a.baseAspects.Add(new HandleEventsAspect());

            return _generator.CreateClassProxy
                (
                    typeof(Program),
                    new ProxyGenerationOptions(a as IProxyGenerationHook)
                    {
                        Selector = a as IInterceptorSelector
                    },
                    a.baseAspects.ToArray()
                ) as T;
        }
    }
}
