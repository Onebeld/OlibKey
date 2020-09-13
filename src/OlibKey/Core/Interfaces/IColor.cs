using ReactiveUI;

namespace OlibKey.Core.Interfaces
{
    public interface IColor : IReactiveNotifyPropertyChanged<IReactiveObject>, IReactiveObject
    {
        IColor Clone();
    }
}
