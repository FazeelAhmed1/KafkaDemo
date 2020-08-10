using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace Block.Bot.Kafka.Core.Model
{
    public interface IMessageHandler<TKey,TValue>
    {
        void Process(ConsumeResult<TKey, TValue> message);
    }
}
