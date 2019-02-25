using Castle.DynamicProxy;

namespace Engine.Factories
{
    public abstract class BaseFactory
    {
        protected static ProxyGenerator _generator = new ProxyGenerator();

        public static T Create<I, T>(params object[] ctorArguments) where I : class, IFactory, new() where T : class
        {
            I i = new I();
            return i.CreateType<T>(ctorArguments);
        }
    }

    public interface IFactory
    {
        T CreateType<T>(params object[] ctorArguments) where T : class;
    }
}
