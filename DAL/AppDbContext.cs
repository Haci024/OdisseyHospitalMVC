using HospitalMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
        {

        }
      
        public DbSet<HospitalSlider> Sliders { get; set; }
        public DbSet<HospitalSliderImage> SliderImages { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Bios> Bios { get; set; }
        public DbSet<Equipments> Equipments { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<HospitalBlogsDetail> HospitalBlogsDetails { get; set; }
        public DbSet<HospitalBlogs> HospitalBlogs { get; set; }
        public DbSet<Illnesses> Illnesses { get; set; }
        public DbSet<OurServices> OurServices { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Patients> Patients { get; set; }
        public DbSet<Reports> Reports { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<GalleryImages> GalleryImages { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<BloodBank> BloodBanks { get; set; }



    }
}
