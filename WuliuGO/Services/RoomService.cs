using WuliuGO.Models;

namespace WuliuGO.Services
{
    public class RoomService
    {
        private readonly Dictionary<string, Room> rooms = [];
        public Room CreateRoom(long userId)
        {
            var room = new Room(userId);
            rooms.Add(room.RoomId, room);
            return room;
        }
        public Room? GetRoomById(string roomId)
        {
            return rooms.ContainsKey(roomId) ? rooms[roomId] : null;
        }

        public void RemoveRoom(string roomId)
        {
            rooms.Remove(roomId);
        }
        public bool RoomExists(string roomId)
        {
            return rooms.ContainsKey(roomId);
        }

    }
}