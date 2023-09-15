using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using zadanie6;
using zadanie6.Models;

namespace zadanie6.Data;

public partial class zadanie6Context : DbContext
{
    public zadanie6Context()
    {
    }

    public zadanie6Context(DbContextOptions<zadanie6Context> options)
        : base(options)
    {
    }

    public DbSet<Purchase> Purchases { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UsePropertyAccessMode(PropertyAccessMode.Property);
    }
}
