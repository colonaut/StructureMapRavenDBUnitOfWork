﻿using System;
using System.Web.Http;
using System.Web.Mvc;
using Raven.Client;
using RavenDBUnitOfWork.App_Start;
using RavenDBUnitOfWork.DependencyResolution;
using RavenDBUnitOfWork.Filters;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(RavenDbUnitOfWorkFilterConfig), "Start")]
[assembly: WebActivator.PostApplicationStartMethod(typeof(RavenDbUnitOfWorkStructureMapRegistry), "Start")]

namespace RavenDBUnitOfWork.App_Start
{

    public static class RavenDbUnitOfWorkFilterConfig
    {
        public static void Start()
        {
            //add raven attribute to filters for mvc (handles save and close).
            //Disable this method if you want to set the attribute manually for some controllers instead of all. Disable this method if you want to set the attribute manually for some controllers instead of all.
            GlobalFilters.Filters.Add(new RavenDbDocumentSessionAttribute());
            //add http raven attribute for WebApi to global filters (handles save and close of raven session for WebApi).
            //Disable this method if you want to set the attribute manually for some controllers instead of all.
            GlobalConfiguration.Configuration.Filters.Add(new RavenDbDocumentSessionHttpAttribute());
        }
    }
    
    public static class RavenDbUnitOfWorkStructureMapRegistry
    {
        public static void Start()
        {
            //add additional StructureMapRavenDBRegistry to ObjectFactory's container _after_ (PostApplicationStart) the initialization is done (in StructuremapMvc).
            ObjectFactory.Configure(r => r.AddRegistry<StructureMapRavenDbRegistry>());
            InitRavenDbStartup();
        }

        //tests only!
        static void InitRavenDbStartup()
        {

            using (var session = ObjectFactory.GetInstance<IDocumentSession>())
            {

                session.Store(new StartUp());
                session.SaveChanges();
            }
        }

        public class StartUp
        {
            public StartUp()
            {
                Created = DateTime.UtcNow;
                TestResult = "If you can read this, everything's fine. You did it! Your RavenDB is up and running! Use your controllers implementing IDocumentSession and start convenient working with RavenDB!";
            }

            public int Id { get; set; }
            public DateTime Created { get; set; }
            public string TestResult { get; set; }

        }

    }

}