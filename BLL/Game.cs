using System;
using System.Collections.Generic;
using System.Linq;

namespace sea_war.BLL
{   
	public class Game
	{
		public int Start = 0;
		public bool ManTurn = true;
		public bool Finished;
		public string Winner;
		public Player Man;
		public Player Bot;
		Random random = new Random();

		public Game()
		{
			Man = new Player();

			Bot = new Player();
			PlaceShip(Bot, 4, 1);
			PlaceShip(Bot, 3, 2);
			PlaceShip(Bot, 2, 3);
			PlaceShip(Bot, 1, 4);
		}

		//== Стрельба Бота ==//
		public bool ShotBot()
		{
			int who = IsBeNotDeadShip(Man);
			if (who == -1)
			{
				if (ShotBotTo0())
					return true;
				else
					return false;
			}
			if (who == 3 || who == 4 || who == 5)
			{
				if (ShotBotTo2(who))
					return true;
				else
					return false;
			}
			if (who == 1 || who == 2 || who == 0)
			{
				if (ShotBotTo3or4(who))
					return true;
				else
					return false;
			}
			return false;
		}

		//== Есть Ли Подбитый Корабль ==//
		int IsBeNotDeadShip(Player player)
		{
			for (int i = 0; i < 10; i++)
			{
				if (player.WasShot(player.Ships[i]))
					if (!player.IsDead(player.Ships[i]))
						return i;
			}
			return -1;
		}

		//== Стрельба Рандомна ==// 
		bool ShotBotTo0()
		{
			while (true)
			{
				int x = random.Next(10);
				int y = random.Next(10);

				if (Man.CellsWithShots[x, y] == 0)
				{
					if (AreShootingBot(x, y))
						return true;
					else
						return false;
				}
			}
		}

		//== Стрельба По 2-ушке ==// 
		bool ShotBotTo2(int who)
		{ 
			for (int i = 0; i < Man.Ships[who].Cells.Length; i++)
			{
				int x = Man.Ships[who].Cells[i].X;
				int y = Man.Ships[who].Cells[i].Y;
				if (Man.CellsWithShots[x, y] == 0)
					continue;
				
				while (true)
				{
					int way = random.Next(4);
					if (way == 0)
					{
						if(CanShoot(x - 1, y))
							x--;
						else
							continue;
					}
					if (way == 1)
					{
						if(CanShoot(x, y + 1))
							y++;
						else
							continue;
					}
					if (way == 2)
					{
						if(CanShoot(x + 1, y))
							x++;
						else
							continue;
					}
					if (way == 3)
					{
						if(CanShoot(x, y - 1))
							y--;
						else
							continue;
					}

					if (AreShootingBot(x, y))
						return true;
					else
						return false;
				}
			}
			return false;
		}

		private bool CanShoot(int x, int y)
		{
			if (x < 0 || x > 9 || y < 0 || y > 9 || Man.CellsWithShots[x, y] == 1)
			{
				return false;
			}

			return true;
		}

		//== Стрельба По 3-ёшке Или 4-ёрке ==// 
		bool ShotBotTo3or4(int who)
		{
			for (int i = 0; i < Man.Ships[who].Cells.Length; i++)
			{
				int x = Man.Ships[who].Cells[i].X;
				int y = Man.Ships[who].Cells[i].Y;

				if (Man.CellsWithShots[x, y] == 0)
					continue;

				int i2 = i + 1;

				if (who == 1 || who == 2)
				{
					if (i2 == Man.Ships[who].Cells.Length)
					{
						if (ShotBotTo2(who))
							return true;
						else
							return false;
					}
				}
				if (who == 0)
				{
					if (i2 == Man.Ships[who].Cells.Length)
					{
						if (ShotBotTo2(who))
							return true;
						else
							return false;
					}
				}
				int x1 = Man.Ships[who].Cells[i2].X;
				int y1 = Man.Ships[who].Cells[i2].Y;

				if (Man.CellsWithShots[x1, y1] == 0)
				{
					if (ShotBotTo2(who))
						return true;
					else
						return false;
				}
				int i3 = i + 2;
				
				if (who == 1 || who == 2)
				{
					if (i3 == Man.Ships[who].Cells.Length)
					{
						if (way3or4(x, y, x1, y1))
							return true;
						else
							return false;
					}
				}
				if (who == 0)
				{
					if (i3 == Man.Ships[who].Cells.Length)
					{
						if (way3or4(x, y, x1, y1))
							return true;
						else
							return false;
					}
				}                
				int x2 = Man.Ships[who].Cells[i3].X;
				int y2 = Man.Ships[who].Cells[i3].Y;
				
				if (Man.CellsWithShots[x2, y2] == 0)
				{
					if (way3or4(x, y, x1, y1))
						return true;
					else
						return false;
				}  
				else
				{
					if (way3or4(x, y, x2, y2))
						return true;
					else
						return false;
				}
			}
			return false;
		}

