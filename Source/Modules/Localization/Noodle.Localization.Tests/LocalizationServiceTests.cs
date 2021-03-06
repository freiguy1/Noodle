﻿using System.Linq;
using NUnit.Framework;
using Noodle.Localization.Services;
using Noodle.Settings;
using Noodle.Tests;

namespace Noodle.Localization.Tests
{
    [TestFixture]
    public class LocalizationServiceTests : LocalizationTestsBase
    {
        [Test]
        public void Can_get_all_resources_by_langage()
        {
            // setup
            var language1 = _languageService.InsertLanguage(CreateLanguage(1));
            var language2 = _languageService.InsertLanguage(CreateLanguage(2));
            CreateSampleResources(language1.Id);
            CreateSampleResources(language2.Id);

            // act
            var resources = _localizationService.GetAllResourcesByLanguageId(language1.Id);

            // assert
            resources.Count.ShouldEqual(23);
            resources.All(x => x.LanguageId == language1.Id).ShouldBeTrue();
        }

        [Test]
        public void Can_get_resource_by_id()
        {
            // setup
            var resource = _localizationService.InsertLocaleStringResource(CreateResource(1, _languageService.InsertLanguage(CreateLanguage()).Id));

            // act
            var dbResource = _localizationService.GetLocaleStringResourceById(resource.Id);

            // assert
            dbResource.ShouldNotBeNull();
            dbResource.Id.ShouldEqual(resource.Id);
        }

        [Test]
        public void Can_get_resource_by_name()
        {
            // setup
            var resource = _localizationService.InsertLocaleStringResource(CreateResource(1, _languageService.InsertLanguage(CreateLanguage()).Id));

            // act
            var dbResource = _localizationService.GetLocaleStringResourceByName(resource.ResourceName, resource.LanguageId);

            // assert
            dbResource.ShouldNotBeNull();
            dbResource.Id.ShouldEqual(resource.Id);
        }

        [Test]
        public void Can_get_resource_from_default_language()
        {
            // setup
            var language1 = _languageService.InsertLanguage(CreateLanguage(1));
            var language2 = _languageService.InsertLanguage(CreateLanguage(1));
            var localizationSetting = _container.Resolve<IConfigurationProvider<LocalizationSettings>>();
            localizationSetting.Settings.DefaultLanguageId = language2.Id.ToString();
            localizationSetting.SaveSettings();
            var resource1 = CreateResource(1, language1.Id);
            resource1.ResourceName = "test default";
            resource1.ResourceValue = "test default value 1";
            _localizationService.InsertLocaleStringResource(resource1);
            var resource2 = CreateResource(1, language2.Id);
            resource2.ResourceName = "test default";
            resource2.ResourceValue = "test default value 2";
            _localizationService.InsertLocaleStringResource(resource2);

            // act
            var value = _container.Resolve<ILocalizationService>().GetResource("test default");

            // assert
            value.ShouldEqual("test default value 2");
        }

        [Test, Ignore]
        public void Can_log_if_resource_not_found()
        {
            // todo
        }
    }
}
