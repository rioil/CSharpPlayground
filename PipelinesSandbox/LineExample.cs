using System.Buffers;
using System.IO.Pipelines;

namespace PipelinesSandbox
{
    internal class LineExample
    {
        public static async Task ExecuteAsync()
        {
            var pipe = new Pipe();
            var writing = FillPipeAsync(pipe.Writer);
            var reading = ReadPipeAsync(pipe.Reader);

            await Task.WhenAll(writing, reading);
        }

        private static async Task FillPipeAsync(PipeWriter writer)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    int bytesWrite = Random.Shared.Next(2, 10);
                    var memory = writer.GetMemory(bytesWrite);
                    Random.Shared.NextBytes(memory.Span[..(bytesWrite - 1)]);
                    memory.Span[bytesWrite - 1] = (byte)'\n';

                    PrintSpan("W", memory.Span[..bytesWrite]);
                    writer.Advance(bytesWrite);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.ToString());
                    break;
                }

                var result = await writer.FlushAsync();

                if (result.IsCompleted)
                {
                    break;
                }

                await Task.Delay(Random.Shared.Next(0, 1000));
            }

            await writer.CompleteAsync();
            Console.WriteLine("Writer completed!");
        }

        private static async Task ReadPipeAsync(PipeReader reader)
        {
            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;

                while (TryReadLine(ref buffer, out var line))
                {
                    PrintSequence("R", line);
                }

                reader.AdvanceTo(buffer.Start, buffer.End);

                if (result.IsCompleted)
                {
                    break;
                }
            }

            await reader.CompleteAsync();
            Console.WriteLine("Reader completed!");
        }

        private static bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
        {
            var position = buffer.PositionOf((byte)'\n');

            if (position == null)
            {
                line = default;
                return false;
            }

            line = buffer.Slice(0, position.Value);
            buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
            return true;
        }

        private static void PrintSequence(string header, ReadOnlySequence<byte> sequence)
        {
            Console.Write($"[{header}]");
            foreach (var memory in sequence)
            {
                foreach (var b in memory.Span)
                {
                    Console.Write($" {b:X2}");
                }
            }
            Console.WriteLine();
        }

        private static void PrintSpan(string header, ReadOnlySpan<byte> span)
        {
            Console.Write($"[{header}]");
            foreach (var b in span)
            {
                Console.Write($" {b:X2}");
            }
            Console.WriteLine();
        }
    }
}
