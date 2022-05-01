using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Models
{
    [Table("UsersBeers")]
    public class UserBeer
    {
        [Column("UsersId")]
        public int UserModelId { set; get; }
        public UserModel _UserModel { set; get; }
        [Column("BeersId")]
        public int BeerDataId { set; get; }

        public BeerData _BeerData { set; get; }

        public decimal? rate { set; get; }
    }
}
