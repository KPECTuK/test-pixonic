namespace Sim.Module.Generic
{
	/// <summary>
	///     Интерфейс контекста - композиционного узла.
	/// </summary>
	public interface IContext
	{
		T Resolve<T>() where T : class;
		bool TryResolve<T>(out T instance) where T : class;
		void Release();
		void RegisterInstance(object instance);
		void RegisterInstance<T>(T instance);
		void RegisterProvider<T>(IProvider<T> provider);
	}
}