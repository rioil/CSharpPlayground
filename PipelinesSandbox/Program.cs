using System.Buffers;
using System.IO.Pipelines;

namespace PipelinesSandbox
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //await LineExample.ExecuteAsync();
            await PacketExample.ExecuteAsync();
        }        
    }
}
