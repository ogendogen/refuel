using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Refuel.POCOs;
using Utils;

namespace Refuel.Components.Breadcrumbs
{
    public class BreadcrumbsViewComponent : ViewComponent
    {
        public List<Breadcrumb> ReadyElements { get; set; }
        private readonly Utils.IDictionaryService _dictionaryService;

        public BreadcrumbsViewComponent(Utils.IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
            ReadyElements = new List<Breadcrumb>();
        }

        public IViewComponentResult Invoke(string urlPath)
        {
            List<string> elements = urlPath.Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();
            int elementsAmount = elements.Count;

            if (elementsAmount == 1)
            {
                var element = elements.First();
                if (element == "Index")
                {
                    AddHomeOnlyBreadcrumb();
                }
                else
                {
                    AddHomeAndSectionBreadcrumbs();
                }
            }
            else if (elementsAmount > 1)
            {
                AddHomeSectionsAndActionBreadcrumbs(urlPath, elements);
            }

            return View("Default", ReadyElements);
        }

        private void AddHomeOnlyBreadcrumb()
        {
            ReadyElements.Add(new Breadcrumb()
            {
                Page = "/Index",
                Content = "Głowna Strona",
                IsActive = true
            });
        }

        private void AddHomeAndSectionBreadcrumbs()
        {
            ReadyElements.Add(new Breadcrumb()
            {
                Page = "/Index",
                Content = "Głowna Strona",
                IsActive = false
            });

            ReadyElements.Add(new Breadcrumb()
            {
                Page = "Index",
                Content = "Głowna Strona",
                IsActive = true
            });
        }

        private void AddHomeSectionsAndActionBreadcrumbs(string urlPath, List<string> elements)
        {
            ReadyElements.Add(new Breadcrumb()
            {
                Page = "/Index",
                Content = "Głowna Strona",
                IsActive = false
            });

            var action = elements.Last();
            elements.Remove(action);

            foreach (var element in elements)
            {
                ReadyElements.Add(new Breadcrumb()
                {
                    Page = $"/{element}/Index",
                    Content = _dictionaryService.GetBreadcrumbsTranslation(element),
                    IsActive = false
                });
            }

            ReadyElements.Add(new Breadcrumb()
            {
                Page = urlPath,
                Content = _dictionaryService.GetBreadcrumbsTranslation(action),
                IsActive = true
            });
        }
    }
}
