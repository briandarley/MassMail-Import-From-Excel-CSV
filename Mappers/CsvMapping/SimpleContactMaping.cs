using CsvHelper.Configuration;

public class SimpleContactMapping : ClassMap<SimpleContact>
{

    public SimpleContactMapping()
    {
        MapToEntity();
    }

    private void MapToEntity()
    {

        Map(m => m.Email).Index(0);
        Map(m => m.DisplayName).Index(1);
    }
}