using Microsoft.Extensions.Configuration;
using Cron.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Cron
{
    public class ProtocolSettings
    {
        public uint Magic { get; }
        public byte AddressVersion { get; }
        public string[] StandbyValidators { get; }
        public string[] SeedList { get; }
        public IReadOnlyDictionary<TransactionType, Fixed8> SystemFee { get; }
        public Fixed8 LowPriorityThreshold { get; }
        public uint SecondsPerBlock { get; }
        public uint FreeGasChangeHeight { get; }

        public uint StateRootEnableIndex { get; }
        public Fixed8 MinimumNetworkFee { get; }
        static ProtocolSettings _default;

        static bool UpdateDefault(IConfiguration configuration)
        {
            var settings = new ProtocolSettings(configuration.GetSection("ProtocolConfiguration"));
            return null == Interlocked.CompareExchange(ref _default, settings, null);
        }

        public static bool Initialize(IConfiguration configuration)
        {
            return UpdateDefault(configuration);
        }

        public static ProtocolSettings Default
        {
            get
            {
                if (_default == null)
                {
                    var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile("protocol.json", true)
                        .AddJsonFile($"protocol.{environmentName}.json", true)
                        .AddEnvironmentVariables()
                        .Build();
                    UpdateDefault(configuration);
                }

                return _default;
            }
        }

        private ProtocolSettings(IConfigurationSection section)
        {
            this.Magic = section.GetValue("Magic", 0x40AF90Au);
            this.AddressVersion = section.GetValue("AddressVersion", (byte)0x17);
            IConfigurationSection section_sv = section.GetSection("StandbyValidators");
            if (section_sv.Exists())
                this.StandbyValidators = section_sv.GetChildren().Select(p => p.Get<string>()).ToArray();
            else
                this.StandbyValidators = new[]
                {
                    "035d64f7ff9a02d7c0f8c11977ac04cc82c7ba1fb7d7932b926e561cdee11fe402",
                    "0266fad048f713b12cf54008bf73e4310b70bdb049873c9126008e512f167b2612",
                    "0326ab6643f8b687d5b2b2a6fb5d593c4a6e35cf0f91c80efaf4c87590f872c70b",
                    "021b0ca082ebc1452c10e794d18ad6c78345ff0b00777a5b842cfa925f3d3b72c4"
                };
            IConfigurationSection section_sl = section.GetSection("SeedList");
            if (section_sl.Exists())
                this.SeedList = section_sl.GetChildren().Select(p => p.Get<string>()).ToArray();
            else
                this.SeedList = new[]
                {
                    "seed1.cron.global:10333",
                    "seed2.cron.global:10333",
                    "seed3.cron.global:10333",
                    "seed4.cron.global:10333",
                    "seed5.cron.global:10333",
                    "seed6.cron.global:10333",
                    "seed7.cron.global:10333",
                    "seed8.cron.global:10333",
                    "seed9.cron.global:10333"
                };
            Dictionary<TransactionType, Fixed8> sys_fee = new Dictionary<TransactionType, Fixed8>
            {
                [TransactionType.EnrollmentTransaction] = Fixed8.FromDecimal(1000),
                [TransactionType.IssueTransaction] = Fixed8.FromDecimal(500),
                [TransactionType.PublishTransaction] = Fixed8.FromDecimal(500),
                [TransactionType.RegisterTransaction] = Fixed8.FromDecimal(10000)
            };
            foreach (IConfigurationSection child in section.GetSection("SystemFee").GetChildren())
            {
                TransactionType key = (TransactionType)Enum.Parse(typeof(TransactionType), child.Key, true);
                sys_fee[key] = Fixed8.Parse(child.Value);
            }
            this.SystemFee = sys_fee;
            this.SecondsPerBlock = section.GetValue("SecondsPerBlock", 15u);
            this.StateRootEnableIndex = section.GetValue("StateRootEnableIndex", 0u);
            this.LowPriorityThreshold = Fixed8.Parse(section.GetValue("LowPriorityThreshold", "0.001"));
            this.MinimumNetworkFee = Fixed8.Parse(section.GetValue("MinimumNetworkFee", "0"));
            this.FreeGasChangeHeight = section.GetValue("FreeGasChangeHeight", 100000000u);
        }
    }
}
