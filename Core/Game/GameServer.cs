using Core.Common.Services;
using Core.Game.Network;
using Core.Logs;
using Core.Security;
using Core.Utils;
using Core.Utils.NetworkBuffers;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;


namespace Core.Game
{
    public class GameServer
    {
        private static Logger<GameServer> _logger = Logger<GameServer>.BuildLogger();
        private readonly TcpListener _connectionListener;
        private readonly LoginServerService _loginServer;
        private List<GameClient> _activeClients = new();
        private List<GameClient> _toRemove = new();

        public GameServer(TcpListener tcpListener, LoginServerService loginServer)
        {
            _connectionListener = tcpListener;
            _loginServer = loginServer;
        }

        public bool Runing => true;

        public void Start()
        {
            _connectionListener.Start();
            _logger.Log($"Listening on : {_connectionListener.LocalEndpoint}");
        }

        public void Stop()
        {
            _connectionListener.Stop();
        }

        public void Tick()
        {
            if (_connectionListener.Pending())
            {
                ConnectClient(_connectionListener.AcceptTcpClient());
            }
            ReadActiveClients();
            RemoveInactiveClients();
        }

        private void ConnectClient(TcpClient client)
        {
            var gameClient = new GameClient(client, new NoCrypter());
            _activeClients.Add(gameClient);
            _logger.Log($"Get connection from : [{client}]");
        }

        private void ReadActiveClients()
        {
            foreach (var gameClient in _activeClients)
            {
                try
                {
                    if (gameClient.HasData())
                    {
                        ReadSingleClient(gameClient);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log($"Error from [{gameClient}] : {ex}");
                    _toRemove.Add(gameClient);
                    gameClient.ForceDisonnect();
                }
            }
        }

        private void RemoveInactiveClients()
        {
            foreach (var c in _toRemove)
            {
                _activeClients.Remove(c);
                _logger.Log($"Client removed from active clients : {c}");
            }
            _toRemove.Clear();
        }

        private void ReadSingleClient(GameClient client)
        {
            _logger.Log($"Recieveing data from [{client}]");
            var rawData = client.ReceiveRawData();
            ReadableBuffer buffer = new ReadableBuffer(rawData.Slice(2));
            int opCode = buffer.ReadByte();
            _logger.Log($"Received [{opCode.ToHex()}]");
            switch (opCode)
            {
                case 0:
                    {
                        int version = buffer.ReadInt();
                        _logger.Log($"Received [PROTOCOL_VERISON]({version}) from [{client}]");
                        var clientCrypt = new GameCrypt();
                        var key = clientCrypt.GetKey();

                        // send CryptInit
                        var cryptInit = new WriteableBuffer();
                        cryptInit.WriteByte(0x00)
                            .WriteByte(1); // 0 protocol missmatch 1 good
                        for (int i = 0; i < 8; i++)
                            cryptInit.WriteByte(key[i]);
                        cryptInit.WriteInt(1) // use encryption
                            .WriteInt(1) // server id??/
                            .WriteByte(1); // unknown

                        client.SendData(cryptInit.toByteArray());
                        client.SetCryptInterface(clientCrypt);
                    }
                    break;
                case 0x8:
                    {
                        _logger.Log($"Received [REQUEST_AUTH] from [{client}]");
                        //swapped on purpose. endian byte positions
                        string accId = buffer.ReadString();
                        int playKey2 = buffer.ReadInt();
                        int playKey1 = buffer.ReadInt();
                        int login1 = buffer.ReadInt();
                        int login2 = buffer.ReadInt();
                        var accDetails = new LSAccountDetails(accId, SessionKeys.FromValues(playKey1, playKey2, login1, login2));

                        _logger.Log($"accid:[{accId}] {playKey1} == {0}|{playKey2} == {0}|{login1} == {0}|{login2} == {0}");
                        if (_loginServer.IsAccountLoggedIn(accDetails))
                        {
                            //send charlist
                            var charInfo = BuildCharInfo(accDetails);
                            client.SendData(charInfo);
                            client.Skeys = accDetails.Skeys;
                        }

                    }
                    break;

                case 0x0d:
                    {
                        var charId = buffer.ReadInt();
                        _logger.Log($"[CHARACTER_SELECTED] id:[{charId}] from:", client);
                        var charSelected = BuildSelectedCharacter(client.Skeys.Play2);

                        client.SendData(charSelected);

                    }
                    break;
                case 0xd0:
                    {
                        _logger.Log($"[EX_PACKET] received from :", client);
                    }
                    break;
                case 0x03:
                    {
                        _logger.Log($"[ENTER_WORLD] received from :", client);
                        var userInfo = BuildMockUserInfo(client);
                        client.SendData(userInfo);
                        var changeMoveType = BuildChangeMoveType();
                        client.SendData(changeMoveType);
                        //quest list
                        client.SendData(new WriteableBuffer().WriteByte(0x80).WriteInt(0).toByteArray());
                        //magic effect icons
                        client.SendData(new WriteableBuffer().WriteByte(0x7f).WriteShort(0).toByteArray());

                        client.SendData(BuildStatusUpdate());

                        client.SendData(BuildHennaInfo());

                        //friend list
                        client.SendData(new WriteableBuffer().WriteByte(0xfa).WriteInt(0).toByteArray());
                        //item list
                        client.SendData(new WriteableBuffer().WriteByte(0x1b).WriteShort(0).WriteShort(0).toByteArray());
                        //short-cut list
                        client.SendData(new WriteableBuffer().WriteByte(0x45).WriteInt(0).toByteArray());

                        client.SendData(BuildExStorageMaxCount());

                        client.SendData(BuildMacroList());

                        client.SendData(BuildClientTime());

                        //skill list
                        client.SendData(new WriteableBuffer().WriteByte(0x58).WriteInt(0).toByteArray());

                        client.SendData(BuildTargetSelected());
                        //set compas zone
                        client.SendData(new WriteableBuffer().WriteByte(0xfe).WriteShort(0x32).WriteInt(0x0f).toByteArray());
                        //action failder
                        client.SendData(new WriteableBuffer().WriteByte(0x25).toByteArray());


                    }
                    break;
                default:
                    {
                        _logger.Log($"Unknown opcode [{opCode.ToHex()}] from [{client}]");
                    }
                    break;
            }
        }
        private byte[] BuildTargetSelected()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0x29)
                .WriteInt(1)
                .WriteInt(1)
                .WriteDouble(0).WriteDouble(0).WriteDouble(0);

