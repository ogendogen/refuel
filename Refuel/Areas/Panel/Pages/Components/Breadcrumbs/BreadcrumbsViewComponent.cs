using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

            List<Parameter> preparedParameters = new List<Parameter>();
            if (HttpContext.Request.Query.Count > 0)
            {
                preparedParameters = GetParametersBreadcrumbs();
            }

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
                AddHomeSectionsAndActionBreadcrumbs(urlPath, elements, preparedParameters);
            }

            return View("Default", ReadyElements);
        }

        private List<Parameter> GetParametersBreadcrumbs()
        {
            List<Parameter> parameters = new List<Parameter>();

            foreach (var parameter in HttpContext.Request.Query)
            {
                string translatedKey = _dictionaryService.GetBreadcrumbsTranslation(parameter.Key);
                string translatedValue = GetParameterReadableValue(parameter.Key, parameter.Value.ToString());
                parameters.Add(new Parameter()
                {
                    RawKey = parameter.Key,
                    TranslatedKey = translatedKey,
                    RawValue = parameter.Value,
                    TranslatedValue = translatedValue
                });
            }

            return parameters;
        }

        private string GetParameterReadableValue(string key, string value)
        {
            switch(key)
            {
                case "vehicleId":
                    return _vehicleManager.GetVehicleManufacturerAndModelById(value);

                default:
                    return value;
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

        private void AddHomeSectionsAndActionBreadcrumbs(string urlPath, List<string> elements, List<Parameter> parameters)
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

            if (parameters.Count == 0)
            {
                ReadyElements.Add(new Breadcrumb()
                {
                    Page = urlPath,
                    Content = _dictionaryService.GetBreadcrumbsTranslation(action),
                    IsActive = true
                });
            }
            else
            {
                foreach (Parameter parameter in parameters)
                {
                    ReadyElements.Add(new Breadcrumb()
                    {
                        Page = $"{urlPath}?{parameter.RawKey}={parameter.RawValue}",
                        Content = $"{parameter.TranslatedKey} {parameter.TranslatedValue}",
                        IsActive = true
                    });
                }
            }
        }
    }
}
