using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memoria;

namespace TemplateCS
{
    class Program
    {
        static void Main(string[] args)
        {
            Mem m = new Mem();

            m.GetProcess("csgo"); // Identificando processo do jogo

            var client = m.GetModuleBase("client.dll"); // Obtendo endereço base

            int localPlayer = 0xDB35DC;
            int health = 0x100;

            var buffer = m.RPM(client, localPlayer);

            while (true)
            {
                var hp = BitConverter.ToInt32(m.ReadBytes(buffer, health, 4), 0);

                Console.WriteLine("Vida do jogador --> " + hp);
            }
        }
    }
}
