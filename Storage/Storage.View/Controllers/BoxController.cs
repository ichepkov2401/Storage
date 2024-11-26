using Storage.BusinessLogic.Service.Interfaces;
using Storage.Data.Models.Output;
using Storage.View.Configuration;

namespace Storage.View.Controllers
{
    internal static class BoxController
    {
        static IBoxService boxService = AutofacIntegration.GetInstance<IBoxService>();

        internal async static void BoxCRUD()
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
                        await boxService.Add(ViewTools.InputBoxDto());
                        break;
                    case "2":
                        {
                            foreach (BoxResponse box in await boxService.GetAll())
                            {
                                Console.WriteLine(ViewTools.ToString(box));
                            }
                        }
                        break;
                    case "3":
                        {
                            Console.Write($"Номер обновляемой коробки - ");
                            int id = int.Parse(Console.ReadLine());
                            await boxService.Update(ViewTools.InputBoxDto(), id);
                        }
                        break;
                    case "4":
                        {
                            Console.Write($"Номер удаляемой коробки - ");
                            await boxService.Delete(int.Parse(Console.ReadLine()));
                        }
                        break;
                }
            }
            catch (ArgumentException ex) { Console.WriteLine(ex.Message); }
        }
    }
}
