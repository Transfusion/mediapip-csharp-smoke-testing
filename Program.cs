using System;
using Akihabara.Framework;
using Akihabara.Framework.Packet;
using Akihabara.Framework.Port;

namespace mediapipe_smoke_test
{
    class Program
    {
        static void Main(string[] args)
        {
            const string graphConfigText = @"
input_stream: ""in""
output_stream: ""out""
node {
  calculator: ""PassThroughCalculator""
  input_stream: ""in""
  output_stream: ""out1""
}
node {
  calculator: ""PassThroughCalculator""
  input_stream: ""out1""
  output_stream: ""out""
}
";
            Console.WriteLine("Hello World!");
            // Akihabara.Framework.ProtoCalculator.CalculatorGraphConfig config = 
            const string inputStream = "in";
            const string outputStream = "out";
            OutputStreamPoller<string> outputStreamPoller;
            CalculatorGraph graph = new CalculatorGraph(graphConfigText);
            outputStreamPoller = graph.AddOutputStreamPoller<string>(outputStream).Value();

            Console.WriteLine("Successfully added Hello World graph!");

            Status graphStartResult = graph.StartRun();
            Console.WriteLine(graphStartResult);


            int timestamp = System.Environment.TickCount & System.Int32.MaxValue;
            var inputPacket = new StringPacket("Hello World", new Timestamp(timestamp));

            StringPacket outputPacket = new StringPacket();
            Status pushedInput = graph.AddPacketToInputStream(inputStream, inputPacket);

            Console.WriteLine("Successfully pushed input packet!");

            if (outputStreamPoller.Next(outputPacket))
            {
                Console.WriteLine($"{outputPacket.Get()}");
            }

        }
    }
}
