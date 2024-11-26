using Storage.BusinessLogic;
using Storage.Data.Entity;

namespace Storage.Test;

public class PalletExtensionsTest
{

    [Test]
    public void GetExpirationDateNull()
    {
        Pallet pallet = new Pallet();
        DateTime? res = pallet.GetExpirationDate();
        Assert.IsNull(res);
    }

    [Test]
    public void GetExpirationDateOneBox()
    {
        Pallet pallet = new Pallet();
        DateTime dateTime = new DateTime(2024, 1, 1);
        pallet.Boxes.Add(new Box() { ExpirationDate =  dateTime });
        DateTime? res = pallet.GetExpirationDate();
        Assert.That(dateTime, Is.EqualTo(res.Value));
    }

    [Test]
    public void GetExpirationDateSort()
    {
        Pallet pallet = new Pallet();
        DateTime dateTime1 = new DateTime(2024, 1, 1);
        DateTime dateTime2 = new DateTime(2023, 1, 1);
        DateTime dateTime3 = new DateTime(2022, 1, 1);
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime1 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime2 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime3 });
        DateTime? res = pallet.GetExpirationDate();
        Assert.That(dateTime3, Is.EqualTo(res.Value));

        pallet = new Pallet();
        dateTime1 = new DateTime(2024, 1, 1);
        dateTime2 = new DateTime(2023, 1, 1);
        dateTime3 = new DateTime(2022, 1, 1);
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime1 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime3 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime2 });
        res = pallet.GetExpirationDate();
        Assert.That(dateTime3, Is.EqualTo(res.Value));

        pallet = new Pallet();
        dateTime1 = new DateTime(2024, 1, 1);
        dateTime2 = new DateTime(2023, 1, 1);
        dateTime3 = new DateTime(2022, 1, 1);
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime2 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime1 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime3 });
        res = pallet.GetExpirationDate();
        Assert.That(dateTime3, Is.EqualTo(res.Value));

        pallet = new Pallet();
        dateTime1 = new DateTime(2024, 1, 1);
        dateTime2 = new DateTime(2023, 1, 1);
        dateTime3 = new DateTime(2022, 1, 1);
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime2 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime3 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime1 });
        res = pallet.GetExpirationDate();
        Assert.That(dateTime3, Is.EqualTo(res.Value));

        pallet = new Pallet();
        dateTime1 = new DateTime(2024, 1, 1);
        dateTime2 = new DateTime(2023, 1, 1);
        dateTime3 = new DateTime(2022, 1, 1);
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime3 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime1 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime2 });
        res = pallet.GetExpirationDate();
        Assert.That(dateTime3, Is.EqualTo(res.Value));

        pallet = new Pallet();
        dateTime1 = new DateTime(2024, 1, 1);
        dateTime2 = new DateTime(2023, 1, 1);
        dateTime3 = new DateTime(2022, 1, 1);
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime3 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime2 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime1 });
        res = pallet.GetExpirationDate();
        Assert.That(dateTime3, Is.EqualTo(res.Value));
    }

    [Test]
    public void GetExpirationDateGroup()
    {
        Pallet pallet = new Pallet();
        DateTime dateTime1 = new DateTime(2024, 1, 1);
        DateTime dateTime2 = new DateTime(2024, 1, 1);
        DateTime dateTime3 = new DateTime(2022, 1, 1);
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime1 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime2 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime3 });
        DateTime? res = pallet.GetExpirationDate();
        Assert.That(dateTime3, Is.EqualTo(res.Value));

        pallet = new Pallet();
        dateTime1 = new DateTime(2024, 1, 1);
        dateTime2 = new DateTime(2022, 1, 1);
        dateTime3 = new DateTime(2022, 1, 1);
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime1 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime3 });
        pallet.Boxes.Add(new Box() { ExpirationDate = dateTime2 });
        res = pallet.GetExpirationDate();
        Assert.That(dateTime3, Is.EqualTo(res.Value));
    }

    [Test]
    public void GetVolumeZero()
    {
        Pallet pallet = new Pallet();
        double res = pallet.GetVolume();
        Assert.That(0, Is.EqualTo(res));
    }

    [Test]
    public void GetVolumeEmpty()
    {
        Pallet pallet = new Pallet() { Width = 1, Depth = 1, Height = 1 };
        double res = pallet.GetVolume();
        Assert.That(1, Is.EqualTo(res));

        pallet = new Pallet() { Width = 2, Depth = 2, Height = 2 };
        res = pallet.GetVolume();
        Assert.That(8, Is.EqualTo(res));

        pallet = new Pallet() { Width = 2, Depth = 3, Height = 4 };
        res = pallet.GetVolume();
        Assert.That(24, Is.EqualTo(res));
    }

    [Test]
    public void GetVolumeOneBox()
    {
        Pallet pallet = new Pallet() { };
        pallet.Boxes.Add(new Box() { Width = 1, Depth = 1, Height = 1 });
        double res = pallet.GetVolume();
        Assert.That(1, Is.EqualTo(res));

        pallet = new Pallet() { };
        pallet.Boxes.Add(new Box() { Width = 2, Depth = 2, Height = 2 });
        res = pallet.GetVolume();
        Assert.That(8, Is.EqualTo(res));

        pallet = new Pallet() { };
        pallet.Boxes.Add(new Box() { Width = 2, Depth = 3, Height = 4 });
        res = pallet.GetVolume();
        Assert.That(24, Is.EqualTo(res));
    }

    [Test]
    public void GetVolumeManyBox()
    {
        Pallet pallet = new Pallet() { };
        pallet.Boxes.Add(new Box() { Width = 1, Depth = 1, Height = 1 });
        double res = pallet.GetVolume();
        Assert.That(1, Is.EqualTo(res));

        pallet = new Pallet() { };
        pallet.Boxes.Add(new Box() { Width = 1, Depth = 1, Height = 1 });
        pallet.Boxes.Add(new Box() { Width = 2, Depth = 2, Height = 2 });
        res = pallet.GetVolume();
        Assert.That(9, Is.EqualTo(res));

        pallet = new Pallet() { };
        pallet.Boxes.Add(new Box() { Width = 1, Depth = 1, Height = 1 });
        pallet.Boxes.Add(new Box() { Width = 2, Depth = 2, Height = 2 });
        pallet.Boxes.Add(new Box() { Width = 2, Depth = 3, Height = 4 });
        res = pallet.GetVolume();
        Assert.That(33, Is.EqualTo(res));
    }

    [Test]
    public void GetVolumeComplex()
    {
        Pallet pallet = new Pallet() { Width = 3, Depth = 3, Height = 3 };
        pallet.Boxes.Add(new Box() { Width = 1, Depth = 1, Height = 1 });
        double res = pallet.GetVolume();
        Assert.That(28, Is.EqualTo(res));

        pallet = new Pallet() { Width = 2, Depth = 2, Height = 2 };
        pallet.Boxes.Add(new Box() { Width = 1, Depth = 1, Height = 1 });
        pallet.Boxes.Add(new Box() { Width = 2, Depth = 2, Height = 2 });
        res = pallet.GetVolume();
        Assert.That(17, Is.EqualTo(res));

        pallet = new Pallet() { Width = 1, Depth = 1, Height = 1 };
        pallet.Boxes.Add(new Box() { Width = 1, Depth = 1, Height = 1 });
        pallet.Boxes.Add(new Box() { Width = 2, Depth = 2, Height = 2 });
        pallet.Boxes.Add(new Box() { Width = 2, Depth = 3, Height = 4 });
        res = pallet.GetVolume();
        Assert.That(34, Is.EqualTo(res));
    }

    [Test]
    public void GetWeightEmpty()
    {
        Pallet pallet = new Pallet() { };
        double res = pallet.GetWeight();
        Assert.That(30, Is.EqualTo(res));
    }

    [Test]
    public void GetWeightOneBox()
    {
        Pallet pallet = new Pallet() { };
        pallet.Boxes.Add(new Box() { Weight = 1 });
        double res = pallet.GetWeight();
        Assert.That(31, Is.EqualTo(res));

        pallet = new Pallet() { };
        pallet.Boxes.Add(new Box() { Weight = 2 });
        res = pallet.GetWeight();
        Assert.That(32, Is.EqualTo(res));

        pallet = new Pallet() { };
        pallet.Boxes.Add(new Box() { Weight = 3 });
        res = pallet.GetWeight();
        Assert.That(33, Is.EqualTo(res));
    }

    [Test]
    public void GetWeightManyBox()
    {
        Pallet pallet = new Pallet() { };
        pallet.Boxes.Add(new Box() { Weight = 1 });
        double res = pallet.GetWeight();
        Assert.That(31, Is.EqualTo(res));

        pallet = new Pallet() { };
        pallet.Boxes.Add(new Box() { Weight = 1 });
        pallet.Boxes.Add(new Box() { Weight = 10 });
        res = pallet.GetWeight();
        Assert.That(41, Is.EqualTo(res));

        pallet = new Pallet() { };
        pallet.Boxes.Add(new Box() { Weight = 1 });
        pallet.Boxes.Add(new Box() { Weight = 10 });
        pallet.Boxes.Add(new Box() { Weight = 20 });
        res = pallet.GetWeight();
        Assert.That(61, Is.EqualTo(res));
    }
}
