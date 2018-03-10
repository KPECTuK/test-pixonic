using Sim.Module.Generic;
using Sim.Module.Logger;

namespace Sim.Render.Realm
{
	public class UnityCompositionRoot : ServiceLocator, IRootContext
	{
		public UnityCompositionRoot(IContext context) : base(context)
		{
			RegisterInstance<IRootContext>(this);
			RegisterInstance<ILoggerFactory>(new DefaultLoggerFactory<DefaultLoggerImpl>());
		}
	}
}