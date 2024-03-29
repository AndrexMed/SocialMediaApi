﻿using AutoMapper;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastructure.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() 
        { 
            CreateMap<Post, PostDTO>();
            CreateMap<PostDTO, Post>();

            CreateMap<Security, SecurityDTO>().ReverseMap();

            CreateMap<Security, SecurityAndUserDTO>().ReverseMap();
            CreateMap<User, SecurityAndUserDTO>().ReverseMap();
        }
    }
}