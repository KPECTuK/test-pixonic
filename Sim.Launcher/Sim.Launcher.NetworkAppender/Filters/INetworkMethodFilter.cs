namespace Sim.Launcher.NetworkAppender.Filters
{
	internal interface INetworkMethodFilter<TChank>
	{
		byte[] ConvertToBuffer(TChank source);
	}
}