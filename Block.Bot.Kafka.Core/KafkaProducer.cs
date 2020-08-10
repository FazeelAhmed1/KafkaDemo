using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Block.Bot.Kafka.Core
{
    public class KafkaProducer<TKey,TValue> : IDisposable
    {
        private readonly IProducer<TKey, TValue> _kafkaProducer;
        private readonly string _topic;
        public KafkaProducer(string topic)
        {
            this._topic = topic;
            this._kafkaProducer = new ProducerBuilder<TKey, TValue>(ClusterConfigurations.GetProducerConfig()).Build();
        }


        public void Send(TKey key,TValue payload)
        {
            this._kafkaProducer.ProduceAsync(this._topic, new Message<TKey, TValue> { Key = key, Value = payload })
                .ContinueWith(task => task.IsFaulted ? throw new Exception("Error producing message to Kafka") : $"Produced to: {task.Result.TopicPartitionOffset} {task.Result.Key}");
            
                      this._kafkaProducer.Flush(TimeSpan.FromSeconds(5));
        }

        public void Dispose()
        {
            this._kafkaProducer.Dispose();
        }
    }

}
