using Storage.Data.Entity;
using Storage.BusinessLogic;

namespace Storage.Test;

public class BoxExtensionsTest
{

    [Test]
    public void GetExpirationDateExpiration()
    {
        DateTime dateTime = new DateTime(2024, 1, 1);
        Box box = new Box()
        {
            ExpirationDate = dateTime
        };
        DateTime? res = box.ExpirationDate;
        Assert.That(dateTime, Is.EqualTo(res.Value));
    }


    [Test]
    public void GetExpirationDateProduction()
    {
        DateTime dateTime = new DateTime(2024, 1, 1);
        Box box = new Box()
        {
            ProductionDate = dateTime
        };
        DateTime? res = box.GetExpirationDate();
        Assert.That(new DateTime(2024, 4, 10), Is.EqualTo(res.Value));
    }


    [Test]
    public void GetExpirationDateComplex()
    {
        DateTime dateTime1 = new DateTime(2023, 1, 1);
        DateTime dateTime2 = new DateTime(2024, 1, 1);
        Box box = new Box()
        {
            ExpirationDate = dateTime1,
            ProductionDate = dateTime2

        };
        DateTime? res = box.ExpirationDate;
        Assert.That(dateTime1, Is.EqualTo(res.Value));

        box = new Box()
        {
            ExpirationDate = dateTime2,
            ProductionDate = dateTime1

        };
        res = box.ExpirationDate;
        Assert.That(dateTime2, Is.EqualTo(res.Value));

        box = new Box()
        {
            ExpirationDate = dateTime1,
            ProductionDate = dateTime1

        };
        res = box.ExpirationDate;
        Assert.That(dateTime1, Is.EqualTo(res.Value));
    }

    [Test]
    public void GetVolume()
    {
        Box box = new Box() { Width = 1, Depth = 1, Height = 1 };
        double res = box.GetVolume();
        Assert.That(1, Is.EqualTo(res));

        box = new Box() { Width = 2, Depth = 2, Height = 2 };
        res = box.GetVolume();
        Assert.That(8, Is.EqualTo(res));

        box = new Box() { Width = 2, Depth = 3, Height = 4 };
        res = box.GetVolume();
        Assert.That(24, Is.EqualTo(res));
    }
}
