using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Services.Catalog.Api.Entities;

namespace Services.Catalog.Api.Profiles
{
    public class PhotosProfile : Profile
    {
        public PhotosProfile()
        {
            CreateMap<Models.PhotoDto, Photo>()
                 .ReverseMap();
        }
    }
}
