// --- main 
#include "stdafx.h"
#include <iostream>
#include <iomanip> 
#include <ctime>
#include "Salesman.h"
#define N 5
int main()
{
    setlocale(LC_ALL, "rus");
    int d[N][N] = {   //0   1    2    3     4        
                    {  INF,  26, 34,  INF,   13},    //  0
                    { 13,   INF,  28,  55,  71},    //  1
                    { 15,  39,   INF,  86,   62},    //  2 
                    { 30,  45,  52,   INF,   39},    //  3
                    { 80,  79,  52,  26,    INF} };   //  4



    int r[N];                     // результат 
    int t1 = clock();
    int s = salesman(
        N,          // [in]  количество городов 
        (int*)d,          // [in]  массив [n*n] расстояний 
        r           // [out] массив [n] маршрут 0 x x x x  

    );
    int t2 = clock();
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
    for (int i = 0; i < N; i++) std::cout << r[i] + 1 << "-->"; std::cout << 1;
    std::cout << std::endl << "-- длина маршрута     : " << s;
    std::cout << std::endl << "Время подсчета       : " << ((double)(t2 - t1)) / ((double)CLOCKS_PER_SEC);
    std::cout << std::endl;
    system("pause");
    return 0;
}