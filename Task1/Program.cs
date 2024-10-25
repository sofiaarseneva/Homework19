using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    internal class Program
    {
        /*символы рамок для табличек*/
        const char leftTopCorner = '\u250C';
        const char rightTopCorner = '\u2510';
        const char leftBottomCorner = '\u2514';
        const char rightBottomCorner = '\u2518';
        const char intersectionTableLinesTop = '\u252C';
        const char intersectionTableLines = '\u253C';
        const char intersectionTableLinesBottom = '\u2534';
        const char intersectionTableLinesLeft = '\u251C';
        const char intersectionTableLinesRight = '\u2524';
        const char longLine = '\u2500';
        const char verticalLine = '\u2502';

        static void Main(string[] args)
        {
            List<PC> pcList = new List<PC>()
            {
                new PC(){ Id = 1, Name = "Acer", Type = "Intel", Hz = 1, FlashMemory = 8, DriveMemory = 512, VideoCardMemory = 4, Price = 40000, NumberAvailible = 2 },
                new PC(){ Id = 2, Name = "Apple", Type = "Apple", Hz = 1, FlashMemory = 8, DriveMemory = 256, VideoCardMemory = 8, Price = 90000, NumberAvailible = 1 },
                new PC(){ Id = 3, Name = "Asus", Type = "AMD", Hz = 3, FlashMemory = 16, DriveMemory = 1024, VideoCardMemory = 16, Price = 50000, NumberAvailible = 3 },
                new PC(){ Id = 4, Name = "Honor", Type = "AMD", Hz = 2, FlashMemory = 16, DriveMemory = 512, VideoCardMemory = 8, Price = 45000, NumberAvailible = 5 },
                new PC(){ Id = 5, Name = "MSI", Type = "Intel", Hz = 2, FlashMemory = 8, DriveMemory = 512, VideoCardMemory = 16, Price = 79000, NumberAvailible = 1 },
                new PC(){ Id = 6, Name = "Huawei", Type = "Intel", Hz = 2, FlashMemory = 8, DriveMemory = 512, VideoCardMemory = 4, Price = 51000, NumberAvailible = 30 },
                new PC(){ Id = 7, Name = "MSI", Type = "Intel", Hz = 4, FlashMemory = 16, DriveMemory = 2048, VideoCardMemory = 16, Price = 150000, NumberAvailible = 1 },
            };

            Console.WriteLine("Список компьютеров!\n\n");

            try
            {
                Console.WriteLine("Весь список компьтеров:\n");
                PrintTable(pcList);
                ConsoleKey consoleKey;
                int type;
                do
                {
                    Console.WriteLine("\nВведите номер задачи: ");
                    type = Convert.ToInt32(Console.ReadLine());
                    switch (type)
                    {
                        case 1:
                            Console.WriteLine("\n1. Все компьютеры с указанным процессором\nВведите наименование процессора:");
                            string processor = Console.ReadLine();
                            //приводим к верхнему регистру для более точно поиска
                            List<PC> pcByProcessor = pcList.Where(x => x.Type.ToUpper() == processor.ToUpper()).ToList();
                            if (pcByProcessor.Count > 0)
                            {
                                PrintTable(pcByProcessor);
                            }
                            else
                            {
                                Console.WriteLine("Компьютера с указанным процессором нет в списке.");
                            }
                            break;
                        case 2:
                            Console.WriteLine("\n2. Все компьютеры с объемом ОЗУ не ниже, чем указано\nВведите объем ОЗУ:");
                            int volumeFlash = Convert.ToInt32(Console.ReadLine());
                            List<PC> pcByFlash = pcList.Where(x => x.FlashMemory >= volumeFlash).ToList();
                            if (pcByFlash.Count > 0)
                            {
                                PrintTable(pcByFlash);
                            }
                            else
                            {
                                Console.WriteLine("Компьютера с объемом ОЗУ не ниже, чем указано, нет в списке.");
                            }
                            break;
                        case 3:
                            Console.WriteLine("\n3. Вывести весь список, отсортированный по увеличению стоимости\n");
                            List<PC> pcByPrice = pcList.OrderBy(x => x.Price).ToList();
                            PrintTable(pcByPrice);
                            break;
                        case 4:
                            Console.WriteLine("\n4. Вывести весь список, сгруппированный по типу процессора\n");
                            IEnumerable<IGrouping<string, PC>> pcByTypeProcessor = pcList.GroupBy(x => x.Type);
                            PrintTableGroupping(pcByTypeProcessor);
                            break;
                        case 5:
                            Console.WriteLine("\n5. Найти самый дорогой и самый бюджетный компьютер\n");
                            List<PC> pcPrice = pcList.OrderBy(x => x.Price).ToList();
                            Console.WriteLine($"Самый бюджетный компьютер - {pcPrice.FirstOrDefault().Name}. Он стоит {pcPrice.FirstOrDefault().Price}.");
                            Console.WriteLine($"Самый дорогой компьютер - {pcPrice.LastOrDefault().Name}. Он стоит {pcPrice.LastOrDefault().Price}.");
                            break;
                        case 6:
                            Console.WriteLine("\n6. Eсть ли хотя бы один компьютер в количестве не менее 30 штук?\n");
                            bool availiblePieces = pcList.Any(x => x.NumberAvailible >= 30);
                            if (availiblePieces)
                            {
                                Console.WriteLine("Есть хотя бы один компьютер в количестве не менее 30шт");
                            }
                            else
                            {
                                Console.WriteLine("Нет компьютеров в количестве не менее 30шт");
                            }
                            break;
                        default:
                            throw new Exception("Данный тип операции не определен!");
                    }
                    Console.WriteLine("\nДля продолжения нажмите любую клавишу!");
                    Console.WriteLine("Для выхода нажмите Escape!\n");
                    consoleKey = Console.ReadKey().Key;
                } while (consoleKey != ConsoleKey.Escape);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка!" + e.Message);
                Console.ReadKey();
            }
        }
        static void PrintTable(List<PC> listPC)
        {
            int i = 0;
            string topLine = leftTopCorner + RepeatUnicode(longLine, 3) + intersectionTableLinesTop + RepeatUnicode(longLine, 10) + intersectionTableLinesTop + RepeatUnicode(longLine, 10) +
                                intersectionTableLinesTop + RepeatUnicode(longLine, 3) + intersectionTableLinesTop + RepeatUnicode(longLine, 12) + intersectionTableLinesTop + RepeatUnicode(longLine, 12) +
                                intersectionTableLinesTop + RepeatUnicode(longLine, 15) + intersectionTableLinesTop + RepeatUnicode(longLine, 10) + intersectionTableLinesTop + RepeatUnicode(longLine, 15) + rightTopCorner;
            string middleLine = intersectionTableLinesLeft + RepeatUnicode(longLine, 3) + intersectionTableLines + RepeatUnicode(longLine, 10) + intersectionTableLines + RepeatUnicode(longLine, 10) +
                                intersectionTableLines + RepeatUnicode(longLine, 3) + intersectionTableLines + RepeatUnicode(longLine, 12) + intersectionTableLines + RepeatUnicode(longLine, 12) +
                                intersectionTableLines + RepeatUnicode(longLine, 15) + intersectionTableLines + RepeatUnicode(longLine, 10) + intersectionTableLines + RepeatUnicode(longLine, 15) +
                                intersectionTableLinesRight;
            string bottomLine = leftBottomCorner + RepeatUnicode(longLine, 3) + intersectionTableLinesBottom + RepeatUnicode(longLine, 10) + intersectionTableLinesBottom + RepeatUnicode(longLine, 10) +
                                intersectionTableLinesBottom + RepeatUnicode(longLine, 3) + intersectionTableLinesBottom + RepeatUnicode(longLine, 12) + intersectionTableLinesBottom + RepeatUnicode(longLine, 12) +
                                intersectionTableLinesBottom + RepeatUnicode(longLine, 15) + intersectionTableLinesBottom + RepeatUnicode(longLine, 10) + intersectionTableLinesBottom + RepeatUnicode(longLine, 15) + rightBottomCorner;
            Console.WriteLine(topLine);
            Console.WriteLine($"{verticalLine}{"Id",3}{verticalLine}{"Name",10}{verticalLine}{"Type",10}{verticalLine}{"Hz",3}{verticalLine}{"FlashMemory",12}{verticalLine}{"DriveMemory",12}{verticalLine}{"VideoCardMemory",15}{verticalLine}{"Price",10}{verticalLine}{"NumberAvailible",15}{verticalLine}");
            Console.WriteLine(middleLine);
            foreach (PC onePC in listPC)
            {
                i++;
                Console.WriteLine($"{verticalLine}{onePC.Id,3}{verticalLine}{onePC.Name,10}{verticalLine}{onePC.Type,10}{verticalLine}{onePC.Hz,3}{verticalLine}{onePC.FlashMemory,12}{verticalLine}{onePC.DriveMemory,12}{verticalLine}{onePC.VideoCardMemory,15}{verticalLine}{onePC.Price,10}{verticalLine}{onePC.NumberAvailible,15}{verticalLine}");
                if (i != listPC.Count)
                    Console.WriteLine(middleLine);
            }
            Console.WriteLine(bottomLine);
        }
        static void PrintTableGroupping(IEnumerable<IGrouping<string, PC>> groupPC)
        {

            string topLine = leftTopCorner + RepeatUnicode(longLine, 3) + intersectionTableLinesTop + RepeatUnicode(longLine, 10) + intersectionTableLinesTop + RepeatUnicode(longLine, 10) +
                                intersectionTableLinesTop + RepeatUnicode(longLine, 3) + intersectionTableLinesTop + RepeatUnicode(longLine, 12) + intersectionTableLinesTop + RepeatUnicode(longLine, 12) +
                                intersectionTableLinesTop + RepeatUnicode(longLine, 15) + intersectionTableLinesTop + RepeatUnicode(longLine, 10) + intersectionTableLinesTop + RepeatUnicode(longLine, 15) + rightTopCorner;
            string middleLine = intersectionTableLinesLeft + RepeatUnicode(longLine, 3) + intersectionTableLines + RepeatUnicode(longLine, 10) + intersectionTableLines + RepeatUnicode(longLine, 10) +
                                intersectionTableLines + RepeatUnicode(longLine, 3) + intersectionTableLines + RepeatUnicode(longLine, 12) + intersectionTableLines + RepeatUnicode(longLine, 12) +
                                intersectionTableLines + RepeatUnicode(longLine, 15) + intersectionTableLines + RepeatUnicode(longLine, 10) + intersectionTableLines + RepeatUnicode(longLine, 15) +
                                intersectionTableLinesRight;
            string bottomLine = leftBottomCorner + RepeatUnicode(longLine, 3) + intersectionTableLinesBottom + RepeatUnicode(longLine, 10) + intersectionTableLinesBottom + RepeatUnicode(longLine, 10) +
                                intersectionTableLinesBottom + RepeatUnicode(longLine, 3) + intersectionTableLinesBottom + RepeatUnicode(longLine, 12) + intersectionTableLinesBottom + RepeatUnicode(longLine, 12) +
                                intersectionTableLinesBottom + RepeatUnicode(longLine, 15) + intersectionTableLinesBottom + RepeatUnicode(longLine, 10) + intersectionTableLinesBottom + RepeatUnicode(longLine, 15) + rightBottomCorner;

            foreach (IGrouping<string, PC> pc in groupPC)
            {
                int i = 0;
                Console.WriteLine(pc.Key);
                Console.WriteLine(topLine);
                Console.WriteLine($"{verticalLine}{"Id",3}{verticalLine}{"Name",10}{verticalLine}{"Type",10}{verticalLine}{"Hz",3}{verticalLine}{"FlashMemory",12}{verticalLine}{"DriveMemory",12}{verticalLine}{"VideoCardMemory",15}{verticalLine}{"Price",10}{verticalLine}{"NumberAvailible",15}{verticalLine}");
                Console.WriteLine(middleLine);
                foreach (PC onePC in pc)
                {
                    i++;
                    Console.WriteLine($"{verticalLine}{onePC.Id,3}{verticalLine}{onePC.Name,10}{verticalLine}{onePC.Type,10}{verticalLine}{onePC.Hz,3}{verticalLine}{onePC.FlashMemory,12}{verticalLine}{onePC.DriveMemory,12}{verticalLine}{onePC.VideoCardMemory,15}{verticalLine}{onePC.Price,10}{verticalLine}{onePC.NumberAvailible,15}{verticalLine}");
                    if (i != pc.Count())
                        Console.WriteLine(middleLine);
                }
                Console.WriteLine(bottomLine);
            }

        }
        /*для формирования строки с повторением символа указанное количество раз*/
        static string RepeatUnicode(char c, int n)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
