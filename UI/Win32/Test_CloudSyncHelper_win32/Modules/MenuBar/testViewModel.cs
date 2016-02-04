﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Windows.Input;
using Telerik.JustMock;
using XElement.CloudSyncHelper.UI.Win32.Model;
using XElement.CloudSyncHelper.UI.Win32.Modules.ApplicationMenu;
using FilterViewModel = XElement.CloudSyncHelper.UI.Win32.Modules.Filter.ViewModel;
using MenuBarViewModel = XElement.CloudSyncHelper.UI.Win32.Modules.MenuBar.ViewModel;
using XeRandom = XElement.TestUtils.Random;

namespace XElement.CloudSyncHelper.UI.Win32.Modules.MenuBar
{
    [TestClass]
    public class testViewModel
    {
        [TestInitialize]
        public void TestInitialize()
        {
            this._mefParameters = new MefParameters();
            this._mefParameters.AppMenuContainer = Mock.Create<IApplicationMenuContainer>();
            this._mefParameters.FilterModel = Mock.Create<FilterModel>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this._mefParameters = null;
            this._target = null;
        }



        [TestMethod]
        public void testMenuBarVM_IsExportedViaMef()
        {
            var mefImport = new MefImportTestHelper();
            var container = this.CreateMefContainer();
            container.ComposeParts( mefImport );

            Assert.IsInstanceOfType( mefImport.MenuBarVM, typeof( MenuBarViewModel ) );
        }


        [TestMethod]
        public void testMenuBarVM_Constructor_InitializesSubElements()
        {
            this.InitializeTargetViaMef();

            Assert.IsNotNull( this._target.FilterVM );
            Assert.IsInstanceOfType( this._target.FilterVM, typeof( FilterViewModel ) );
            Assert.IsNotNull( this._target.ShowApplicationMenu );
            Assert.IsInstanceOfType( this._target.ShowApplicationMenu, typeof( ICommand ) );
        }


        [TestMethod]
        public void testMenuBarVM_IsFilterVisible_GetSet()
        {
            this.InitializeTargetViaMef();

            this._target.IsFilterVisible = true;
            Assert.IsTrue( this._target.IsFilterVisible );
            this._target.IsFilterVisible = false;
            Assert.IsFalse( this._target.IsFilterVisible );
        }

        [TestMethod]
        public void testMenuBarVM_IsFilterVisible_ClearFilterOnFalse()
        {
            var filterModelMock = Mock.Create<FilterModel>();
            filterModelMock.Filter = XeRandom.RandomString();
            this._mefParameters.FilterModel = filterModelMock;
            this.InitializeTargetViaMef();

            this._target.IsFilterVisible = false;

            Assert.AreEqual( String.Empty, filterModelMock.Filter );
        }


        [TestMethod]
        public void testMenuBarVM_ShowApplicationMenu_CallsIApplicationMenuContainer()
        {
            var appMenuContainerMock = Mock.Create<IApplicationMenuContainer>();
            Mock.Arrange( () => appMenuContainerMock.ShowApplicationMenu() ).MustBeCalled();
            this._mefParameters.AppMenuContainer = appMenuContainerMock;
            this.InitializeTargetViaMef();

            this._target.ShowApplicationMenu.Execute( "irrelevant" );

            Mock.Assert( appMenuContainerMock );
        }



        private ComposablePartCatalog CreateMefCatalog()
        {
            var assembly = typeof( App ).Assembly;
            return new AssemblyCatalog( assembly );
        }

        private CompositionContainer CreateMefContainer()
        {
            var catalog = this.CreateMefCatalog();
            var container = new CompositionContainer( catalog );

            container.ComposeExportedValue<IApplicationMenuContainer>( this._mefParameters.AppMenuContainer );
            container.ComposeExportedValue<FilterModel>( this._mefParameters.FilterModel );

            return container;
        }

        private void InitializeTargetViaMef()
        {
            var container = this.CreateMefContainer();
            container.ComposeParts( this );
        }

        [Import]
        private MenuBarViewModel _target;

        private MefParameters _mefParameters;

        private class MefImportTestHelper
        {
            [Import]
            public MenuBarViewModel MenuBarVM { get; private set; }
        }

        private class MefParameters
        {
            public IApplicationMenuContainer AppMenuContainer { get; set; }
            public FilterModel FilterModel { get; set; }
        }
    }
}
