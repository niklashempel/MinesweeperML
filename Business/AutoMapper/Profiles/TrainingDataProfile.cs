using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MinesweeperML.Models;
using MinesweeperML.ViewModels;

namespace MinesweeperML.Business.AutoMapper.Profiles
{
    /// <summary>
    /// The training data profile to map between <see cref="TrainingDataViewModel" /> and
    /// <see cref="TrainingData" />.
    /// </summary>
    /// <seealso cref="Profile" />
    public class TrainingDataProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrainingDataProfile" /> class.
        /// </summary>
        public TrainingDataProfile()
        {
            CreateMap<TrainingDataViewModel, TrainingData>()
                .ForMember(dest => dest.X00, opt => opt.MapFrom(src => (float)src.X00))
                .ForMember(dest => dest.X01, opt => opt.MapFrom(src => (float)src.X01))
                .ForMember(dest => dest.X02, opt => opt.MapFrom(src => (float)src.X02))
                .ForMember(dest => dest.X03, opt => opt.MapFrom(src => (float)src.X03))
                .ForMember(dest => dest.X04, opt => opt.MapFrom(src => (float)src.X04))
                .ForMember(dest => dest.X10, opt => opt.MapFrom(src => (float)src.X10))
                .ForMember(dest => dest.X11, opt => opt.MapFrom(src => (float)src.X11))
                .ForMember(dest => dest.X12, opt => opt.MapFrom(src => (float)src.X12))
                .ForMember(dest => dest.X13, opt => opt.MapFrom(src => (float)src.X13))
                .ForMember(dest => dest.X14, opt => opt.MapFrom(src => (float)src.X14))
                .ForMember(dest => dest.X20, opt => opt.MapFrom(src => (float)src.X20))
                .ForMember(dest => dest.X21, opt => opt.MapFrom(src => (float)src.X21))
                .ForMember(dest => dest.X23, opt => opt.MapFrom(src => (float)src.X23))
                .ForMember(dest => dest.X24, opt => opt.MapFrom(src => (float)src.X24))
                .ForMember(dest => dest.X30, opt => opt.MapFrom(src => (float)src.X30))
                .ForMember(dest => dest.X31, opt => opt.MapFrom(src => (float)src.X31))
                .ForMember(dest => dest.X32, opt => opt.MapFrom(src => (float)src.X32))
                .ForMember(dest => dest.X33, opt => opt.MapFrom(src => (float)src.X33))
                .ForMember(dest => dest.X34, opt => opt.MapFrom(src => (float)src.X34))
                .ForMember(dest => dest.X40, opt => opt.MapFrom(src => (float)src.X40))
                .ForMember(dest => dest.X41, opt => opt.MapFrom(src => (float)src.X41))
                .ForMember(dest => dest.X42, opt => opt.MapFrom(src => (float)src.X42))
                .ForMember(dest => dest.X43, opt => opt.MapFrom(src => (float)src.X43))
                .ForMember(dest => dest.X44, opt => opt.MapFrom(src => (float)src.X44)).ReverseMap();
        }
    }
}