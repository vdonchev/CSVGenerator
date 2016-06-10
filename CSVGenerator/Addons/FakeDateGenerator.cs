namespace CsvGenerator.Addons
{
    using System;
    using Exceptions;
    using Helpers;
    using Interfaces;

    public class FakeDateGenerator : IFakeDateGenerator
    {
        private DateTime? startDate;
        private DateTime? endDate;
        private int numberOfReleases;

        public FakeDateGenerator()
        {
            this.startDate = DateTime.Parse(CsvGenerator.Settings["FakeDatesStart"]);

            if (CsvGenerator.Settings["FakeDatesStart"] == "now")
            {
                this.endDate = DateTime.UtcNow;
            }
            else
            {
                this.endDate = DateTime.Parse(CsvGenerator.Settings["FakeDatesEnd"]);
            }
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

            if (this.endDate == null)
            {
                throw new CsvGeneratorException(Constants.FakeDateNotSet);
            }
            
            var start = this.startDate.Value;

            var secAvail = (long)(this.endDate.Value - start).TotalMinutes / this.numberOfReleases;
            var next = start;
            next = next.AddMinutes(secAvail);

            this.startDate = next;
            this.numberOfReleases--;

            return next;
        }
    }
}