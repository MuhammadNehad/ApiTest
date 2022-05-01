

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTest.Models
{
    [Table("t_Beers")]
    public class BeerData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        public string name { set; get; }
        public BeerType? type { set; get; }
        public decimal? rating { set; get; }


    }
    public enum BeerType
    {
        Pale_ale,
        Stout
    }
}
