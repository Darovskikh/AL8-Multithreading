using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Advanced_Lesson_6_Multithreading
{
    class Practice
    {
        /// <summary>
        /// LA8.P1/X. Написать консольные часы, которые можно останавливать и запускать с 
        /// консоли без перезапуска приложения.
        /// </summary>
        public static void LA8_P1_5()
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine(DateTime.Now);
                    System.Threading.Thread.Sleep(1000);
                    Console.Clear();
                }
            });
            thread.Start();
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "s":
                        thread.Resume();
                        break;
                    case "b":
                        thread.Suspend();
                        break;
                }
            }

        }

        /// <summary>
        /// LA8.P2/X. Написать консольное приложение, которое “делает массовую рассылку”. 
        /// </summary>
        public static void LA8_P2_5()
        {
            Random random = new Random();
            for (int i = 0; i < 50; i++)
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    FileInfo file = new FileInfo($"{random.Next(0, 100)}.txt");
                    using (FileStream fs = file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                        {
                            sw.WriteLine("Some text");

                        }
                    }
                    LA8_P5_5();
                    //Thread.Sleep(1000);
                });
            }
            Console.WriteLine("Done");
        }

        /// <summary>
        /// Написать код, который в цикле (10 итераций) эмулирует посещение 
        /// сайта увеличивая на единицу количество посещений для каждой из страниц.  
        /// </summary>
        public static void LA8_P3_5()
        {
        }

        /// <summary>
        /// LA8.P4/X. Отредактировать приложение по “рассылке” “писем”. 
        /// Сохранять все “тела” “писем” в один файл. Использовать блокировку потоков, чтобы избежать проблем синхронизации.  
        /// </summary>
        public static void LA8_P4_5()
        {
            FileInfo file = new FileInfo("Emails.txt");
            var obj = new object();
            int count = 0;
            using (FileStream fs = file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    for (int i = 0; i < 50; i++)
                    {
                        ThreadPool.QueueUserWorkItem(state =>
                        {
                            lock (obj)
                            {
                                sw.WriteLine($"{count++}");
                            }

                        });
                    }
                    Thread.Sleep(1000);
                }
            }
            Console.WriteLine("Done");
        }
        /// <summary>
        /// LA8.P5/5. Асинхронная “отсылка” “письма” с блокировкой вызывающего потока 
        /// и информировании об окончании рассылки (вывод на консоль информации 
        /// удачно ли завершилась отсылка). 
        /// </summary>
        public async static void LA8_P5_5()
        {
            var random = new Random();
            bool result = await SmptServer.SendEmail($"darosukikh.v{random.Next(0,50)}@gmail.com");
            if (result)
                Console .WriteLine("Success");
            else
                Console.WriteLine("Wrong");
        }
    }
}
