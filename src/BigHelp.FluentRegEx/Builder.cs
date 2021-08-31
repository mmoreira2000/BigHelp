using System;

namespace BigHelp.FluentRegEx
{
    public static class Builder
    {
        /// <summary>
        /// Starts a new regular expression builder.
        /// </summary>
        /// <returns>A new regular expression builder instance.</returns>
        public static RegExBuilder Start() => new RegExBuilder();
    }
}
