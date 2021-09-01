using System;

namespace Labs
{
    class Menu

    {

        public void ShowMenu(int n)
        {

            Console.WriteLine("\n**МЕНЮ**\n");

            Console.WriteLine($"|Выберите команду:\n| 1 - для создания нового полинома\n| 2 - для вывода имеющихся полиномов (Текущее кол-во созданых полиномов: {n}) \n| 3 - для дифференцирования выбранного полинома\n| 4 - для умножения двух выбранных полиномов\n| 5 - для завершения программы");

        }

        public void NewPoly(Polynomial[] X, int n)

        {

            Console.WriteLine("\n**CОЗДАНИЕ НОВОГО ПОЛИНОМА**\n");

            Polynomial p = new Polynomial();

            bool q = true;

            int schet = 0;

            while (q)

            {

                Console.WriteLine($"|Текущее кол-во мономов в полиноме: {schet} \n|Ввод нового монома вида а*x^(b)*e^(c*x) \n");

                schet++;

                UInt32 coef, stx, ste, a;

                Console.WriteLine("|Введите коэффициент монома: a = ");

                coef = UInt32.Parse(Console.ReadLine());

                Console.WriteLine("|Введите степень х: b = ");

                stx = UInt32.Parse(Console.ReadLine());

                Console.WriteLine("|Введите степень e: c = ");

                ste = UInt32.Parse(Console.ReadLine());

                p.AddMonom(new Monomial(coef, stx, ste));

                Console.WriteLine("|Текущий вид многочлена: ");

                Console.WriteLine(p.ToString());

                Console.WriteLine("|Чтобы продолжить ввод мономов нажмите 1. Чтобы закончить создание полинома нажмите 2.\n|Ввод: ");

                a = UInt32.Parse(Console.ReadLine());

                if (a == 2)

                    q = false;

            }

            X[n] = p;

            Console.WriteLine($"|Получившийся полином сохранен под номером {n}\n");

            X[n].ToString();

        }

        public void Spisok(Polynomial[] X, int n)

        {

            Console.WriteLine("\n**ВЫВОД ИМЕЮЩИХСЯ МНОГОЧЛЕНОВ**");

            int i;

            for (i = 1; i <= n; i++)

            {

                Console.WriteLine($"\n|Многочлен под номером {i}\n-> ");

                Console.WriteLine(X[i].ToString());

            }

        }

        public void multi(Polynomial[] X, int n)

        {

            Console.WriteLine("\n**УМНОЖЕНИЕ ДВУХ МНОГОЧЛЕНОВ**\n");

            Console.WriteLine("|Выберите два многочлена которые хотите перемножить.");

            for (int i = 1; i <= n; i++)

            {

                Console.WriteLine($"\n|Многочлен с номером {i}\n-> ");

                Console.WriteLine(X[i].ToString());

            }

            Console.WriteLine("|Введите номера многочленов: ");

            int a, b;

            a = int.Parse(Console.ReadLine());

            b = int.Parse(Console.ReadLine());

            X[n + 1] = X[a] * X[b];

            Console.WriteLine($"|Получившийся многочлен записан под номером {n + 1} и равен:\n-> ");

            Console.WriteLine(X[n + 1].ToString());

        }

        public void Dif(Polynomial[] X, int n)

        {

            Console.WriteLine("\n**ДИФФЕРЕНЦИРОВАНИЕ ПОЛИНОМА**\n");

            Console.WriteLine("|Выберите полином, который хотите продифференцировать.");

            for (int i = 1; i <= n; i++)

            {

                Console.WriteLine($"\n|Полином с номером {i}\n-> ");

                Console.WriteLine(X[i].ToString());

            }

            Console.WriteLine("|Введите номер полинома: ");

            int a;

            a = int.Parse(Console.ReadLine());

            X[n + 1] = X[a].Differentiate();

            Console.WriteLine($"|Получившийся полином \n-> ");

            Console.WriteLine(X[n + 1].ToString());

        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
