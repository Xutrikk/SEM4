// Main      
//#include "stdafx.h"
#include <iostream>
#include "Combi.h"
#include <time.h>
#include <iomanip> 
#include "Auxil.h"
#include "Salesman.h"

// Определите макросы для включения/выключения заданий
#define TASK_SUBSET_GENERATOR 1
#define TASK_COMBINATION_GENERATOR 1
#define PERMUTATION 1
#define ACCOMODATION 1
#define SALESMAN 1
#define SALESMAN_TIME 1

#define NN (sizeof(c)/sizeof(int))
#define MM 3
#define SPACE(n) std::setw(n)<<" "
#define N 5
#define R 4
#define F 3

int main()
{
    setlocale(LC_ALL, "rus");

#if TASK_SUBSET_GENERATOR
    // Генератор множества подмножеств
    char  AA[][2] = { "A", "B", "C", "D" };
    std::cout << std::endl << " - Генератор множества всех подмножеств -";
    std::cout << std::endl << "Исходное множество: ";
    std::cout << "{ ";
    for (int i = 0; i < sizeof(AA) / 2; i++)
        std::cout << AA[i] << ((i < sizeof(AA) / 2 - 1) ? ", " : " ");
    std::cout << "}";
    std::cout << std::endl << "Генерация всех подмножеств  ";
    combi::subset s1(sizeof(AA) / 2);         // создание генератора   
    int  n = s1.getfirst();                // первое (пустое) подмножество    
    while (n >= 0)                          // пока есть подмножества 
    {
        std::cout << std::endl << "{ ";
        for (int i = 0; i < n; i++)
            std::cout << AA[s1.ntx(i)] << ((i < n - 1) ? ", " : " ");
        std::cout << "}";
        n = s1.getnext();                   // cледующее подмножество 
    };
    std::cout << std::endl << "всего: " << s1.count() << std::endl;
#endif

#if TASK_COMBINATION_GENERATOR
    // Генератор сочетаний
    char  AAComb[][2] = { "A", "B", "C", "D", "E" };
    std::cout << std::endl << " --- Генератор сочетаний ---";
    std::cout << std::endl << "Исходное множество: ";
    std::cout << "{ ";
    for (int i = 0; i < sizeof(AAComb) / 2; i++)
        std::cout << AAComb[i] << ((i < sizeof(AAComb) / 2 - 1) ? ", " : " ");

    std::cout << "}";
    std::cout << std::endl << "Генерация сочетаний ";
    combi::xcombination xc(sizeof(AAComb) / 2, 3);
    std::cout << "из " << xc.n << " по " << xc.m;
    int  nComb = xc.getfirst();
    while (nComb >= 0)
    {

        std::cout << std::endl << xc.nc << ": { ";

        for (int i = 0; i < nComb; i++)


            std::cout << AAComb[xc.ntx(i)] << ((i < nComb - 1) ? ", " : " ");

        std::cout << "}";

        nComb = xc.getnext();
    };
    std::cout << std::endl << "всего: " << xc.count() << std::endl;
#endif


#if PERMUTATION
    char  AAPermut[][2] = { "A", "B", "C", "D" };
    std::cout << std::endl << " --- Генератор перестановок ---";
    std::cout << std::endl << "Исходное множество: ";
    std::cout << "{ ";
    for (int i = 0; i < sizeof(AAPermut) / 2; i++)

        std::cout << AAPermut[i] << ((i < sizeof(AAPermut) / 2 - 1) ? ", " : " ");
    std::cout << "}";
    std::cout << std::endl << "Генерация перестановок ";
    combi::permutation p(sizeof(AAPermut) / 2);
    __int64  nPermut = p.getfirst();
    while (nPermut >= 0)
    {
        std::cout << std::endl << std::setw(4) << p.np << ": { ";

        for (int i = 0; i < p.n; i++)

            std::cout << AAPermut[p.ntx(i)] << ((i < p.n - 1) ? ", " : " ");

        std::cout << "}";

        nPermut = p.getnext();
    };
    std::cout << std::endl << "всего: " << p.count() << std::endl;
#endif
#if ACCOMODATION
    char  AAAcc[][2] = { "A", "B", "C", "D" };
    std::cout << std::endl << " --- Генератор размещений ---";
    std::cout << std::endl << "Исходное множество: ";
    std::cout << "{ ";
    for (int i = 0; i < R; i++)
        std::cout << AAAcc[i] << ((i < R - 1) ? ", " : " "); 
    std::cout << "}";
    std::cout << std::endl << "Генерация размещений  из  " <<  R << " по " << F;
    combi::accomodation acc(R, F); 
    int  nAcc = acc.getfirst(); 
    while (nAcc >= 0)
    {
        std::cout << std::endl << std::setw(2) << acc.na << ": { ";
        for (int i = 0; i < 3; i++)
            std::cout << AAAcc[acc.ntx(i)] << ((i < nAcc - 1) ? ", " : " ");
        std::cout << "}";
        nAcc = acc.getnext();
    };
    std::cout << std::endl << "всего: " << acc.count() << std::endl;
#endif
#if SALESMAN
    int d[N][N] = {   //0   1    2    3     4        
                    {  INF,  26, 34,  INF,   13},    //  0
                    { 13,   INF,  28,  55,  71},    //  1
                    { 15,  39,   INF,  86,   62},    //  2 
                    { 30,  45,  52,   INF,   39},    //  3
                    { 80,  79,  52,  26,    INF} };   //  4
    int r[N];                     // результат 
    int s = salesman(
        N,          // [in]  количество городов 
        (int*)d,          // [in]  массив [n*n] расстояний 
        r           // [out] массив [n] маршрут 0 x x x x  

    );
    std::cout << std::endl << "-- Задача коммивояжера -- ";
    std::cout << std::endl << "-- количество  городов: " << N;
    std::cout << std::endl << "-- матрица расстояний : ";
    for (int i = 0; i < N; i++)
    {
        std::cout << std::endl;
        for (int j = 0; j < N; j++)

            if (d[i][j] != INF) std::cout << std::setw(3) << d[i][j] << " ";

            else std::cout << std::setw(3) << "INF" << " ";
    }
    std::cout << std::endl << "-- оптимальный маршрут: ";
    for (int i = 0; i < N; i++) std::cout << r[i] << "-->"; std::cout << 0;
    std::cout << std::endl << "-- длина маршрута     : " << s;
    std::cout << std::endl;
#endif

#if SALESMAN_TIME
    int dSalesmanTime[N * N + 1], rSalesmanTime[N];
    auxil::start();
    for (int i = 0; i <= N * N; i++) {
        dSalesmanTime[i] = auxil::iget(10, 100);
    }
    std::cout << std::endl << "-- Задача коммивояжера -- ";
    std::cout << std::endl << "-- количество ------ продолжительность -- ";
    std::cout << std::endl << "      городов           вычисления  ";
    clock_t t1, t2;
    for (int i = 7; i <= N; i++)
    {
        t1 = clock();
        salesman(i, (int*)dSalesmanTime, rSalesmanTime);
        t2 = clock();
        std::cout << std::endl << SPACE(7) << std::setw(2) << i
            << SPACE(15) << std::setw(5) << (t2 - t1);
    }
    std::cout << std::endl;
#endif

    system("pause");
    return 0;
}