using CsvHelper.Configuration;

public class SimpleContactMapping : ClassMap<SimpleContact>
{

    public SimpleContactMapping()
    {
        MapToEntity();
    }

    private void MapToEntity()
    {

        //this is zero based index
        Map(m => m.Email).Index(3);
        Map(m => m.DisplayName).Index(2);
    }
}