using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GameServerForRPG.Test
{
    public class ServerTest
    {
        public ServerTest()
        {
        }

        private FakeTransport transport;
        private FakeClock clock;
        private GameServer server;

        //Called before every [Test]
        [SetUp]
        public void SetUp()
        {
            transport = new FakeTransport();
            clock = new FakeClock();
            server = new GameServer(transport, clock);
        }

        //Test if a new server has a serverCloak at zero
        [Test]
        public void TestGameServerTimeZeroGreenLigth()
        {
            Assert.That(server.Now, Is.EqualTo(0));
        }
        [Test]
        public void TestGameServerTimeZeroRedLigth()
        {
            Assert.That(server.Now, Is.Not.EqualTo(1));
        }

        //Test if a new server has no room we created
        [Test]
        public void TestGameServerRoomZeroGreenLight()
        {
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(0));
        }
        [Test]
        public void TestGameServerRoomZeroRedLight()
        {
            Assert.That(server.GetNumberOfRoom(), Is.Not.EqualTo(1));
        }

        //Test if a new server has no room we created
        [Test]
        public void TestGameServerClientsZeroGreenLight()
        {
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(0));
        }
        [Test]
        public void TestGameServerClientsZeroRedLight()
        {
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.Not.EqualTo(1));
        }

        //Test if a new server has no room we created
        [Test]
        public void TestGameServerGameObjectZeroGreenLight()
        {
            Assert.That(server.GetAllGameObjectsSpawnedInThisServer(), Is.EqualTo(0));
        }
        [Test]
        public void TestGameServerGameObjectZeroRedLight()
        {
            Assert.That(server.GetAllGameObjectsSpawnedInThisServer(), Is.Not.EqualTo(1));
        }

        //Test join number of client
        [Test]
        public void TestGameServerJoinNumOfClientsGreenLigth()
        {
            Packet packet = new Packet(0);
            transport.ClientEnqueue(packet, "tester", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(1));
        }
        [Test]
        public void TestGameServerJoinNumOfClientsRedLigth()
        {
            Packet packet = new Packet(0);
            transport.ClientEnqueue(packet, "tester", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.Not.EqualTo(0));
        }

        //Test join room creation
        [Test]
        public void TestGameServerRoomCreationGreenLigth()
        {
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(0));
            Packet packet = new Packet(0);
            transport.ClientEnqueue(packet, "tester", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(1));

            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(1));
            uint roomID = BitConverter.ToUInt32(transport.ClientDequeue().data, 1);
            ServerRoom room = server.GetRoomFromID(roomID);
            Assert.That(room, Is.Not.EqualTo(null));
        }
        [Test]
        public void TestGameServerRoomCreationRedLigth()
        {
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(0));
            Packet packet = new Packet(0);
            transport.ClientEnqueue(packet, "tester", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.Not.EqualTo(0));

            Assert.That(server.GetNumberOfRoom(), Is.Not.EqualTo(0));
            uint roomID = BitConverter.ToUInt32(transport.ClientDequeue().data, 1);
            ServerRoom room = server.GetRoomFromID(roomID);
            Assert.That(room, Is.Not.EqualTo(null));
        }

        //Test client enter in a emptyRoom
        [Test]
        public void TestGameServerJoinEmptyRoomGreenLigth()
        {
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(0));
            Packet packet = new Packet(0);
            transport.ClientEnqueue(packet, "tester", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(1));
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(1));
            uint roomID = BitConverter.ToUInt32(transport.ClientDequeue().data, 1);
            ServerRoom room = server.GetRoomFromID(roomID);
            Assert.That(room, Is.Not.EqualTo(null));

            Assert.That(room.RoomID, Is.EqualTo(roomID));
            Assert.That(room.GetClientsNumber, Is.EqualTo(1));
        }
        [Test]
        public void TestGameServerJoinEmptyRoomRedLigth()
        {
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(0));
            Packet packet = new Packet(0);
            transport.ClientEnqueue(packet, "tester", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(1));
            Assert.That(server.GetNumberOfRoom(), Is.Not.EqualTo(0));
            uint roomID = BitConverter.ToUInt32(transport.ClientDequeue().data, 1);
            ServerRoom room = server.GetRoomFromID(roomID);
            Assert.That(room, Is.Not.EqualTo(null));

            Assert.That(room.RoomID, Is.EqualTo(roomID));
            Assert.That(room.GetClientsNumber, Is.Not.EqualTo(0));
        }

        //Test room capacity, second client join
        [Test]
        public void TestRoomJoinInANotEmptyRoomGreenLight()
        {
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(0));
            Packet packetClient00 = new Packet(0);
            transport.ClientEnqueue(packetClient00, "tester", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(1));
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(1));
            uint roomID = BitConverter.ToUInt32(transport.ClientDequeue().data, 1);
            ServerRoom room = server.GetRoomFromID(roomID);
            Assert.That(room, Is.Not.EqualTo(null));
            Assert.That(room.RoomID, Is.EqualTo(roomID));
            Assert.That(room.GetClientsNumber, Is.EqualTo(1));

            Packet packetClient01 = new Packet(0);
            transport.ClientEnqueue(packetClient01, "foobar", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(2));
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(1));
            Assert.That(room.GetClientsNumber, Is.EqualTo(2));
        }
        [Test]
        public void TestRoomJoinInANotEmptyRoomRedLight()
        {
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(0));
            Packet packet = new Packet(0);
            transport.ClientEnqueue(packet, "tester", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(1));
            Assert.That(server.GetNumberOfRoom(), Is.Not.EqualTo(0));
            uint roomID = BitConverter.ToUInt32(transport.ClientDequeue().data, 1);
            ServerRoom room = server.GetRoomFromID(roomID);
            Assert.That(room, Is.Not.EqualTo(null));
            Assert.That(room.RoomID, Is.EqualTo(roomID));
            Assert.That(room.GetClientsNumber, Is.Not.EqualTo(0));


            Packet packetClient01 = new Packet(0);
            transport.ClientEnqueue(packetClient01, "foobar", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(2));
            Assert.That(server.GetNumberOfRoom(), Is.Not.EqualTo(2));
            Assert.That(room.GetClientsNumber, Is.Not.EqualTo(1));
        }

        //Test room capacity, client join a full room
        [Test]
        public void TestRoomJoinInAFullRoomGreenLight()
        {
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(0));
            Packet packetClient00 = new Packet(0);
            transport.ClientEnqueue(packetClient00, "tester", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(1));
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(1));
            uint roomID = BitConverter.ToUInt32(transport.ClientDequeue().data, 1);
            ServerRoom room = server.GetRoomFromID(roomID);
            Assert.That(room, Is.Not.EqualTo(null));
            Assert.That(room.RoomID, Is.EqualTo(roomID));
            Assert.That(room.GetClientsNumber, Is.EqualTo(1));

            Assert.That(room.IsOccupy, Is.EqualTo(false));

            Packet packetClient01 = new Packet(0);
            transport.ClientEnqueue(packetClient01, "foobar", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(2));
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(1));
            Assert.That(room.GetClientsNumber, Is.EqualTo(2));

            Packet packetClient02 = new Packet(0);
            transport.ClientEnqueue(packetClient02, "anotherTester", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(3));
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(2));
            Assert.That(room.GetClientsNumber, Is.EqualTo(2));

            Assert.That(room.IsOccupy, Is.EqualTo(true));

            uint newRoomID = BitConverter.ToUInt32(transport.ClientDequeue().data, 0);
            ServerRoom newRoom = server.GetRoomFromID(newRoomID);
            Assert.That(newRoom, Is.Not.EqualTo(null));
            Assert.That(newRoom.RoomID, Is.EqualTo(newRoomID));
            Assert.That(newRoom.GetClientsNumber, Is.EqualTo(1));
            Assert.That(newRoom.IsOccupy, Is.EqualTo(false));
        }
        [Test]
        public void TestRoomJoinInAFullRoomRedLight()
        {
            Assert.That(server.GetNumberOfRoom(), Is.EqualTo(0));
            Packet packet = new Packet(0);
            transport.ClientEnqueue(packet, "tester", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(1));
            Assert.That(server.GetNumberOfRoom(), Is.Not.EqualTo(0));
            uint roomID = BitConverter.ToUInt32(transport.ClientDequeue().data, 1);
            ServerRoom room = server.GetRoomFromID(roomID);
            Assert.That(room, Is.Not.EqualTo(null));
            Assert.That(room.RoomID, Is.EqualTo(roomID));
            Assert.That(room.GetClientsNumber, Is.Not.EqualTo(0));

            Assert.That(room.IsOccupy, Is.Not.EqualTo(true));

            Packet packetClient01 = new Packet(0);
            transport.ClientEnqueue(packetClient01, "foobar", 0);
            server.SingleStep();
            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(2));
            Assert.That(server.GetNumberOfRoom(), Is.Not.EqualTo(2));
            Assert.That(room.GetClientsNumber, Is.Not.EqualTo(1));

            Packet packetClient02 = new Packet(0);
            transport.ClientEnqueue(packetClient02, "anotherTester", 0);
            server.SingleStep();

            Assert.That(server.GetAllClientsConnetedToThisServer(), Is.EqualTo(3));
            Assert.That(server.GetNumberOfRoom(), Is.Not.EqualTo(1));
            Assert.That(room.GetClientsNumber, Is.EqualTo(2));

            Assert.That(room.IsOccupy, Is.Not.EqualTo(false));

            uint newRoomID = BitConverter.ToUInt32(transport.ClientDequeue().data, 0);
            ServerRoom newRoom = server.GetRoomFromID(newRoomID);
            Assert.That(newRoom, Is.Not.EqualTo(null));
            Assert.That(newRoom.RoomID, Is.EqualTo(newRoomID));
            Assert.That(newRoom.GetClientsNumber, Is.Not.EqualTo(0));
            Assert.That(newRoom.IsOccupy, Is.Not.EqualTo(true));
        }
    }
}
