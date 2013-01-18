﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using NUnit.Framework;
using Ninject;
using Ninject.Syntax;
using Noodle.Tests;
using Noodle.Web;

namespace Noodle.Settings.Tests
{
    [TestFixture]
    public class SettingServiceTests : DataTestBase
    {
        private Setting _testsetting;
        private ISettingService _settingService;

        public override void SetUp()
        {
            _kernel.Resolve<MongoCollection<Setting>>().RemoveAll();
            _settingService = _kernel.Resolve<ISettingService>();
            base.SetUp();
        }

        [Test]
        public void Can_delete_a_setting()
        {
            // add setting
            _settingService.GetAllSettings().Count.ShouldEqual(0);
            _settingService.SetSetting("CanDeleteSetting", true);
            _settingService.GetAllSettings().ContainsKey("CanDeleteSetting").ShouldBeTrue();

            // delete it
            _settingService.DeleteSettingByKey("CanDeleteSetting");

            // ensure its not there
            _settingService.GetAllSettings().ContainsKey("CanDeleteSetting").ShouldBeFalse();
            _settingService.GetAllSettings().Count.ShouldEqual(0);
        }

        [Test]
        public void Can_get_all_settings()
        {
            _settingService.GetAllSettings().Count.ShouldEqual(0);

            _settingService.SetSetting("CanGetAllSettings1", "1");
            _settingService.SetSetting("CanGetAllSettings2", "2");

            _settingService.GetAllSettings().Count.ShouldEqual(2);
        }


        [Test]
        public void Can_save_a_settings_object()
        {
            var testSetting = new TestSetting();
            testSetting.Decimal = (decimal) 3;
            testSetting.Enum = TestSetting.TestEnumType.TestValue2;
            testSetting.Integer = (int) 4;
            testSetting.Long = (long) 5;
            testSetting.String = "String...";

            _settingService.SaveSetting(testSetting);
            testSetting = new ConfigurationProvider<TestSetting>(_settingService, new ErrorNotifier()).Settings;

            testSetting.Decimal.ShouldEqual((decimal) 3);
            testSetting.Enum.ShouldEqual(TestSetting.TestEnumType.TestValue2);
            testSetting.Integer.ShouldEqual((int) 4);
            testSetting.Long.ShouldEqual((long) 5);
            testSetting.String.ShouldEqual("String...");
        }

        public class TestSetting : ISettings
        {
            public enum TestEnumType
            {
                TestValue1 = 1,
                TestValue2 = 2
            }
            public string String { get; set; }
            public int Integer { get; set; }
            public double Double;
            public long Long { get; set; }
            public decimal Decimal { get; set; }
            public TestEnumType Enum { get; set; }
        }

        public override IEnumerable<Engine.IDependencyRegistrar> GetDependencyRegistrars()
        {
            var registrars = base.GetDependencyRegistrars().ToList();
            registrars.Add(new MongoDB.DependencyRegistrar());
            registrars.Add(new DependencyRegistrar());
            return registrars;
        }
    }
}