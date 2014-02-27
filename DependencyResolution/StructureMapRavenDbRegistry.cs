using Raven.Client;
using Raven.Client.Embedded;
using StructureMap.Configuration.DSL;
//using Raven.Client.Document;

namespace MedienKultur.RavenDBUnitOfWork.DependencyResolution
{
    public sealed class StructureMapRavenDbRegistry : Registry
    {
        public StructureMapRavenDbRegistry()
        {
            // register RavenDB document store
            ForSingletonOf<IDocumentStore>().Use(() =>
            {
                //use if you are not having admin rights on your dev machine
                //NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8080);

                /* Use for self configured RavenDB Client */
                //var documentStore = new DocumentStore
                //{
                //    //use if you have a connection string in your web.config
                //    ConnectionStringName = "RavenDB"
                //}
                //.Initialize();

                /* Use for Embedded (Requires RavenDB.Embedded) */
                var documentStore = new EmbeddableDocumentStore
                {
                    //use to run raven db in memory for test (requires embedded)
                    RunInMemory = true,
                    //use if you have a connection string in your web.config
                    //ConnectionStringName = "RavenDB",
                    //use if your wan to directly access your raven database
                    //DataDirectory =  @"~\App_Data\RavenDB" (requires embedded)                    
                }.Initialize();
                
                return documentStore;
            });

            // register RavenDB document session
            For<IDocumentSession>().HybridHttpOrThreadLocalScoped()
                .Use(context => context.GetInstance<IDocumentStore>().OpenSession());

        }

    }

}