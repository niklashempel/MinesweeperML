using System;
using AutoMapper;
using MinesweeperML.Enumerations;
using MinesweeperML.Models;
using MinesweeperML.ViewsModel;

namespace MinesweeperML.Business.AutoMapper.Profiles
{
    /// <summary>
    /// The highscore profile to map between <see cref="Highscore" /> and <see
    /// cref="HighscoresViewModel" />.
    /// </summary>
    /// <seealso cref="Profile" />
    public class HighscoreProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HighscoreProfile" /> class.
        /// </summary>
        public HighscoreProfile()
        {
            CreateMap<Highscore, HighscoreViewModel>()
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => (Difficulty)src.Difficulty))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => new TimeSpan(src.Time)));
            CreateMap<HighscoreViewModel, Highscore>()
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => (int)src.Difficulty))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time.Ticks));
        }
    }
}