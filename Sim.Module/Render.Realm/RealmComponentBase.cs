using System.Linq;
using Sim.Module.Client;
using Sim.Module.Data;
using Sim.Module.Data.Config;
using Sim.Module.Data.State;
using Sim.Module.Generic;
using Sim.Module.Simulation;
using UnityEngine;

namespace Sim.Render.Realm
{
	public abstract class RealmComponentBase : MonoBehaviour
	{
		private IContext _context;
		private readonly Color[] _colors = { Color.red, Color.blue, Color.green, Color.yellow };

		private void RenderFrame(IContext clientContext)
		{
			// TODO: use behaviour

			if(!clientContext.Resolve<INetworkClientConnection>().IsEstablished)
			{
				return;
			}

			var repository = clientContext.Resolve<IRepository>();
			var realmState = repository.GetPlayerStates(null).ToArray();
			var heroConfig = repository.GetConfig<HeroData>(null).ToArray();
			var index = -1;
			foreach(var state in realmState.Where(_ => !ReferenceEquals(null, _)))
			{
				var color = _colors[++index];
				var config = heroConfig.FirstOrDefault(_ => _.Name.Equals(state.HeroName));
				DrawPawn(color, state);
				DrawInfluenceArea(color, state, config);
				DrawTitle(state, config);
			}
		}

		protected virtual void DrawPawn(Color color, PlayerState state)
		{
			var rotation = Quaternion.FromToRotation(Vector3.forward, state.Speed);
			var leftPoint = rotation * (Vector3.up + Vector3.left * .3f);
			var rightPoint = rotation * (Vector3.up + Vector3.right * .3f);
			Debug.DrawLine(state.Position, state.Position + leftPoint, color);
			Debug.DrawLine(state.Position, state.Position + rightPoint, color);
			Debug.DrawLine(state.Position + leftPoint, state.Position + rightPoint, color);
			Debug.DrawLine(state.Position, state.Position + state.Speed, Color.blue);
		}

		protected virtual void DrawInfluenceArea(Color color, PlayerState state, HeroData config)
		{
			var center = state.Position;
			const float step = 2f * Mathf.PI / 12;
			for(float sector = 0; sector < 2f * Mathf.PI; sector += step)
			{
				var start = new Vector3(Mathf.Sin(sector), 0f, Mathf.Cos(sector)) * config.AttackRadius;
				var end = new Vector3(Mathf.Sin(sector - step), 0f, Mathf.Cos(sector - step)) * config.AttackRadius;
				Debug.DrawLine(center + start, center + end, color);
			}
		}
		
		protected virtual void DrawTitle(PlayerState state, HeroData config)
		{
			var rotation = Quaternion.FromToRotation(Vector3.forward, state.Speed);
			var leftPoint = rotation * (Vector3.up * 1.2f + Vector3.left * .3f);
			var rightPoint = rotation * (Vector3.up * 1.2f + Vector3.right * .3f);
			var full = (rightPoint - leftPoint) * state.HitPoints / config.HitPoints;

			Debug.DrawLine(state.Position + leftPoint, state.Position + leftPoint + full, Color.red);
		}

		// ReSharper disable once UnusedMember.Global
		protected virtual void Awake()
		{
			if(!Application.isPlaying)
			{
				return;
			}

			_context = new ServerCompositionRoot(new UnityCompositionRoot(null));
			_context.Resolve<SimulatorServer>().Initialize();
		}

		// ReSharper disable once UnusedMember.Global
		protected virtual void Update()
		{
			if(ReferenceEquals(null, _context))
			{
				return;
			}

			var simulator = _context.Resolve<SimulatorServer>();
			simulator.Update();
			RenderFrame(simulator.GetConnectionContext(simulator.GetLocalPlayerId()));
		}

		// ReSharper disable once UnusedMember.Global
		protected virtual void OnDisable()
		{
			_context?.Release();
		}
	}
}