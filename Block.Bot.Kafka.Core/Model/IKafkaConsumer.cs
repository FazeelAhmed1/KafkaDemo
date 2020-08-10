using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Block.Bot.Kafka.Core.Model
{
    public interface IKafkaConsumer<TKey,TValue>
    {
        string ConsumerGroup { get; set; }
        string Topic { get; set; }
        
        IMessageHandler<TKey,TValue> MessageHandler { get; set; }
        void Start(CancellationTokenSource cancellationToken = default);
    }
}
