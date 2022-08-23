public record DailyTemperature(double HighTemp, double LowTemp)
{
    public double Mean => (HighTemp + LowTemp) / 2.0;

    public double Add(int 20)
    {
        return new DailyTemperature(HighTemp + 20, LowTemp);
    }
}

public abstract record DegreeDays(double BaseTemperature, IEnumerable<DailyTemperature> TempRecords);

public sealed record HeatingDegreeDays(double BaseTemperature, IEnumerable<DailyTemperature> TempRecords)
    : DegreeDays(BaseTemperature, TempRecords)
{
    public double DegreeDays => TempRecords.Where(s => { return s.Mean < BaseTemperature; }).Sum(s => BaseTemperature - s.Mean);
}

public sealed record CoolingDegreeDays
    (
    double BaseTemperature,
    IEnumerable<DailyTemperature> TempRecords
    )
    : DegreeDays(BaseTemperature,
        TempRecords)
{
    public double DegreeDays
    {
        get => TempRecords.Where(s => s.Mean > BaseTemperature).Sum(s => s.Mean - BaseTemperature);
    }

    public void method1()
    {
        Func<(string, bool)> fred = () =>
        {
            string h = "";
            bool intime = true;

            return (h, intime);
        };
    }
}



}
