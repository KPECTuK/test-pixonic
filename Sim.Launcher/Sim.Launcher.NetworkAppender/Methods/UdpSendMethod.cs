using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using log4net.Util;
using Sim.Launcher.NetworkAppender.Filters;
using Sim.Module.Extensions;

namespace Sim.Launcher.NetworkAppender.Methods
{
	internal class UdpSendMethod : NetworkMethodBase<string>
	{
		private const string DEFAULT_PORT = "60050";
		private const string DEFAULT_ADDRESS = "localhost";

		private readonly IPEndPoint[] _targets;
		private readonly HashSet<BufferContainer> _buffers = new HashSet<BufferContainer>();

		private class BufferContainer
		{
			internal readonly byte[] Buffer;

			private readonly HashSet<UdpClient> _emitters = new HashSet<UdpClient>();

			internal int Emitters
			{
				get
				{
					lock(_emitters)
					{
						return _emitters.Count;
					}
				}
			}

			internal bool IsValuable => Buffer != null && Buffer.Length > 0;
			internal int Size => Buffer?.Length ?? 0;

			internal BufferContainer(byte[] buffer)
			{
				Buffer = buffer;
			}

			internal UdpClient GetEmitter()
			{
				if(!IsValuable)
				{
					throw new Exception("not valuable");
				}
				var emitter = new UdpClient();
				_emitters.Add(emitter);
				return emitter;
			}

			internal void ReleaseEmitter(UdpClient emitter)
			{
				lock(_emitters)
				{
					_emitters.Remove(emitter);
				}
			}
		}

		private class EmitterBinder
		{
			internal BufferContainer Buffer { get; private set; }
			internal UdpClient Emitter { get; private set; }

			internal EmitterBinder(BufferContainer buffer, UdpClient emitter)
			{
				Buffer = buffer;
				Emitter = emitter;
			}

			internal void Release()
			{
				Buffer = null;
				Emitter = null;
			}
		}

		internal UdpSendMethod(string connectionString, IEnumerator<string> cursor, INetworkMethodFilter<string> filter)
			: base(cursor, filter)
		{
			_targets =
				connectionString
					.Split(';')
					.Where(inspecting => !string.IsNullOrEmpty(inspecting))
					.SelectMany(record =>
					{
						var tokens = record.Split(':');
						var address = tokens.Length > 0
							? tokens[0]
							: DEFAULT_ADDRESS;
						var portToken = tokens.Length > 1
							? tokens[1]
							: DEFAULT_PORT;
						var converter = TypeDescriptor.GetConverter(typeof(int));
						var port =
							converter
								.CanConvertFrom(
									portToken
										.GetType())
								? converter.ConvertFrom(portToken)
								: converter.ConvertFrom(DEFAULT_PORT);
						return ResolveTarget(address, (int)port);
					})
					.ToArray();
			LogLog.Debug(GetType(), $"start emit from:\n{_targets.ToText()}");
		}

		public override void Commit()
		{
			BufferContainer buffer;
			while((buffer = new BufferContainer(GetBuffer())).IsValuable)
			{
				_buffers.Add(buffer);
				var binder = new EmitterBinder(buffer, buffer.GetEmitter());
				Array.ForEach(_targets, target => binder.Emitter.BeginSend(binder.Buffer.Buffer, buffer.Size, target, Committed, binder));
			}
		}

		private void Committed(IAsyncResult state)
		{
			var binder = state.AsyncState as EmitterBinder;
			if(binder != null)
			{
				binder.Buffer.ReleaseEmitter(binder.Emitter);
				if(binder.Buffer.Emitters == 0)
				{
					_buffers.Remove(binder.Buffer);
				}
				binder.Release();
			}
		}

		public override void Dispose()
		{
			Commit();
		}
	}
}