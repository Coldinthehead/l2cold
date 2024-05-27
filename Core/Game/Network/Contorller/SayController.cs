using Core.Common.Network;
using Core.Game.Network.Controller;
using Core.Game.World.Components;
using Core.Utils.Math;


namespace Core.Game.Network.Contorller
{
    internal class SayController : IPacketController
    {
        public void Run(GameClient client, ReadableBuffer message)
        {
            var text = message.ReadString();
            var type = message.ReadInt();
            Console.WriteLine($"[{type}]<{text}>");
            try
            {
                var split = text.Split(" ");
                if (split[0].ToLower().Equals("speed"))
                {
                    int newSpeed = int.Parse(split[1]);
                    newSpeed = MathC.Clamp(newSpeed, 1, 2000);
                    var player = client.Player.GetComponent<PlayerState>();
                    player.setSpeed(newSpeed);
                Console.WriteLine("new speed : " + newSpeed);
                }
            }
            catch ( Exception ex )
            {
                
            }
        }
    }
}
