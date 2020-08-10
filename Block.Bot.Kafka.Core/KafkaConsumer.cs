using Block.Bot.Kafka.Core.Model;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Block.Bot.Kafka.Core
{
    public class KafkaConsumer<TKey, TValue> : IKafkaConsumer<TKey,TValue>, IDisposable
    {
        private IConsumer<TKey, TValue> Consumer { get; set; }
        public string ConsumerGroup { get; set; }
        public string Topic { get; set; }
        public IMessageHandler<TKey, TValue> MessageHandler { get; set; }
        private Thread _pollLoopThread;

        public KafkaConsumer(string consumerGroup, string topic, IMessageHandler<TKey, TValue> handler)
        {
            // Resolve writer through config
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
            Trace.Indent();
            Trace.WriteLine("Initializing Consumer :: {0}", Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().CodeBase));
            ConsumerGroup = consumerGroup;
            Topic = topic;
            MessageHandler = handler;
            BuildConsumer();
            Consumer.Subscribe(Topic);
            Trace.WriteLine("Started Consumer - Ready to Listen");
            Trace.WriteLine("--------------------------------------------------------------------------------------------");

        }

        private void BuildConsumer()
        {
            var consumerConfig = ClusterConfigurations.GetConsumerConfig();
            consumerConfig.GroupId = ConsumerGroup;            
            Consumer = new ConsumerBuilder<TKey, TValue>(consumerConfig).Build();
        }


        public void Dispose()
        {
            Consumer.Close();
            Consumer.Dispose();
        }



        public void Start(CancellationTokenSource cancellationToken)
        {
            _pollLoopThread = new Thread(() => {
                try
                {
                    try
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            var message = Consumer.Consume(cancellationToken.Token);
                            MessageHandler.Process(message);
                        }
                    }
                    catch (ConsumeException cex)
                    {
                        Console.WriteLine($" Error: { cex.Error.Reason} ");
                    }
                    Dispose();
                    

                }
                catch(OperationCanceledException)
                {
                    StopAsync(cancellationToken).GetAwaiter().GetResult();
                    Dispose();
                }
            });

            _pollLoopThread.Start();
          
        }
        public async Task StopAsync(CancellationTokenSource cancellationToken)
        {
            await Task.Run(() =>
            {
                cancellationToken.Cancel();
                _pollLoopThread.Join();
            });
        }
    }

}
