﻿using System.Collections.Generic;
using System.Security.Cryptography;
using Antlr.Runtime.Misc;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;

namespace Noodle.Management.Library.Navigation
{
    public class FluentSiteMapNodeProvider : ISiteMapNodeProvider
    {
        private readonly ISiteMapNodeFactory _sitemapNodeFactory;

        public FluentSiteMapNodeProvider(ISiteMapNodeFactory sitemapNodeFactory)
        {
            _sitemapNodeFactory = sitemapNodeFactory;
        }

        public IEnumerable<ISiteMapNodeToParentRelation> GetSiteMapNodes(ISiteMapNodeHelper helper)
        {
            var builders = new List<MenuItemBuilder>();
            var menuItemFactory = new MenuItemFactory(builders, helper);

            menuItemFactory.Add().SetTitle("home").SetUrl("http://www.google.com/").Items(children =>
            {
                children.Add().SetTitle("test").SetController("default").SetAction("index");
                //children.Add().SetTitle("test3").SetController("default").SetAction("index").Items(c =>
                //{
                //    c.Add().SetTitle("test2").SetController("default").SetAction("index");
                //});
            });

            var nodes = new List<ISiteMapNodeToParentRelation>();
            foreach (var builder in builders)
                RecursivelyBuildNodes(helper, null, builder, nodes);
            return nodes;
        }

        private void RecursivelyBuildNodes(ISiteMapNodeHelper helper, ISiteMapNodeToParentRelation parent, MenuItemBuilder builder, List<ISiteMapNodeToParentRelation> nodes)
        {
            var node = builder.CreateNode(helper, parent != null ? parent.Node : null);
            nodes.Add(node);
            if (builder.Children != null)
                foreach (var child in builder.Children)
                    RecursivelyBuildNodes(helper, node, child, nodes);
        }
    }
}
