using System.ComponentModel.DataAnnotations.Schema;

namespace WuliuGO.Models
{
    [Table("User")]
    public class User : CommonModel
    {
        [Column("name")]
        public required string Name { get; set; }
        [Column("nickname")]
        public string NickName { get; set; } = string.Empty;
        [Column("age")]
        public short Age { get; set; }
        [Column("gender")]
        public bool Gender { get; set; }
        [Column("email")]
        public required string Email { get; set; }
        [Column("phone")]
        public required string Phone { get; set; }
        [Column("rank")]
        public int Rank { get; set; }
        [Column("avatar")]
        public string Avarar { get; set; } = string.Empty;
        [Column("nationality")]
        public string Nationality { get; set; } = string.Empty;
        [Column("password")]
        public string Password { get; set; } = string.Empty;
    }

    ///<summary>
    /// 好友系统
    /// </summary>
    [Table("FriendVex")]
    public class FriendVex : CommonModel
    {
        [Column("user_id")]
        public long UserId { get; set; }
        [Column("first_in")]
        public long FirstInt { get; set; }
        [Column("first_out")]
        public long FirstOut { get; set; }
        [Column("status")]
        public int status;
    }
    [Table("FriendEdge")]
    public class FriendEdge : CommonModel
    {
        [Column("tailvex")]
        public long TailVex { get; set; }

        [Column("headvex")]
        public long HeadVex { get; set; }
        [Column("headlink")]
        public long HeadLink { get; set; }
        [Column("taillink")]
        public long TailLink { get; set; }
        [Column("status")]
        public int status;

    }
}