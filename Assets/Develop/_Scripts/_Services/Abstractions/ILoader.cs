namespace Develop._Scripts._Services.Abstractions
{
    public interface ILoader<T>
    {
        public T Load(string path);
    }
}