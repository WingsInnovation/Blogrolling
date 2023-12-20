using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blogrolling.Database.Sources;

[Comment("数据源")]
public class DataSource
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("源ID")]
    public int Id { get; set; }
    
    [Required]
    [Comment("数据源状态")]
    public DataSourceStatus Status { get; set; }

    [Required]
    [Comment("数据源类型")]
    public DataSourceType Type { get; set; }
    
    [Required]
    [Comment("链接")]
    public string Link { get; set; }

    #region Foreign Keys
    
    public virtual Blog Blog { get; set; }

    #endregion
}