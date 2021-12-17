using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    /// <summary>
    /// --- Day 16: Packet Decoder ---
    /// </summary>
    public class Day16 : ISolveAoc
    {
        private readonly string _unitTest = @"D2FE28";
        private readonly string _unitTest1 = @"8A004A801A8002F478";
        private readonly string _unitTest2 = @"620080001611562C8802118E34";
        private readonly string _unitTest3 = @"C0015000016115A2E0802F182340";
        private readonly string _unitTest4 = @"A0016C880162017C3686B18A3D4780";

        private readonly string _unitTest5 = @"C200B40A82";
        private readonly string _unitTest6 = @"04005AC33890";
        private readonly string _unitTest7 = @"880086C3E88112";
        private readonly string _unitTest8 = @"CE00C43D881120";
        private readonly string _unitTest9 = @"D8005AC2A8F0";
        private readonly string _unitTest10 = @"F600BC2D8F";
        private readonly string _unitTest11 = @"9C005AC2F8F0";
        private readonly string _unitTest12 = @"9C0141080250320F1802104A08";

        public string Solve1stPart()
        {
            //var input = _unitTest4;
            var input = Utils.ReadInput("InputDay16.txt");
            var binaryInput = ToBinary(Convert.FromHexString(input));

            var paquet = new PacketReader(binaryInput).ReadPacket();

            var result = paquet.GetVersionSum();

            return $"{result}";
        }

        public string Solve2ndPart()
        {
            //var input = _unitTest12;
            var input = Utils.ReadInput("InputDay16.txt");
            var binaryInput = ToBinary(Convert.FromHexString(input));

            var paquet = new PacketReader(binaryInput).ReadPacket();

            var result = paquet.Eval();

            return $"{result}";
        }

        public static String ToBinary(Byte[] data)
        {
            return string.Join("", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }

        private class Packet
        {          

            public int Version { get; set; }
            public int TypeID { get; set; }

            public long Value { get; set; }

            public List<Packet> SubPackets { get; private set; } = new List<Packet>();

            public int BitsRead { get; set; }

            public int GetVersionSum()
            {
                var sum = this.Version;
                SubPackets.ForEach(x => sum += x.GetVersionSum());
                return sum;
            }

            private static readonly Func<Packet, long> Sum = p => { var total = 0L; p.SubPackets.ForEach(x => total += x.Eval()); return total; };
            private static readonly Func<Packet, long> Mul = p => {var total = 1L;p.SubPackets.ForEach(x => total *= x.Eval()); return total; };
            private static readonly Func<Packet, long> Min = p => {
                var total = long.MaxValue;
                p.SubPackets.ForEach(x =>
                {
                    var value = x.Eval();
                    if (value < total)
                    {
                        total = value;
                    }
                });
                return total;
            };
            private static readonly Func<Packet, long> Max = p => {
                var total = long.MinValue;
                p.SubPackets.ForEach(x =>
                {
                    var value = x.Eval();
                    if (value > total)
                    {
                        total = value;
                    }
                });
                return total;
            };
            private static readonly Func<Packet, long> Literal = p => p.Value;
            private static readonly Func<Packet, long> Greater = p => p.SubPackets[0].Eval() > p.SubPackets[1].Eval() ? 1L : 0L;
            private static readonly Func<Packet, long> Less = p => p.SubPackets[0].Eval() < p.SubPackets[1].Eval() ? 1L : 0L;
            private static readonly Func<Packet, long> Equal = p => p.SubPackets[0].Eval() == p.SubPackets[1].Eval() ? 1L : 0L;

            private readonly Func<Packet, long>[] _operators = new Func<Packet, long>[]
            {
               Sum,
               Mul,
               Min,
               Max,
               Literal,
               Greater,
               Less,
               Equal
            };

            public long Eval()
            {
                return _operators[TypeID](this);                
            }
        }
        private class PacketReader
        {

            private readonly StringReader _inputReader;

            private bool _endOfStream;

            public PacketReader(string input)
            {
                _inputReader = new StringReader(input);

            }

            public Packet ReadPacket()
            {
                var paquet = new Packet();

                paquet.Version = ReadPacketBits(paquet, 3);
                paquet.TypeID = ReadPacketBits(paquet, 3);

                if (paquet.TypeID == 4)
                {
                    var value = 0L;
                    var endLiteralFound = false;

                    while (!endLiteralFound)
                    {
                        var subNum = ReadPacketBits(paquet, 5);

                        // le masque à 16 permet de detecter si le premier bit est à 1... 
                        // (13)01101 & 16(10000) = 00000 -> 0
                        // (22)10110 & 16(10000) = 10000 -> 16 
                        endLiteralFound = (subNum & 16) == 0;

                        value <<= 4;
                        value |= (long)subNum & 0xf;
                    }
                    paquet.Value = value;
                }
                else
                {
                    var byPackets = ReadPacketBits(paquet, 1) == 1;
                    if (byPackets)
                    {
                        // Par nombre de paquets
                        ReadSubPacketsByPackets(paquet, ReadPacketBits(paquet, 11));
                    }
                    else
                    {
                        // Par Taille des paquets
                        ReadSubPacketsByLength(paquet, ReadPacketBits(paquet, 15));
                    }
                }
                return paquet;
            }

            private void ReadSubPacketsByPackets(Packet packet, long nbPackets)
            {
                while (!_endOfStream && nbPackets > 0)
                {
                    Packet subpacket = ReadPacket();
                    packet.BitsRead += subpacket.BitsRead;
                    nbPackets--;
                    packet.SubPackets.Add(subpacket);
                }
            }

            private void ReadSubPacketsByLength(Packet packet, long bitsLength)
            {
                while (!_endOfStream && bitsLength > 0)
                {
                    Packet subpacket = ReadPacket();
                    packet.BitsRead += subpacket.BitsRead;
                    bitsLength -= subpacket.BitsRead;
                    packet.SubPackets.Add(subpacket);
                }
            }

            private int ReadPacketBits(Packet packet, int length)
            {
                var buffer = new char[length];
                var byteReaded = _inputReader.Read(buffer);
                _endOfStream = byteReaded == 0;
                if (_endOfStream)
                    return -1;

                packet.BitsRead += byteReaded;
                var toConvert = new string(buffer);

                return Convert.ToInt32(toConvert, 2);
            }

        }
    }
}
