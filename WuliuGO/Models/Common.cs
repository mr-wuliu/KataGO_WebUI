using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuliuGO.Models
{
    public class CommonModel
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("create_time")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间 
        /// </summary>
        [Column("update_time")]
        public DateTime UpdateTime { get; set; } 

    }
}