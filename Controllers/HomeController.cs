using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sea_war.Models;
using sea_war.BLL;

namespace sea_war.Controllers
{   
	public class HomeController : Controller
	{
		public static Game game = new Game();
		
		[HttpGet]
		public IActionResult Index()
		{
			return View(game);
		}
		[HttpGet]
		public IActionResult StartGame()
		{
			game = new Game();
			// return View("Index", game);
			return RedirectToAction("Index", game);
		}
		[HttpGet]
		public ActionResult ManShot(int x, int y)
		{
			if (game.Start != 0)
			{
				if (game.ManTurn && game.Bot.CellsWithShots[x, y] != 1)
				{
					game.Bot.CellsWithShots[x, y] = 1;

					Random random = new Random();

					for (int i = 0; i < 10; i++)
					{
						if (game.Bot.IsDead(game.Bot.Ships[i]))
							game.Bot.NeedToShotFarther(game.Bot.Ships[i]);
					}

					if (game.Bot.CellsWithShips[x, y] == -1)
					{
						game.ManTurn = false;
					}

					game.TryFinish();
				}
			}

			return RedirectToAction("Index", game);
		}
		[HttpGet]
		public ActionResult How(int i)
		{
			if (i == 2)
			{
				if (game.Start == 0)
				{
					game.Start = 2;
					game.PlaceShip(game.Man, 4, 1);
					game.PlaceShip(game.Man, 3, 2);
					game.PlaceShip(game.Man, 2, 3);
					game.PlaceShip(game.Man, 1, 4);
				}
			}
			if (i == 1)
			{
				if (game.Start == 0)
				{
					game.Start = 1;
				}
			}
			return RedirectToAction("Index", game);
		}

		[HttpGet]
		public ActionResult BotShot()
		{
			if (game.ShotBot())
			{
				game.ManTurn = false;
			}
			else
			{
				game.ManTurn = true;
			}

			game.TryFinish();

			return RedirectToAction("Index", game);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
} 
