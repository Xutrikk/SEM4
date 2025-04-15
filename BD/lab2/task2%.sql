UPDATE Клиенты
SET Признак_скидки = 1
WHERE ID_Клиента IN (
    SELECT ID_Клиента FROM Заказы
    GROUP BY ID_Клиента
    HAVING SUM(Общая_стоимость) > 50000
);

UPDATE Заказы
SET Общая_стоимость = Общая_стоимость * 0.98
WHERE ID_Клиента IN (
    SELECT ID_Клиента FROM Клиенты WHERE Признак_скидки = 1
);
