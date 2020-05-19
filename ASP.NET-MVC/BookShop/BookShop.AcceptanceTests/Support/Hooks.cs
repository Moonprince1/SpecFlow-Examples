﻿using System;
using System.IO;
using BoDi;
using BookShop.AcceptanceTests.Drivers;
using BookShop.AcceptanceTests.Drivers.Integrated;
using BookShop.AcceptanceTests.Drivers.Selenium;
using Microsoft.Extensions.Configuration;
using TechTalk.SpecFlow;
using TechTalk.SpecRun;

namespace BookShop.AcceptanceTests.Support
{
    [Binding]
    public class Hooks
    {
        private readonly TestRunContext _testRunContext;

        public Hooks(TestRunContext testRunContext)
        {
            _testRunContext = testRunContext;
        }

        [BeforeScenario(Order = 1)]
        public void RegisterDependencies(IObjectContainer objectContainer)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(_testRunContext.TestDirectory, "appsettings.json"), optional: true, reloadOnChange: true)
                .Build();

            objectContainer.RegisterInstanceAs(config);


            switch (Environment.GetEnvironmentVariable("Mode"))
            {
                case "Integrated":
                    objectContainer.RegisterTypeAs<IntegratedBookDetailsDriver, IBookDetailsDriver>();
                    objectContainer.RegisterTypeAs<IntegratedHomeDriver, IHomeDriver>();
                    objectContainer.RegisterTypeAs<IntegratedShoppingCartDriver, IShoppingCartDriver>();
                    objectContainer.RegisterTypeAs<IntegratedSearchDriver, ISearchDriver>();
                    objectContainer.RegisterTypeAs<IntegratedSearchResultDriver, ISearchResultDriver>();
                    break;
                case "Chrome":
                    objectContainer.RegisterTypeAs<SeleniumBookDetailsDriver, IBookDetailsDriver>();
                    objectContainer.RegisterTypeAs<SeleniumHomeDriver, IHomeDriver>();
                    objectContainer.RegisterTypeAs<SeleniumShoppingCartDriver, IShoppingCartDriver>();
                    objectContainer.RegisterTypeAs<SeleniumSearchDriver, ISearchDriver>();
                    objectContainer.RegisterTypeAs<SeleniumSearchResultDriver, ISearchResultDriver>();
                    break;
            }
        }
    }
}
