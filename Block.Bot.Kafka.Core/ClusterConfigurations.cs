using Confluent.Kafka;
using System;

namespace Block.Bot.Kafka.Core
{
    public class ClusterConfigurations
    {
        public static ProducerConfig GetProducerConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = "pkc-lq8v7.eu-central-1.aws.confluent.cloud:9092",
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = "YSGJKFEIFH26VZYG",
                SaslPassword = "OkjlAxHuIR1z03uPlGqcUiWj6b0B4Mj5gHOEN+h58+gaTwSE9m9Zhr3yEI7YLl6G",
                EnableSslCertificateVerification = false
            };
        }


        public static ConsumerConfig GetConsumerConfig()
        {
            return new ConsumerConfig
            {
                BootstrapServers = "pkc-lq8v7.eu-central-1.aws.confluent.cloud:9092",
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = "YSGJKFEIFH26VZYG",
                SaslPassword = "OkjlAxHuIR1z03uPlGqcUiWj6b0B4Mj5gHOEN+h58+gaTwSE9m9Zhr3yEI7YLl6G",
                AutoOffsetReset = AutoOffsetReset.Earliest

            };
        }

    }
}
