﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkillTestSegfy.Infrastructure.Services.Youtube;
using SkillTestSegfy.Web.Models.Youtube;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillTestSegfy.Web.Controllers
{
    public class YoutubeController : Controller
    {
        public YoutubeController(IYoutubeApiService youtubeApiService)
        {
            YoutubeApiService = youtubeApiService;
        }

        private IYoutubeApiService YoutubeApiService { get; }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Search));
        }

        public async Task<IActionResult> Search(YoutubeSearchModel model)
        {
            var response = await YoutubeApiService.Search(model.Term, maxResults: model.MaxResults ?? 15, type: model.Type);
            if (response.Success)
            {
                model.ResponseItems = response.Items.Select(o => new YoutubeSearchItemModel(o)).ToList();
                if (!model.ResponseItems.Any())
                {
                    model.ResponseMessage = "Nenhum resultado encontrado.";
                }
            }
            else
            {
                model.ResponseMessage = response.Error;
            }

            model.AvailableTypes = new List<SelectListItem>
            {
                new SelectListItem("Tipo: Todos", null, true),
                new SelectListItem("Tipo: Somente Vídeos", ((int)YoutubeItemType.Video).ToString(), false),
                new SelectListItem("Tipo: Somente Canais", ((int)YoutubeItemType.Channel).ToString(), false),
                new SelectListItem("Tipo: Somente Playlists", ((int)YoutubeItemType.Playlist).ToString(), false)
            };

            model.AvailableMaxResults = new List<SelectListItem>
            {
                new SelectListItem("Resultados: 5", "5", false),
                new SelectListItem("Resultados: 15", "15", true),
                new SelectListItem("Resultados: 30", "30", false),
                new SelectListItem("Resultados: 50", "50", false)
            };

            return View(model);
        }

        public async Task<IActionResult> History()
        {
            return View();
        }
    }
}