using Storage.Bl.Service.Interfaces;
using Storage.Data.Models.Output;
using Storage.View.Configuration;

namespace Storage.View.Controllers
{
    internal static class PalletController
    {
        static IPalletService palletService = AutofacIntegration.GetInstance<IPalletService>();
        internal async static void PalletCRUD()
        {
            Console.WriteLine("1 - Create");
            Console.WriteLine("2 - Read");
            Console.WriteLine("3 - Update");
            Console.WriteLine("4 - Delete");
            try
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        await palletService.Add(ViewTools.InputPalletDto());
                        break;
                    case "2":
                        {
                            foreach (PalletOutputDto pallet in await palletService.GetAll())
                            {
                                Console.WriteLine(ViewTools.ToString(pallet));
                            }
                        }
                        break;
                    case "3":
                        {
                            Console.Write($"Номер обновляемого паллета - ");
                            int id = int.Parse(Console.ReadLine());
                            await palletService.Update(ViewTools.InputPalletDto(), id);
                        }
                        break;
                    case "4":
                        {
                            Console.Write($"Номер удаляемого паллета - ");
                            await palletService.Delete(int.Parse(Console.ReadLine()));
                        }
                        break;
                }
            }
            catch (ArgumentException ex) { Console.WriteLine(ex.Message); }
        }
    }
}