            return packet.toByteArray();
        }

        private byte[] BuildClientTime()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0xec).WriteInt(7000).WriteInt(6);

            return packet.toByteArray();
        }

        private byte[] BuildMacroList()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0xe7).WriteInt(0).WriteBytes([0, 0, 0]);

            return packet.toByteArray();
        }

        private byte[] BuildExStorageMaxCount()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0xfe).WriteShort(0x2e);
            for (int i =0; i < 7;i++)
            {
                packet.WriteInt(0);
            }

            return packet.toByteArray();
        }

        private byte[] BuildHennaInfo()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0xE4);
            packet.WriteBytes([0, 0, 0, 0, 0, 0]);
            packet.WriteInt(0)
                .WriteInt(0);

            return packet.toByteArray();
        }

        private byte[] BuildStatusUpdate()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0xf3)
                .WriteInt(0) // dulist power
                .WriteInt(0) //weight penalty
                .WriteInt(0) // chat block
                .WriteInt(0) // danger zone
                .WriteInt(0)
                .WriteInt(0)
                .WriteInt(0); // penalty level 1-15

            return packet.toByteArray();
        }

        private byte[] BuildChangeMoveType()
        {
            var packet = new WriteableBuffer();
            packet.WriteByte(0x2e)
                .WriteInt(1)//obj id
                .WriteInt(1)//is running
                .WriteInt(0);

            return packet.toByteArray();
        }

        private byte[] BuildMockUserInfo(GameClient client)
        {
            var packet = new WriteableBuffer();
            var character = BuildMockCharacter();
            var info = character.Info;
            var stats = character.Stats;
            packet.WriteByte(0x04)

                .WriteInt((int)character.x)
                .WriteInt((int)character.y)
                .WriteInt((int)character.z)
                .WriteInt(0) // heading
                .WriteInt(info.ObjectId)

                .WriteString(info.Name)

                .WriteInt(info.Race)
                .WriteInt(info.Female ? 1 : 0)
                .WriteInt(info.CurrentClass)
                .WriteInt(info.Level)

                .WriteLong(info.Exp)

                .WriteInt(stats.STR)
                .WriteInt(stats.DEX)
                .WriteInt(stats.CON)
                .WriteInt(stats.INT)
                .WriteInt(stats.MEN)
                .WriteInt(stats.WIT)

                .WriteInt((int)info.MaxHealth)
                .WriteInt((int)info.CurrentHealth)
                .WriteInt((int)info.MaxMana)
                .WriteInt((int)info.CurrentMana)

                .WriteInt(info.Sp)
                .WriteInt(0) // current weight;
                .WriteInt(0) // max weight
                .WriteInt(20); // is weapon equipped 20 no 40 yes

            var itemIds = info.ObjectsId;
            packet.WriteInt(itemIds.D_HAIR)
                .WriteInt(itemIds.R_EAR)
                .WriteInt(itemIds.L_EAR)
                .WriteInt(itemIds.NECK)
                .WriteInt(itemIds.R_FINGER)
                .WriteInt(itemIds.L_FINGER)
                .WriteInt(itemIds.HEAD)
                .WriteInt(itemIds.R_HAND)
                .WriteInt(itemIds.L_HAND)
                .WriteInt(itemIds.GLOVES)
                .WriteInt(itemIds.CHEST)
                .WriteInt(itemIds.LEGS)
                .WriteInt(itemIds.FEET)
                .WriteInt(itemIds.BACK)
                .WriteInt(itemIds.LR_HAND)
                .WriteInt(itemIds.HAIR)
                .WriteInt(itemIds.FACE);

            itemIds = info.ItemsId;
            packet.WriteInt(itemIds.D_HAIR)
              .WriteInt(itemIds.R_EAR)
              .WriteInt(itemIds.L_EAR)
              .WriteInt(itemIds.NECK)
              .WriteInt(itemIds.R_FINGER)
              .WriteInt(itemIds.L_FINGER)
              .WriteInt(itemIds.HEAD)
              .WriteInt(itemIds.R_HAND)
              .WriteInt(itemIds.L_HAND)
              .WriteInt(itemIds.GLOVES)
              .WriteInt(itemIds.CHEST)
              .WriteInt(itemIds.LEGS)
              .WriteInt(itemIds.FEET)
              .WriteInt(itemIds.BACK)
              .WriteInt(itemIds.LR_HAND)
              .WriteInt(itemIds.HAIR)
              .WriteInt(itemIds.FACE);

            for (int i = 0; i < 14; i++)
                packet.WriteShort(0x00);

            packet.WriteInt(0x00);

            for (int i = 0; i < 12; i++)
                packet.WriteShort(0x00);

            packet.WriteInt(0);

            for (int i = 0; i < 4; i++)
                packet.WriteShort(0x00);

            packet.WriteInt(stats.Patk)
                .WriteInt(stats.PatkSpd)
                .WriteInt(stats.Pdef)
                .WriteInt(stats.Evasion)
                .WriteInt(stats.Accuracy)
                .WriteInt(stats.Crit)
                .WriteInt(stats.Matk)
                .WriteInt(stats.MatkSpd)
                .WriteInt(stats.PatkSpd)
                .WriteInt(stats.Mdef);

            packet.WriteInt(0) // pvp flag 
                .WriteInt(info.Karma);

            packet.WriteInt(stats.RunSpd)
                .WriteInt(stats.WalkSpd)
                .WriteInt(stats.RunSpd)//swim
                .WriteInt(stats.WalkSpd) //swim
                .WriteInt(0)
                .WriteInt(0)
                .WriteInt(0) // fly speed;
                .WriteInt(0) // fly speed;

                .WriteDouble(1.0)// mov multiplier
                .WriteDouble(1.0) // attck spd multiplier
                .WriteDouble(16.0) // coll radius
                .WriteDouble(32.0); // coll height

            packet.WriteInt(info.HairStyle)
                .WriteInt(info.HairColor)
                .WriteInt(info.Face)
                .WriteInt(1) // is gm

                .WriteString(character.Title);

            packet
                .WriteInt(info.ClanId)
                .WriteInt(0) // clan crest id
                .WriteInt(0) // ally id
                .WriteInt(0) // ally crest id
                .WriteInt(0) // is clan leader

                .WriteByte(0) // mount type?
                .WriteByte(0) // private store type 
                .WriteByte(0) // can craft

                .WriteInt(0) // pvp kills
                .WriteInt(0); // pk kills

            packet.WriteShort(0); // cubics
            packet.WriteByte(0); // end cubics

            packet.WriteInt(0); // abnormal effect mask
            packet.WriteByte(0);

            packet.WriteInt(0) // clan privs

                .WriteShort(0) // rec have
                .WriteShort(0) // rec left

                .WriteInt(0) // mount id?

                .WriteShort(7777) // inventory limit

                .WriteInt(info.CurrentClass)
                .WriteInt(0)
                .WriteInt(110) // max cp
                .WriteInt(0) // current cp

                .WriteByte(0) // enchant effect
                .WriteByte(0) // event circle 

                .WriteInt(0) // clan crest large id?

                .WriteByte(0) // is noble
                .WriteByte(0) // is hero

                .WriteByte(0) // is fishing

                .WriteInt(0)
                .WriteInt(0)
                .WriteInt(0) // fishing xyz

                .WriteInt(int.MaxValue) // name color
                .WriteByte(1) // is running

                .WriteInt(0) // pledge class
                .WriteInt(0) // pledge type
                .WriteInt(int.MaxValue /2 ) //title color
                .WriteInt(0); // curesed weapon


            var res = packet.toByteArray();
            return res;
        }

        private GameCharacter BuildMockCharacter()
        {
            var character =  new GameCharacter();
            character.Title = "1234567";
            character.Info = BuildMockCharacterSlot();
            character.Stats = BuildMockCharacterStats();
            return character;
        }

        struct CharacterStats
        {
            public int STR;
            public int DEX;
            public int CON;
            public int INT;
            public int WIT;
            public int MEN;

            public int Patk;
            public int PatkSpd;
            public int Pdef;
            public int Evasion;
            public int Accuracy;
            public int Crit;
            public int Matk;
            public int MatkSpd;
            public int Mdef;

            public int RunSpd;
            public int WalkSpd;
        }

        private CharacterStats BuildMockCharacterStats()
        {
            var stats = new CharacterStats();
            stats.STR = 10;
            stats.DEX = 10;
            stats.CON = 10;
            stats.INT = 10;
            stats.WIT = 10;
            stats.MEN = 10;

            stats.Patk = 10;
            stats.PatkSpd = 10;
            stats.Pdef = 10;
            stats.Evasion = 10;
            stats.Accuracy = 10;
            stats.Crit = 10;
            stats.Matk = 10;
            stats.MatkSpd = 10;
            stats.Mdef = 10;

            stats.RunSpd = 250;
            stats.WalkSpd = 127;

            return stats;
        }

        struct GameCharacter
        {
            public string Title;
            public CharacterSlotInfo Info;
            public double x, y, z;
            public CharacterStats Stats;
        }


        public byte[] BuildSelectedCharacter(int pkey2)
        {
            var character = BuildMockCharacter();
            var info = character.Info;
            var packet = new WriteableBuffer();
            packet.WriteByte(0x15)
                .WriteString(info.Name)
                .WriteInt(info.ObjectId)
                .WriteString(character.Title)
                .WriteInt(pkey2)
                .WriteInt(info.ClanId)
                .WriteInt(0)
                .WriteInt(info.Female ? 1 : 0)
                .WriteInt(info.Race)
                .WriteInt(info.CurrentClass)
                .WriteInt(1) // is active ?
                .WriteInt((int)character.x).WriteInt((int)character.y).WriteInt((int)character.z)
                .WriteDouble(info.CurrentHealth)
                .WriteDouble(info.CurrentMana)
                .WriteInt(info.Sp)
                .WriteLong(info.Exp)
                .WriteInt(info.Level)
                .WriteInt(info.Karma)
                .WriteInt(0);

            for (int i = 0; i < 38; i++)
                packet.WriteInt(0);

            packet.WriteInt(1234); // game time on server

            for (int i = 0; i < 14; i++)
                packet.WriteInt(0);


            return packet.toByteArray();
        }
        struct CharacterGear
        {
            public int D_HAIR;
            public int R_EAR;
            public int L_EAR;
            public int NECK;
            public int R_FINGER;
            public int L_FINGER;
            public int HEAD;
            public int R_HAND;
            public int L_HAND;
            public int GLOVES;
            public int CHEST;
            public int LEGS;
            public int FEET;
            public int BACK;
            public int LR_HAND;
            public int HAIR;
            public int FACE;
        }

        struct CharacterSlotInfo
        {
            public string Name;
            public int ObjectId;
            public int ClanId;
            public bool Female;
            public int Race;
            public int BaseClass;
            public double CurrentHealth;
            public double CurrentMana;
            public int Sp;
            public long Exp;
            public int Level;
            public int Karma;
            public CharacterGear ObjectsId;
            public CharacterGear ItemsId;
            public int HairStyle;
            public int HairColor;
            public int Face;
            public double MaxHealth;
            public double MaxMana;
            public int CurrentClass;

        }

        private CharacterSlotInfo BuildMockCharacterSlot()
        {
            var info = new CharacterSlotInfo();
            info.Name = "Hello world";
            info.ObjectId = 1125521;
            info.ClanId = 0;
            info.Female = true;
            info.Race = 1;
            info.BaseClass = 1;
            info.CurrentHealth = 500;
            info.CurrentMana = 250;
            info.Sp = 777;
            info.Exp = 1;
            info.Karma = 1;
            info.Level = 1;
            info.ObjectsId = new CharacterGear();
            info.ItemsId = new CharacterGear();
            info.MaxHealth = 9955;
            info.MaxMana = 1313;
            info.CurrentClass = 1;

            return info;
        }


        private byte[] BuildCharInfo(LSAccountDetails accDetails)
        {
            var characterSlosts = new List<CharacterSlotInfo>();
            characterSlosts.Add(BuildMockCharacterSlot());
            characterSlosts.Add(BuildMockCharacterSlot());
            characterSlosts.Add(BuildMockCharacterSlot());
            characterSlosts.Add(BuildMockCharacterSlot());
            characterSlosts.Add(BuildMockCharacterSlot());
            var packet = new WriteableBuffer();
            packet.WriteByte(0x13)
                .WriteInt(characterSlosts.Count); // characters count;

            foreach (var character in characterSlosts)
            {
                packet.WriteString(character.Name)
                    .WriteInt(character.ObjectId)
                    .WriteString(accDetails.Id)
                    .WriteInt(accDetails.Skeys.Play1)
                    .WriteInt(character.ClanId)
                    .WriteInt(0) // ?
                    .WriteInt(character.Female ? 1 : 0)
                    .WriteInt(character.Race)
                    .WriteInt(character.BaseClass)
                    .WriteInt(1)  // ?
                    .WriteInt(0).WriteInt(0).WriteInt(0) // xyz
                    .WriteDouble(character.CurrentHealth)
                    .WriteDouble(character.CurrentMana)
                    .WriteInt(character.Sp)
                    .WriteLong(character.Exp)
                    .WriteInt(character.Level)
                    .WriteInt(character.Karma);
                for (int i = 0; i < 9; i++)
                    packet.WriteInt(0);

                var itemIds = character.ObjectsId;
                packet.WriteInt(itemIds.D_HAIR)
                    .WriteInt(itemIds.R_EAR)
                    .WriteInt(itemIds.L_EAR)
                    .WriteInt(itemIds.NECK)
                    .WriteInt(itemIds.R_FINGER)
                    .WriteInt(itemIds.L_FINGER)
                    .WriteInt(itemIds.HEAD)
                    .WriteInt(itemIds.R_HAND)
                    .WriteInt(itemIds.L_HAND)
                    .WriteInt(itemIds.GLOVES)
                    .WriteInt(itemIds.CHEST)
                    .WriteInt(itemIds.LEGS)
                    .WriteInt(itemIds.FEET)
                    .WriteInt(itemIds.BACK)
                    .WriteInt(itemIds.LR_HAND)
                    .WriteInt(itemIds.HAIR)
                    .WriteInt(itemIds.FACE);
                itemIds = character.ItemsId;
                packet.WriteInt(itemIds.D_HAIR)
                  .WriteInt(itemIds.R_EAR)
                  .WriteInt(itemIds.L_EAR)
                  .WriteInt(itemIds.NECK)
                  .WriteInt(itemIds.R_FINGER)
                  .WriteInt(itemIds.L_FINGER)
                  .WriteInt(itemIds.HEAD)
                  .WriteInt(itemIds.R_HAND)
                  .WriteInt(itemIds.L_HAND)
                  .WriteInt(itemIds.GLOVES)
                  .WriteInt(itemIds.CHEST)
                  .WriteInt(itemIds.LEGS)
                  .WriteInt(itemIds.FEET)
                  .WriteInt(itemIds.BACK)
                  .WriteInt(itemIds.LR_HAND)
                  .WriteInt(itemIds.HAIR)
                  .WriteInt(itemIds.FACE);

                packet.WriteInt(character.HairStyle)
                    .WriteInt(character.HairColor)
                    .WriteInt(character.Face)
                    .WriteDouble(character.MaxHealth)
                    .WriteDouble(character.MaxMana)
                    .WriteInt(0) // deleting time
                    .WriteInt(character.CurrentClass)
                    .WriteInt(0) // is active
                    .WriteByte(0) // enchant effect
                    .WriteInt(0); // augmentation id
            }


            return packet.toByteArray();
        }
    }
}
