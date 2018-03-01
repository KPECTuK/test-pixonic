using System.Collections.Generic;
using Newtonsoft.Json;
using Sim.Module.Generic;
using Sim.Module.Resources;
using Sim.Module.Serialization;

namespace Sim.Module.Data
{
	public class Repository
	{
		private readonly IContext _context;
		private readonly JsonSerializerSettings _serializerSettings;
		// config
		private RealmData _reamData;
		private HeroData[] _heroesCache;
		// stste
		private Dictionary<PlayerId, HeroData> _heroes = new Dictionary<PlayerId, HeroData>();

		public Repository(IContext context)
		{
			_context = context;
			_serializerSettings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Objects,
				SerializationBinder = new CustomTypeBinder(),
				Converters = new JsonConverter[] { new TimeSpanConverter(), }
			};
		}

		public void Load()
		{
			var resources = _context.Resolve<IResourceFactory>();
			_reamData = JsonConvert
				.DeserializeObject<RealmData>(
					resources.GetResource<string>(new ResourceLocator { Filename = "realm.json" }),
					_serializerSettings);
			_heroesCache = JsonConvert
				.DeserializeObject<HeroData[]>(
					resources.GetResource<string>(new ResourceLocator { Filename = "heroes.json" }),
					_serializerSettings);
		}

		public T GetDataContainer<T>() where T : class
		{
			//return _ultimateDataCache.OfType<T>().FirstOrDefault() ?? (_reamData as object) as T;
			return default(T);
		}
	}
}