		//== Выбор Направления Для Стрельбы По Тройкам И Четвёркам ==// 
		bool way3or4(int x, int y, int x1, int y1)
		{
			int position = WhatPosition(x, y, x1, y1);
			if (position == 2) // горизонталь
			{
				while (true)
				{
					int way = random.Next(2);
					if (way == 0)
					{
						y = y1 + 1;
					}
					if (way == 1)
					{
						y = y - 1;
					}
					if (x < 0 || x > 9 || y < 0 || y > 9 || Man.CellsWithShots[x, y] == 1)
						continue;
					break;
				}
			}
			else // вертикаль
			{
				while (true)
				{
					int way = random.Next(2);
					if (way == 0)
					{
						x = x1 - 1;
					}
					if (way == 1)
					{
						x = x + 1;
					}
					if (x < 0 || x > 9 || y < 0 || y > 9 || Man.CellsWithShots[x, y] == 1)
						continue;
					break;
				}
			}
			if (AreShootingBot(x, y))
				return true;
			else
				return false;
		}

		//== Определение Направления Корабля ==// 
		int WhatPosition (int x1, int y1, int x2, int y2) //1 - по вертикали, 2 - по горизонтали
		{
			if (x1 == x2)
				return 2;
			else
				return 1;
		}
		
		//== Сам Выстрел Боты ==//
		bool AreShootingBot(int x, int y)
		{
			Man.CellsWithShots[x, y] = 1;

			if (Man.CellsWithShips[x, y] >= 0)
			{
				for (int i = 0; i < 10; i++)
				{
					if (Man.IsDead(Man.Ships[i]))
						Man.NeedToShotFarther(Man.Ships[i]);
				}
				return true;
			}
			return false;
		}  

		//== Расстановка Кораблей ==// 
		public void PlaceShip(Player player, int lengthShip, int shipCount)
		{
			int shipCounter = 0;
			while (shipCounter < shipCount)
			{
				int x = random.Next(10);
				int y = random.Next(10);
				int way = random.Next(2);

				bool free = CanPlaceShip(x, y, lengthShip, way, player);
				if (free)
				{
					Ship ship = new Ship();
					ship.Cells = new Cell[lengthShip];

					for (int i = 0; i < lengthShip; i++)
					{
						player.CellsWithShips[x, y] = player.Ships.Count;

						ship.Cells[i] = new Cell() {
							X = x, Y = y
						};

						if (way == 0)
							x = x - 1;
						if (way == 1)
							y = y + 1;
					   }
					player.Ships.Add(ship);

					shipCounter++;  
				}
			}
		}

		//== Проверка Расстановки Кораблей ==//
		bool CanPlaceShip(int x, int y, int LengthShip, int way, Player player)
		{
			for (int CountLength = 0; CountLength < LengthShip; CountLength++)
			{
				bool free;
				free = CanPlaceCell(x, y, player);
				if (!free)
				{
					return false;
				}
				if (way == 0)
					x = x - 1;
				if (way == 1)
					y = y + 1;
			}
			return true;
		}

		//== Проверка Расстановки Ячейки ==//
		bool CanPlaceCell(int x, int y, Player player)
		{
			if (x < 0 || y > 9 )
				return false;
			for (int i = x - 1; i <= x + 1; i++)
			{
				if (i < 0 || i > 9)
					continue;
				for (int j = y - 1; j <= y + 1; j++)
				{
					if (j < 0 || j > 9)
						continue;
					if (player.CellsWithShips[i, j] >= 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		//== Проверка Завершения Игры ==//
		public void TryFinish() 
		{
			bool finish_M = false;
			bool finish_B = false;
			finish_M = Man.FinishGame();
			finish_B = Bot.FinishGame();
			if (finish_M)
			{
				//конец. выиграл Bot
				Finished = true;
				Winner = "Bot";
			}
			if (finish_B)
			{
				//конец. выиграл игрок
				Finished = true;    
				Winner = "Вы";
			}
		}
	}

	public static class Helper
	{
		public static T RandomElement<T>(this IEnumerable<T> collection)
		{
			Random random = new Random();
			T[] array = collection.ToArray();
			return array[random.Next(array.Length)];
		}
	}

	public enum OrientationType
	{
		Up,
		Down,
		Left,
		Right
	}
}