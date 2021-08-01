using System.ComponentModel;

namespace MinesweeperML.Business.Attributes
{
    /// <summary>
    /// Localized description attribute.
    /// </summary>
    /// <seealso cref="System.ComponentModel.DescriptionAttribute" />
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string key;

        /// <summary>
        /// Gets the description stored in this attribute.
        /// </summary>
        public override string Description => Cultures.Resources.ResourceManager.GetString(key) ?? $"[[{key}]]";

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedDescriptionAttribute" />
        /// class.
        /// </summary>
        /// <param name="key">The key.</param>
        public LocalizedDescriptionAttribute(string key)
        {
            this.key = key;
        }
    }
}