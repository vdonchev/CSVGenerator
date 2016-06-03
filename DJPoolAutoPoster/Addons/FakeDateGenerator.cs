namespace CsvGenerator.Addons
{
    using System;
    using Exceptions;
    using Helpers;
    using Interfaces;

    public class FakeDateGenerator : IFakeDateGenerator
    {
        private DateTime? startDate;
        private int numberOfReleases;

        public FakeDateGenerator()
        {
            this.startDate = DateTime.Parse(CsvGenerator.Settings["FakeDatesStart"]);
        }

        public int NumberOfReleases
        {
            get
            {
                return this.numberOfReleases;
            }

            set
            {
                this.numberOfReleases = value;
            }
        }

        public DateTime Next()
        {
            if (this.startDate == null)
            {
                throw new CsvGeneratorException(Constants.FakeDateNotSet);
            }

            var now = DateTime.UtcNow;
            var start = this.startDate.Value;

            var secAvail = (long)(now - start).TotalMinutes / this.numberOfReleases;
            var next = start;
            next = next.AddMinutes(secAvail);

            this.startDate = next;
            this.numberOfReleases--;

            return next;
        }
    }
}