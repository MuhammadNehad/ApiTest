using ApiTest.Data;
using ApiTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest
{
    public static class ExtensionMethods
    {
        public static bool checkIfNull(this string text)
        {
            return String.IsNullOrWhiteSpace(text);
        }

        public static bool checkIfDefined(this BeerType? beerType)
        {
            return Enum.IsDefined(typeof(BeerType), beerType);
        }

        public static ApiTestContext DBContextMethod(this string conn )
        {

            var OptionsBuilder = new DbContextOptionsBuilder<ApiTestContext>();
            OptionsBuilder.UseSqlServer(conn);
            return new ApiTestContext(OptionsBuilder.Options);

        }
    }
}
