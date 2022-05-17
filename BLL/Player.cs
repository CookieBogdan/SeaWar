using System;
using System.Collections.Generic;

namespace sea_war.BLL
{   
	public class Player
	{
		public int[,] CellsWithShips; //ship - корабль; -1 - корабля нет; 0 и больше - корабль есть
		public int[,] CellsWithShots; //shot - выстрел; 0 - выстрела еще не было; 1 - выстрел был
		//public int[,] Dead;
		public List<Ship> Ships;
		public Player()
		{
			Ships = new List<Ship>();
			CellsWithShips = new int[10, 10];
			CellsWithShots = new int[10, 10];
			for (int i_1 = 0; i_1 < 10; i_1++)
			for (int j_1 = 0; j_1 < 10; j_1++)
			{
				CellsWithShips[i_1, j_1] = -1;
				CellsWithShots[i_1, j_1] = 0;
			}
		}

//== Проверка Закончена Ли Игра ==//    
		public bool FinishGame()
		{
			int Count_Finish = 0;
			for (int i_finish = 0; i_finish < 10; i_finish++)
			for (int j_finish = 0; j_finish < 10; j_finish++)
			{                   
				if (CellsWithShips[i_finish, j_finish] >= 0)
				{
					if (CellsWithShots[i_finish, j_finish] == 1)
					{
						Count_Finish++;
					}
				}
			}
			if (Count_Finish == 20)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

//== Проверка Умер Ли Корабль ==// false - есть куда стрелять(жив ещё), true - некуда(умер)
		public bool IsDead(Ship ship)
		{
			for (int i = 0; i < ship.Cells.Length; i++)
			{
				int x1 = ship.Cells[i].X;
				int y1 = ship.Cells[i].Y;
				if (CellsWithShots[x1, y1] == 0)
					return false;
			}
			int x = ship.Cells[0].X;
			int y = ship.Cells[0].Y;
			return true;
		}

//== Хоть Раз Стреляли В Этот Корабль ==// false - не стреляли, true - да был
		public bool WasShot(Ship ship)
		{
			for (int i = 0; i < ship.Cells.Length; i++)
			{
				int x1 = ship.Cells[i].X;
				int y1 = ship.Cells[i].Y;
				if (CellsWithShots[x1, y1] == 1)
					return true;
			}
			return false;
		}

//== Выстрелы Вокруг Убитого Коробля ==//  
		public void NeedToShotFarther(Ship ship)
		{
			int x0 = ship.Cells[0].X + 1;
			int y0 = ship.Cells[0].Y - 1;

			int maxLength = ship.Cells.Length - 1;

			int x1 = ship.Cells[maxLength].X - 1;
			int y1 = ship.Cells[maxLength].Y + 1;
			int xm = x0;

			while (true)
			{
				if(x0 >= 0 && x0 <= 9 && y0 >= 0 && y0 <= 9)
					CellsWithShots[x0, y0] = 1;
				x0 = x0 - 1;
				if (x0 < x1)
				{
					y0++;
					x0 = xm;
					if(y0 > y1)
						break;
				}
			}
			
		}
	}
}
