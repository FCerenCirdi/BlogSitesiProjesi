using System.ComponentModel.DataAnnotations;

namespace BlogSitesiProjesi.ViewModels.Article.Topic
{
    public class TopicViewModel
    {
        [Display(Name = "Topic Name")]
        [Required(ErrorMessage = "Must to the type into a name")]
        public string Name { get; set; }
    }
}
