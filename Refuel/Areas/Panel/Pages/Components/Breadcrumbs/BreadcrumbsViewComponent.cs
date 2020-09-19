using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Refuel.POCOs;
using Utils;

namespace Refuel.Components.Breadcrumbs
{
    public class BreadcrumbsViewComponent : ViewComponent
    {
        public List<Breadcrumb> ReadyElements { get; set; }
        private readonly Utils.IDictionaryService _dictionaryService;
        private readonly IVehiclesManager _vehicleManager;

        public BreadcrumbsViewComponent(Utils.IDictionaryService dictionaryService, IVehiclesManager vehicleManager)
        {
            _dictionaryService = dictionaryService;
            _vehicleManager = vehicleManager;
            ReadyElements = new List<Breadcrumb>();
        }

        public IViewComponentResult Invoke()
        {
            string urlPath = Url.ActionContext.ActionDescriptor.DisplayName;
            List<string> elements = urlPath.Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();

            string fullPath = HttpContext.Request.Path;

            Parameter parameter = GetParameterBreadcrumbs();

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
                AddHomeSectionsAndActionBreadcrumbs(fullPath, elements, parameter);
            }

            return View("Default", ReadyElements);
        }

        private Parameter GetParameterBreadcrumbs()
        {
            string path = HttpContext.Request.Path;
            string[] parts = path.Split('/');
            if (parts.Length >= 3 && Int32.TryParse(parts.Last(), out int id))
            {
                string translatedName = GetSectionTranslatedName(parts[2], parts.Last(), parts[3]);

                return new Parameter()
                {
                    TranslatedName = translatedName,
                    DirectPath = path
                };
            }

            return null;
        }

        private string GetSectionTranslatedName(string key, string value, string subkey)
        {
            switch(key)
            {
                case "Vehicles":
                    if (Int32.TryParse(value, out int id))
                    {
                        return _vehicleManager.GetVehicleManufacturerAndModelById(id);
                    }
                    
                    return "*** UNKNOWN VEHICLE ***";

                case "Refuels":

                    if (subkey == "List")
                    {
                        if (Int32.TryParse(value, out int id2))
                        {
                            return _vehicleManager.GetVehicleManufacturerAndModelById(id2);
                        }
                    }
                    else if (subkey == "Details" || subkey == "Edit" || subkey == "Delete")
                    {
                        return $"Tankowanie #{value}";
                    }
                    else if (subkey == "Add")
                    {
                        return "Nowe tankowanie";
                    }
                    
                    return "*** BŁĄD ***";

                default:
                    return "*** SECTION NAME MISSING ***";
            }
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

        private void AddHomeSectionsAndActionBreadcrumbs(string urlPath, List<string> elements, Parameter parameter)
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

            if (parameter != null)
            {
                ReadyElements.Add(new Breadcrumb()
                {
                    Page = Utils.Utils.GetUrlWithoutParameter(urlPath),
                    Content = _dictionaryService.GetBreadcrumbsTranslation(action),
                    IsActive = false
                });

                ReadyElements.Add(new Breadcrumb()
                {
                    Page = parameter.DirectPath,
                    Content = parameter.TranslatedName,
                    IsActive = true
                });
            }
            else
            {
                ReadyElements.Add(new Breadcrumb()
                {
                    Page = urlPath,
                    Content = _dictionaryService.GetBreadcrumbsTranslation(action),
                    IsActive = true
                });
            }
        }
    }
}
