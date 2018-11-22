namespace Eventures.ViewModels.Events
{
    using System;
    using System.Globalization;

    public class BaseEventViewModel
    {
        public string Name { get; set; }

        public string Place { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string GetStartDate => this.Start.ToString("dd-MMM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

        public string GetEndDate => this.End.ToString("dd-MMM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
    }
}
