using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ShopTARpe25.Data
{
    public class ShopTARpe25Context : DbContext
    {
        public ShopTARpe25Context(DbContextOptions<ShopTARpe25Context> options) : base(options)
        { }
        //teha Core projekti alla Domain nimega kaust ja sinna class nimega Spaceship
    }
}
