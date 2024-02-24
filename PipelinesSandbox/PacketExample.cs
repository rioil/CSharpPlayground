using System.Buffers;
using System.IO.Pipelines;

namespace PipelinesSandbox
{
    /**
     * The packet that this program parses has the following structure.
     * | STX (1byte [0x02]) | No. (1byte) | Length (1byte) | Content ($Length byte) | ETX (1byte [0x03]) |
     */
    internal class PacketExample
    {
        private const byte STX = 0x02;
        private const byte ETX = 0x03;

        private static readonly object _lockObject = new();

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
                    var contentLength = Random.Shared.Next(1, 10);
                    var packet = CreatePacket(i, contentLength);
                    await writer.WriteAsync(packet);
                    PrintSpan("W", packet);
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

        private static byte[] CreatePacket(int no, int contentLength)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(no, byte.MinValue);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(no, byte.MaxValue);
            ArgumentOutOfRangeException.ThrowIfLessThan(contentLength, byte.MinValue);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(contentLength, byte.MaxValue);

            byte[] contentBytes = new byte[contentLength];
            Random.Shared.NextBytes(contentBytes);

            return [STX, (byte)no, (byte)contentLength, .. contentBytes, ETX];
        }

        private static async Task ReadPipeAsync(PipeReader reader)
        {
            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;

                while (TryReadPacket(ref buffer, out var packet))
                {
                    PrintSequence("R", packet);
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

        private static bool TryReadPacket(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> packet)
        {
            int index = 0;

            // check if the first byte is STX
            var stxPosition = buffer.PositionOf(STX);
            if (stxPosition == null)
            {
                packet = default;
                return false;
            }
            if (!buffer.Start.Equals(stxPosition.Value))
            {
                packet = default;
                return false;
            }
            if (buffer.FirstSpan[0] != STX)
            {
                packet = default;
                return false;
            }
            index++;

            // get no. of packet
            if (!TryGetSliced(buffer, index++, 1, out var _))
            {
                packet = default;
                return false;
            }

            // get length of packet
            if (!TryGetSliced(buffer, index++, 1, out var lengthByte))
            {
                packet = default;
                return false;
            }
            var length = lengthByte.FirstSpan[0];

            // get content of packet
            if (!TryGetSliced(buffer, index, length, out var _))
            {
                packet = default;
                return false;
            }
            index += length;

            // check if the last byte is ETX
            if(!TryGetSliced(buffer, index++, 1, out var etxByte) || etxByte.FirstSpan[0] != ETX)
            {
                packet = default;
                return false;
            }

            /*
             * ... | ETX | ...
             *     |     ^ etxByte.End
             *     ^ etxByte.Start
             * 
             * buffer.Slice(0, etxByte.Start) excludes ETX
             * buffer.Slice(0, etxByte.End) includes it
             */
            packet = buffer.Slice(0, etxByte.End);
            buffer = buffer.Slice(etxByte.End);
            return true;
        }

        private static void PrintSequence(string header, ReadOnlySequence<byte> sequence)
        {
            lock (_lockObject)
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
        }

        private static void PrintSpan(string header, ReadOnlySpan<byte> span)
        {
            lock (_lockObject)
            {
                Console.Write($"[{header}]");
                foreach (var b in span)
                {
                    Console.Write($" {b:X2}");
                }
                Console.WriteLine();
            }
        }

        private static bool TryGetSliced(ReadOnlySequence<byte> sequence, int offset, int length, out ReadOnlySequence<byte> sliced)
        {
            if (sequence.Length < offset + length)
            {
                sliced = default;
                return false;
            }

            sliced = sequence.Slice(offset, length);
            return true;
        }
    }
}
