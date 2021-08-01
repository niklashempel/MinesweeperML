using AutoMapper;
using MinesweeperML.Business.AutoMapper.Profiles;

namespace MinesweeperML.Business.AutoMapper
{
    /// <summary>
    /// The auto mapper configuration helper class.
    /// </summary>
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// Configures this instance.
        /// </summary>
        /// <returns>The mapper configuration.</returns>
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // configure here
                cfg.AddProfile<HighscoreProfile>();
            });
            return config.CreateMapper();
        }
    }
}