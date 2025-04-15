#include "pch.h"
#include "Auxil.h"                            // вспомогательные функции                         // вспомогательные функции 
#include <iostream>
#include <ctime>
#include <locale>

#define  CYCLE  1000000                       // количество циклов  

int fibonacci(int i)
{
	if (i < 1) return 0;
	if (i == 1) return 1;
	return fibonacci(i - 1) + fibonacci(i - 2);
}

int main()
{

	double  av1 = 0, av2 = 0;
	clock_t  t1 = 0, t2 = 0;

	setlocale(LC_ALL, "rus");

	auxil::start();                          // старт генерации 
	t1 = clock();                            // фиксация времени 
	for (int i = 0; i < CYCLE; i++)
	{
		av1 += (double)auxil::iget(-100, 100); // сумма случайных чисел 
		av2 += auxil::dget(-100, 100);         // сумма случайных чисел 
	}
	t2 = clock();                            // фиксация времени 


	std::cout << std::endl << "количество циклов:         " << CYCLE;
	std::cout << std::endl << "среднее значение (int):    " << av1 / CYCLE;
	std::cout << std::endl << "среднее значение (double): " << av2 / CYCLE;
	std::cout << std::endl << "продолжительность (у.е):   " << (t2 - t1);
	std::cout << std::endl << "                  (сек):   "
		<< ((double)(t2 - t1)) / ((double)CLOCKS_PER_SEC);
	std::cout << std::endl;

	for (int n = 22; n < 43; n++)
	{
		t1 = clock();
		int num = fibonacci(n);
		t2 = clock();
		std::cout << std::endl << n << "-е число Фибоначчи " << (t2 - t1) << " у.е.";
	}

	std::cout << std::endl;


	system("pause");

	return 0;
}
