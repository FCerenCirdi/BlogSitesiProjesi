using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSitesiProjesi.Models.Entity
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}
